namespace Fittify.Common
{
    interface IEntityName<TId> : IEntityUniqueIdentifier<TId> where TId : struct
    {
        string Name { get; set; }
    }
}
