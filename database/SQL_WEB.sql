CREATE DATABASE QuanLyThuVien;
GO

USE QuanLyThuVien;
--DROP DATABASE QuanLyThuVien;
CREATE TABLE NhanVien (
	MaNV INT PRIMARY KEY IDENTITY NOT NULL ,
	HoTenNV NVARCHAR(50),
	GioiTinh NVARCHAR(50),
	DiaChi NVARCHAR(50),
	NGAYSINH DATE,
	SDT NVARCHAR(50),
	ChucVu NVARCHAR(50)
);

create TABLE DocGia (
	MaDG INT PRIMARY KEY IDENTITY NOT NULL,
	HoTenDG NVARCHAR(50),
	GioiTinh NVARCHAR(50),
	NgaySinh DATE,
	SDT NVARCHAR(50),
	DiaChi NVARCHAR(50)
);

CREATE TABLE TheDocGia (
	MaThe INT PRIMARY KEY IDENTITY NOT NULL,
	--HanThe NVARCHAR(50),
	NgayDK DATE,
	NgayHH DATE,
	TienThe INT,
	MaNV INT,
	MaDG INT,
	FOREIGN KEY (MaNV) REFERENCES NhanVien (MaNV),
	FOREIGN KEY (MaDG) REFERENCES DocGia (MaDG)
);

CREATE TABLE NhaCungCap (
	MaNCC INT PRIMARY KEY IDENTITY NOT NULL,
	TenNCC NVARCHAR(150),
	DiaChiNCC NVARCHAR(100),
	sdtNCC NVARCHAR(50)
);

CREATE TABLE PhieuNhapSach (
	MaPN INT PRIMARY KEY IDENTITY NOT NULL,
	NgayNhap DATE,
	MaNV INT,
	MaNCC INT,
	FOREIGN KEY (MaNV) REFERENCES NhanVien (MaNV),
	FOREIGN KEY (MaNCC) REFERENCES NhaCungCap (MaNCC)
);

CREATE TABLE Sach (
	MaSach INT PRIMARY KEY IDENTITY NOT NULL,
	TenSach NVARCHAR(150),
	TheLoai NVARCHAR(50),
	TacGia NVARCHAR(50),
	NgonNgu NVARCHAR(50),
	NXB NVARCHAR(100),
	NamXB INT,
	SoLuongHIENTAI INT
);

CREATE TABLE CHITIETPN(
	MaPN INT,
	FOREIGN KEY (MaPN) REFERENCES PhieuNhapSach (MaPN),
	MaSACH INT,
	FOREIGN KEY (MaSACH) REFERENCES SACH (MaSACH),
	GiaSach MONEY,
	SoLuongNHAP INT,
	constraint ChiTietPN_MaPN_MaSach PRIMARY KEY (MaPN, MaSach)
	);

CREATE TABLE DonViTL (
	MaDV INT PRIMARY KEY IDENTITY NOT NULL,
	TenDV NVARCHAR(150),
	DiaChiDV NVARCHAR(100),
	SDTDV NVARCHAR(50)
);

CREATE TABLE PhieuMuon (
	MaPM INT PRIMARY KEY IDENTITY NOT NULL,
	NgayMuon DATE,
	HanTra DATE,
	MaThe INT,
	MaNV INT,
	FOREIGN KEY (MaNV) REFERENCES NhanVien (MaNV),
	FOREIGN KEY (MaThe) REFERENCES TheDocGia (MaThe),
	Tinhtrang bit ,--default '0'--Nvarchar(10),
	MaDK int
);

CREATE TABLE ChiTietPM (
	MaPM INT,
	MaSach INT,
	Soluongmuon INT,
	FOREIGN KEY (MaPM) REFERENCES PhieuMuon (MaPM),
	FOREIGN KEY (MaSach) REFERENCES Sach (MaSach),
	constraint ChiTietPM_MaPM_MaSach PRIMARY KEY (MaPM, MaSach)

);

CREATE TABLE PhieuTra (
	MaPT INT PRIMARY KEY IDENTITY NOT NULL,
	NgayTra DATE,
	MaNV INT,
	MaThe INT,
	MaPM INT,
	FOREIGN KEY (MaPM) REFERENCES PhieuMuon (MaPM),
	FOREIGN KEY (MaNV) REFERENCES NhanVien (MaNV),
	FOREIGN KEY (MaThe) REFERENCES TheDocGia (MaThe)
);

CREATE TABLE ChiTietPT (
	MaPT INT,
	MaSach INT,
	Soluongtra INT,
	Soluongloi INT,
	Soluongmat INT,
	PhuThu MONEY,
	FOREIGN KEY (MaPT) REFERENCES PhieuTra (MaPT),
	FOREIGN KEY (MaSach) REFERENCES Sach (MaSach),
	constraint ChiTietPT_MaPT_MaSach PRIMARY KEY (MaPT, MaSach)---id
);

