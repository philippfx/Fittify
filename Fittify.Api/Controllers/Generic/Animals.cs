using Fittify.Common;

namespace Fittify.Api.Controllers.Generic
{
    [ApiEntityAttribute]
    public class Animals : IEntityName<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
