namespace Fittify.Web.Common
{
    public interface IUniqueIdentifier<TId> where TId : struct
    {
        TId Id { get; set; }
    }
}
