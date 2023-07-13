namespace api.Data
{
    public enum TinhTrangDonHang
    {
        New =0 , Payment = 1,Complete =2,Cancel=-1
    }
    public class DonHang
    {
        public DonHang()
        {
            DonHangChiTiet = new List<DonHangChiTiet>();
        }
        public Guid MaDonHang{set;get;}
        public DateTime NgayDat{set;get;}
        public DateTime? NgayGia{set;get;}
        public TinhTrangDonHang TinhTrang{set;get;}
        public string NguoiNhan{set;get;}
        public string DiaChiGiao{set;get;}
        public string SoDienThoai{set;get;}
        public ICollection<DonHangChiTiet> DonHangChiTiet{set;get;}
    }
}