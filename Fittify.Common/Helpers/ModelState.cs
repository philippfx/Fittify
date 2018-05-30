using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fittify.Common.Helpers
{
    [ExcludeFromCodeCoverage] // As of 29.05.2018 not referenced, but may be useful in the future
    public static class ModelState
    {
        // Validates an object for the data annotations for each class Property
        public static bool Validate<T>(T obj, ref List<string> stringResults)
        {
            var results = new List<ValidationResult>();

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
    }
}
