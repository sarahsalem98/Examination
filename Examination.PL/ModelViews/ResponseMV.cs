namespace Examination.PL.ModelViews
{
    public class ResponseMV
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;

        public string? RedirectUrl { get; set; }
        public object? Data { get; set; }

    }

    public class PaginatedData<T> where T : class
    {
        public IEnumerable<T> Items { get; set; } = null!;
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}
