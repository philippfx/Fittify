﻿////using System;
////using System.Diagnostics.CodeAnalysis;
////using Microsoft.AspNetCore.Mvc.ApplicationModels;

////namespace Fittify.Api.Controllers.Generic
////{
////    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
////    [ExcludeFromCodeCoverage]
////    public class GenericControllerNameAttribute : Attribute, IControllerModelConvention
////    {
////        public void Apply(ControllerModel controller)
////        {
////            if (controller.ControllerType.GetGenericTypeDefinition() == typeof(GenericController<>))
////            {
////                var entityType = controller.ControllerType.GenericTypeArguments[0];
////                controller.ControllerName = entityType.Name;
////            }
////        }
////    }
////}