CREATE TABLE KhoSachThanhLy (
    masachkho int PRIMARY KEY,
    soluongkhotl INT
);

CREATE TABLE PhieuThanhLy (
	MaPTL INT PRIMARY KEY IDENTITY NOT NULL,
	NgayTL DATE,
	MaDV INT,
	MaNV INT,
	FOREIGN KEY (MaNV) REFERENCES NhanVien (MaNV),
	FOREIGN KEY (MaDV) REFERENCES DonViTL (MaDV)
);

CREATE  TABLE ChiTietPTL (
    MaPTL INT,
	MaSachkho INT,
	Soluongtl INT,
	GiaTL MONEY,
    FOREIGN KEY (MaPTL) REFERENCES PhieuThanhLy (MaPTL),
	FOREIGN KEY (MaSachkho) REFERENCES KhoSachThanhLy (MaSachkho), -- Thay vì tham chiếu đến MaSach, tham chiếu đến ID của KhoSachThanhLy
	constraint ChiTietPTL_MaPTL_MaSach PRIMARY KEY (MaPTL, MaSachkho)
);


--khong cho  nó đăng kí thẻ onlline 
CREATE TABLE LOGIN_DG (
	SDT NVARCHAR(50) PRIMARY KEY,
	PASSWORD_DG NVARCHAR(255),
	HoTen NVARCHAR(50),
	Email Nvarchar(255)
	--MaDG INT,
	--	FOREIGN KEY (MaDG) REFERENCES DOCGIA (MaDG),
);

CREATE TABLE LOGIN_NV (
	USERNAME_NV NVARCHAR(50) PRIMARY KEY,
	PASSWORD_NV NVARCHAR(MAX),
	MANV INT,
		FOREIGN KEY (MANV) REFERENCES NHANVIEN (MANV),
);

CREATE TABLE TT_SACH(
MA_TT_SACH  INT PRIMARY KEY IDENTITY NOT NULL,
URL_IMAGE TEXT,
MOTA NTEXT,
MASACH INT,
FOREIGN KEY (MASACH) REFERENCES Sach (MASACH),
);

CREATE TABLE DkiMuonSach (
	MaDK INT PRIMARY KEY IDENTITY NOT NULL,
	SDT NVARCHAR(50),
	NgayDKMuon DATE,
	NgayHen DATE,
	FOREIGN KEY (SDT) REFERENCES LOGIN_DG (SDT),
	Tinhtrang int --DANG CHỜ XÁC THỰC  0
	--đã duyệt  1
	--đã mượn  2
	--đã hủy 3
);

CREATE TABLE ChiTietDk (
	MaDK INT,
	MaSach INT,
	Soluongmuon INT,
	FOREIGN KEY (MaDK) REFERENCES DkiMuonSach (MaDK),
	FOREIGN KEY (MaSach) REFERENCES Sach (MaSach),
	constraint ChiTietDk_MaDK_MaSach PRIMARY KEY (MaDK, MaSach)
);


----------------+**************************************************************+----------------
--RANG BUOC NHAN VIEN 
ALTER TABLE NHANVIEN ADD CONSTRAINT CHK_GIOITINH_NV CHECK (GIOITINH IN(N'Nam',N'Nữ'));
ALTER TABLE NHANVIEN ADD CONSTRAINT CHK_SDT_NV   UNIQUE(SDT);
ALTER TABLE NHANVIEN ADD CONSTRAINT CHK_CHUCVU_NV CHECK (CHUCVU IN('ADMIN','QuanLyKho','ThuThu'));
ALTER TABLE NHANVIEN ADD CONSTRAINT CHK_NGAYSINH_NV CHECK (NGAYSINH < GETDATE());


--RANG BUOC DOC GIA
ALTER TABLE DOCGIA ADD CONSTRAINT CHK_GIOITINH_DG CHECK (GIOITINH IN(N'Nam',N'Nữ'));
ALTER TABLE DOCGIA ADD CONSTRAINT CHK_SDT_DG   UNIQUE(SDT);
ALTER TABLE DOCGIA ADD CONSTRAINT CHK_NGAYSINH_DG CHECK (NGAYSINH < GETDATE());

--RANG BUOC THE DOC GIA
ALTER TABLE THEDOCGIA ADD CONSTRAINT CHK_NGAYDK_THEDG CHECK (NGAYDK < NGAYHH);
ALTER TABLE THEDOCGIA ADD CONSTRAINT CHK_TIENTHE_THEDG CHECK (TIENTHE >0 );


--RANG BUOC NCC
ALTER TABLE NHACUNGCAP ADD CONSTRAINT CHK_SDT_NCC   UNIQUE(SDTNCC);

