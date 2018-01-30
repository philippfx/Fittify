using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fittify.DataModelRepositories
{
    public interface ILogger
    {
        [NotMapped]
        EntityErrorLogger EntityErrorLogger { get; set; }
    }
}
