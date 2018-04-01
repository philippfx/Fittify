using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fittify.Common.Helpers
{
    public static class ModelState
    {
        // Validates an object for the data annotations for each class Property
        public static bool Validate<T>(T obj, ref List<string> stringResults)
        {
            var results = new List<ValidationResult>();
            //var stringResults = new List<string>();
            

            if (!Validator.TryValidateObject(obj, new ValidationContext(obj), results, true))
            {
                foreach (var vr in results)
                {
                    stringResults.Add(vr.ErrorMessage);
                }

                return false;
            }

            return true;
        }

        //// Validates an object for the data annotations for each class Property
        //public static bool Validate<T>(T obj, ref List<ValidationResult> results)
        //{
        //    results = new List<ValidationResult>();

        //    return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        //}
    }
}
