namespace QuanLyKaraokeAPI.ModelDTO.ImportProducts
{
    public class CreateImportProductDTO
    {
        // public int IdImport { get; set; }

        public int UserId { get; set; }

        // public string UserName { get; set; }

        public int SupplierID { get; set; }

        //  public string SupplierName { get; set; }

        public float TotalMoney { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
