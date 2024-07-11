namespace QuanLyKaraokeAPI.ModelDTO.Products
{
    public class CreateProductDTO
    {
        public int CategoriesID { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }

        public float CostPrite { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }

        public IFormFile Image { get; set; }
        public int Status { get; set; }
    }
}
