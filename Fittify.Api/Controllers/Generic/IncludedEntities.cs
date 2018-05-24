using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fittify.Api.Controllers.Generic
{
    public static class IncludedEntities
    {
        public static readonly IReadOnlyList<TypeInfo> Types;

        static IncludedEntities()
        {
            var assembly = typeof(IncludedEntities).GetTypeInfo().Assembly;
            var typeList = new List<TypeInfo>();

            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(ApiEntityAttribute), true).Length > 0)
                {
                    typeList.Add(type.GetTypeInfo());
                }
            }

            Types = typeList;
        }
    }
}
