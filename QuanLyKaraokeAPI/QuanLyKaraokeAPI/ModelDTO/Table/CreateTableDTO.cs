namespace QuanLyKaraokeAPI.ModelDTO.Table
{
    public class CreateTableDTO
    {
        public string CreateTabelName { get; set; }

        public IFormFile Image { get; set; }

        public int Status { get; set; }
    }
}
