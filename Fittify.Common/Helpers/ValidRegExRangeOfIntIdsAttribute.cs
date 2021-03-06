﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Fittify.Common.Helpers
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class ValidRegExRangeOfIntIdsAttribute : ValidationAttribute
    {
        private readonly string _pattern;
        public ValidRegExRangeOfIntIdsAttribute(string pattern)
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
            this.ErrorMessage = "Your concatenated range of integer ids is badly formatted. It must meet the regular expression '" + FittifyRegularExpressions.RangeOfIntIds.Replace("\\", "") + "'";

            return this.ErrorMessage;
        }
    }
}