--RANG BUOC PHIEU NHAP SACH
ALTER TABLE PHIEUNHAPSACH ADD CONSTRAINT CHECK_NGAYNHAP_PNS CHECK (NGAYNHAP <= GETDATE());
--ALTER TABLE PHIEUNHAPSACH ADD CONSTRAINT CHECK_TONGSACH_PNS CHECK (TONGSACHNHAP >0);
--ALTER TABLE PHIEUNHAPSACH ADD CONSTRAINT CHECK_TONGTIEN_PNS CHECK (TONGTIEN >0);


--RANG BUOC SACH
ALTER TABLE SACH ADD CONSTRAINT CHK_THELOAI_SACH CHECK (THELOAI IN(N'Truyện ngắn',N'Truyện thiếu nhi',N'Tiểu thuyết', N'Ngôn tình',N'Sách giáo khoa',N'Sách tham khảo', N'Văn học',N'Sách ngoại ngữ',N'Kỹ năng sống'));
ALTER TABLE SACH ADD CONSTRAINT CHK_NGONNGU_SACH CHECK (NGONNGU IN(N'Tiếng anh',N'Tiếng việt',N'Tiếng hàn',N'Tiếng trung',N'Tiếng pháp'));
ALTER TABLE SACH ADD CONSTRAINT CHK_NAMXB_SACH CHECK (NAMXB <= YEAR(GETDATE()));
--ALTER TABLE SACH ADD CONSTRAINT CHK_GIASACH_SACH CHECK (GIASACH >0);
ALTER TABLE SACH ADD CONSTRAINT CHK_SOLUONG_SACH CHECK (SOLUONGHIENTAI >=0);

--RANG BUOC CHI TIET PHIEU NHAP 
ALTER TABLE CHITIETPN ADD CONSTRAINT CHK_SOLUONG_CTPN CHECK (SOLUONGNHAP > 0);
ALTER TABLE CHITIETPN ADD CONSTRAINT CHK_GIASACH_CTPN CHECK (GIASACH >0);

--RANG BUOC DONVITL
ALTER TABLE DONVITL ADD CONSTRAINT CHK_SDT_DVTL   UNIQUE(SDTDV);

--RANG BUOC PHIEU MUON 
ALTER TABLE PHIEUMUON ADD CONSTRAINT CHK_NGAYMUON_PM CHECK (NGAYMUON < HANTRA);
--ALTER TABLE PHIEUMUON ADD CONSTRAINT CHK_PHIEUMUON_Tinhtrang CHECK (Tinhtrang IN(0,1));

--ALTER TABLE PHIEUMUON ADD CONSTRAINT CHK_SOLUONGTONG_PM CHECK (SOLUONGTONG > 0);


--RANG BUOC CHI TIET PHIEU MUON 
ALTER TABLE CHITIETPM ADD CONSTRAINT CHK_SOLUONG_CTPM CHECK (SOLUONGmuon > 0);


--RANG BUOC CHI TIET PHIEU TRA 
ALTER TABLE CHITIETPT ADD CONSTRAINT CHK_SOLUONGT_CTPT CHECK (SOLUONGtra >= 0);
ALTER TABLE CHITIETPT ADD CONSTRAINT CHK_SOLUONGL_CTPT CHECK (SOLUONGloi >= 0);
ALTER TABLE CHITIETPT ADD CONSTRAINT CHK_PHUTHU_CTPT CHECK (PhuThu >= 0);
--ALTER TABLE CHITIETPT ADD CONSTRAINT CHK_SOLUONG_CTPT CHECK (SOLUONGloi+SOLUONGtra = SOLUONGmuon);

--RANG BUOC KHO SACH THANH LY
ALTER TABLE KHOSACHTHANHLY ADD CONSTRAINT CHK_SOLUONG_KHOSACH CHECK (SOLUONGKHOTL >=0);

--RANG BUOC PHIEU THANH LY
ALTER TABLE PHIEUTHANHLY ADD CONSTRAINT CHK_NgayTL_PTL CHECK(NgayTL<= GETDATE());
--ALTER TABLE PHIEUTHANHLY ADD CONSTRAINT CHK_TongSachTL_PTL CHECK(TongSachTL>0);

--RANG BUOC CHI TIET PHIEU THANH LY 
ALTER TABLE CHITIETPTL ADD CONSTRAINT CHK_SOLUONG_CTPTL CHECK (SOLUONGtL > 0);
ALTER TABLE CHITIETPTL ADD CONSTRAINT CHK_GiaTL_CTPTL CHECK (GiaTL > 0);


--RANG BUOC phieu dk
ALTER TABLE DkiMuonSach ADD CONSTRAINT CHK_NGAYMUON_DK CHECK (NgayDKMuon <= NgayHen);
ALTER TABLE DkiMuonSach add CONSTRAINT CHK_TINHTRANG_DK CHECK (Tinhtrang IN (0,1, 2,3));


--ALTER TABLE PHIEUMUON ADD CONSTRAINT CHK_PHIEUMUON_Tinhtrang CHECK (Tinhtrang IN(0,1));

