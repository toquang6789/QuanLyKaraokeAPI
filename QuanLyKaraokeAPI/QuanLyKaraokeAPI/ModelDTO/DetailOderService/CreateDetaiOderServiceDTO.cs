namespace QuanLyKaraokeAPI.ModelDTO.DetailOderService
{
    public class CreateDetaiOderServiceDTO
    {
        //public int Id { get; set; }
        public int OderID { get; set; }

        //public string oders { get; set; }

        public int ServiceID { get; set; }

        // public string serviceTime { get; set; }

        public int Quantity { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Status { get; set; }
    }
}
