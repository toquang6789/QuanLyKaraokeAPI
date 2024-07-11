namespace QuanLyKaraokeAPI.ModelDTO.Products
{
    public class ProductDTO
    {
        public int ProductID { get; set; }

        public int CategoriesID { get; set; }

        public string CategoriesName { get; set; }

        public string ProductName { get; set; }

        public string Unit { get; set; }

        public float CostPrite { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }

        public string Image { get; set; }
        public int Status { get; set; }
    }
}