--ALTER TABLE PHIEUMUON ADD CONSTRAINT CHK_SOLUONGTONG_PM CHECK (SOLUONGTONG > 0);


--RANG BUOC CHI TIET PHIEU MUON 
ALTER TABLE CHITIETDK ADD CONSTRAINT CHK_SOLUONG_CTDK CHECK (SOLUONGmuon > 0);

--RANG BUOC login dg
ALTER TABLE Login_DG ADD CONSTRAINT CHK_SDT_LOGIN_DG   UNIQUE(SDT);
ALTER TABLE Login_DG ADD CONSTRAINT CHK_EMAIL_LOGIN_DG   UNIQUE(EMAIL);


--***************CẬP NHẬT SỐ LƯỢNG CÁC THAO TÁC+******************
/* CẬP NHẬT SÁCH TRONG KHO SAU KHI NHAP  */
CREATE or ALTER TRIGGER TRG_SACHNHAP ON CHITIETPN AFTER INSERT AS 
BEGIN
	UPDATE SACH
	SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI + (
		SELECT SOLUONGNHAP
		FROM INSERTED
		WHERE MASACH = SACH.MASACH
	)
	FROM SACH
	JOIN INSERTED ON INSERTED.MASACH = SACH.MASACH
END

GO
/* CẬP NHẬT SÁCH TRONG KHO SAU KHI MƯỢN  */
--CREATE or ALTER TRIGGER TRG_SACHMUON ON CHITIETPM AFTER INSERT AS 
--BEGIN
   
--	UPDATE SACH
--	SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI - (
--		SELECT SOLUONGmuon
--		FROM INSERTED
--		WHERE MASACH = SACH.MASACH
--	)
--	FROM SACH
--	JOIN INSERTED ON INSERTED.MASACH = SACH.MASACH
--END

CREATE OR ALTER  TRIGGER TRG_SACHMUON ON CHITIETPM AFTER INSERT AS 
BEGIN
    DECLARE @Madk INT;

    SELECT TOP 1 @Madk = MADK 
    FROM INSERTED JOIN PHIEUMUON ON INSERTED.MAPM = PHIEUMUON.MAPM;
     
    IF isnull(@Madk,0) = 0
    BEGIN
        -- Nếu Madk = 0, cập nhật kho sách
      
        
            PRINT 'Updating SACH';
            -- Nếu Madk = 0, cập nhật kho sách
            UPDATE SACH
            SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI - (
                SELECT SOLUONGmuon
                FROM INSERTED
                WHERE MASACH = SACH.MASACH
            )
            FROM SACH
            JOIN INSERTED ON INSERTED.MASACH = SACH.MASACH;
        
    END
END;


--/* CẬP NHẬT SÁCH TRONG KHO SAU KHI TRẢ  */
CREATE OR ALTER TRIGGER TRG_SACHTRAvaTL ON CHITIETPT AFTER INSERT AS 
BEGIN
    -- Update the current quantity of books in the 'SACH' table after a book is returned
	UPDATE SACH
	SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI + (
		SELECT SOLUONGtra
		FROM INSERTED
		WHERE MASACH = SACH.MASACH
	)
	FROM SACH
	JOIN INSERTED ON INSERTED.MASACH = SACH.MASACH;

    -- Update the quantity of books and faulty books in the 'KhoSachThanhLy' table after a book is returned
    DECLARE @MaSach INT;
    DECLARE @soluongLOI INT;

    -- Iterate over the inserted rows
    DECLARE Cursor_SachTra CURSOR FOR
    SELECT MASACH, SOLUONGloi
    FROM INSERTED;

    OPEN Cursor_SachTra;

    FETCH NEXT FROM Cursor_SachTra INTO @MaSach, @soluongLOI;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Check if the book exists in 'KhoSachThanhLy'
        IF EXISTS (SELECT 1 FROM KhoSachThanhLy WHERE masachkho = @MaSach)
        BEGIN
            -- If exists, update the quantity of faulty books
            UPDATE KhoSachThanhLy
            SET soluongkhotl = soluongkhotl + @soluongLOI
            WHERE masachkho = @MaSach;
        END
        ELSE
        BEGIN
            -- If not exists and quantity of faulty books > 0, insert a new row for the book in 'KhoSachThanhLy'
            IF @soluongLOI > 0
            BEGIN
                INSERT INTO KhoSachThanhLy (masachkho, soluongkhotl)
                VALUES (@MaSach, @soluongLOI);
            END;
        END;

        FETCH NEXT FROM Cursor_SachTra INTO @MaSach, @soluongLOI;
    END;

    CLOSE Cursor_SachTra;
    DEALLOCATE Cursor_SachTra;
END;
--GO



