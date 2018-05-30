using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Fittify.Common.Extensions;

namespace Fittify.Api.OuterFacingModels.Helpers
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class MaxStringLengthAttribute : StringLengthAttribute
    {
        private readonly int _maximumLength;

        public MaxStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
            _maximumLength = maximumLength;
        }

        [ExcludeFromCodeCoverage]
        public override string FormatErrorMessage(string propertyName)
        {
            return this.ErrorMessage = "The maxinum string length of '" + propertyName.ToCamelCase() + "' is '" + _maximumLength + "'";
        }
    }
}
