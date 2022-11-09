using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class WEBBANGIAYContext : DbContext
    {
        public WEBBANGIAYContext()
        {
        }

        public WEBBANGIAYContext(DbContextOptions<WEBBANGIAYContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public virtual DbSet<DanhGia> DanhGia { get; set; }
        public virtual DbSet<DanhMuc> DanhMucs { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<GioHang> GioHangs { get; set; }
        public virtual DbSet<KichCo> KichCos { get; set; }
        public virtual DbSet<LoaiNguoiDung> LoaiNguoiDungs { get; set; }
        public virtual DbSet<MatHang> MatHangs { get; set; }
        public virtual DbSet<MatHangAnh> MatHangAnhs { get; set; }
        public virtual DbSet<MauSac> MauSacs { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<NguoiDungDiaChi> NguoiDungDiaChis { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public virtual DbSet<TheoDoi> TheoDois { get; set; }
        public virtual DbSet<ThuongHieu> ThuongHieus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=WEBBANGIAY;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => new { e.MaChiTietDonHang, e.MaDonHang })
                    .HasName("PK_ChiTietDonHang_1");

                entity.ToTable("ChiTietDonHang");

                entity.HasIndex(e => e.MaDonHang, "IX_ChiTietDonHang_MaDonHang");

                entity.HasIndex(e => e.MaMatHang, "IX_ChiTietDonHang_MaMatHang");

                entity.Property(e => e.MaChiTietDonHang).ValueGeneratedOnAdd();

                entity.Property(e => e.Gia).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietDonHang_DonHang1");

                entity.HasOne(d => d.MaMatHangNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaMatHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietDo__MaMat__5FB337D6");
            });

            modelBuilder.Entity<ChiTietGioHang>(entity =>
            {
                entity.HasKey(e => new { e.MaGioHang, e.MaMatHang });

                entity.ToTable("ChiTietGioHang");

                entity.HasIndex(e => e.MaGioHang, "AK_ChiTietGioHang_MaGioHang")
                    .IsUnique();

                entity.HasIndex(e => e.MaMatHang, "IX_ChiTietGioHang_MaMatHang");

                entity.HasIndex(e => e.MaGioHang, "UQ_ChiTietGioHang")
                    .IsUnique();

                entity.HasOne(d => d.MaMatHangNavigation)
                    .WithMany(p => p.ChiTietGioHangs)
                    .HasForeignKey(d => d.MaMatHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietGi__MaMat__48CFD27E");
            });

            modelBuilder.Entity<DanhGia>(entity =>
            {
                entity.HasKey(e => e.MaDanhGia);

                entity.HasIndex(e => e.MaDonHang, "IX_DanhGia_MaDonHang");

                entity.HasIndex(e => e.MaMatHang, "IX_DanhGia_MaMatHang");

                entity.HasIndex(e => e.MaNguoiDung, "IX_DanhGia_MaNguoiDung");

                entity.Property(e => e.NoiDung).HasMaxLength(150);

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.DanhGia)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DanhGia_DonHang1");

                entity.HasOne(d => d.MaMatHangNavigation)
                    .WithMany(p => p.DanhGia)
                    .HasForeignKey(d => d.MaMatHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DanhGia__MaMatHa__5629CD9C");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.DanhGia)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DanhGia_NguoiDung");
            });

            modelBuilder.Entity<DanhMuc>(entity =>
            {
                entity.HasKey(e => e.MaDanhMuc)
                    .HasName("PK__DanhMuc__B375088709613860");

                entity.ToTable("DanhMuc");

                entity.HasIndex(e => e.Slug, "UQ_Slug")
                    .IsUnique();

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenDanhMuc)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.MaDonHang)
                    .HasName("PK_DonHang_1");

                entity.ToTable("DonHang");

                entity.HasIndex(e => e.MaNguoiDung, "IX_DonHang_MaNguoiDung");

                entity.Property(e => e.DiaChi)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SDT")
                    .IsFixedLength(true);

                entity.Property(e => e.TinhTrang)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TongTien).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.DonHangMaNguoiDungNavigations)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DonHang_NguoiDung");

                entity.HasOne(d => d.MaNguoiGiaoHangNavigation)
                    .WithMany(p => p.DonHangMaNguoiGiaoHangNavigations)
                    .HasForeignKey(d => d.MaNguoiGiaoHang)
                    .HasConstraintName("FK_DonHang_NguoiDung1");
            });

            modelBuilder.Entity<GioHang>(entity =>
            {
                entity.HasKey(e => e.MaGioHang);

                entity.ToTable("GioHang");

                entity.HasIndex(e => e.MaNguoiDung, "UQ_GioHang")
                    .IsUnique();

                entity.Property(e => e.MaGioHang).ValueGeneratedNever();

                entity.HasOne(d => d.MaGioHangNavigation)
                    .WithOne(p => p.GioHang)
                    .HasPrincipalKey<ChiTietGioHang>(p => p.MaGioHang)
                    .HasForeignKey<GioHang>(d => d.MaGioHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GioHang_ChiTietGioHang");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithOne(p => p.GioHang)
                    .HasForeignKey<GioHang>(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GioHang_NguoiDung");
            });

            modelBuilder.Entity<KichCo>(entity =>
            {
                entity.HasKey(e => e.MaKichCo);

                entity.ToTable("KichCo");

                entity.Property(e => e.KichCo1).HasColumnName("KichCo");
            });

            modelBuilder.Entity<LoaiNguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaLoaiNguoiDung);

                entity.ToTable("LoaiNguoiDung");

                entity.Property(e => e.MaLoaiNguoiDung).HasMaxLength(10);

                entity.Property(e => e.TenLoaiNguoiDung)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MatHang>(entity =>
            {
                entity.HasKey(e => e.MaMatHang)
                    .HasName("PK__MatHang__A92254E571897811");

                entity.ToTable("MatHang");

                entity.HasIndex(e => e.MaDanhMuc, "IX_MatHang_MaDanhMuc");

                entity.HasIndex(e => e.MaKichCo, "IX_MatHang_MaKichCo");

                entity.HasIndex(e => e.MaMauSac, "IX_MatHang_MaMauSac");

                entity.HasIndex(e => e.MaNhaCungCap, "IX_MatHang_MaNhaCungCap");

                entity.HasIndex(e => e.MaThuongHieu, "IX_MatHang_MaThuongHieu");

                entity.Property(e => e.GiaBan).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.MoTa)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.TenMatHang)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.MaDanhMucNavigation)
                    .WithMany(p => p.MatHangs)
                    .HasForeignKey(d => d.MaDanhMuc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MatHang__MaDanhM__300424B4");

                entity.HasOne(d => d.MaKichCoNavigation)
                    .WithMany(p => p.MatHangs)
                    .HasForeignKey(d => d.MaKichCo)
                    .HasConstraintName("FK_MatHang_KichCo");

                entity.HasOne(d => d.MaMauSacNavigation)
                    .WithMany(p => p.MatHangs)
                    .HasForeignKey(d => d.MaMauSac)
                    .HasConstraintName("FK_MatHang_MauSac");

                entity.HasOne(d => d.MaNhaCungCapNavigation)
                    .WithMany(p => p.MatHangs)
                    .HasForeignKey(d => d.MaNhaCungCap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MatHang__MaNhaCu__2E1BDC42");

                entity.HasOne(d => d.MaThuongHieuNavigation)
                    .WithMany(p => p.MatHangs)
                    .HasForeignKey(d => d.MaThuongHieu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MatHang__MaThuon__2F10007B");
            });

            modelBuilder.Entity<MatHangAnh>(entity =>
            {
                entity.HasKey(e => new { e.MaMatHang, e.MaAnh });

                entity.ToTable("MatHang_Anh");

                entity.Property(e => e.MaAnh).ValueGeneratedOnAdd();

                entity.Property(e => e.Anh).IsRequired();

                entity.HasOne(d => d.MaMatHangNavigation)
                    .WithMany(p => p.MatHangAnhs)
                    .HasForeignKey(d => d.MaMatHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MatHang_A__MaMat__5070F446");
            });

            modelBuilder.Entity<MauSac>(entity =>
            {
                entity.HasKey(e => e.MaMauSac);

                entity.ToTable("MauSac");

                entity.Property(e => e.TenMauSac)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaNguoiDung)
                    .HasName("PK__NguoiDun__C539D762078210E6");

                entity.ToTable("NguoiDung");

                entity.HasIndex(e => e.MaLoaiNguoiDung, "IX_NguoiDung_MaLoaiNguoiDung");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaLoaiNguoiDung)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.MatKhauHash)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SDT")
                    .IsFixedLength(true);

                entity.Property(e => e.TenDangNhap)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenNguoiDung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ViDienTu).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.MaLoaiNguoiDungNavigation)
                    .WithMany(p => p.NguoiDungs)
                    .HasForeignKey(d => d.MaLoaiNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NguoiDung_LoaiNguoiDung");
            });

            modelBuilder.Entity<NguoiDungDiaChi>(entity =>
            {
                entity.HasKey(e => new { e.MaDiaChi, e.MaNguoiDung })
                    .HasName("PK__NguoiDun__7C39CE6E46E6A4D8");

                entity.ToTable("NguoiDung_DiaChi");

                entity.HasIndex(e => e.MaNguoiDung, "IX_NguoiDung_DiaChi_MaNguoiDung");

                entity.Property(e => e.MaDiaChi).ValueGeneratedOnAdd();

                entity.Property(e => e.DiaChi).HasMaxLength(50);

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.NguoiDungDiaChis)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__NguoiDung__MaNgu__534D60F1");
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.MaNhaCungCap)
                    .HasName("PK__NhaCungC__53DA92056B8D20D9");

                entity.ToTable("NhaCungCap");

                entity.Property(e => e.Std)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("STD")
                    .IsFixedLength(true);

                entity.Property(e => e.TenNhaCungCap)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TheoDoi>(entity =>
            {
                entity.HasKey(e => new { e.MaTheoDoi, e.MaNguoiDung, e.MaMatHang })
                    .HasName("PK__TheoDoi__3156C07993ADD5B7");

                entity.ToTable("TheoDoi");

                entity.HasIndex(e => e.MaMatHang, "IX_TheoDoi_MaMatHang");

                entity.HasIndex(e => new { e.MaNguoiDung, e.MaMatHang }, "UQ__TheoDoi__8FABF22D99F1F197")
                    .IsUnique();

                entity.Property(e => e.MaTheoDoi).ValueGeneratedOnAdd();

                entity.HasOne(d => d.MaMatHangNavigation)
                    .WithMany(p => p.TheoDois)
                    .HasForeignKey(d => d.MaMatHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TheoDoi__MaMatHa__4D94879B");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.TheoDois)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TheoDoi__MaNguoi__4CA06362");
            });

            modelBuilder.Entity<ThuongHieu>(entity =>
            {
                entity.HasKey(e => e.MaThuongHieu)
                    .HasName("PK__ThuongHi__A3733E2CCDB502B5");

                entity.ToTable("ThuongHieu");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TenThuongHieu)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
