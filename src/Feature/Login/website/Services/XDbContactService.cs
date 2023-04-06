using System;
using System.Web;
using SYMB2C.Feature.Login.Models.XDb;
using SYMB2C.Foundation.DependencyInjection;
using Sitecore.Analytics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;

namespace SYMB2C.Feature.Login.Services
{
    [Service(typeof(IXDBContactService))]
    public class XDbContactService : IXDBContactService
    {
        public void IdentifyCurrent(IXDbContactWithEmail contact)
        {
            CheckIdentifier(contact);
            Tracker.Initialize();
            if (Tracker.Current.Interaction == null)
            {
                Tracker.StartTracking();
            }

            if (Tracker.Current?.Session != null)
            {
                Tracker.Current?.Session.IdentifyAs(contact.IdentifierSource, contact.IdentifierValue);
            }
        }

        private static void CheckIdentifier(IXDbContact contact)
        {
            if (string.IsNullOrEmpty(contact.IdentifierSource) || string.IsNullOrEmpty(contact.IdentifierValue))
            {
                throw new Exception("A contact must have an identifiersource and identifiervalue!");
            }
        }

        public void UpdateOrCreateServiceContact(IXDbContactWithEmail serviceContact)
        {
            CheckIdentifier(serviceContact);
            using (var client = SitecoreXConnectClientConfiguration.GetClient())
            {
                var reference = new IdentifiedContactReference(serviceContact.IdentifierSource, serviceContact.IdentifierValue);
                var contact = client.Get(reference, new ContactExpandOptions(CollectionModel.FacetKeys.EmailAddressList, CollectionModel.FacetKeys.PersonalInformation));
                if (contact == null)
                {
                    contact = new Contact(new ContactIdentifier(reference.Source, reference.Identifier, ContactIdentifierType.Known));
                    client.AddContact(contact);

                    if (reference.Source == "Login")
                    {
                        Guid channelId = Guid.Parse("{B418E4F2-1013-4B42-A053-B6D4DCA988BF}");
                        string userAgent = HttpContext.Current.Request.UserAgent;
                        var interaction = new Interaction(contact, InteractionInitiator.Contact, channelId, userAgent);

                        Guid goalID = Guid.Parse("{A661ADE4-2F12-42C5-A66D-F0D940035752}"); // ID of goal item
                        var goal = new Goal(goalID, DateTime.UtcNow);

                        Sitecore.Data.Items.Item personalLogin = Sitecore.Context.Database.Items.GetItem("{A661ADE4-2F12-42C5-A66D-F0D940035751}");
                        goal.EngagementValue = Convert.ToInt32(personalLogin["Points"]);

                        interaction.Events.Add(goal);

                        client.AddInteraction(interaction);
                    }
                }

                var pIFacet = contact.GetFacet<PersonalInformation>(PersonalInformation.DefaultFacetKey);
                if (pIFacet == null)
                {
                    pIFacet = new PersonalInformation()
                    {
                        FirstName = serviceContact.FirstName,
                        LastName = serviceContact.LastName,
                        JobTitle = serviceContact.JobTitle,
                        Nickname = serviceContact.NickName,
                    };
                }
                else
                {
                    pIFacet.FirstName = serviceContact.FirstName;
                    pIFacet.LastName = serviceContact.LastName;
                    pIFacet.JobTitle = serviceContact.JobTitle;
                    pIFacet.Nickname = serviceContact.NickName;
                }

                client.SetPersonal(contact, pIFacet);

                var emailFacet = contact.GetFacet<EmailAddressList>(EmailAddressList.DefaultFacetKey);

                if (emailFacet == null)
                {
                    emailFacet = new EmailAddressList(new EmailAddress(serviceContact.Email, true), "Preferred");
                }
                else
                {
                    if (emailFacet.PreferredEmail?.SmtpAddress != serviceContact.Email)
                    {
                        emailFacet.PreferredEmail = new EmailAddress(serviceContact.Email, true);
                    }
                }

                client.SetEmails(contact, emailFacet);

                client.Submit();
            }
        }
    }
}