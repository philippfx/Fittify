﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Fittify.Common.Helpers;

namespace Fittify.Api.OuterFacingModels.Helpers
{
    public class ValidRegularExpressionRangeOfIntIdsAttribute : ValidationAttribute
    {
        private readonly string _pattern;
        public ValidRegularExpressionRangeOfIntIdsAttribute(string pattern)
        {
            _pattern = pattern;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            string strValue = value as string;
            return strValue != null && Regex.IsMatch(strValue, _pattern);
        }
        public override string FormatErrorMessage(string name)
        {
            this.ErrorMessage = "Your concatenated range of integer ids is badly formatted. It must meet the regular expression '" + RegularExpressions.RangeOfIntIds.Replace("\\", "") + "'";

            return this.ErrorMessage;
        }
    }
}