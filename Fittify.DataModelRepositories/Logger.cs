using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.DataModelRepositories
{
    public class EntityErrorLogger : ILogger
    {
        public bool IsOperationSucces { get; set; }
        public string ErrorMessage { get; set; }
        EntityErrorLogger ILogger.EntityErrorLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
