namespace Examination.PL.ModelViews
{
    public class ResponseMV
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;

        public string? RedirectUrl { get; set; } 


    }
}
