namespace QuanLyKaraokeAPI.ModelDTO.Categories
{
    public class CreateCategoryDTO
    {
        public string CategoryName { get; set; }
        public IFormFile Image { get; set; }
    }
}