/* CẬP NHẬT SÁCH TRONG KHO SAU KHI THANH LÍ  */
CREATE or ALTER TRIGGER TRG_SACHTL ON CHITIETPTL AFTER INSERT AS 
BEGIN
	UPDATE KhoSachThanhLy 
	SET KhoSachThanhLy.SOLUONGKHOTL = KhoSachThanhLy.SOLUONGKHOTL - (
		SELECT Soluongtl
		FROM INSERTED
		WHERE MASACHKHO = KhoSachThanhLy.MASACHKHO
	)
	FROM KhoSachThanhLy 
	JOIN INSERTED ON INSERTED.MASACHKHO = KhoSachThanhLy.MASACHKHO
END

/* CẬP NHẬT SÁCH TRONG KHO SAU KHI duyệt  */
CREATE or ALTER TRIGGER TRG_SachMuonOnl ON DkiMuonSach after UPDATE AS 
BEGIN
-- DECLARE @tinhtrang INT;
  DECLARE @tinhtrang_before INT;
    DECLARE @tinhtrang_after INT;

    SELECT TOP 1 @tinhtrang_before = d.tinhtrang, @tinhtrang_after = i.tinhtrang
    FROM DELETED d
    JOIN INSERTED i ON d.madk = i.madk;
    
	--SELECT TOP 1 @tinhtrang_after = tinhtrang 
 --   FROM INSERTED;
	if @tinhtrang_after=1
	begin 
		print @tinhtrang_after;
		UPDATE SACH
		SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI - (
			SELECT SOLUONGmuon
			FROM INSERTED JOIN ChiTietDk ON INSERTED.MADK = ChiTietDk.MaDK-- join DkiMuonSach on 
			WHERE ChiTietDk.MASACH = SACH.MASACH AND Tinhtrang = 1
		)
		FROM SACH JOIN ChiTietDk  ON  ChiTietDk.MASACH = SACH.MASACH 
		JOIN INSERTED ON INSERTED.MADK = ChiTietDk.MaDK 
	end
	if @tinhtrang_after =3
	begin
		print @tinhtrang_after;
		UPDATE SACH
		SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI + (
			SELECT SOLUONGmuon
			FROM INSERTED JOIN ChiTietDk ON INSERTED.MADK = ChiTietDk.MaDK
			WHERE ChiTietDk.MASACH = SACH.MASACH AND @tinhtrang_after = 3
		)
		FROM SACH JOIN ChiTietDk  ON  ChiTietDk.MASACH = SACH.MASACH 
		JOIN INSERTED ON INSERTED.MADK = ChiTietDk.MaDK 
		join deleted on deleted.MaDK =INSERTED.MADK where   @tinhtrang_before = 1
	end
	
END



--******************CÁC TRIGGER VỀ TÌNH TRẠNG XÉT DUYỆT*********************
/* CẬP NHẬT TÌNH TRẠNG PHIẾU MƯỢN */
CREATE OR ALTER  TRIGGER SetTinhTrangPM
ON PHIEUMUON 
FOR INSERT AS
BEGIN 
	UPDATE PHIEUMUON 
	SET TINHTRANG = 0
	FROM PHIEUMUON 
	JOIN INSERTED ON INSERTED.MAPM = PHIEUMUON.MaPM
END;


CREATE OR ALTER TRIGGER UpdateTinhTrangPhieuMuon
ON chitietpt
AFTER INSERT
AS
BEGIN
-- Kiểm tra số lượng sách trả và số lượng sách mượn
    IF (SELECT COUNT(*) FROM inserted) > 0
		begin 
		   UPDATE PM
			SET Tinhtrang = '1'
			FROM PhieuMuon PM
			JOIN (
				SELECT PM.mapm, count(pm.mapm) as loaisach
				FROM PhieuMuon PM 
				JOIN chitietpm ctpm ON PM.mapm  = ctpm.mapm 
				WHERE PM.Tinhtrang = '0'
				GROUP BY Pm.mapm
				INTERSECT
				SELECT mapm, count(mapm) as soluongls 
				FROM (
					SELECT PM.mapm , ctpm.masach, Soluongmuon
					FROM PhieuMuon PM 
					JOIN chitietpm ctpm ON PM.mapm  = ctpm.mapm 
					WHERE PM.Tinhtrang = '0'
					INTERSECT
					SELECT Pt.mapm,  ctpt.masach, SUM(ISNULL(ctpt.Soluongtra, 0) + ISNULL(ctpt.Soluongloi, 0)+ ISNULL(ctpt.Soluongmat, 0)) AS soluongtra
					FROM phieutra Pt
					JOIN chitietpt ctpt ON Pt.mapt = ctpt.mapt
					LEFT JOIN chitietpm ctpm ON Pt.mapm = ctpm.mapm AND ctpt.masach = ctpm.masach
					GROUP BY Pt.mapm, ctpt.masach
				) AS a
				GROUP BY a.mapm
			) AS CountResult ON PM.mapm = CountResult.mapm
		end
   -- WHERE CountResult.loaisach = CountResult.soluongls;
