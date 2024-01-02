
USE QuanLyThuVien;
SELECT * FROM donvitl
SELECT * FROM PHIEUMUON
SELECT * FROM CHITIETPM
SELECT * FROM PHIEUTRA 
SELECT * FROM CHITIETPT 
SELECT * FROM SACH
SELECT * FROM KHOSACHTHANHLY
SELECT * FROM PhieuThanhLy
SELECT * FROM ChiTietPTL

SELECT * FROM login_DG
SELECT * FROM nhanvien

SELECT * FROM dkimuonsach-- WHERE TINHTRANG = 2
SELECT * FROM CHITIETDK
DELETE FROM LOGIN_NV WHERE MaNV = 3;

insert into PhieuMuon ( MaThe, NgayMuon, HanTra, MaNV, MADK) values ( 3,'2023-12-23', '2023-12-27', 2,7);--10

-- Them chi tiet phieu muon
insert into ChiTietPM ( MaPM, MaSach, Soluongmuon) values ( 15, 3, 4);
insert into ChiTietPM ( MaPM, MaSach, Soluongmuon) values ( 7, 14, 2);
insert into ChiTietDK ( MaDK, MaSach, Soluongmuon) values ( 15, 3, 4);


UPDATE dkimuonsach SET TINHTRANG = 0 WHERE  MADK = 9  and tinhtrang = 1




select chitietpn.masach,tensach, giasach from chitietpm join chitietpn 
on chitietpm.masach = chitietpn.masach  
join sach on chitietpm.masach = sach.masach  
group by chitietpn.masach,tensach, giasach
order by giasach desc
SELECT
    PM.MaPM,
    PM.MaThe,
    DG.HoTenDG AS 'HoTenDocGia',
    DG.SDT AS 'SDTDocGia',
    PM.NgayMuon,
    PM.HanTra,
	ctpm.masach
FROM
    PHIEUMUON PM
    JOIN CHITIETPM CTPM ON PM.MaPM = CTPM.MaPM
    JOIN DOCGIA DG ON PM.MaThe = DG.MaDG
WHERE NOT EXISTS (
    SELECT 1
    FROM
        CHITIETPT CTPT
        JOIN PHIEUTRA PT ON CTPT.MaPT = PT.MaPT
        JOIN DOCGIA DG_TRA ON PT.MaThe = DG_TRA.MaDG
    WHERE
        PM.MaThe = PT.MaThe
        AND CTPM.MaSach = CTPT.MaSach
        AND PM.MaPM = PT.MaPM
        AND CTPM.SoLuongMuon = (
            SELECT
                SUM(SoLuongTra + SoLuongLoi)
            FROM
                CHITIETPT
                JOIN PHIEUTRA ON CHITIETPT.MaPT = PHIEUTRA.MaPT
                JOIN DOCGIA ON PHIEUTRA.MaThe = DOCGIA.MaDG
            WHERE
                PM.MaThe = PHIEUTRA.MaThe
                AND CTPM.MaSach = CHITIETPT.MaSach
            GROUP BY
                PHIEUTRA.MaThe, -- Thêm cột này vào phần GROUP BY
                CHITIETPT.MaSach
        )
    )
GROUP BY
    PM.MaPM,
    PM.MaThe,
    DG.HoTenDG,
    DG.SDT,
    PM.NgayMuon,
    PM.HanTra,
	ctpm.masach;

	--********************************
	SELECT ChiTietPM.mapm ,ChiTietPM.MaSach,Soluongmuon ,sum(soluongtra+soluongloi) as setTinhTrang
	FROM ChiTietPM JOIN PHIEUTRA ON ChiTietPM.MaPM = PHIEUTRA.MaPm JOIN ChiTietPT ON PHIEUTRA.MAPT = ChiTietPT.MaPT
	WHERE ChiTietPM.MaSach = ChiTietPT.MaSach  and ChiTietPM.mapm = 1
	group by ChiTietPM.mapm ,ChiTietPM.MaSach,Soluongmuon
	having soluongmuon = sum(soluongtra+soluongloi) 


	--********************************

	--*******
	SELECT * FROM Phieutra WHERE MaPm = 1;
	SELECT * FROM ChiTietPt WHERE MaPt in(3,4,7);

	SELECT * FROM PhieuTra WHERE MaPt = 6;
	SELECT * FROM ChiTietPT WHERE MAPt IN (3,4)
	--******

	SELECT * FROM Phieutra  pt

	WHERE pt.MaPT = 1

		;
		select * from phieunhapsach --where masach =2 ;

select masach, giasach*30/100 as gia from chitietpn  --where masach =2 ;
SELECT * FROM PHIEUMUON-- where mapm = 1;
SELECT * FROM CHITIETPM --- where mapm = 1;
SELECT * FROM PHIEUTRA --where mapm = 1;
SELECT * FROM CHITIETPT --where mapt = 3;
SELECT * FROM KHOSACHTHANHLY
select * from chitietpn where masach = 2;
SELECT * FROM PhieuThanhLy
SELECT * FROM ChiTietPTL
select * from sach;
-- Them Phieu thanh ly
insert into PhieuThanhLy (MaDV, NgayTL,  MaNV) values (2, '2023-11-25',  3);

