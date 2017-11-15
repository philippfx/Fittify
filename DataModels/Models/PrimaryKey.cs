using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.Models
{
    public class PrimaryKey<TId>
    {
        public virtual TId Id { get; set; }
    }
}