END;

/* CẬP NHẬT TÌNH TRẠNG ĐK MƯỢN SÁCH */
CREATE OR ALTER  TRIGGER SetTinhTrangPhieuDK
ON DkiMuonSach 
FOR INSERT AS
BEGIN 
	UPDATE DkiMuonSach 
	SET TINHTRANG = 0
	FROM DkiMuonSach 
	JOIN INSERTED ON INSERTED.MaDK = DkiMuonSach.MaDK
END;



--******************TẠO VIEW CHECK TỔNG TIEN *********************
CREATE or alter VIEW PHIEUNHAP_VIEW 
AS
	SELECT PN.MAPN, NgayNhap, MaNV, MaNCC, SUM(ABS(S.GIASACH*SOLUONGNHAP)) AS N'TỔNG TIỀN', SUM(SOLUONGNHAP) AS N'TỔNG SÁCH'
	FROM PHIEUNHAPSACH PN JOIN CHITIETPN S ON PN.MAPN= S.MAPN JOIN SACH ON SACH.MaSach = S.MaSACH
	GROUP BY PN.MAPN, NgayNhap, MaNV, MaNCC;
		

CREATE or alter VIEW PHIEUTra_VIEW 
AS
	SELECT pt.mapt,MaPM, MaThe, NgayTra, MaNV, SUM(PhuThu) AS N'Tổng tiền xử lý'
	FROM PhieuTra Pt JOIN CHITIETPt ctpt ON Pt.MAPt= ctpt.MAPt JOIN SACH ON SACH.MaSach = ctpt.MaSACH
	GROUP BY pt.mapt,MaPM, MaThe, NgayTra, MaNV;


CREATE or alter VIEW PHIEUThanhLy_VIEW 
AS
	SELECT ptl.maptl, Madv, NgayTL, MaNV, SUM(GiaTL*Soluongtl) AS N'Tổng tiền thanh lý'
	FROM PhieuThanhLy Ptl JOIN CHITIETPtl ctptl ON Ptl.MAPtl= ctptl.MAPtl JOIN SACH ON SACH.MaSach = ctptl.MaSACHkho
	GROUP BY ptl.maptl, Madv, NgayTL, MaNV;

	-----------------******************************-------------------
CREATE PROCEDURE pro_UpdateTinhTrangHuyDkiMuon
AS
BEGIN
    -- Thực hiện cập nhật tình trạng của bảng
    UPDATE DkiMuonSach
    SET Tinhtrang = '3'
    WHERE NgayHen <= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), -1) and Tinhtrang in(0,1);
END





----/* CẬP NHẬT SÁCH TRONG KHO khi insert  */
--CREATE OR ALTER drop PROCEDURE KiemTraMaVaCapNhatSachthanhly
--    @MaSachkho INT,
--    @soluongkhotl INT,
--    @InsertLocation INT -- Thêm tham số mới để phân biệt nơi thực hiện insert
--AS
--BEGIN
--    -- Kiểm tra xem mã sách đã tồn tại trong kho thanh lý chưa
--    IF EXISTS (SELECT 1 FROM KhosachThanhLy WHERE masachkho = @MaSachkho)
--    BEGIN
--        -- Nếu tồn tại, tăng số lượng sách
--        UPDATE KhosachThanhLy
--        SET soluongkhotl = soluongkhotl + @soluongkhotl
--        WHERE MaSachkho = @MaSachkho;
--    END
--    ELSE
--    BEGIN
--        -- Nếu chưa tồn tại, thêm mới sách vào kho thanh lý
--        INSERT INTO KhosachThanhLy (MaSachkho, soluongkhotl)
--        VALUES (@MaSachkho, @soluongkhotl);
--    END

--    -- Giảm số lượng sách trong bảng chính
--    IF @InsertLocation = 1 -- Thêm điều kiện để phân biệt nơi thực hiện insert
--    BEGIN
--        UPDATE SACH
--        SET SoLuongHIENTAI = SoLuongHIENTAI - @soluongkhotl
--        WHERE MaSach = @MaSachkho;
--    END;
--END;
	
--CREATE OR ALTER drop TRIGGER Trig_CapNhatSachThanhLy
--ON KhosachThanhLy
--AFTER INSERT
--AS
--BEGIN
--    DECLARE @MaSachkho INT, @soluongkhotl INT;

--    -- Lấy dữ liệu từ bảng Inserted (chứa dữ liệu mới được thêm vào)
--    SELECT @MaSachkho = MaSachkho, @soluongkhotl = soluongkhotl
--    FROM Inserted;

