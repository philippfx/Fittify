using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fittify.Api.Test.TestHelpers
{
    public class CreatedAtRouteResultWithDefaultConstructor : CreatedAtRouteResult
    {
        [JsonConstructor]
        public CreatedAtRouteResultWithDefaultConstructor()
            : base("mockRouteName", "mockRouteValues", "mockValue")
        {
            
        }

        public CreatedAtRouteResultWithDefaultConstructor(object routeValues, object value) : base(routeValues, value)
        {
        }

        public CreatedAtRouteResultWithDefaultConstructor(string routeName, object routeValues, object value) : base(routeName, routeValues, value)
        {
        }
    }
}
