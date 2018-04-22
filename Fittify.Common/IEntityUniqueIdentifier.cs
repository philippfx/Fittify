namespace Fittify.Common
{
    public interface IEntityUniqueIdentifier<TId> 
        where TId : struct
    {
        TId Id { get; set; }
    }
}
