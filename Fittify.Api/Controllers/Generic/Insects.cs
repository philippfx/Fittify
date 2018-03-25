using Fittify.Common;

namespace Fittify.Api.Controllers.Generic
{
    [ApiEntityAttribute]
    public class Insects : IEntityName<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
