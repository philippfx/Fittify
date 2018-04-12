using System.ComponentModel.DataAnnotations;

namespace Fittify.Common.Helpers
{
    public class ValidAscendingOrderRangeOfIntIdsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string str = value as string;
            if (str == null) return true;
            var split = str.Split(new char[] { ',', '-' });
            int current;
            int next;
            if (!int.TryParse(split[0], out current)) return false; // Covering the case when input is only a single int
            for (int i = 0; i < split.Length - 1; i++)
            {
                if (!int.TryParse(split[i], out current)) return false;
                if (!int.TryParse(split[i + 1], out next)) return false;
                if (current > next) return false;
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return this.ErrorMessage = "The range of integer ids is invalid or not in an ascending order. For example, '10-8,7,6-1' is not in an ascending order and should be '1-6,7,8-10' instead";
        }
    }
}
