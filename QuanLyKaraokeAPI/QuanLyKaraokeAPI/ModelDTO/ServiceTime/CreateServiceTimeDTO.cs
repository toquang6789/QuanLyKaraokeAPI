namespace QuanLyKaraokeAPI.ModelDTO.ServiceTime
{
    public class CreateServiceTimeDTO
    {
        // public int ServiceID { get; set; }
        public int CategoriesID { get; set; }
        public string ServiceName { get; set; }

        public string Unit { get; set; }

        public float OpenPrice { get; set; }

        public float Price { get; set; }

        public IFormFile Image { get; set; }

        public int Status { get; set; }
    }
}
