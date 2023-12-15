using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Do_an_CCNPMM.Data.Migrations
{
    public partial class fixdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_ChiTietGioHang",
                table: "GioHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GioHang",
                table: "GioHang");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ChiTietGioHang_MaGioHang",
                table: "ChiTietGioHang");

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
                defaultValueSql: "(N'')",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "NguoiDung",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MatKhauHash",
                table: "NguoiDung",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaLoaiNguoiDung",
                table: "NguoiDung",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "MatHang",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaLoaiNguoiDung",
                table: "LoaiNguoiDung",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "MaNguoiGiaoHang",
                table: "DonHang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayXuatDonHang",
                table: "DonHang",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GioHang_1",
                table: "GioHang",
                column: "MaGioHang");

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
                name: "FK_ChiTietGioHang_GioHang",
                table: "ChiTietGioHang",
                column: "MaGioHang",
                principalTable: "GioHang",
                principalColumn: "MaGioHang",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_ChiTietGioHang_GioHang",
                table: "ChiTietGioHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_NguoiDung1",
                table: "DonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GioHang_1",
                table: "GioHang");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_MaNguoiGiaoHang",
                table: "DonHang");

            migrationBuilder.DropIndex(
                name: "AK_ChiTietGioHang_MaGioHang",
                table: "ChiTietGioHang");

            migrationBuilder.DropColumn(
                name: "MaNguoiGiaoHang",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "NgayXuatDonHang",
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
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValueSql: "(N'')");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "NguoiDung",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "MatKhauHash",
                table: "NguoiDung",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "MaLoaiNguoiDung",
                table: "NguoiDung",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "MatHang",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MaLoaiNguoiDung",
                table: "LoaiNguoiDung",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GioHang",
                table: "GioHang",
                column: "MaGioHang");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ChiTietGioHang_MaGioHang",
                table: "ChiTietGioHang",
                column: "MaGioHang");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_ChiTietGioHang",
                table: "GioHang",
                column: "MaGioHang",
                principalTable: "ChiTietGioHang",
                principalColumn: "MaGioHang",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
