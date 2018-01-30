using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.Controllers
{
    public class WeightLiftingSetWebController : Controller
    {
        [HttpPost]
        [Route("weightliftingset/new")] /*?workouthistory={workouthistoryid:int}*/
        public RedirectToActionResult CreateNewWeightLiftingSet([FromForm] WeightLiftingSetViewModel weightLiftingSetViewModel, [FromQuery] int workouthistoryid)
        {
            var gppdRepoExercises = new GppdRepository<WeightLiftingSetViewModel>("http://localhost:52275/api/weightliftingsets/new");
            var result = gppdRepoExercises.Post(weightLiftingSetViewModel).Result;
            return RedirectToAction("historydetails", "Workout", new { workoutHistoryId = workouthistoryid });
        }

        [HttpPost]
        [Route("workout/HistoryDetails/{workoutHistoryId}/SavingChanges")]
        public RedirectToActionResult SaveChangesForSets(int workoutHistoryId, IFormCollection formCollection)
        {
            // The formCollection is a flat array (there is no nested item).
            // Each form item needs to be parsed and assigned to its unique weightliftingSet
            // Each form item's name is composed of the weightliftingSetId and the property name to be patched
            var listWls = new List<WeightLiftingSetViewModel>();
            
            var regexProperty = new Regex(@"^\w+-{1}\d+-{1}\w+$"); // For example "CurrentWeightLiftingSetId-98-RepetitionsFull"
            foreach (var formItem in formCollection)
            {
                if (regexProperty.IsMatch(formItem.Key))
                {
                    if (formItem.Key.ToLower().Contains("CurrentWeightLiftingSet".ToLower()))
                    {
                        // Getting the weightliftingSetId
                        int firstIndexOfIdInString = formItem.Key.IndexOf("-") + 1;
                        int lengthOfIdString = formItem.Key.LastIndexOf("-") - firstIndexOfIdInString;
                        var weightLiftingSetId = Int32.Parse(formItem.Key.Substring(firstIndexOfIdInString, lengthOfIdString));
                        
                        var propertyName = formItem.Key.Substring(formItem.Key.LastIndexOf("-") + 1);

                        var weightLiftingSet = listWls.FirstOrDefault(wls => wls.Id == weightLiftingSetId);
                        if (weightLiftingSet == null)
                        {
                            // Creating not yet created weightLiftingSetViewModel
                            listWls.Add(new WeightLiftingSetViewModel() { Id = weightLiftingSetId});
                            weightLiftingSet = listWls.FirstOrDefault(wls => wls.Id == weightLiftingSetId);
                        }

                        var property = weightLiftingSet?.GetType().GetProperty(propertyName);

                        if (Int32.TryParse(formItem.Value, out int parsedFormValue))
                        {
                            property?.SetValue(weightLiftingSet, parsedFormValue);
                        }
                        else if (string.IsNullOrWhiteSpace(formItem.Value))
                        {
                            property?.SetValue(weightLiftingSet, null);
                        }
                    }
                }
            }

            // Now that we have collected all data for all weightliftingSets we can create and send the PATCH request
            //var jsonPatchCollection = new List<JsonPatchDocument>();

            foreach (var wls in listWls)
            {
                JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();
                foreach (var prop in wls.GetType().GetProperties())
                {
                    if (prop.GetValue(wls) != null && !prop.Name.ToLower().Contains("id"))
                    {
                        var jsonPatchOperation = new Operation("replace", prop.Name, null, prop.GetValue(wls));
                        jsonPatchDocument.Operations.Add(jsonPatchOperation);
                    }
                }

                var gppdRepoExercises = new GppdRepository<WeightLiftingSetViewModel>("http://localhost:52275/api/weightliftingsets/" + wls.Id);
                if (jsonPatchDocument != null)
                {
                    var result = gppdRepoExercises.Patch(jsonPatchDocument).Result;
                }
            }
            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
