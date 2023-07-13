using Microsoft.EntityFrameworkCore;
using api.Models;
namespace api.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options){}
        public DbSet<Product> Products{set;get;}
        public DbSet<Category> Categories{set;get;}
        public DbSet<DonHang> DonHangs{set;get;}
        public DbSet<DonHangChiTiet> DonHangChiTiets{set;get;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHang>(entity=>{
                entity.ToTable("DonHang");
                entity.HasKey("MaDonHang");
                entity.Property(donHang=>donHang.NgayDat).HasDefaultValue<DateTime>(DateTime.UtcNow);
            });
            modelBuilder.Entity<DonHangChiTiet>(entity=>
            {
                entity.ToTable("ChiTietDonHang");
                entity.HasKey(e=>new{e.MaDonHang,e.Id});
                entity.HasOne(ct=>ct.DonHang)
                    .WithMany(dh=>dh.DonHangChiTiet)
                    .HasForeignKey(dh=>dh.MaDonHang)
                    .HasConstraintName("FK_DonHangCT_DonHang");
                entity.HasOne(e=>e.Product)
                    .WithMany(p=>p.DonHangChiTiet)
                    .HasForeignKey(p=>p.Id)
                    .HasConstraintName("FK_DonHangCT_SanPham");
            });

            base.OnModelCreating(modelBuilder);

        }
    }
}