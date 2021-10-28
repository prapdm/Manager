using Manager.Entities;

namespace Manager.Models
{
    public class Query
    {
        public string searchPhrase { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string SortBy { get; set; } = "Id";
        public SortDirection SortDirection { get; set; } = SortDirection.ASC;
    }
}
