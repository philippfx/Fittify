namespace Fittify.Common
{
    public interface IEntityName<TId> : IEntityUniqueIdentifier<TId> where TId : struct
    {
        string Name { get; set; }
    }
}
