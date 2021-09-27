using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SYMB2C.Foundation.Account.Services;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace SYMB2C.Foundation.Account.Rules.Conditions.AccountConditions
{
    public class HasPowerAccount<T> : WhenCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            var profileService = ServiceLocator.ServiceProvider.GetService<IProfileService>();
            if (profileService == null)
            {
                return false;
            }

            var profile = profileService.GetProfile();

            if (profile == null)
            {
                return false;
            }

            if (profile.PowerAccounts.Any())
            {
                return true;
            }

            return false;
        }
    }
}