namespace LibraryApi.Pagination
{
    public class BooksFilterPrice : QueryStringParameters
    {
        public decimal? Price { get; set; }
        public string? PriceCriteria { get; set; }
    }
}
