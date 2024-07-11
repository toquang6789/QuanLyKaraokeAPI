namespace QuanLyKaraokeAPI.ModelDTO.Oder
{
    public class CreateOderDTO
    {
        // public int OderID { get; set; }
        public int AccountID { get; set; }

        //  public string AccountName { get; set; }

        public int UserId { get; set; }

        //   public string UserName { get; set; }

        public int TableID { get; set; }

        //   public string TableName { get; set; }

        public float TotalMoney { get; set; }

        public int Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
