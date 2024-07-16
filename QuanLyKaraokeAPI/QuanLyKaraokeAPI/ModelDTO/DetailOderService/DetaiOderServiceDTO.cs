namespace QuanLyKaraokeAPI.ModelDTO.DetailOderService
{
    public class DetaiOderServiceDTO
    {
        public int Id { get; set; }
        public int OderID { get; set; }

       // public string oders { get; set; }

        public int ServiceID { get; set; }

        public string serviceTime { get; set; }

        public int Quantity { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Status { get; set; }

        public float TotalPrice { get; set; }
        public float OpenPrice { get; set; }
        public float PricePerHour { get; set; }
    }
}
