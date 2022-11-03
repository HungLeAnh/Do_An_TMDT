using Microsoft.EntityFrameworkCore.Migrations;

namespace Do_an_TMDT.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMuc",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DanhMuc__B375088709613860", x => x.MaDanhMuc);
                });

            migrationBuilder.CreateTable(
                name: "KichCo",
                columns: table => new
                {
                    MaKichCo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KichCo = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KichCo", x => x.MaKichCo);
                });

            migrationBuilder.CreateTable(
                name: "LoaiNguoiDung",
                columns: table => new
                {
                    MaLoaiNguoiDung = table.Column<string>(type: "nvarchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    TenLoaiNguoiDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiNguoiDung", x => x.MaLoaiNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "MauSac",
                columns: table => new
                {
                    MaMauSac = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMauSac = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauSac", x => x.MaMauSac);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    STD = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhaCungC__53DA92056B8D20D9", x => x.MaNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieu",
                columns: table => new
                {
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(50)", fixedLength: true, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ThuongHi__A3733E2CCDB502B5", x => x.MaThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaLoaiNguoiDung = table.Column<string>(type: "nvarchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenDangNhap = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MatKhauHash = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Salt = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SDT = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    ViDienTu = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NguoiDun__C539D762078210E6", x => x.MaNguoiDung);
                    table.ForeignKey(
                        name: "FK_NguoiDung_LoaiNguoiDung",
                        column: x => x.MaLoaiNguoiDung,
                        principalTable: "LoaiNguoiDung",
                        principalColumn: "MaLoaiNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatHang",
                columns: table => new
                {
                    MaMatHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMatHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GiaBan = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    DangDuocBan = table.Column<bool>(type: "bit", nullable: false),
                    SoSao = table.Column<double>(type: "float", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    SoLuongDaBan = table.Column<int>(type: "int", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DangDuocHienThi = table.Column<bool>(type: "bit", nullable: false),
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false),
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false),
                    MaKichCo = table.Column<int>(type: "int", nullable: true),
                    MaMauSac = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MatHang__A92254E571897811", x => x.MaMatHang);
                    table.ForeignKey(
                        name: "FK__MatHang__MaDanhM__300424B4",
                        column: x => x.MaDanhMuc,
                        principalTable: "DanhMuc",
                        principalColumn: "MaDanhMuc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__MatHang__MaNhaCu__2E1BDC42",
                        column: x => x.MaNhaCungCap,
                        principalTable: "NhaCungCap",
                        principalColumn: "MaNhaCungCap",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__MatHang__MaThuon__2F10007B",
                        column: x => x.MaThuongHieu,
                        principalTable: "ThuongHieu",
                        principalColumn: "MaThuongHieu",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatHang_KichCo",
                        column: x => x.MaKichCo,
                        principalTable: "KichCo",
                        principalColumn: "MaKichCo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatHang_MauSac",
                        column: x => x.MaMauSac,
                        principalTable: "MauSac",
                        principalColumn: "MaMauSac",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SDT = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DaThanhToan = table.Column<bool>(type: "bit", nullable: false),
                    TongTien = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang_1", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHang_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung_DiaChi",
                columns: table => new
                {
                    MaDiaChi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NguoiDun__7C39CE6E46E6A4D8", x => new { x.MaDiaChi, x.MaNguoiDung });
                    table.ForeignKey(
                        name: "FK__NguoiDung__MaNgu__534D60F1",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHang",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false),
                    MaMatHang = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHang", x => new { x.MaGioHang, x.MaMatHang });
                    table.UniqueConstraint("AK_ChiTietGioHang_MaGioHang", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK__ChiTietGi__MaMat__48CFD27E",
                        column: x => x.MaMatHang,
                        principalTable: "MatHang",
                        principalColumn: "MaMatHang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatHang_Anh",
                columns: table => new
                {
                    MaAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaMatHang = table.Column<int>(type: "int", nullable: false),
                    Anh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatHang_Anh", x => new { x.MaMatHang, x.MaAnh });
                    table.ForeignKey(
                        name: "FK__MatHang_A__MaMat__5070F446",
                        column: x => x.MaMatHang,
                        principalTable: "MatHang",
                        principalColumn: "MaMatHang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TheoDoi",
                columns: table => new
                {
                    MaTheoDoi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaMatHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TheoDoi__3156C07993ADD5B7", x => new { x.MaTheoDoi, x.MaNguoiDung, x.MaMatHang });
                    table.ForeignKey(
                        name: "FK__TheoDoi__MaMatHa__4D94879B",
                        column: x => x.MaMatHang,
                        principalTable: "MatHang",
                        principalColumn: "MaMatHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TheoDoi__MaNguoi__4CA06362",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    MaChiTietDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaMatHang = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang_1", x => new { x.MaChiTietDonHang, x.MaDonHang });
                    table.ForeignKey(
                        name: "FK__ChiTietDo__MaMat__5FB337D6",
                        column: x => x.MaMatHang,
                        principalTable: "MatHang",
                        principalColumn: "MaMatHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang1",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanhGia",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SoSao = table.Column<double>(type: "float", nullable: false),
                    MaMatHang = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGia", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK__DanhGia__MaMatHa__5629CD9C",
                        column: x => x.MaMatHang,
                        principalTable: "MatHang",
                        principalColumn: "MaMatHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanhGia_DonHang1",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanhGia_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_GioHang_ChiTietGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "ChiTietGioHang",
                        principalColumn: "MaGioHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GioHang_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaDonHang",
                table: "ChiTietDonHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaMatHang",
                table: "ChiTietDonHang",
                column: "MaMatHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_MaMatHang",
                table: "ChiTietGioHang",
                column: "MaMatHang");

            migrationBuilder.CreateIndex(
                name: "UQ_ChiTietGioHang",
                table: "ChiTietGioHang",
                column: "MaGioHang",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_MaDonHang",
                table: "DanhGia",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_MaMatHang",
                table: "DanhGia",
                column: "MaMatHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_MaNguoiDung",
                table: "DanhGia",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "UQ_Slug",
                table: "DanhMuc",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaNguoiDung",
                table: "DonHang",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "UQ_GioHang",
                table: "GioHang",
                column: "MaNguoiDung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatHang_MaDanhMuc",
                table: "MatHang",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_MatHang_MaKichCo",
                table: "MatHang",
                column: "MaKichCo");

            migrationBuilder.CreateIndex(
                name: "IX_MatHang_MaMauSac",
                table: "MatHang",
                column: "MaMauSac");

            migrationBuilder.CreateIndex(
                name: "IX_MatHang_MaNhaCungCap",
                table: "MatHang",
                column: "MaNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_MatHang_MaThuongHieu",
                table: "MatHang",
                column: "MaThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_MaLoaiNguoiDung",
                table: "NguoiDung",
                column: "MaLoaiNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_DiaChi_MaNguoiDung",
                table: "NguoiDung_DiaChi",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_TheoDoi_MaMatHang",
                table: "TheoDoi",
                column: "MaMatHang");

            migrationBuilder.CreateIndex(
                name: "UQ__TheoDoi__8FABF22D99F1F197",
                table: "TheoDoi",
                columns: new[] { "MaNguoiDung", "MaMatHang" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "DanhGia");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "MatHang_Anh");

            migrationBuilder.DropTable(
                name: "NguoiDung_DiaChi");

            migrationBuilder.DropTable(
                name: "TheoDoi");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "ChiTietGioHang");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "MatHang");

            migrationBuilder.DropTable(
                name: "LoaiNguoiDung");

            migrationBuilder.DropTable(
                name: "DanhMuc");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "ThuongHieu");

            migrationBuilder.DropTable(
                name: "KichCo");

            migrationBuilder.DropTable(
                name: "MauSac");
        }
    }
}
