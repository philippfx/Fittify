using Fittify.Common;

namespace Fittify.Api.Controllers.Generic
{
    [ApiEntity]
    public class Animals : IEntityName<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