--    -- Kiểm tra xem mã sách đã tồn tại trong kho thanh lý chưa
--    IF EXISTS (SELECT 1 FROM KhoSachThanhLy WHERE masachkho = @MaSachkho)
--    BEGIN
--        -- Nếu tồn tại, cập nhật số lượng sách lỗi
--        UPDATE KhoSachThanhLy
--        SET soluongkhotl = soluongkhotl + @soluongkhotl
--        WHERE masachkho = @MaSachkho;
--   end   
--    ELSE
--    BEGIN
--        -- Nếu không tồn tại, thêm mới sách vào kho thanh lý
--        INSERT INTO KhosachThanhLy (MaSachkho, soluongkhotl)
--        VALUES (@MaSachkho, @soluongkhotl);
--    END
--	  -- Giảm số lượng sách trong bảng chính
--        UPDATE SACH
--        SET SoLuongHIENTAI = SoLuongHIENTAI - @soluongkhotl
--        WHERE MaSach = @MaSachkho;    
--END;


--/* CẬP NHẬT  PHIẾU THANH LÝ */
--CREATE OR ALTER  TRIGGER trg_CapNhatGiaTL
--ON ChiTietPTL
--AFTER INSERT
--AS
--BEGIN
--    SET NOCOUNT ON;

--    DECLARE @GiaSachMax MONEY; 
--    SET @GiaSachMax = (
--        SELECT TOP 1 GiaSach
--        FROM INSERTED i
--        INNER JOIN ChitietPN ctpn ON i.MaSachKho = ctpn.MaSach
--        ORDER BY ctpn.GiaSach ASC
--    )
  
--        UPDATE ChiTietPTl
--        SET GiaTL = @GiaSachMax * INSERTED.Soluongtl *0.3
--        FROM ChiTietPTL
--		INNER JOIN PhieuThanhLy ON ChiTietPTL.MaPTL = PhieuThanhLy.MaPTL
--        JOIN INSERTED ON ChiTietPTL.MaPTL = INSERTED.MaPTL 
--		where ChiTietPTL.MaSachkho = INSERTED.MaSachkho
--	END


--/* CẬP NHẬT TÌNH TRẠNG PHIẾU TRẢ */ 
--CREATE OR ALTER  TRIGGER trg_CapNhatPhuThu
--ON ChiTietPT
--AFTER INSERT
--AS
--BEGIN
--    SET NOCOUNT ON;

--    DECLARE @GiaSachMax MONEY; 
--    SET @GiaSachMax = (
--        SELECT TOP 1 GiaSach
--        FROM INSERTED i
--        INNER JOIN ChitietPN ctpn ON i.MaSach = ctpn.MaSach
--        ORDER BY ctpn.GiaSach DESC
--    );
   
--    UPDATE ChiTietPT
--    SET PhuThu = CASE
--        WHEN DATEDIFF(DAY, 
--            ISNULL((SELECT HanTra FROM PhieuMuon WHERE MaPM = (SELECT MaPM FROM INSERTED i join phieutra on  i.mapt = phieutra.mapt)), GETDATE()), 
--            ISNULL((SELECT NgayTra FROM PhieuTra WHERE MaPT = (SELECT MaPT FROM INSERTED)), GETDATE())
--        ) > 0
--        THEN DATEDIFF(DAY, ISNULL((SELECT HanTra FROM PhieuMuon WHERE MaPM = (SELECT MaPM FROM INSERTED i JOIN phieutra ON i.mapt = phieutra.mapt)), GETDATE()), ISNULL((SELECT NgayTra FROM PhieuTra WHERE MaPT = (SELECT MaPT FROM INSERTED)), GETDATE())) * 2000
--                + (@GiaSachMax * 0.5 * ChiTietPT.Soluongloi)
--        ELSE (@GiaSachMax * 0.5 * ChiTietPT.Soluongloi)
--    END
--    FROM ChiTietPT
--    INNER JOIN ChitietPM ON ChiTietPT.MaSach = ChitietPM.MaSach
--    INNER JOIN PhieuTra ON ChiTietPT.MaPT = PhieuTra.MaPT
--    INNER JOIN PhieuMuon ON PhieuTra.MaPM = PhieuMuon.MaPM
--    INNER JOIN INSERTED ON ChiTietPT.MaSach = INSERTED.MaSach
--        AND ChiTietPT.MaPT = INSERTED.MaPT; -- Thêm điều kiện join cho MaSach và MaPT
--END;

--GO
/* CẬP NHẬT SÁCH TRONG KHO SAU KHI CẬP NHẬT NHAP */
--CREATE TRIGGER TRG_CAPNHATNHAP ON CHITIETPN AFTER UPDATE AS
--BEGIN
--   UPDATE SACH SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI +
--	   (SELECT SOLUONGNHAP FROM INSERTED WHERE MASACH = SACH.MASACH) -
--	   (SELECT SOLUONGNHAP FROM DELETED WHERE MASACH = SACH.MASACH)
--   FROM SACH 
--   JOIN DELETED ON SACH.MASACH = DELETED.MASACH
--END
--UPDATE CHITIETPM SET SOLUONG = 1 WHERE MASACH = 18 AND MAPM = 5


