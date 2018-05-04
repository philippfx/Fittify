namespace Fittify.Common.ResourceParameters
{
    public class BaseResourceParameters : IResourceParameters
    {
        public string Ids { get; set; }

        private const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 100;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string OrderBy { get; set; } = "Id"; // Todo hardcoded "Id" property could be made dynamic

        public string Fields { get; set; }
    }
}
