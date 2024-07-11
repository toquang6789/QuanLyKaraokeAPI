namespace QuanLyKaraokeAPI.ModelDTO.DetailOderProduct
{
    public class DetailOderProductDTO
    {
        public int Id { get; set; }
        public int OderID { get; set; }

        public string odersName { get; set; }
        public int ProductID { get; set; }

        public string productsName { get; set; }

        public int Quantity { get; set; }

        public DateTime TimeOder { get; set; }

        public int Status { get; set; }
    }
}