-- Them Chi tiet phieu thanh ly 
insert into ChiTietPTL (MaPTL, MaSachkho, Soluongtl) values (2, 2, 1);



		insert into PhieuNhapSach ( NgayNhap,  MaNV, MaNCC) values ( '2023-10-30',  3,  2);
INSERT INTO CHITIETPN( MaPN, MaSACH, GiaSach, SoLuongNHAP) VALUES(6, 2, 100000,20)

-- Them chi tiet phieu muon
		insert into PhieuMuon ( MaThe, NgayMuon, HanTra,  MaNV) values (6, '2023-11-18', '2023-12-16',2);
insert into ChiTietPM ( MaPM, MaSach, Soluongmuon) values ( 11, 10, 2);

 insert into phieutra(ngaytra,manv,mathe,mapm) values ('2023-11-21',2,6,11)

 insert into ChiTietPT(MaPT, MaSach, Soluongtra, Soluongloi, soluongmat) values(17,10,0,0,2)
  insert into ChiTietPT(MaPT, MaSach, Soluongtra, Soluongloi) values(9,20,1,0)
  --insert into ChiTietPT(MaPT, MaSach, Soluongtra, Soluongloi) values(6,4,1,0)


--so loai sách mượn
SELECT PM.mapm, count(pm.mapm) as loaisach
FROM PhieuMuon PM join chitietpm ctpm on PM.mapm  = ctpm.mapm 
WHERE PM.Tinhtrang = N'CHƯA TRẢ'
GROUP BY Pm.mapm
intersect
select mapm, count(mapm) as soluongls from 
--lấy sách trả roi
(SELECT PM.mapm , ctpm.masach, Soluongmuon
FROM PhieuMuon PM join chitietpm ctpm on PM.mapm  = ctpm.mapm 
WHERE PM.Tinhtrang = N'CHƯA TRẢ'
intersect
SELECT Pt.mapm,  ctpt.masach, SUM(ISNULL(ctpt.Soluongtra, 0) + ISNULL(ctpt.Soluongloi, 0)) AS soluongtra
FROM phieutra Pt
JOIN chitietpt ctpt ON Pt.mapt = ctpt.mapt
LEFT JOIN chitietpm ctpm ON Pt.mapm = ctpm.mapm AND ctpt.masach = ctpm.masach
GROUP BY Pt.mapm, ctpt.masach) as a
GROUP BY a.mapm;


SELECT PHIEUTRA.MAPT, PHIEUTRA.NGAYTRA, MAPM, MATHE, HOTENDG, DOCGIA.SDT , NHANVIEN.MANV--, HOTENNV
FROM PHIEUTRA 
	--JOIN CHITIETPT ON PHIEUTRA.MAPT = CHITIETPT.MAPT 
	JOIN DOCGIA ON DOCGIA.MADG = PHIEUTRA.MATHE 
	JOIN NHANVIEN ON NHANVIEN.MANV = PHIEUTRA.MANV


select mapm, GROUP_CONCAT(PT.MAPT ORDER BY PT.MAPT) AS AllMAPT,  PHIEUTRA.NGAYTRA,  MATHE, HOTENDG, DOCGIA.SDT,NHANVIEN.MANV
FROM PHIEUTRA 
	JOIN DOCGIA ON DOCGIA.MADG = PHIEUTRA.MATHE 
	JOIN NHANVIEN ON NHANVIEN.MANV = PHIEUTRA.MANV
group by mapm, mapt, PHIEUTRA.NGAYTRA,  MATHE, HOTENDG, DOCGIA.SDT,NHANVIEN.MANV
order by MaPM asc




SELECT 
    PM.MaPM, 
    STUFF((
        SELECT ',' + CONVERT(NVARCHAR, PT.MAPT)
        FROM PHIEUTRA PT
        WHERE PM.MaPM = PT.MaPM
        ORDER BY PT.MAPT
        FOR XML PATH('')), 1, 1, '') AS AllMAPT, 
    PT.NGAYTRA, 
    PM.MATHE, 
    DG.HOTENDG, 
    DG.SDT, 
    NV.MANV
FROM 
    PHIEUMUON PM
JOIN 
    PHIEUTRA PT ON PM.MaPM = PT.MaPM
JOIN 
    DOCGIA DG ON DG.MADG = PM.MATHE 
JOIN 
    NHANVIEN NV ON NV.MANV = PT.MANV
GROUP BY 
    PM.MaPM, PT.NGAYTRA, PM.MATHE, DG.HOTENDG, DG.SDT, NV.MANV
ORDER BY 
    PM.MaPM ASC;






--lấy sách chưa trả
SELECT PM.mapm, ctpm.masach, ctpm.Soluongmuon
FROM PhieuMuon PM
JOIN chitietpm ctpm ON PM.mapm = ctpm.mapm
LEFT JOIN (
  SELECT Pt.mapm, ctpt.masach
  FROM phieutra Pt
  JOIN chitietpt ctpt ON Pt.mapt = ctpt.mapt
) AS Tra ON PM.mapm = Tra.mapm AND ctpm.masach = Tra.masach
WHERE PM.Tinhtrang = N'CHƯA TRẢ' AND Tra.mapm IS NULL;
