using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using SYMB2C.Foundation.LinkTracker.Data.Constants;
using Sitecore.Analytics;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Analytics.Data;

namespace SYMB2C.Foundation.LinkTracker.Controllers
{
    public class LoadGoalsController : Controller
    {
        [HttpGet]
        public JsonResult GetGoals()
        {
            var result = new JsonResult();
            List<GoalName> goalNames = new List<GoalName>();
            string GoalsPath = LinkTrackerConstants.SitecoreGoalPath;
            string GoalsTemplateId = LinkTrackerConstants.GoalTemplateId.ToString();
            var context = Sitecore.Configuration.Factory.GetDatabase("master");

            Item item = context.SelectSingleItem(GoalsPath);
            List<Item> items = item.Axes.GetDescendants().Where(x => x.TemplateID.ToString() == GoalsTemplateId).ToList();

            foreach (var itemC in items)
            {
                GoalName gn = new GoalName();
                gn.GoalTypeName = itemC.Name;
                gn.GoalId = itemC.ID.ToString();
                goalNames.Add(gn);
            }

            result.Data = JsonConvert.SerializeObject(goalNames);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetGoals(GoalName goal)
        {
            var result = new JsonResult();
            ID scId;
            try
            {
                if (!string.IsNullOrEmpty(goal.GoalId) && ID.TryParse(goal.GoalId, out scId))
                {
                    if (Tracker.IsActive == false)
                    {
                        Tracker.StartTracking();
                    }
                    if (Tracker.Current.CurrentPage != null && Tracker.Current.Interaction != null)
                    {

                        //PageEvent
                        Item defItem = Sitecore.Context.Database.GetItem(scId);
                        var eventToTrigger = new PageEventData(defItem.Name, scId.Guid)
                        {
                            Data = goal.GoalDataVal
                        };
                        Tracker.Current.CurrentPage.Register(eventToTrigger);
                        result.Data = new { Success = true, msg = "Successfully triggered!" };
                    }
                }
            }
            catch (Exception e)
            {
                result.Data = new { Success = false, msg = "Couldn't be triggered!" };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}