namespace EthCoffee.api.Helpers
{
    public class PaginationParams : IPaginationParams
    {
        private const int MaxPageSize = 60;
        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string SortBy { get; set; } = "dateAdded_desc";
        
    }
}