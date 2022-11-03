using Microsoft.EntityFrameworkCore.Migrations;

namespace Do_an_TMDT.Migrations
{
    public partial class newdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "ThuongHieu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(20)",
                oldFixedLength: true,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "NguoiDung_DiaChi",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "KichCo",
                table: "KichCo",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaNguoiGiaoHang",
                table: "DonHang",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaNguoiGiaoHang",
                table: "DonHang",
                column: "MaNguoiGiaoHang");

            migrationBuilder.CreateIndex(
                name: "AK_ChiTietGioHang_MaGioHang",
                table: "ChiTietGioHang",
                column: "MaGioHang",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_NguoiDung1",
                table: "DonHang",
                column: "MaNguoiGiaoHang",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_NguoiDung1",
                table: "DonHang");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_MaNguoiGiaoHang",
                table: "DonHang");

            migrationBuilder.DropIndex(
                name: "AK_ChiTietGioHang_MaGioHang",
                table: "ChiTietGioHang");

            migrationBuilder.DropColumn(
                name: "MaNguoiGiaoHang",
                table: "DonHang");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "ThuongHieu",
                type: "nchar(20)",
                fixedLength: true,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "NguoiDung_DiaChi",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<double>(
                name: "KichCo",
                table: "KichCo",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
