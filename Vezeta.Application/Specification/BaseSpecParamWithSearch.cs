namespace Vezeta.Application.Specification
{
    // Pagination model 
    public class BaseSpecParamWithSearch
    {
        private const int MaxPageSize = 10;
        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int PageIndex { get; set; } = 1;

        private string searchVal;
        public string? SearchVal
        {
            get { return searchVal; }
           
            set { searchVal = value.ToLower()??""; }
        }

    }
}
