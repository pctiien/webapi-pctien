namespace api.Models
{
    public class HangHoa
    {
        public string tenHangHoa{set;get;}
        public int donGia{get;set;}
    }
    public class HangHoaVM: HangHoa
    {
        public Guid maHangHoa{set;get;}
    }
}