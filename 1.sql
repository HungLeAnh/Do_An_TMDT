IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221016192749_Init', N'5.0.17');
GO
BEGIN TRANSACTION;
GO

CREATE TABLE [DanhMuc] (
    [MaDanhMuc] int NOT NULL IDENTITY,
    [TenDanhMuc] nvarchar(50) NOT NULL,
    [Slug] varchar(50) NOT NULL,
    CONSTRAINT [PK__DanhMuc__B375088709613860] PRIMARY KEY ([MaDanhMuc])
);
GO

CREATE TABLE [KichCo] (
    [MaKichCo] int NOT NULL IDENTITY,
    [KichCo] float NULL,
    CONSTRAINT [PK_KichCo] PRIMARY KEY ([MaKichCo])
);
GO

CREATE TABLE [LoaiNguoiDung] (
    [MaLoaiNguoiDung] nchar(10) NOT NULL,
    [TenLoaiNguoiDung] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_LoaiNguoiDung] PRIMARY KEY ([MaLoaiNguoiDung])
);
GO

CREATE TABLE [MauSac] (
    [MaMauSac] int NOT NULL IDENTITY,
    [TenMauSac] nchar(10) NULL,
    CONSTRAINT [PK_MauSac] PRIMARY KEY ([MaMauSac])
);
GO

CREATE TABLE [NhaCungCap] (
    [MaNhaCungCap] int NOT NULL IDENTITY,
    [TenNhaCungCap] nvarchar(50) NOT NULL,
    [STD] nchar(10) NOT NULL,
    CONSTRAINT [PK__NhaCungC__53DA92056B8D20D9] PRIMARY KEY ([MaNhaCungCap])
);
GO

CREATE TABLE [ThuongHieu] (
    [MaThuongHieu] int NOT NULL IDENTITY,
    [TenThuongHieu] nvarchar(50) NOT NULL,
    [Slug] nchar(20) NOT NULL,
    CONSTRAINT [PK__ThuongHi__A3733E2CCDB502B5] PRIMARY KEY ([MaThuongHieu])
);
GO

CREATE TABLE [NguoiDung] (
    [MaNguoiDung] int NOT NULL IDENTITY,
    [MaLoaiNguoiDung] nchar(10) NOT NULL,
    [TenNguoiDung] nvarchar(50) NOT NULL,
    [AnhDaiDien] nvarchar(max) NULL,
    [TenDangNhap] varchar(50) NOT NULL,
    [MatKhauHash] varchar(50) NOT NULL,
    [Salt] varchar(50) NOT NULL,
    [Email] varchar(50) NOT NULL,
    [SDT] nchar(10) NOT NULL,
    [ViDienTu] numeric(18,0) NULL,
    CONSTRAINT [PK__NguoiDun__C539D762078210E6] PRIMARY KEY ([MaNguoiDung]),
    CONSTRAINT [FK_NguoiDung_LoaiNguoiDung] FOREIGN KEY ([MaLoaiNguoiDung]) REFERENCES [LoaiNguoiDung] ([MaLoaiNguoiDung]) ON DELETE NO ACTION
);
GO

CREATE TABLE [MatHang] (
    [MaMatHang] int NOT NULL IDENTITY,
    [TenMatHang] nvarchar(20) NOT NULL,
    [GiaBan] numeric(18,0) NOT NULL,
    [DangDuocBan] bit NOT NULL,
    [SoSao] float NOT NULL,
    [SoLuong] int NULL,
    [SoLuongDaBan] int NULL,
    [MoTa] nvarchar(150) NOT NULL,
    [DangDuocHienThi] bit NOT NULL,
    [MaNhaCungCap] int NOT NULL,
    [MaThuongHieu] int NOT NULL,
    [MaDanhMuc] int NOT NULL,
    [MaKichCo] int NULL,
    [MaMauSac] int NULL,
    CONSTRAINT [PK__MatHang__A92254E571897811] PRIMARY KEY ([MaMatHang]),
    CONSTRAINT [FK__MatHang__MaDanhM__300424B4] FOREIGN KEY ([MaDanhMuc]) REFERENCES [DanhMuc] ([MaDanhMuc]) ON DELETE NO ACTION,
    CONSTRAINT [FK__MatHang__MaNhaCu__2E1BDC42] FOREIGN KEY ([MaNhaCungCap]) REFERENCES [NhaCungCap] ([MaNhaCungCap]) ON DELETE NO ACTION,
    CONSTRAINT [FK__MatHang__MaThuon__2F10007B] FOREIGN KEY ([MaThuongHieu]) REFERENCES [ThuongHieu] ([MaThuongHieu]) ON DELETE NO ACTION,
    CONSTRAINT [FK_MatHang_KichCo] FOREIGN KEY ([MaKichCo]) REFERENCES [KichCo] ([MaKichCo]) ON DELETE NO ACTION,
    CONSTRAINT [FK_MatHang_MauSac] FOREIGN KEY ([MaMauSac]) REFERENCES [MauSac] ([MaMauSac]) ON DELETE NO ACTION
);
GO

