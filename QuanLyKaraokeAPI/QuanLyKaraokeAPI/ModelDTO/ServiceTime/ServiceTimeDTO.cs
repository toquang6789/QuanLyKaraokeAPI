namespace QuanLyKaraokeAPI.ModelDTO.ServiceTime
{
    public class ServiceTimeDTO
    {
        public int ServiceID { get; set; }
        public int CategoriesID { get; set; }
        public string CategoriesName { get; set; }
        public string ServiceName { get; set; }

        public string Unit { get; set; }

        public float OpenPrice { get; set; }

        public float Price { get; set; }

        public string Image { get; set; }

        public int Status { get; set; }

    }
}
