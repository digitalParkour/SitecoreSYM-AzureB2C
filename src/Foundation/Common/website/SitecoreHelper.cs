using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SYMB2C.Foundation.DependencyInjection;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;

namespace SYMB2C.Foundation.Common
{
    [Service(typeof(ISitecoreHelper))]
    public class SitecoreHelpers : ISitecoreHelper
    {
        public SitecoreHelpers()
        {
            this.Database = Factory.GetDatabase(Sitecore.Context.Database.Name);
        }
        public Database Database
        {
            get;
            private set;
        }

        /// <summary>
        /// Get item from sitecore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        public T GetItem<T>(string path, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = this.Database.GetItem(path);
            return item as T;
        }

        /// <summary>
        /// Get item from sitecore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        public Item GetItem(string itemName)
        {
            Item renderingParameters = this.Database.GetItem("");
            if (renderingParameters != null && renderingParameters.Children.Count > 0)
            {
                return renderingParameters.Children.Where(x => x.Name.Equals(itemName)).FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// Get child items of a parent item from Sitecore
        /// </summary>
        /// <param name="datasourceItem"></param>
        /// <returns></returns>
        public IEnumerable<Item> GetCompositeItems(Item datasourceItem)
        {
            List<Item> children = datasourceItem.Children.ToList<Item>();
            return children;
        }

        /// <summary>
        /// Get Image and its URL from Sitecore.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="imageField"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public ImageField ImageUrlGeneric(Item item, string imageField, MediaUrlOptions options = null)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            string imageUrl = string.Empty;
            var image = (ImageField)item.Fields[imageField];
            //if (image?.MediaItem != null)
            //{
            //var imageValue = new MediaItem(image.MediaItem);
            //imageUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(imageValue));
            //}
            return image;
        }

        public string ImageUrl(ImageField imageField, MediaUrlOptions options = null)
        {
            if (imageField == null)
            {

            }
            string imageUrl = string.Empty;
            //var image = (ImageField)item.Fields[imageField];
            if (imageField?.MediaItem != null)
            {
                var imageValue = new MediaItem(imageField.MediaItem);
                imageUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(imageValue));
            }
            return imageUrl;
        }

        #region Sitecore Utility Methods
        //Get Database
        public Database FetchDB()
        {
            Database masterDB = Factory.GetDatabase("master");
            return masterDB;
        }

        //Publish Item
        public void PublishItem(Item item, bool includeChildren)
        {
            Sitecore.Publishing.PublishOptions publishOptions =
              new Sitecore.Publishing.PublishOptions(item.Database,
                                                     Database.GetDatabase("web"),
                                                     Sitecore.Publishing.PublishMode.Smart,
                                                     item.Language,
                                                     System.DateTime.Now);
            Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);
            publisher.Options.RootItem = item;
            // Publish child items | for safer side, Deep is set to true only if child items count is less than 100
            if (item.HasChildren)
                publisher.Options.Deep = includeChildren;
            // Do the publish
            publisher.Publish();
        }

        public Item CreateItem(string ParentItemPath, string curItemName, string templatePath)
        {
            using (new SecurityDisabler())
            {
                Item parentItem = FetchDB().GetItem(ParentItemPath);
                TemplateItem templateItem = FetchDB().GetItem(templatePath);
                Item createdItem = parentItem.Add(curItemName, templateItem);
                return createdItem;
            }
        }

        public Item CreateItem(Item parentItem, string curItemName, string templatePath)
        {
            using (new SecurityDisabler())
            {
                TemplateItem templateItem = FetchDB().GetItem(templatePath);
                Item createdItem = parentItem.Add(curItemName, templateItem);
                return createdItem;
            }
        }

        public void RemoveLinkReferences(Item item)
        {
            if (item == null)
                return;

            var itemReferrers = Sitecore.Globals.LinkDatabase.GetItemReferrers(item, true);
            if (itemReferrers?.Any() == true)
            {
                foreach (ItemLink link in itemReferrers)
                {
                    Item linkItem = Sitecore.Context.ContentDatabase.GetItem(link.SourceItemID);

                    Field field = linkItem?.Fields[link.SourceFieldID];
                    if (field != null)
                    {
                        CustomField customField = FieldTypeManager.GetField(field);
                        if (customField != null)
                        {
                            using (new SecurityDisabler())
                            {
                                linkItem.Editing.BeginEdit();
                                try
                                {
                                    customField.RemoveLink(link);
                                }
                                finally
                                {
                                    linkItem.Editing.EndEdit();
                                }
                            }
                        }
                    }
                }
            }
            Sitecore.Globals.LinkDatabase.RemoveReferences(item);
        }
        public string GetValidItemName(string name)
        {
            Assert.ArgumentNotNullOrEmpty(name, "original item name");
            string updatedName = Regex.Replace(name, @"[^0-9a-zA-Z]+", "-");
            updatedName = ItemUtil.ProposeValidItemName(updatedName);
            return updatedName.ToLowerInvariant();
        }
        #endregion

    }
}