namespace Fittify.Web.Common
{
    public abstract class UniqueIdentifier<TId> : IUniqueIdentifier<TId> where TId : struct
    {
        public virtual TId Id { get; set; }
    }
}
