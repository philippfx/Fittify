using System;

namespace Fittify.DataModelRepositories.EFCoreCustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CascadeDeleteAttribute : Attribute { }
}