--INSERT INTO CHITIETPM ( MAPM, MASACH, SOLUONG) VALUES ( 5, 18, 5);

/* CẬP NHẬT SÁCH TRONG KHO SAU KHI CẬP NHẬT MƯỢN */
--CREATE TRIGGER TRG_CAPNHATMUON ON CHITIETPM AFTER UPDATE AS
--BEGIN
--   UPDATE SACH SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI -
--	   (SELECT SOLUONGmuon FROM INSERTED WHERE MASACH = SACH.MASACH) +
--	   (SELECT SOLUONGmuon FROM DELETED WHERE MASACH = SACH.MASACH)
--   FROM SACH 
--   JOIN DELETED ON SACH.MASACH = DELETED.MASACH
--END
--UPDATE CHITIETPM SET SOLUONG = 1 WHERE MASACH = 18 AND MAPM = 5



/* CẬP NHẬT SÁCH TRONG KHO sach và kho thanh ly SAU KHI TRẢ  */
--CREATE or ALTER  TRIGGER TRG_SACHTRAvaTL ON CHITIETPT AFTER INSERT AS 
--BEGIN
--	UPDATE SACH
--	SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI + (
--		SELECT SOLUONGtra
--		FROM INSERTED
--		WHERE MASACH = SACH.MASACH
--	)
--	FROM SACH
--	JOIN INSERTED ON INSERTED.MASACH = SACH.MASACH
--END




--INSERT INTO CHITIETPT ( MAPT, MASACH, SOLUONG) VALUES ( 2, 18, 5);
		
--GO

/* CẬP NHẬT SÁCH TRONG KHO SAU KHI CẬP NHẬT TRẢ */
--CREATE TRIGGER TRG_CAPNHATTRA ON CHITIETPT AFTER UPDATE AS
--BEGIN
--   UPDATE SACH SET SACH.SOLUONGHIENTAI = SACH.SOLUONGHIENTAI +
--	   (SELECT SOLUONGtra FROM INSERTED WHERE MASACH = SACH.MASACH) -
--	   (SELECT SOLUONGtra FROM DELETED WHERE MASACH = SACH.MASACH)
--   FROM SACH 
--   JOIN DELETED ON SACH.MASACH = DELETED.MASACH
--END
--UPDATE CHITIETPT SET SOLUONG = 1 WHERE MASACH = 18 AND MAPT = 2



--INSERT INTO CHITIETPTL ( MAPTL, MASACH, SOLUONG) VALUES ( 1, 18, 5);


/* CẬP NHẬT SÁCH TRONG KHO SAU KHI CẬP NHẬT THANH LÍ */
--CREATE TRIGGER TRG_CAPNHATTL ON CHITIETPTL AFTER UPDATE AS
--BEGIN
--   UPDATE KhoSachThanhLy SET KhoSachThanhLy.soluongkhotl = KhoSachThanhLy.soluongkhotl -
--	   (SELECT Soluongtl FROM INSERTED WHERE MASACHKHO = KhoSachThanhLy.MASACHKHO) +
--	   (SELECT Soluongtl FROM DELETED WHERE MASACHKHO = KhoSachThanhLy.MASACHKHO)
--   FROM KhoSachThanhLy 
--   JOIN DELETED ON KhoSachThanhLy.MASACHKHO = DELETED.MASACHKHO
--END
--UPDATE CHITIETPTL SET SOLUONG = 8 WHERE MASACH = 18 AND MAPTL = 1


--------------********************************--------------


--insert into PhieuNhapSach ( NgayNhap,  MaNV, MaNCC) values ( '2023-10-15',  3,  1);
--insert into Sach ( TenSach, TheLoai, TacGia, NgonNgu, NXB, NamXB, GiaSach, SoLuong, MaPN) values (N'Thỏ Bảy Màu',N'Truyện thiếu nhi',N'HUỲNH THÁI NGỌC',N'Tiếng việt',N'Dân Trí', 2023, '55000', 20,4);
--insert into Sach ( TenSach, TheLoai, TacGia, NgonNgu, NXB, NamXB, GiaSach, SoLuong, MaPN) values (N'Thần Đồng Đất Việt 2 - Trí Nhớ Siêu Phàm',N'Truyện thiếu nhi',N'Nhiều Tác Giả',N'Tiếng việt',N'NXB Đại Học Sư Phạm', 2023, '100000', 30,4);
--insert into Sach ( TenSach, TheLoai, TacGia, NgonNgu, NXB, NamXB, GiaSach, SoLuong, MaPN) values (N'Mùa Hè CỦA HỒ LY',N'TIỂU THUYẾT',N'Nguyễn Nhật Ánh',N'Tiếng việt',N'NXB Trẻ', 2023, '50000', 20,4);

----------------+**************************************************************+----------------


