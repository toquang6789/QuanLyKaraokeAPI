namespace QuanLyKaraokeAPI.ModelDTO
{
    public class BaseResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
