using api.Models;

namespace api.Data
{
    public class DonHangChiTiet
    {
        public Guid Id{set;get;}
        public Guid MaDonHang{set;get;}
        public int SoLuong{set;get;}
        public double DonGia{set;get;}
        public double GiamGia{set;get;}
        // Relationship
        public DonHang DonHang{set;get;}
        public Product Product{set;get;}
    }
}