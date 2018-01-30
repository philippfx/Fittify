namespace Fittify.Common
{
    public interface IUniqueIdentifierDataModels<TId> where TId : struct
    {
        TId Id { get; set; }
    }
}
