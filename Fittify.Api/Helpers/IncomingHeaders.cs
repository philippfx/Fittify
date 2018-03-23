namespace Fittify.Api.Helpers
{
    public class IncomingHeaders
    {
        public string ContentType { get; set; }
        public bool IncludeHateoas { get; set; }
        public int ApiVersion { get; set; }
    }
}