CREATE TABLE [DonHang] (
    [MaDonHang] int NOT NULL IDENTITY,
    [MaNguoiDung] int NOT NULL,
    [DiaChi] nvarchar(50) NOT NULL,
    [SDT] nchar(10) NOT NULL,
    [TinhTrang] nvarchar(50) NOT NULL,
    [DaThanhToan] bit NOT NULL,
    [TongTien] numeric(18,0) NOT NULL,
    CONSTRAINT [PK_DonHang_1] PRIMARY KEY ([MaDonHang]),
    CONSTRAINT [FK_DonHang_NguoiDung] FOREIGN KEY ([MaNguoiDung]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE NO ACTION
);
GO

CREATE TABLE [NguoiDung_DiaChi] (
    [MaDiaChi] int NOT NULL IDENTITY,
    [MaNguoiDung] int NOT NULL,
    [DiaChi] nvarchar(50) NULL,
    CONSTRAINT [PK__NguoiDun__7C39CE6E46E6A4D8] PRIMARY KEY ([MaDiaChi], [MaNguoiDung]),
    CONSTRAINT [FK__NguoiDung__MaNgu__534D60F1] FOREIGN KEY ([MaNguoiDung]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE NO ACTION
);
GO

CREATE TABLE [ChiTietGioHang] (
    [MaGioHang] int NOT NULL,
    [MaMatHang] int NOT NULL,
    [SoLuong] int NOT NULL,
    [Gia] int NOT NULL,
    CONSTRAINT [PK_ChiTietGioHang] PRIMARY KEY ([MaGioHang], [MaMatHang]),
    CONSTRAINT [AK_ChiTietGioHang_MaGioHang] UNIQUE ([MaGioHang]),
    CONSTRAINT [FK__ChiTietGi__MaMat__48CFD27E] FOREIGN KEY ([MaMatHang]) REFERENCES [MatHang] ([MaMatHang]) ON DELETE NO ACTION
);
GO

CREATE TABLE [MatHang_Anh] (
    [MaAnh] int NOT NULL IDENTITY,
    [MaMatHang] int NOT NULL,
    [Anh] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_MatHang_Anh] PRIMARY KEY ([MaMatHang], [MaAnh]),
    CONSTRAINT [FK__MatHang_A__MaMat__5070F446] FOREIGN KEY ([MaMatHang]) REFERENCES [MatHang] ([MaMatHang]) ON DELETE NO ACTION
);
GO

CREATE TABLE [TheoDoi] (
    [MaTheoDoi] int NOT NULL IDENTITY,
    [MaNguoiDung] int NOT NULL,
    [MaMatHang] int NOT NULL,
    CONSTRAINT [PK__TheoDoi__3156C07993ADD5B7] PRIMARY KEY ([MaTheoDoi], [MaNguoiDung], [MaMatHang]),
    CONSTRAINT [FK__TheoDoi__MaMatHa__4D94879B] FOREIGN KEY ([MaMatHang]) REFERENCES [MatHang] ([MaMatHang]) ON DELETE NO ACTION,
    CONSTRAINT [FK__TheoDoi__MaNguoi__4CA06362] FOREIGN KEY ([MaNguoiDung]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE NO ACTION
);
GO

CREATE TABLE [ChiTietDonHang] (
    [MaChiTietDonHang] int NOT NULL IDENTITY,
    [MaDonHang] int NOT NULL,
    [MaMatHang] int NOT NULL,
    [Gia] numeric(18,0) NOT NULL,
    [SoLuong] int NOT NULL,
    CONSTRAINT [PK_ChiTietDonHang_1] PRIMARY KEY ([MaChiTietDonHang], [MaDonHang]),
    CONSTRAINT [FK__ChiTietDo__MaMat__5FB337D6] FOREIGN KEY ([MaMatHang]) REFERENCES [MatHang] ([MaMatHang]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ChiTietDonHang_DonHang1] FOREIGN KEY ([MaDonHang]) REFERENCES [DonHang] ([MaDonHang]) ON DELETE NO ACTION
);
GO

CREATE TABLE [DanhGia] (
    [MaDanhGia] int NOT NULL IDENTITY,
    [NoiDung] nvarchar(150) NULL,
    [SoSao] float NOT NULL,
    [MaMatHang] int NOT NULL,
    [MaNguoiDung] int NOT NULL,
    [MaDonHang] int NOT NULL,
    CONSTRAINT [PK_DanhGia] PRIMARY KEY ([MaDanhGia]),
    CONSTRAINT [FK__DanhGia__MaMatHa__5629CD9C] FOREIGN KEY ([MaMatHang]) REFERENCES [MatHang] ([MaMatHang]) ON DELETE NO ACTION,
    CONSTRAINT [FK_DanhGia_DonHang1] FOREIGN KEY ([MaDonHang]) REFERENCES [DonHang] ([MaDonHang]) ON DELETE NO ACTION,
    CONSTRAINT [FK_DanhGia_NguoiDung] FOREIGN KEY ([MaNguoiDung]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE NO ACTION
);
GO

CREATE TABLE [GioHang] (
    [MaGioHang] int NOT NULL,
    [MaNguoiDung] int NOT NULL,
    CONSTRAINT [PK_GioHang] PRIMARY KEY ([MaGioHang]),
    CONSTRAINT [FK_GioHang_ChiTietGioHang] FOREIGN KEY ([MaGioHang]) REFERENCES [ChiTietGioHang] ([MaGioHang]) ON DELETE NO ACTION,
    CONSTRAINT [FK_GioHang_NguoiDung] FOREIGN KEY ([MaNguoiDung]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_ChiTietDonHang_MaDonHang] ON [ChiTietDonHang] ([MaDonHang]);
GO

CREATE INDEX [IX_ChiTietDonHang_MaMatHang] ON [ChiTietDonHang] ([MaMatHang]);
GO

CREATE INDEX [IX_ChiTietGioHang_MaMatHang] ON [ChiTietGioHang] ([MaMatHang]);
GO

CREATE UNIQUE INDEX [UQ_ChiTietGioHang] ON [ChiTietGioHang] ([MaGioHang]);
GO

CREATE INDEX [IX_DanhGia_MaDonHang] ON [DanhGia] ([MaDonHang]);
GO

CREATE INDEX [IX_DanhGia_MaMatHang] ON [DanhGia] ([MaMatHang]);
GO

CREATE INDEX [IX_DanhGia_MaNguoiDung] ON [DanhGia] ([MaNguoiDung]);
GO

CREATE UNIQUE INDEX [UQ_Slug] ON [DanhMuc] ([Slug]);
GO

CREATE INDEX [IX_DonHang_MaNguoiDung] ON [DonHang] ([MaNguoiDung]);
GO

CREATE UNIQUE INDEX [UQ_GioHang] ON [GioHang] ([MaNguoiDung]);
GO

CREATE INDEX [IX_MatHang_MaDanhMuc] ON [MatHang] ([MaDanhMuc]);
GO

CREATE INDEX [IX_MatHang_MaKichCo] ON [MatHang] ([MaKichCo]);
GO

CREATE INDEX [IX_MatHang_MaMauSac] ON [MatHang] ([MaMauSac]);
GO

CREATE INDEX [IX_MatHang_MaNhaCungCap] ON [MatHang] ([MaNhaCungCap]);
GO

CREATE INDEX [IX_MatHang_MaThuongHieu] ON [MatHang] ([MaThuongHieu]);
GO

CREATE INDEX [IX_NguoiDung_MaLoaiNguoiDung] ON [NguoiDung] ([MaLoaiNguoiDung]);
GO

CREATE INDEX [IX_NguoiDung_DiaChi_MaNguoiDung] ON [NguoiDung_DiaChi] ([MaNguoiDung]);
GO

CREATE INDEX [IX_TheoDoi_MaMatHang] ON [TheoDoi] ([MaMatHang]);
GO

CREATE UNIQUE INDEX [UQ__TheoDoi__8FABF22D99F1F197] ON [TheoDoi] ([MaNguoiDung], [MaMatHang]);
GO



COMMIT;
GO

