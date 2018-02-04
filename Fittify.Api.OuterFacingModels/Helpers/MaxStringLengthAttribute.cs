using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Fittify.Common.Extensions;

namespace Fittify.Api.OuterFacingModels.Helpers
{
    public class MaxStringLengthAttribute : StringLengthAttribute
    {
        private int _maximumLength;

        public MaxStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
            _maximumLength = maximumLength;
        }

        public override string FormatErrorMessage(string propertyName)
        {
            return this.ErrorMessage = "The maxinum string length of '" + propertyName.ToCamelCase() + "' is '" + _maximumLength + "'";
        }
    }
}
