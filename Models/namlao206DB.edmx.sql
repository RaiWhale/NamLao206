
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/06/2025 16:40:50
-- Generated from EDMX file: D:\Source\NamLao206\SourceCode\Models\namlao206DB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [namlao206_website];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Contracts__Creat__214BF109]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contracts] DROP CONSTRAINT [FK__Contracts__Creat__214BF109];
GO
IF OBJECT_ID(N'[dbo].[FK__Contracts__Modif__22401542]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contracts] DROP CONSTRAINT [FK__Contracts__Modif__22401542];
GO
IF OBJECT_ID(N'[dbo].[FK__DocumentT__Creat__2610A626]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentTypes] DROP CONSTRAINT [FK__DocumentT__Creat__2610A626];
GO
IF OBJECT_ID(N'[dbo].[FK__GiamSatTh__Contr__1C5231C2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiamSatThiCongs] DROP CONSTRAINT [FK__GiamSatTh__Contr__1C5231C2];
GO
IF OBJECT_ID(N'[dbo].[FK__GiamSatTh__Creat__4A4E069C]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiamSatThiCongs] DROP CONSTRAINT [FK__GiamSatTh__Creat__4A4E069C];
GO
IF OBJECT_ID(N'[dbo].[FK__GiamSatTh__DonVi__44952D46]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiamSatThiCongs] DROP CONSTRAINT [FK__GiamSatTh__DonVi__44952D46];
GO
IF OBJECT_ID(N'[dbo].[FK__GiamSatTh__Modif__4B422AD5]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiamSatThiCongs] DROP CONSTRAINT [FK__GiamSatTh__Modif__4B422AD5];
GO
IF OBJECT_ID(N'[dbo].[FK__GiamSatTh__Proje__2022C2A6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiamSatThiCongs] DROP CONSTRAINT [FK__GiamSatTh__Proje__2022C2A6];
GO
IF OBJECT_ID(N'[dbo].[FK__GiamSatTh__TinhT__2116E6DF]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiamSatThiCongs] DROP CONSTRAINT [FK__GiamSatTh__TinhT__2116E6DF];
GO
IF OBJECT_ID(N'[dbo].[FK__GroupPerm__Group__546180BB]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupPermissions] DROP CONSTRAINT [FK__GroupPerm__Group__546180BB];
GO
IF OBJECT_ID(N'[dbo].[FK__GroupPerm__Permi__5555A4F4]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupPermissions] DROP CONSTRAINT [FK__GroupPerm__Permi__5555A4F4];
GO
IF OBJECT_ID(N'[dbo].[FK__HoatDongN__DonVi__7BB05806]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoatDongNhanSu] DROP CONSTRAINT [FK__HoatDongN__DonVi__7BB05806];
GO
IF OBJECT_ID(N'[dbo].[FK__HoatDongNhanSu__Create__2F9A1060]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoatDongNhanSu] DROP CONSTRAINT [FK__HoatDongNhanSu__Create__2F9A1060];
GO
IF OBJECT_ID(N'[dbo].[FK__HoatDongNhanSu__Modifi__308E3499]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoatDongNhanSu] DROP CONSTRAINT [FK__HoatDongNhanSu__Modifi__308E3499];
GO
IF OBJECT_ID(N'[dbo].[FK__HoSoPhapL__Creat__36470DEF]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoSoPhapLys] DROP CONSTRAINT [FK__HoSoPhapL__Creat__36470DEF];
GO
IF OBJECT_ID(N'[dbo].[FK__HoSoPhapL__Docum__3552E9B6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoSoPhapLys] DROP CONSTRAINT [FK__HoSoPhapL__Docum__3552E9B6];
GO
IF OBJECT_ID(N'[dbo].[FK__HoSoPhapL__Modif__373B3228]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoSoPhapLys] DROP CONSTRAINT [FK__HoSoPhapL__Modif__373B3228];
GO
IF OBJECT_ID(N'[dbo].[FK__HoSoPhapL__Proje__345EC57D]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoSoPhapLys] DROP CONSTRAINT [FK__HoSoPhapL__Proje__345EC57D];
GO
IF OBJECT_ID(N'[dbo].[FK__KhaoSats__Create__40C49C62]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KhaoSats] DROP CONSTRAINT [FK__KhaoSats__Create__40C49C62];
GO
IF OBJECT_ID(N'[dbo].[FK__KhaoSats__DocumentType__25DB9BFC]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KhaoSats] DROP CONSTRAINT [FK__KhaoSats__DocumentType__25DB9BFC];
GO
IF OBJECT_ID(N'[dbo].[FK__KhaoSats__DonViK__3A179ED3]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KhaoSats] DROP CONSTRAINT [FK__KhaoSats__DonViK__3A179ED3];
GO
IF OBJECT_ID(N'[dbo].[FK__KhaoSats__Modifi__41B8C09B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KhaoSats] DROP CONSTRAINT [FK__KhaoSats__Modifi__41B8C09B];
GO
IF OBJECT_ID(N'[dbo].[FK__KhaoSats__Projec__29AC2CE0]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KhaoSats] DROP CONSTRAINT [FK__KhaoSats__Projec__29AC2CE0];
GO
IF OBJECT_ID(N'[dbo].[FK__KhaoSats__TinhTr__2AA05119]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KhaoSats] DROP CONSTRAINT [FK__KhaoSats__TinhTr__2AA05119];
GO
IF OBJECT_ID(N'[dbo].[FK__KhaoSats__UnitId__2B947552]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KhaoSats] DROP CONSTRAINT [FK__KhaoSats__UnitId__2B947552];
GO
IF OBJECT_ID(N'[dbo].[FK__NghiemThu__Creat__4F12BBB9]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NghiemThus] DROP CONSTRAINT [FK__NghiemThu__Creat__4F12BBB9];
GO
IF OBJECT_ID(N'[dbo].[FK__NghiemThu__Modif__5006DFF2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NghiemThus] DROP CONSTRAINT [FK__NghiemThu__Modif__5006DFF2];
GO
IF OBJECT_ID(N'[dbo].[FK__NghiemThu__Proje__2E70E1FD]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NghiemThus] DROP CONSTRAINT [FK__NghiemThu__Proje__2E70E1FD];
GO
IF OBJECT_ID(N'[dbo].[FK__Phases__CreateUs__12FDD1B2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Phases] DROP CONSTRAINT [FK__Phases__CreateUs__12FDD1B2];
GO
IF OBJECT_ID(N'[dbo].[FK__Phases__Modified__13F1F5EB]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Phases] DROP CONSTRAINT [FK__Phases__Modified__13F1F5EB];
GO
IF OBJECT_ID(N'[dbo].[FK__Projects__Create__2F9A1060]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK__Projects__Create__2F9A1060];
GO
IF OBJECT_ID(N'[dbo].[FK__Projects__DonViI__324172E1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK__Projects__DonViI__324172E1];
GO
IF OBJECT_ID(N'[dbo].[FK__Projects__Invest__2AD55B43]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK__Projects__Invest__2AD55B43];
GO
IF OBJECT_ID(N'[dbo].[FK__Projects__Modifi__308E3499]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK__Projects__Modifi__308E3499];
GO
IF OBJECT_ID(N'[dbo].[FK__Projects__TinhTr__351DDF8C]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK__Projects__TinhTr__351DDF8C];
GO
IF OBJECT_ID(N'[dbo].[FK__StatusPro__Creat__1C873BEC]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StatusProjects] DROP CONSTRAINT [FK__StatusPro__Creat__1C873BEC];
GO
IF OBJECT_ID(N'[dbo].[FK__StatusPro__Modif__1D7B6025]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StatusProjects] DROP CONSTRAINT [FK__StatusPro__Modif__1D7B6025];
GO
IF OBJECT_ID(N'[dbo].[FK__Suppliers__Creat__0E391C95]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [FK__Suppliers__Creat__0E391C95];
GO
IF OBJECT_ID(N'[dbo].[FK__Suppliers__Modif__0F2D40CE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [FK__Suppliers__Modif__0F2D40CE];
GO
IF OBJECT_ID(N'[dbo].[FK__Teams__Creat__0E391C95]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teams] DROP CONSTRAINT [FK__Teams__Creat__0E391C95];
GO
IF OBJECT_ID(N'[dbo].[FK__Teams__Modif__0F2D40CE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teams] DROP CONSTRAINT [FK__Teams__Modif__0F2D40CE];
GO
IF OBJECT_ID(N'[dbo].[FK__ThiCongs__Contr__1C5231C2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThiCongs] DROP CONSTRAINT [FK__ThiCongs__Contr__1C5231C2];
GO
IF OBJECT_ID(N'[dbo].[FK__ThiCongs__Creat__4A4E069C]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThiCongs] DROP CONSTRAINT [FK__ThiCongs__Creat__4A4E069C];
GO
IF OBJECT_ID(N'[dbo].[FK__ThiCongs__DonVi__44952D46]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThiCongs] DROP CONSTRAINT [FK__ThiCongs__DonVi__44952D46];
GO
IF OBJECT_ID(N'[dbo].[FK__ThiCongs__Modif__4B422AD5]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThiCongs] DROP CONSTRAINT [FK__ThiCongs__Modif__4B422AD5];
GO
IF OBJECT_ID(N'[dbo].[FK__ThiCongs__Projec__76EBA2E9]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThiCongs] DROP CONSTRAINT [FK__ThiCongs__Projec__76EBA2E9];
GO
IF OBJECT_ID(N'[dbo].[FK__ThiCongs__TinhTr__77DFC722]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThiCongs] DROP CONSTRAINT [FK__ThiCongs__TinhTr__77DFC722];
GO
IF OBJECT_ID(N'[dbo].[FK__Units__CreateUse__17C286CF]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Units] DROP CONSTRAINT [FK__Units__CreateUse__17C286CF];
GO
IF OBJECT_ID(N'[dbo].[FK__Units__ModifiedU__18B6AB08]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Units] DROP CONSTRAINT [FK__Units__ModifiedU__18B6AB08];
GO
IF OBJECT_ID(N'[dbo].[FK__UserPermi__Accou__592635D8]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserPermissionGroups] DROP CONSTRAINT [FK__UserPermi__Accou__592635D8];
GO
IF OBJECT_ID(N'[dbo].[FK__UserPermi__Group__5A1A5A11]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserPermissionGroups] DROP CONSTRAINT [FK__UserPermi__Group__5A1A5A11];
GO
IF OBJECT_ID(N'[dbo].[FK_Accounts_Employees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT [FK_Accounts_Employees];
GO
IF OBJECT_ID(N'[dbo].[FK_Accounts_Levels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT [FK_Accounts_Levels];
GO
IF OBJECT_ID(N'[dbo].[FK_Administrators_DM_PhongBans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Administrators] DROP CONSTRAINT [FK_Administrators_DM_PhongBans];
GO
IF OBJECT_ID(N'[dbo].[FK_Administrators_ToAdminLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Administrators] DROP CONSTRAINT [FK_Administrators_ToAdminLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_AdminLevelPermissions_ToAdminLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdminLevelPermissions] DROP CONSTRAINT [FK_AdminLevelPermissions_ToAdminLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_AlbumPictures_Album]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AlbumPictures] DROP CONSTRAINT [FK_AlbumPictures_Album];
GO
IF OBJECT_ID(N'[dbo].[FK_AlbumPictures_Chucvus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AlbumPictures] DROP CONSTRAINT [FK_AlbumPictures_Chucvus];
GO
IF OBJECT_ID(N'[dbo].[FK_Customers_Genders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_Customers_Genders];
GO
IF OBJECT_ID(N'[dbo].[FK_Customers_ToDM_Donvihanhchinhs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_Customers_ToDM_Donvihanhchinhs];
GO
IF OBJECT_ID(N'[dbo].[FK_DM_AddBangs_DonVis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DM_AddBangs] DROP CONSTRAINT [FK_DM_AddBangs_DonVis];
GO
IF OBJECT_ID(N'[dbo].[FK_DM_Nhanviens_ToChucvus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_DM_Nhanviens_ToChucvus];
GO
IF OBJECT_ID(N'[dbo].[FK_DM_Nhanviens_ToDM_Donvihanhchinhs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_DM_Nhanviens_ToDM_Donvihanhchinhs];
GO
IF OBJECT_ID(N'[dbo].[FK_DM_Nhanviens_ToDM_Hocvis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_DM_Nhanviens_ToDM_Hocvis];
GO
IF OBJECT_ID(N'[dbo].[FK_DM_Nhanviens_ToLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_DM_Nhanviens_ToLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_DM_Nhanviens_ToNghenghieps]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_DM_Nhanviens_ToNghenghieps];
GO
IF OBJECT_ID(N'[dbo].[FK_DM_PhongBans_DM_Donvis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DM_PhongBans] DROP CONSTRAINT [FK_DM_PhongBans_DM_Donvis];
GO
IF OBJECT_ID(N'[dbo].[FK_DM_PhongBans_ToDM_NhomPhongBans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DM_PhongBans] DROP CONSTRAINT [FK_DM_PhongBans_ToDM_NhomPhongBans];
GO
IF OBJECT_ID(N'[dbo].[FK_DM_PhongBans_ToPicture]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DM_PhongBans] DROP CONSTRAINT [FK_DM_PhongBans_ToPicture];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentTypes_Projects]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_DocumentTypes_Projects];
GO
IF OBJECT_ID(N'[dbo].[FK_Employees_Genders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_Genders];
GO
IF OBJECT_ID(N'[dbo].[FK_Employees_PhongBans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_PhongBans];
GO
IF OBJECT_ID(N'[dbo].[FK_GiamSatThiCongs_Units]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiamSatThiCongs] DROP CONSTRAINT [FK_GiamSatThiCongs_Units];
GO
IF OBJECT_ID(N'[dbo].[FK_HoatDongNhanSu_Employees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoatDongNhanSu] DROP CONSTRAINT [FK_HoatDongNhanSu_Employees];
GO
IF OBJECT_ID(N'[dbo].[FK_HoSoPhapLys_AddBangs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoSoPhapLys] DROP CONSTRAINT [FK_HoSoPhapLys_AddBangs];
GO
IF OBJECT_ID(N'[dbo].[FK_HoSoPhapLys_DonVis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoSoPhapLys] DROP CONSTRAINT [FK_HoSoPhapLys_DonVis];
GO
IF OBJECT_ID(N'[dbo].[FK_HoSoPhapLys_ThietBis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoSoPhapLys] DROP CONSTRAINT [FK_HoSoPhapLys_ThietBis];
GO
IF OBJECT_ID(N'[dbo].[FK_LevelPermissions_ToLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LevelPermissions] DROP CONSTRAINT [FK_LevelPermissions_ToLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_MenuItem_Parent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MenuItems] DROP CONSTRAINT [FK_MenuItem_Parent];
GO
IF OBJECT_ID(N'[dbo].[FK_MenuItems_Donvis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MenuItems] DROP CONSTRAINT [FK_MenuItems_Donvis];
GO
IF OBJECT_ID(N'[dbo].[FK_News_NewsPictures]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[News] DROP CONSTRAINT [FK_News_NewsPictures];
GO
IF OBJECT_ID(N'[dbo].[FK_News_ToAccounts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[News] DROP CONSTRAINT [FK_News_ToAccounts];
GO
IF OBJECT_ID(N'[dbo].[FK_News_ToSubMenus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[News] DROP CONSTRAINT [FK_News_ToSubMenus];
GO
IF OBJECT_ID(N'[dbo].[FK_News_ToTopics]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[News] DROP CONSTRAINT [FK_News_ToTopics];
GO
IF OBJECT_ID(N'[dbo].[FK_NghiemThus_Phases]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NghiemThus] DROP CONSTRAINT [FK_NghiemThus_Phases];
GO
IF OBJECT_ID(N'[dbo].[FK_NghiemThus_Units]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NghiemThus] DROP CONSTRAINT [FK_NghiemThus_Units];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_CLMu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_CLMu];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_DonVis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_DonVis];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_DonViTienTe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_DonViTienTe];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_KeToan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_KeToan];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_LoaiHoSo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_LoaiHoSo];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_LoaiThongKe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_LoaiThongKe];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_NguoiNhap]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_NguoiNhap];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_NguoiPheDuyet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_NguoiPheDuyet];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_NguoiSua]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_NguoiSua];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_NhapBanMu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_NhapBanMu];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_Suppliers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_Suppliers];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_Teams]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_Teams];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_TinhTrang]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_TinhTrang];
GO
IF OBJECT_ID(N'[dbo].[FK_NhapBanMu_TroLyKeHoach]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhapBanMu] DROP CONSTRAINT [FK_NhapBanMu_TroLyKeHoach];
GO
IF OBJECT_ID(N'[dbo].[FK_Pictures_ToDM_PhongBans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pictures] DROP CONSTRAINT [FK_Pictures_ToDM_PhongBans];
GO
IF OBJECT_ID(N'[dbo].[FK_SubMenu_ToTopics]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubMenus] DROP CONSTRAINT [FK_SubMenu_ToTopics];
GO
IF OBJECT_ID(N'[dbo].[FK_Suppliers_Donvis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [FK_Suppliers_Donvis];
GO
IF OBJECT_ID(N'[dbo].[FK_Teams_Donvis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teams] DROP CONSTRAINT [FK_Teams_Donvis];
GO
IF OBJECT_ID(N'[dbo].[FK_ThiCongs_Units]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThiCongs] DROP CONSTRAINT [FK_ThiCongs_Units];
GO
IF OBJECT_ID(N'[dbo].[FK_ThietBiXeMay_DonVi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThietBiXeMay] DROP CONSTRAINT [FK_ThietBiXeMay_DonVi];
GO
IF OBJECT_ID(N'[dbo].[FK_ThietBiXeMay_NguoiSuDung]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThietBiXeMay] DROP CONSTRAINT [FK_ThietBiXeMay_NguoiSuDung];
GO
IF OBJECT_ID(N'[dbo].[FK_TransportFiles_AccountModified]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransportFiles] DROP CONSTRAINT [FK_TransportFiles_AccountModified];
GO
IF OBJECT_ID(N'[dbo].[FK_TransportFiles_AccountsCreate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransportFiles] DROP CONSTRAINT [FK_TransportFiles_AccountsCreate];
GO
IF OBJECT_ID(N'[dbo].[FK_TransportFiles_AccountsPheDuyet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransportFiles] DROP CONSTRAINT [FK_TransportFiles_AccountsPheDuyet];
GO
IF OBJECT_ID(N'[dbo].[FK_TransportFileUrls_TransportFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransportFileUrls] DROP CONSTRAINT [FK_TransportFileUrls_TransportFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_Transports_TransportFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transports] DROP CONSTRAINT [FK_Transports_TransportFiles];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accounts];
GO
IF OBJECT_ID(N'[dbo].[Administrators]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Administrators];
GO
IF OBJECT_ID(N'[dbo].[AdminLevelPermissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdminLevelPermissions];
GO
IF OBJECT_ID(N'[dbo].[AdminLevels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdminLevels];
GO
IF OBJECT_ID(N'[dbo].[AlbumPictures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AlbumPictures];
GO
IF OBJECT_ID(N'[dbo].[Albums]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Albums];
GO
IF OBJECT_ID(N'[dbo].[Banners]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Banners];
GO
IF OBJECT_ID(N'[dbo].[CalrouselPictures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CalrouselPictures];
GO
IF OBJECT_ID(N'[dbo].[Contracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contracts];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[DM_AddBangs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_AddBangs];
GO
IF OBJECT_ID(N'[dbo].[DM_AdminListUpItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_AdminListUpItem];
GO
IF OBJECT_ID(N'[dbo].[DM_Chucvus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_Chucvus];
GO
IF OBJECT_ID(N'[dbo].[DM_Donvihanhchinhs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_Donvihanhchinhs];
GO
IF OBJECT_ID(N'[dbo].[DM_DonVis]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_DonVis];
GO
IF OBJECT_ID(N'[dbo].[DM_hinhthuckinhdoanh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_hinhthuckinhdoanh];
GO
IF OBJECT_ID(N'[dbo].[DM_Hocvis]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_Hocvis];
GO
IF OBJECT_ID(N'[dbo].[DM_Nghenghieps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_Nghenghieps];
GO
IF OBJECT_ID(N'[dbo].[DM_NhomPhongBans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_NhomPhongBans];
GO
IF OBJECT_ID(N'[dbo].[DM_PhongBans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DM_PhongBans];
GO
IF OBJECT_ID(N'[dbo].[DocumentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentTypes];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Feedbacks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Feedbacks];
GO
IF OBJECT_ID(N'[dbo].[Genders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Genders];
GO
IF OBJECT_ID(N'[dbo].[GiamSatThiCongs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GiamSatThiCongs];
GO
IF OBJECT_ID(N'[dbo].[GroupPermissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupPermissions];
GO
IF OBJECT_ID(N'[dbo].[HoatDongNhanSu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HoatDongNhanSu];
GO
IF OBJECT_ID(N'[dbo].[HoSoPhapLys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HoSoPhapLys];
GO
IF OBJECT_ID(N'[dbo].[KhaoSats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KhaoSats];
GO
IF OBJECT_ID(N'[dbo].[LevelPermissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LevelPermissions];
GO
IF OBJECT_ID(N'[dbo].[Levels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Levels];
GO
IF OBJECT_ID(N'[dbo].[LuuVet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LuuVet];
GO
IF OBJECT_ID(N'[dbo].[MaQuocGia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaQuocGia];
GO
IF OBJECT_ID(N'[dbo].[MenuItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MenuItems];
GO
IF OBJECT_ID(N'[dbo].[Nationalities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Nationalities];
GO
IF OBJECT_ID(N'[dbo].[News]', 'U') IS NOT NULL
    DROP TABLE [dbo].[News];
GO
IF OBJECT_ID(N'[dbo].[NewsPictures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NewsPictures];
GO
IF OBJECT_ID(N'[dbo].[NghiemThus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NghiemThus];
GO
IF OBJECT_ID(N'[dbo].[NhapBanMu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NhapBanMu];
GO
IF OBJECT_ID(N'[dbo].[PermissionGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PermissionGroups];
GO
IF OBJECT_ID(N'[dbo].[Permissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Permissions];
GO
IF OBJECT_ID(N'[dbo].[Phases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Phases];
GO
IF OBJECT_ID(N'[dbo].[Pictures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pictures];
GO
IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[StatusProjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StatusProjects];
GO
IF OBJECT_ID(N'[dbo].[SubMenus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubMenus];
GO
IF OBJECT_ID(N'[dbo].[Suppliers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Suppliers];
GO
IF OBJECT_ID(N'[dbo].[Teams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teams];
GO
IF OBJECT_ID(N'[dbo].[ThiCongs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThiCongs];
GO
IF OBJECT_ID(N'[dbo].[ThietBiXeMay]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThietBiXeMay];
GO
IF OBJECT_ID(N'[dbo].[Topics]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Topics];
GO
IF OBJECT_ID(N'[dbo].[TransportFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransportFiles];
GO
IF OBJECT_ID(N'[dbo].[TransportFileUrls]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransportFileUrls];
GO
IF OBJECT_ID(N'[dbo].[Transports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transports];
GO
IF OBJECT_ID(N'[dbo].[Units]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Units];
GO
IF OBJECT_ID(N'[dbo].[UserPermissionGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserPermissionGroups];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LevelId] int  NOT NULL,
    [EmployeeId] int  NOT NULL,
    [LoginName] nvarchar(100)  NOT NULL,
    [Password] varchar(100)  NOT NULL,
    [ActivateCode] varchar(6)  NULL,
    [Coso] varchar(50)  NULL,
    [Token] varchar(100)  NULL,
    [CheckDate] datetime  NULL,
    [IsActive] bit  NOT NULL,
    [AccountType] int  NULL
);
GO

-- Creating table 'Administrators'
CREATE TABLE [dbo].[Administrators] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdminName] nvarchar(50)  NOT NULL,
    [LoginName] nvarchar(50)  NOT NULL,
    [Password] varchar(100)  NOT NULL,
    [Email] varchar(250)  NULL,
    [AdminLevelId] int  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Profile] nvarchar(max)  NULL,
    [PhongBanId] int  NOT NULL
);
GO

-- Creating table 'AdminLevelPermissions'
CREATE TABLE [dbo].[AdminLevelPermissions] (
    [AdminLevelId] int  NOT NULL,
    [TableName] nvarchar(255)  NOT NULL,
    [Permission] int  NOT NULL
);
GO

-- Creating table 'AdminLevels'
CREATE TABLE [dbo].[AdminLevels] (
    [Id] int  NOT NULL,
    [AdminLevelName] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'AlbumPictures'
CREATE TABLE [dbo].[AlbumPictures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PictureName] nvarchar(max)  NOT NULL,
    [AlbumId] int  NOT NULL,
    [ten] nvarchar(255)  NULL,
    [mota] nvarchar(max)  NULL,
    [chucvuId] int  NULL
);
GO

-- Creating table 'Albums'
CREATE TABLE [dbo].[Albums] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AlbumName] nvarchar(50)  NOT NULL,
    [ParentId] int  NULL
);
GO

-- Creating table 'Banners'
CREATE TABLE [dbo].[Banners] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BannerName] nvarchar(max)  NOT NULL,
    [Url] varchar(max)  NULL,
    [isActive] bit  NOT NULL,
    [tenCTY] nvarchar(255)  NULL,
    [mota] nvarchar(max)  NULL
);
GO

-- Creating table 'CalrouselPictures'
CREATE TABLE [dbo].[CalrouselPictures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Url] varchar(150)  NULL
);
GO

-- Creating table 'Contracts'
CREATE TABLE [dbo].[Contracts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContractTypeName] nvarchar(255)  NOT NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Phone] nvarchar(50)  NOT NULL,
    [Email] nvarchar(150)  NULL,
    [Address] nvarchar(200)  NULL,
    [CityId] varchar(50)  NULL,
    [DistrictId] varchar(50)  NULL,
    [WardId] varchar(50)  NULL,
    [IsActive] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [Avatar] nvarchar(255)  NULL,
    [GenderId] int  NOT NULL,
    [Birthday] datetime  NULL
);
GO

-- Creating table 'DM_AddBangs'
CREATE TABLE [dbo].[DM_AddBangs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TenBang] nvarchar(255)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL,
    [DonVi_Id] int  NULL
);
GO

-- Creating table 'DM_AdminListUpItem'
CREATE TABLE [dbo].[DM_AdminListUpItem] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ItemName] nvarchar(100)  NOT NULL,
    [Name] nvarchar(100)  NULL,
    [NhomItemId] int  NULL
);
GO

-- Creating table 'DM_Chucvus'
CREATE TABLE [dbo].[DM_Chucvus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Chucvu] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'DM_Donvihanhchinhs'
CREATE TABLE [dbo].[DM_Donvihanhchinhs] (
    [Id] varchar(50)  NOT NULL,
    [Ten] nvarchar(200)  NOT NULL,
    [Ma] varchar(50)  NOT NULL,
    [ParentId] varchar(50)  NOT NULL,
    [CapId] varchar(50)  NOT NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'DM_DonVis'
CREATE TABLE [dbo].[DM_DonVis] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TenDonVi] nvarchar(100)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Parent_Id] int  NOT NULL,
    [CreateBranch] varchar(50)  NULL,
    [EditBranch] varchar(50)  NULL
);
GO

-- Creating table 'DM_hinhthuckinhdoanh'
CREATE TABLE [dbo].[DM_hinhthuckinhdoanh] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Hinhthuc] nvarchar(150)  NOT NULL,
    [mota] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'DM_Hocvis'
CREATE TABLE [dbo].[DM_Hocvis] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [tenHocvi] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'DM_Nghenghieps'
CREATE TABLE [dbo].[DM_Nghenghieps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nghenghiep] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'DM_NhomPhongBans'
CREATE TABLE [dbo].[DM_NhomPhongBans] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nhomkhoa] nvarchar(150)  NOT NULL,
    [ParentId] int  NULL
);
GO

-- Creating table 'DM_PhongBans'
CREATE TABLE [dbo].[DM_PhongBans] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TenKhoa] nvarchar(100)  NOT NULL,
    [ChucNang] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [NhomKhoaId] int  NULL,
    [PictureId] int  NULL,
    [CreateDate] datetime  NOT NULL,
    [donvi_Id] int  NULL
);
GO

-- Creating table 'DocumentTypes'
CREATE TABLE [dbo].[DocumentTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DocumentTypeName] nvarchar(255)  NOT NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [PhanLoai] nchar(10)  NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Phone] varchar(30)  NULL,
    [Email] nvarchar(150)  NULL,
    [Address] nvarchar(200)  NULL,
    [LevelId] int  NULL,
    [IsActive] bit  NOT NULL,
    [KhoaphongId] int  NULL,
    [NghenghiepId] int  NULL,
    [ChucvuId] int  NULL,
    [HocviId] int  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CityId] varchar(50)  NULL,
    [DistrictId] varchar(50)  NULL,
    [WardId] varchar(50)  NULL,
    [Avatar] nvarchar(255)  NULL,
    [GenderId] int  NOT NULL,
    [TrangThaiChuyen] bit  NOT NULL,
    [Canhan] bit  NULL,
    [Import] bit  NOT NULL,
    [GroupId] int  NULL,
    [Birthday] datetime  NOT NULL
);
GO

-- Creating table 'Feedbacks'
CREATE TABLE [dbo].[Feedbacks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FullName] nvarchar(250)  NOT NULL,
    [Phone] varchar(250)  NOT NULL,
    [Message] nvarchar(500)  NULL,
    [DateUp] datetime  NOT NULL,
    [IsClosed] tinyint  NOT NULL
);
GO

-- Creating table 'Genders'
CREATE TABLE [dbo].[Genders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GioiTinh] nvarchar(50)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [STT] varchar(50)  NULL
);
GO

-- Creating table 'GiamSatThiCongs'
CREATE TABLE [dbo].[GiamSatThiCongs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DonViGiamSatId] int  NULL,
    [ProjectID] int  NULL,
    [ContractID] int  NULL,
    [GiaTriHopDong] decimal(18,2)  NULL,
    [GiaTriPLHopDong] decimal(18,2)  NULL,
    [LandVolumeContract] decimal(18,2)  NULL,
    [WaterVolumeContract] decimal(18,2)  NULL,
    [LandVolumeNghiemThu] decimal(18,2)  NULL,
    [WaterVolumeNghiemThu] decimal(18,2)  NULL,
    [GiamSat] nvarchar(255)  NULL,
    [TinhTrangDuAn] int  NULL,
    [TinhTrangCongNo] bit  NOT NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL,
    [UnitId] int  NULL,
    [CongNo] decimal(18,2)  NULL,
    [SoPLHD] nvarchar(2000)  NULL
);
GO

-- Creating table 'HoatDongNhanSus'
CREATE TABLE [dbo].[HoatDongNhanSus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DonViId] int  NULL,
    [NguoiTruc_Id] int  NULL,
    [TongNhanSu] decimal(18,2)  NULL,
    [CongTac] decimal(18,2)  NULL,
    [NghiPhep] decimal(18,2)  NULL,
    [DiHoc] decimal(18,2)  NULL,
    [LyDoKhac] decimal(18,2)  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [IsActive] bit  NULL,
    [Note] nvarchar(max)  NULL
);
GO

-- Creating table 'HoSoPhapLys'
CREATE TABLE [dbo].[HoSoPhapLys] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [STT] varchar(50)  NULL,
    [ProjectID] int  NULL,
    [DocumentTypeId] int  NULL,
    [TenHoSo] nvarchar(max)  NULL,
    [AddBangId] int  NULL,
    [Url] nvarchar(1000)  NULL,
    [IsActive] bit  NOT NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL,
    [DonVi_Id] int  NULL,
    [NhapBanMu_Id] int  NULL,
    [ThietBi_Id] int  NULL
);
GO

-- Creating table 'KhaoSats'
CREATE TABLE [dbo].[KhaoSats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DonViKhaoSatId] int  NULL,
    [ProjectID] int  NULL,
    [ContractID] int  NULL,
    [GiaTriHopDong] decimal(18,2)  NULL,
    [GiaTriPLHopDong] decimal(18,2)  NULL,
    [LandArea] decimal(18,2)  NULL,
    [WaterArea] decimal(18,2)  NULL,
    [LandAreaKhaoSat] decimal(18,2)  NULL,
    [WaterAreaKhaoSat] decimal(18,2)  NULL,
    [UnitId] int  NULL,
    [GiaTriThamDinh] decimal(18,2)  NULL,
    [GiaTriDuToan] decimal(18,2)  NULL,
    [GiaTriKhaoSat] decimal(18,2)  NULL,
    [GiaTriDuToanPheDuyet] decimal(18,2)  NULL,
    [GiaTriKhaoSatPheDuyet] decimal(18,2)  NULL,
    [GiaTriThamDinhPheDuyet] decimal(18,2)  NULL,
    [KetQuaKhaoSat] nvarchar(max)  NULL,
    [ChiPhiGiamSat] decimal(18,2)  NULL,
    [NguoiGiamSat] nvarchar(255)  NULL,
    [TinhTrangDuAn] int  NULL,
    [TinhTrangCongNo] bit  NOT NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL,
    [SoPLHD] nvarchar(2000)  NULL,
    [CongNo] decimal(18,2)  NULL
);
GO

-- Creating table 'LevelPermissions'
CREATE TABLE [dbo].[LevelPermissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LevelId] int  NOT NULL,
    [TableName] nvarchar(50)  NOT NULL,
    [DisplayName] nvarchar(100)  NOT NULL,
    [PermissionCode] int  NOT NULL
);
GO

-- Creating table 'Levels'
CREATE TABLE [dbo].[Levels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LevelName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'LuuVets'
CREATE TABLE [dbo].[LuuVets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdApi] int  NULL,
    [EmployeeCheckinId] int  NULL,
    [KhoaCheckinId] int  NULL,
    [thoiGianCheckin] varchar(150)  NULL,
    [KhuPhanLuongId] int  NULL,
    [ticketNumber] varchar(100)  NULL,
    [qrQgCode] varchar(200)  NULL,
    [qrQgCode2] varchar(200)  NULL
);
GO

-- Creating table 'MaQuocGias'
CREATE TABLE [dbo].[MaQuocGias] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Ma] varchar(50)  NOT NULL,
    [idQuocgia] varchar(50)  NOT NULL,
    [ten] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'Nationalities'
CREATE TABLE [dbo].[Nationalities] (
    [Id] varchar(50)  NOT NULL,
    [QuocTich] nvarchar(100)  NULL,
    [Ma] varchar(100)  NULL,
    [tenKhac] nvarchar(max)  NULL
);
GO

-- Creating table 'News'
CREATE TABLE [dbo].[News] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Summary] nvarchar(max)  NULL,
    [Details] nvarchar(max)  NOT NULL,
    [TopicId] int  NOT NULL,
    [SubMenuId] int  NULL,
    [DateUp] datetime  NOT NULL,
    [AdminId] int  NULL,
    [Views] int  NOT NULL,
    [Picture] varchar(max)  NULL,
    [DateModified] datetime  NULL,
    [cosoId] int  NULL,
    [catid] int  NULL,
    [uutien] bit  NOT NULL,
    [Duyet] bit  NULL,
    [TitleChange] nvarchar(max)  NULL,
    [Author] nvarchar(50)  NULL
);
GO

-- Creating table 'NewsPictures'
CREATE TABLE [dbo].[NewsPictures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Url] varchar(150)  NULL
);
GO

-- Creating table 'NghiemThus'
CREATE TABLE [dbo].[NghiemThus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectID] int  NULL,
    [PhaseId] int  NULL,
    [PhaseValue] decimal(18,2)  NULL,
    [PhaseLand] decimal(18,2)  NULL,
    [PhaseWater] decimal(18,2)  NULL,
    [DoanhThu] decimal(18,2)  NULL,
    [DaThanhToan] decimal(18,2)  NULL,
    [NoConLai] decimal(18,2)  NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL,
    [UnitId] int  NULL,
    [CongNo] decimal(18,2)  NULL
);
GO

-- Creating table 'NhapBanMus'
CREATE TABLE [dbo].[NhapBanMus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DonVi_Id] int  NULL,
    [Team_Id] int  NULL,
    [LoaiMu] nvarchar(500)  NULL,
    [NguoiNhap_ACC_Id] int  NULL,
    [NgayNhap] datetime  NULL,
    [NguoiPheDuyet_EMP_Id] int  NULL,
    [TroLyKeHoach_EMP_Id] int  NULL,
    [KhoiLuongThuVao] decimal(18,2)  NULL,
    [KhoiLuongTTTC] decimal(18,2)  NULL,
    [KhoiLuongThuVaoLast] decimal(18,2)  NULL,
    [KhoiLuongTTTCLast] decimal(18,2)  NULL,
    [LoaiHs] int  NULL,
    [LoaiTK] int  NULL,
    [DonGia] decimal(18,2)  NULL,
    [Note] nvarchar(max)  NULL,
    [SoDienThoai] varchar(50)  NULL,
    [KeToan_EMP_Id] int  NULL,
    [KhoiLuongTTL] decimal(18,2)  NULL,
    [DanhGiaCLMu] int  NULL,
    [DoiTac_Id] int  NULL,
    [DonViTienTe_Id] int  NULL,
    [NguoiCan_EMP_Id] int  NULL,
    [MaPhieu] varchar(500)  NULL,
    [TenPhieu] nvarchar(500)  NULL,
    [IsActive] bit  NULL,
    [ModifiedAccount_Id] int  NULL,
    [ModifiedDate] datetime  NULL,
    [TinhTrang] int  NULL
);
GO

-- Creating table 'Phases'
CREATE TABLE [dbo].[Phases] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PhaseName] nvarchar(255)  NOT NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL
);
GO

-- Creating table 'Pictures'
CREATE TABLE [dbo].[Pictures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Url] varchar(150)  NULL,
    [KhoaphongId] int  NOT NULL
);
GO

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TenDuAn] nvarchar(255)  NOT NULL,
    [MaDuAn] nvarchar(50)  NOT NULL,
    [DonViId] int  NULL,
    [XaId] int  NULL,
    [HuyenId] int  NULL,
    [TinhId] int  NULL,
    [InvestorId] int  NULL,
    [ProjectValue] decimal(18,2)  NULL,
    [GiamSat] nvarchar(255)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [TinhTrangDuAn] int  NULL,
    [TinhTrangCongNo] bit  NOT NULL,
    [ContractId] int  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [IsActive] bit  NULL,
    [Note] nvarchar(max)  NULL,
    [Longtitude] decimal(9,6)  NULL,
    [Latitude] decimal(9,6)  NULL,
    [DiaChi] nvarchar(2000)  NULL,
    [CongNo] decimal(18,2)  NULL
);
GO

-- Creating table 'StatusProjects'
CREATE TABLE [dbo].[StatusProjects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StatusName] nvarchar(255)  NOT NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL,
    [PhanLoai] nchar(10)  NULL
);
GO

-- Creating table 'SubMenus'
CREATE TABLE [dbo].[SubMenus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [subMenuName] nvarchar(200)  NOT NULL,
    [ParentId] int  NULL,
    [TopicId] int  NOT NULL,
    [UserId] int  NULL
);
GO

-- Creating table 'Suppliers'
CREATE TABLE [dbo].[Suppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierName] nvarchar(255)  NOT NULL,
    [Address] nvarchar(255)  NULL,
    [Phone] varchar(15)  NULL,
    [NguoiDaiDien] nvarchar(50)  NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [DonviId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL
);
GO

-- Creating table 'Teams'
CREATE TABLE [dbo].[Teams] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TeamName] nvarchar(255)  NOT NULL,
    [NguoiDaiDien] nvarchar(50)  NULL,
    [SoLuongNguoi] varchar(50)  NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [DonviId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL
);
GO

-- Creating table 'ThiCongs'
CREATE TABLE [dbo].[ThiCongs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DonViThiCongId] int  NULL,
    [ProjectID] int  NULL,
    [ContractID] int  NULL,
    [GiaTriHopDong] decimal(18,2)  NULL,
    [GiaTriPLHopDong] decimal(18,2)  NULL,
    [LandVolumeContract] decimal(18,2)  NULL,
    [WaterVolumeContract] decimal(18,2)  NULL,
    [LandVolumeNghiemThu] decimal(18,2)  NULL,
    [WaterVolumeNghiemThu] decimal(18,2)  NULL,
    [GiamSat] nvarchar(255)  NULL,
    [TinhTrangDuAn] int  NULL,
    [TinhTrangCongNo] bit  NOT NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL,
    [UnitId] int  NULL,
    [CongNo] decimal(18,2)  NULL,
    [SoPLHD] nvarchar(2000)  NULL
);
GO

-- Creating table 'Topics'
CREATE TABLE [dbo].[Topics] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TopicName] nvarchar(50)  NOT NULL,
    [PictureId] int  NULL,
    [NhomNews] tinyint  NULL,
    [ParentId] int  NULL,
    [HienThiNhom] bit  NOT NULL
);
GO

-- Creating table 'TransportFiles'
CREATE TABLE [dbo].[TransportFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [tenFile] nvarchar(255)  NOT NULL,
    [url] nvarchar(max)  NULL,
    [CreateDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL,
    [CreateUserId] int  NOT NULL,
    [ModifiedUserId] int  NULL,
    [IsActive] bit  NOT NULL,
    [KhanCap] bit  NOT NULL,
    [Mota] nvarchar(max)  NULL,
    [NgayBanHanh] datetime  NULL,
    [SoTrang] int  NULL,
    [NguoiPheDuyetId] int  NULL,
    [DoMat] nvarchar(50)  NULL,
    [PheDuyet] bit  NULL
);
GO

-- Creating table 'TransportFileUrls'
CREATE TABLE [dbo].[TransportFileUrls] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Url] nvarchar(max)  NOT NULL,
    [TransportFilesId] int  NOT NULL
);
GO

-- Creating table 'Transports'
CREATE TABLE [dbo].[Transports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FileId] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [TransportDate] datetime  NOT NULL,
    [ReceiverUserId] int  NULL,
    [ReceiveGroups] varchar(50)  NULL,
    [ReceiveUnit] varbinary(50)  NULL,
    [ModifiedDate] datetime  NULL,
    [DaXem] bit  NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'Units'
CREATE TABLE [dbo].[Units] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UnitName] nvarchar(255)  NOT NULL,
    [IsActive] bit  NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedUserId] int  NULL,
    [Note] nvarchar(max)  NULL,
    [PhanLoai] nchar(10)  NULL
);
GO

-- Creating table 'GroupPermissions'
CREATE TABLE [dbo].[GroupPermissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupId] int  NOT NULL,
    [PermissionId] int  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL
);
GO

-- Creating table 'UserPermissionGroups'
CREATE TABLE [dbo].[UserPermissionGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountId] int  NOT NULL,
    [GroupId] int  NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL
);
GO

-- Creating table 'PermissionGroups'
CREATE TABLE [dbo].[PermissionGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupName] nvarchar(100)  NOT NULL,
    [Description] nvarchar(255)  NULL,
    [IsActive] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL
);
GO

-- Creating table 'Permissions'
CREATE TABLE [dbo].[Permissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PermissionName] nvarchar(100)  NOT NULL,
    [PermissionCode] nvarchar(50)  NOT NULL,
    [Description] nvarchar(255)  NULL,
    [Module] nvarchar(50)  NULL,
    [IsActive] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL
);
GO

-- Creating table 'MenuItems'
CREATE TABLE [dbo].[MenuItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MenuName] nvarchar(100)  NOT NULL,
    [MenuUrl] nvarchar(255)  NULL,
    [ParentId] int  NULL,
    [IconClass] nvarchar(50)  NULL,
    [PermissionCode] nvarchar(50)  NULL,
    [DisplayOrder] int  NULL,
    [IsActive] bit  NOT NULL,
    [DepartmentId] int  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'ThietBiXeMays'
CREATE TABLE [dbo].[ThietBiXeMays] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DonVi_Id] int  NOT NULL,
    [NgayNhap] datetime  NULL,
    [LoaiTrangThietBi] nvarchar(255)  NULL,
    [BienSo] nvarchar(50)  NULL,
    [NhanHieu] nvarchar(255)  NULL,
    [SoKhung] nvarchar(100)  NULL,
    [SoMay] nvarchar(100)  NULL,
    [NamSanXuat] varchar(50)  NULL,
    [XuatXu] nvarchar(100)  NULL,
    [NguoiSuDung_Id] int  NULL,
    [TinhTrangKyThuat] nvarchar(max)  NULL,
    [HoSoPhapLy_Id] int  NULL,
    [GhiChu] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL,
    [CreateUser_Id] int  NULL,
    [IsActive] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Administrators'
ALTER TABLE [dbo].[Administrators]
ADD CONSTRAINT [PK_Administrators]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AdminLevelId], [TableName] in table 'AdminLevelPermissions'
ALTER TABLE [dbo].[AdminLevelPermissions]
ADD CONSTRAINT [PK_AdminLevelPermissions]
    PRIMARY KEY CLUSTERED ([AdminLevelId], [TableName] ASC);
GO

-- Creating primary key on [Id] in table 'AdminLevels'
ALTER TABLE [dbo].[AdminLevels]
ADD CONSTRAINT [PK_AdminLevels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AlbumPictures'
ALTER TABLE [dbo].[AlbumPictures]
ADD CONSTRAINT [PK_AlbumPictures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Albums'
ALTER TABLE [dbo].[Albums]
ADD CONSTRAINT [PK_Albums]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Banners'
ALTER TABLE [dbo].[Banners]
ADD CONSTRAINT [PK_Banners]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CalrouselPictures'
ALTER TABLE [dbo].[CalrouselPictures]
ADD CONSTRAINT [PK_CalrouselPictures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [PK_Contracts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_AddBangs'
ALTER TABLE [dbo].[DM_AddBangs]
ADD CONSTRAINT [PK_DM_AddBangs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_AdminListUpItem'
ALTER TABLE [dbo].[DM_AdminListUpItem]
ADD CONSTRAINT [PK_DM_AdminListUpItem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_Chucvus'
ALTER TABLE [dbo].[DM_Chucvus]
ADD CONSTRAINT [PK_DM_Chucvus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_Donvihanhchinhs'
ALTER TABLE [dbo].[DM_Donvihanhchinhs]
ADD CONSTRAINT [PK_DM_Donvihanhchinhs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_DonVis'
ALTER TABLE [dbo].[DM_DonVis]
ADD CONSTRAINT [PK_DM_DonVis]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_hinhthuckinhdoanh'
ALTER TABLE [dbo].[DM_hinhthuckinhdoanh]
ADD CONSTRAINT [PK_DM_hinhthuckinhdoanh]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_Hocvis'
ALTER TABLE [dbo].[DM_Hocvis]
ADD CONSTRAINT [PK_DM_Hocvis]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_Nghenghieps'
ALTER TABLE [dbo].[DM_Nghenghieps]
ADD CONSTRAINT [PK_DM_Nghenghieps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_NhomPhongBans'
ALTER TABLE [dbo].[DM_NhomPhongBans]
ADD CONSTRAINT [PK_DM_NhomPhongBans]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DM_PhongBans'
ALTER TABLE [dbo].[DM_PhongBans]
ADD CONSTRAINT [PK_DM_PhongBans]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DocumentTypes'
ALTER TABLE [dbo].[DocumentTypes]
ADD CONSTRAINT [PK_DocumentTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Feedbacks'
ALTER TABLE [dbo].[Feedbacks]
ADD CONSTRAINT [PK_Feedbacks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Genders'
ALTER TABLE [dbo].[Genders]
ADD CONSTRAINT [PK_Genders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GiamSatThiCongs'
ALTER TABLE [dbo].[GiamSatThiCongs]
ADD CONSTRAINT [PK_GiamSatThiCongs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HoatDongNhanSus'
ALTER TABLE [dbo].[HoatDongNhanSus]
ADD CONSTRAINT [PK_HoatDongNhanSus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HoSoPhapLys'
ALTER TABLE [dbo].[HoSoPhapLys]
ADD CONSTRAINT [PK_HoSoPhapLys]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'KhaoSats'
ALTER TABLE [dbo].[KhaoSats]
ADD CONSTRAINT [PK_KhaoSats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LevelPermissions'
ALTER TABLE [dbo].[LevelPermissions]
ADD CONSTRAINT [PK_LevelPermissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Levels'
ALTER TABLE [dbo].[Levels]
ADD CONSTRAINT [PK_Levels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LuuVets'
ALTER TABLE [dbo].[LuuVets]
ADD CONSTRAINT [PK_LuuVets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaQuocGias'
ALTER TABLE [dbo].[MaQuocGias]
ADD CONSTRAINT [PK_MaQuocGias]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Nationalities'
ALTER TABLE [dbo].[Nationalities]
ADD CONSTRAINT [PK_Nationalities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [PK_News]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NewsPictures'
ALTER TABLE [dbo].[NewsPictures]
ADD CONSTRAINT [PK_NewsPictures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NghiemThus'
ALTER TABLE [dbo].[NghiemThus]
ADD CONSTRAINT [PK_NghiemThus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [PK_NhapBanMus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Phases'
ALTER TABLE [dbo].[Phases]
ADD CONSTRAINT [PK_Phases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pictures'
ALTER TABLE [dbo].[Pictures]
ADD CONSTRAINT [PK_Pictures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StatusProjects'
ALTER TABLE [dbo].[StatusProjects]
ADD CONSTRAINT [PK_StatusProjects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubMenus'
ALTER TABLE [dbo].[SubMenus]
ADD CONSTRAINT [PK_SubMenus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [PK_Suppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [PK_Teams]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ThiCongs'
ALTER TABLE [dbo].[ThiCongs]
ADD CONSTRAINT [PK_ThiCongs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Topics'
ALTER TABLE [dbo].[Topics]
ADD CONSTRAINT [PK_Topics]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransportFiles'
ALTER TABLE [dbo].[TransportFiles]
ADD CONSTRAINT [PK_TransportFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransportFileUrls'
ALTER TABLE [dbo].[TransportFileUrls]
ADD CONSTRAINT [PK_TransportFileUrls]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transports'
ALTER TABLE [dbo].[Transports]
ADD CONSTRAINT [PK_Transports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Units'
ALTER TABLE [dbo].[Units]
ADD CONSTRAINT [PK_Units]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GroupPermissions'
ALTER TABLE [dbo].[GroupPermissions]
ADD CONSTRAINT [PK_GroupPermissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserPermissionGroups'
ALTER TABLE [dbo].[UserPermissionGroups]
ADD CONSTRAINT [PK_UserPermissionGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PermissionGroups'
ALTER TABLE [dbo].[PermissionGroups]
ADD CONSTRAINT [PK_PermissionGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Permissions'
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT [PK_Permissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MenuItems'
ALTER TABLE [dbo].[MenuItems]
ADD CONSTRAINT [PK_MenuItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ThietBiXeMays'
ALTER TABLE [dbo].[ThietBiXeMays]
ADD CONSTRAINT [PK_ThietBiXeMays]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CreateUserId] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [FK__Contracts__Creat__214BF109]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Contracts__Creat__214BF109'
CREATE INDEX [IX_FK__Contracts__Creat__214BF109]
ON [dbo].[Contracts]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [FK__Contracts__Modif__22401542]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Contracts__Modif__22401542'
CREATE INDEX [IX_FK__Contracts__Modif__22401542]
ON [dbo].[Contracts]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'DocumentTypes'
ALTER TABLE [dbo].[DocumentTypes]
ADD CONSTRAINT [FK__DocumentT__Creat__2610A626]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__DocumentT__Creat__2610A626'
CREATE INDEX [IX_FK__DocumentT__Creat__2610A626]
ON [dbo].[DocumentTypes]
    ([CreateUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'GiamSatThiCongs'
ALTER TABLE [dbo].[GiamSatThiCongs]
ADD CONSTRAINT [FK__GiamSatTh__Creat__4A4E069C]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__GiamSatTh__Creat__4A4E069C'
CREATE INDEX [IX_FK__GiamSatTh__Creat__4A4E069C]
ON [dbo].[GiamSatThiCongs]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'GiamSatThiCongs'
ALTER TABLE [dbo].[GiamSatThiCongs]
ADD CONSTRAINT [FK__GiamSatTh__Modif__4B422AD5]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__GiamSatTh__Modif__4B422AD5'
CREATE INDEX [IX_FK__GiamSatTh__Modif__4B422AD5]
ON [dbo].[GiamSatThiCongs]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'HoatDongNhanSus'
ALTER TABLE [dbo].[HoatDongNhanSus]
ADD CONSTRAINT [FK__HoatDongNhanSu__Create__2F9A1060]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__HoatDongNhanSu__Create__2F9A1060'
CREATE INDEX [IX_FK__HoatDongNhanSu__Create__2F9A1060]
ON [dbo].[HoatDongNhanSus]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'HoatDongNhanSus'
ALTER TABLE [dbo].[HoatDongNhanSus]
ADD CONSTRAINT [FK__HoatDongNhanSu__Modifi__308E3499]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__HoatDongNhanSu__Modifi__308E3499'
CREATE INDEX [IX_FK__HoatDongNhanSu__Modifi__308E3499]
ON [dbo].[HoatDongNhanSus]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'HoSoPhapLys'
ALTER TABLE [dbo].[HoSoPhapLys]
ADD CONSTRAINT [FK__HoSoPhapL__Creat__36470DEF]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__HoSoPhapL__Creat__36470DEF'
CREATE INDEX [IX_FK__HoSoPhapL__Creat__36470DEF]
ON [dbo].[HoSoPhapLys]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'HoSoPhapLys'
ALTER TABLE [dbo].[HoSoPhapLys]
ADD CONSTRAINT [FK__HoSoPhapL__Modif__373B3228]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__HoSoPhapL__Modif__373B3228'
CREATE INDEX [IX_FK__HoSoPhapL__Modif__373B3228]
ON [dbo].[HoSoPhapLys]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'KhaoSats'
ALTER TABLE [dbo].[KhaoSats]
ADD CONSTRAINT [FK__KhaoSats__Create__40C49C62]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__KhaoSats__Create__40C49C62'
CREATE INDEX [IX_FK__KhaoSats__Create__40C49C62]
ON [dbo].[KhaoSats]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'KhaoSats'
ALTER TABLE [dbo].[KhaoSats]
ADD CONSTRAINT [FK__KhaoSats__Modifi__41B8C09B]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__KhaoSats__Modifi__41B8C09B'
CREATE INDEX [IX_FK__KhaoSats__Modifi__41B8C09B]
ON [dbo].[KhaoSats]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'NghiemThus'
ALTER TABLE [dbo].[NghiemThus]
ADD CONSTRAINT [FK__NghiemThu__Creat__4F12BBB9]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__NghiemThu__Creat__4F12BBB9'
CREATE INDEX [IX_FK__NghiemThu__Creat__4F12BBB9]
ON [dbo].[NghiemThus]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'NghiemThus'
ALTER TABLE [dbo].[NghiemThus]
ADD CONSTRAINT [FK__NghiemThu__Modif__5006DFF2]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__NghiemThu__Modif__5006DFF2'
CREATE INDEX [IX_FK__NghiemThu__Modif__5006DFF2]
ON [dbo].[NghiemThus]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'Phases'
ALTER TABLE [dbo].[Phases]
ADD CONSTRAINT [FK__Phases__CreateUs__12FDD1B2]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Phases__CreateUs__12FDD1B2'
CREATE INDEX [IX_FK__Phases__CreateUs__12FDD1B2]
ON [dbo].[Phases]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'Phases'
ALTER TABLE [dbo].[Phases]
ADD CONSTRAINT [FK__Phases__Modified__13F1F5EB]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Phases__Modified__13F1F5EB'
CREATE INDEX [IX_FK__Phases__Modified__13F1F5EB]
ON [dbo].[Phases]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK__Projects__Create__2F9A1060]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Projects__Create__2F9A1060'
CREATE INDEX [IX_FK__Projects__Create__2F9A1060]
ON [dbo].[Projects]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK__Projects__Modifi__308E3499]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Projects__Modifi__308E3499'
CREATE INDEX [IX_FK__Projects__Modifi__308E3499]
ON [dbo].[Projects]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'StatusProjects'
ALTER TABLE [dbo].[StatusProjects]
ADD CONSTRAINT [FK__StatusPro__Creat__1C873BEC]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__StatusPro__Creat__1C873BEC'
CREATE INDEX [IX_FK__StatusPro__Creat__1C873BEC]
ON [dbo].[StatusProjects]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'StatusProjects'
ALTER TABLE [dbo].[StatusProjects]
ADD CONSTRAINT [FK__StatusPro__Modif__1D7B6025]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__StatusPro__Modif__1D7B6025'
CREATE INDEX [IX_FK__StatusPro__Modif__1D7B6025]
ON [dbo].[StatusProjects]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [FK__Suppliers__Creat__0E391C95]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Suppliers__Creat__0E391C95'
CREATE INDEX [IX_FK__Suppliers__Creat__0E391C95]
ON [dbo].[Suppliers]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [FK__Suppliers__Modif__0F2D40CE]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Suppliers__Modif__0F2D40CE'
CREATE INDEX [IX_FK__Suppliers__Modif__0F2D40CE]
ON [dbo].[Suppliers]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [FK__Teams__Creat__0E391C95]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Teams__Creat__0E391C95'
CREATE INDEX [IX_FK__Teams__Creat__0E391C95]
ON [dbo].[Teams]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [FK__Teams__Modif__0F2D40CE]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Teams__Modif__0F2D40CE'
CREATE INDEX [IX_FK__Teams__Modif__0F2D40CE]
ON [dbo].[Teams]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'ThiCongs'
ALTER TABLE [dbo].[ThiCongs]
ADD CONSTRAINT [FK__ThiCongs__Creat__4A4E069C]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ThiCongs__Creat__4A4E069C'
CREATE INDEX [IX_FK__ThiCongs__Creat__4A4E069C]
ON [dbo].[ThiCongs]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'ThiCongs'
ALTER TABLE [dbo].[ThiCongs]
ADD CONSTRAINT [FK__ThiCongs__Modif__4B422AD5]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ThiCongs__Modif__4B422AD5'
CREATE INDEX [IX_FK__ThiCongs__Modif__4B422AD5]
ON [dbo].[ThiCongs]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'Units'
ALTER TABLE [dbo].[Units]
ADD CONSTRAINT [FK__Units__CreateUse__17C286CF]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Units__CreateUse__17C286CF'
CREATE INDEX [IX_FK__Units__CreateUse__17C286CF]
ON [dbo].[Units]
    ([CreateUserId]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'Units'
ALTER TABLE [dbo].[Units]
ADD CONSTRAINT [FK__Units__ModifiedU__18B6AB08]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Units__ModifiedU__18B6AB08'
CREATE INDEX [IX_FK__Units__ModifiedU__18B6AB08]
ON [dbo].[Units]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [EmployeeId] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_Accounts_Employees]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Accounts_Employees'
CREATE INDEX [IX_FK_Accounts_Employees]
ON [dbo].[Accounts]
    ([EmployeeId]);
GO

-- Creating foreign key on [LevelId] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_Accounts_Levels]
    FOREIGN KEY ([LevelId])
    REFERENCES [dbo].[Levels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Accounts_Levels'
CREATE INDEX [IX_FK_Accounts_Levels]
ON [dbo].[Accounts]
    ([LevelId]);
GO

-- Creating foreign key on [AdminId] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [FK_News_ToAccounts]
    FOREIGN KEY ([AdminId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_News_ToAccounts'
CREATE INDEX [IX_FK_News_ToAccounts]
ON [dbo].[News]
    ([AdminId]);
GO

-- Creating foreign key on [NguoiNhap_ACC_Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_NguoiNhap]
    FOREIGN KEY ([NguoiNhap_ACC_Id])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_NguoiNhap'
CREATE INDEX [IX_FK_NhapBanMu_NguoiNhap]
ON [dbo].[NhapBanMus]
    ([NguoiNhap_ACC_Id]);
GO

-- Creating foreign key on [ModifiedUserId] in table 'TransportFiles'
ALTER TABLE [dbo].[TransportFiles]
ADD CONSTRAINT [FK_TransportFiles_AccountModified]
    FOREIGN KEY ([ModifiedUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransportFiles_AccountModified'
CREATE INDEX [IX_FK_TransportFiles_AccountModified]
ON [dbo].[TransportFiles]
    ([ModifiedUserId]);
GO

-- Creating foreign key on [CreateUserId] in table 'TransportFiles'
ALTER TABLE [dbo].[TransportFiles]
ADD CONSTRAINT [FK_TransportFiles_AccountsCreate]
    FOREIGN KEY ([CreateUserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransportFiles_AccountsCreate'
CREATE INDEX [IX_FK_TransportFiles_AccountsCreate]
ON [dbo].[TransportFiles]
    ([CreateUserId]);
GO

-- Creating foreign key on [NguoiPheDuyetId] in table 'TransportFiles'
ALTER TABLE [dbo].[TransportFiles]
ADD CONSTRAINT [FK_TransportFiles_AccountsPheDuyet]
    FOREIGN KEY ([NguoiPheDuyetId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransportFiles_AccountsPheDuyet'
CREATE INDEX [IX_FK_TransportFiles_AccountsPheDuyet]
ON [dbo].[TransportFiles]
    ([NguoiPheDuyetId]);
GO

-- Creating foreign key on [PhongBanId] in table 'Administrators'
ALTER TABLE [dbo].[Administrators]
ADD CONSTRAINT [FK_Administrators_DM_PhongBans]
    FOREIGN KEY ([PhongBanId])
    REFERENCES [dbo].[DM_PhongBans]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Administrators_DM_PhongBans'
CREATE INDEX [IX_FK_Administrators_DM_PhongBans]
ON [dbo].[Administrators]
    ([PhongBanId]);
GO

-- Creating foreign key on [AdminLevelId] in table 'Administrators'
ALTER TABLE [dbo].[Administrators]
ADD CONSTRAINT [FK_Administrators_ToAdminLevels]
    FOREIGN KEY ([AdminLevelId])
    REFERENCES [dbo].[AdminLevels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Administrators_ToAdminLevels'
CREATE INDEX [IX_FK_Administrators_ToAdminLevels]
ON [dbo].[Administrators]
    ([AdminLevelId]);
GO

-- Creating foreign key on [AdminLevelId] in table 'AdminLevelPermissions'
ALTER TABLE [dbo].[AdminLevelPermissions]
ADD CONSTRAINT [FK_AdminLevelPermissions_ToAdminLevels]
    FOREIGN KEY ([AdminLevelId])
    REFERENCES [dbo].[AdminLevels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AlbumId] in table 'AlbumPictures'
ALTER TABLE [dbo].[AlbumPictures]
ADD CONSTRAINT [FK_AlbumPictures_Album]
    FOREIGN KEY ([AlbumId])
    REFERENCES [dbo].[Albums]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AlbumPictures_Album'
CREATE INDEX [IX_FK_AlbumPictures_Album]
ON [dbo].[AlbumPictures]
    ([AlbumId]);
GO

-- Creating foreign key on [chucvuId] in table 'AlbumPictures'
ALTER TABLE [dbo].[AlbumPictures]
ADD CONSTRAINT [FK_AlbumPictures_Chucvus]
    FOREIGN KEY ([chucvuId])
    REFERENCES [dbo].[DM_Chucvus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AlbumPictures_Chucvus'
CREATE INDEX [IX_FK_AlbumPictures_Chucvus]
ON [dbo].[AlbumPictures]
    ([chucvuId]);
GO

-- Creating foreign key on [GenderId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_Customers_Genders]
    FOREIGN KEY ([GenderId])
    REFERENCES [dbo].[Genders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customers_Genders'
CREATE INDEX [IX_FK_Customers_Genders]
ON [dbo].[Customers]
    ([GenderId]);
GO

-- Creating foreign key on [CityId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_Customers_ToDM_Donvihanhchinhs]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[DM_Donvihanhchinhs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customers_ToDM_Donvihanhchinhs'
CREATE INDEX [IX_FK_Customers_ToDM_Donvihanhchinhs]
ON [dbo].[Customers]
    ([CityId]);
GO

-- Creating foreign key on [DonVi_Id] in table 'DM_AddBangs'
ALTER TABLE [dbo].[DM_AddBangs]
ADD CONSTRAINT [FK_DM_AddBangs_DonVis]
    FOREIGN KEY ([DonVi_Id])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DM_AddBangs_DonVis'
CREATE INDEX [IX_FK_DM_AddBangs_DonVis]
ON [dbo].[DM_AddBangs]
    ([DonVi_Id]);
GO

-- Creating foreign key on [AddBangId] in table 'HoSoPhapLys'
ALTER TABLE [dbo].[HoSoPhapLys]
ADD CONSTRAINT [FK_HoSoPhapLys_AddBangs]
    FOREIGN KEY ([AddBangId])
    REFERENCES [dbo].[DM_AddBangs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HoSoPhapLys_AddBangs'
CREATE INDEX [IX_FK_HoSoPhapLys_AddBangs]
ON [dbo].[HoSoPhapLys]
    ([AddBangId]);
GO

-- Creating foreign key on [ChucvuId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_DM_Nhanviens_ToChucvus]
    FOREIGN KEY ([ChucvuId])
    REFERENCES [dbo].[DM_Chucvus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DM_Nhanviens_ToChucvus'
CREATE INDEX [IX_FK_DM_Nhanviens_ToChucvus]
ON [dbo].[Employees]
    ([ChucvuId]);
GO

-- Creating foreign key on [CityId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_DM_Nhanviens_ToDM_Donvihanhchinhs]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[DM_Donvihanhchinhs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DM_Nhanviens_ToDM_Donvihanhchinhs'
CREATE INDEX [IX_FK_DM_Nhanviens_ToDM_Donvihanhchinhs]
ON [dbo].[Employees]
    ([CityId]);
GO

-- Creating foreign key on [DonViId] in table 'HoatDongNhanSus'
ALTER TABLE [dbo].[HoatDongNhanSus]
ADD CONSTRAINT [FK__HoatDongN__DonVi__7BB05806]
    FOREIGN KEY ([DonViId])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__HoatDongN__DonVi__7BB05806'
CREATE INDEX [IX_FK__HoatDongN__DonVi__7BB05806]
ON [dbo].[HoatDongNhanSus]
    ([DonViId]);
GO

-- Creating foreign key on [DonViId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK__Projects__DonViI__324172E1]
    FOREIGN KEY ([DonViId])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Projects__DonViI__324172E1'
CREATE INDEX [IX_FK__Projects__DonViI__324172E1]
ON [dbo].[Projects]
    ([DonViId]);
GO

-- Creating foreign key on [donvi_Id] in table 'DM_PhongBans'
ALTER TABLE [dbo].[DM_PhongBans]
ADD CONSTRAINT [FK_DM_PhongBans_DM_Donvis]
    FOREIGN KEY ([donvi_Id])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DM_PhongBans_DM_Donvis'
CREATE INDEX [IX_FK_DM_PhongBans_DM_Donvis]
ON [dbo].[DM_PhongBans]
    ([donvi_Id]);
GO

-- Creating foreign key on [DonVi_Id] in table 'HoSoPhapLys'
ALTER TABLE [dbo].[HoSoPhapLys]
ADD CONSTRAINT [FK_HoSoPhapLys_DonVis]
    FOREIGN KEY ([DonVi_Id])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HoSoPhapLys_DonVis'
CREATE INDEX [IX_FK_HoSoPhapLys_DonVis]
ON [dbo].[HoSoPhapLys]
    ([DonVi_Id]);
GO

-- Creating foreign key on [DonVi_Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_DonVis]
    FOREIGN KEY ([DonVi_Id])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_DonVis'
CREATE INDEX [IX_FK_NhapBanMu_DonVis]
ON [dbo].[NhapBanMus]
    ([DonVi_Id]);
GO

-- Creating foreign key on [DonviId] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [FK_Suppliers_Donvis]
    FOREIGN KEY ([DonviId])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Suppliers_Donvis'
CREATE INDEX [IX_FK_Suppliers_Donvis]
ON [dbo].[Suppliers]
    ([DonviId]);
GO

-- Creating foreign key on [DonviId] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [FK_Teams_Donvis]
    FOREIGN KEY ([DonviId])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Teams_Donvis'
CREATE INDEX [IX_FK_Teams_Donvis]
ON [dbo].[Teams]
    ([DonviId]);
GO

-- Creating foreign key on [HocviId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_DM_Nhanviens_ToDM_Hocvis]
    FOREIGN KEY ([HocviId])
    REFERENCES [dbo].[DM_Hocvis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DM_Nhanviens_ToDM_Hocvis'
CREATE INDEX [IX_FK_DM_Nhanviens_ToDM_Hocvis]
ON [dbo].[Employees]
    ([HocviId]);
GO

-- Creating foreign key on [NghenghiepId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_DM_Nhanviens_ToNghenghieps]
    FOREIGN KEY ([NghenghiepId])
    REFERENCES [dbo].[DM_Nghenghieps]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DM_Nhanviens_ToNghenghieps'
CREATE INDEX [IX_FK_DM_Nhanviens_ToNghenghieps]
ON [dbo].[Employees]
    ([NghenghiepId]);
GO

-- Creating foreign key on [NhomKhoaId] in table 'DM_PhongBans'
ALTER TABLE [dbo].[DM_PhongBans]
ADD CONSTRAINT [FK_DM_PhongBans_ToDM_NhomPhongBans]
    FOREIGN KEY ([NhomKhoaId])
    REFERENCES [dbo].[DM_NhomPhongBans]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DM_PhongBans_ToDM_NhomPhongBans'
CREATE INDEX [IX_FK_DM_PhongBans_ToDM_NhomPhongBans]
ON [dbo].[DM_PhongBans]
    ([NhomKhoaId]);
GO

-- Creating foreign key on [PictureId] in table 'DM_PhongBans'
ALTER TABLE [dbo].[DM_PhongBans]
ADD CONSTRAINT [FK_DM_PhongBans_ToPicture]
    FOREIGN KEY ([PictureId])
    REFERENCES [dbo].[Pictures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DM_PhongBans_ToPicture'
CREATE INDEX [IX_FK_DM_PhongBans_ToPicture]
ON [dbo].[DM_PhongBans]
    ([PictureId]);
GO

-- Creating foreign key on [KhoaphongId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_Employees_PhongBans]
    FOREIGN KEY ([KhoaphongId])
    REFERENCES [dbo].[DM_PhongBans]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employees_PhongBans'
CREATE INDEX [IX_FK_Employees_PhongBans]
ON [dbo].[Employees]
    ([KhoaphongId]);
GO

-- Creating foreign key on [KhoaphongId] in table 'Pictures'
ALTER TABLE [dbo].[Pictures]
ADD CONSTRAINT [FK_Pictures_ToDM_PhongBans]
    FOREIGN KEY ([KhoaphongId])
    REFERENCES [dbo].[DM_PhongBans]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Pictures_ToDM_PhongBans'
CREATE INDEX [IX_FK_Pictures_ToDM_PhongBans]
ON [dbo].[Pictures]
    ([KhoaphongId]);
GO

-- Creating foreign key on [ContractID] in table 'GiamSatThiCongs'
ALTER TABLE [dbo].[GiamSatThiCongs]
ADD CONSTRAINT [FK__GiamSatTh__Contr__1C5231C2]
    FOREIGN KEY ([ContractID])
    REFERENCES [dbo].[DocumentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__GiamSatTh__Contr__1C5231C2'
CREATE INDEX [IX_FK__GiamSatTh__Contr__1C5231C2]
ON [dbo].[GiamSatThiCongs]
    ([ContractID]);
GO

-- Creating foreign key on [DocumentTypeId] in table 'HoSoPhapLys'
ALTER TABLE [dbo].[HoSoPhapLys]
ADD CONSTRAINT [FK__HoSoPhapL__Docum__3552E9B6]
    FOREIGN KEY ([DocumentTypeId])
    REFERENCES [dbo].[DocumentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__HoSoPhapL__Docum__3552E9B6'
CREATE INDEX [IX_FK__HoSoPhapL__Docum__3552E9B6]
ON [dbo].[HoSoPhapLys]
    ([DocumentTypeId]);
GO

-- Creating foreign key on [ContractID] in table 'KhaoSats'
ALTER TABLE [dbo].[KhaoSats]
ADD CONSTRAINT [FK__KhaoSats__DocumentType__25DB9BFC]
    FOREIGN KEY ([ContractID])
    REFERENCES [dbo].[DocumentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__KhaoSats__DocumentType__25DB9BFC'
CREATE INDEX [IX_FK__KhaoSats__DocumentType__25DB9BFC]
ON [dbo].[KhaoSats]
    ([ContractID]);
GO

-- Creating foreign key on [ContractID] in table 'ThiCongs'
ALTER TABLE [dbo].[ThiCongs]
ADD CONSTRAINT [FK__ThiCongs__Contr__1C5231C2]
    FOREIGN KEY ([ContractID])
    REFERENCES [dbo].[DocumentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ThiCongs__Contr__1C5231C2'
CREATE INDEX [IX_FK__ThiCongs__Contr__1C5231C2]
ON [dbo].[ThiCongs]
    ([ContractID]);
GO

-- Creating foreign key on [ContractId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_DocumentTypes_Projects]
    FOREIGN KEY ([ContractId])
    REFERENCES [dbo].[DocumentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentTypes_Projects'
CREATE INDEX [IX_FK_DocumentTypes_Projects]
ON [dbo].[Projects]
    ([ContractId]);
GO

-- Creating foreign key on [LoaiHs] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_LoaiHoSo]
    FOREIGN KEY ([LoaiHs])
    REFERENCES [dbo].[DocumentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_LoaiHoSo'
CREATE INDEX [IX_FK_NhapBanMu_LoaiHoSo]
ON [dbo].[NhapBanMus]
    ([LoaiHs]);
GO

-- Creating foreign key on [LevelId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_DM_Nhanviens_ToLevels]
    FOREIGN KEY ([LevelId])
    REFERENCES [dbo].[Levels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DM_Nhanviens_ToLevels'
CREATE INDEX [IX_FK_DM_Nhanviens_ToLevels]
ON [dbo].[Employees]
    ([LevelId]);
GO

-- Creating foreign key on [GenderId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_Employees_Genders]
    FOREIGN KEY ([GenderId])
    REFERENCES [dbo].[Genders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employees_Genders'
CREATE INDEX [IX_FK_Employees_Genders]
ON [dbo].[Employees]
    ([GenderId]);
GO

-- Creating foreign key on [NguoiTruc_Id] in table 'HoatDongNhanSus'
ALTER TABLE [dbo].[HoatDongNhanSus]
ADD CONSTRAINT [FK_HoatDongNhanSu_Employees]
    FOREIGN KEY ([NguoiTruc_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HoatDongNhanSu_Employees'
CREATE INDEX [IX_FK_HoatDongNhanSu_Employees]
ON [dbo].[HoatDongNhanSus]
    ([NguoiTruc_Id]);
GO

-- Creating foreign key on [KeToan_EMP_Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_KeToan]
    FOREIGN KEY ([KeToan_EMP_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_KeToan'
CREATE INDEX [IX_FK_NhapBanMu_KeToan]
ON [dbo].[NhapBanMus]
    ([KeToan_EMP_Id]);
GO

-- Creating foreign key on [NguoiPheDuyet_EMP_Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_NguoiPheDuyet]
    FOREIGN KEY ([NguoiPheDuyet_EMP_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_NguoiPheDuyet'
CREATE INDEX [IX_FK_NhapBanMu_NguoiPheDuyet]
ON [dbo].[NhapBanMus]
    ([NguoiPheDuyet_EMP_Id]);
GO

-- Creating foreign key on [TroLyKeHoach_EMP_Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_TroLyKeHoach]
    FOREIGN KEY ([TroLyKeHoach_EMP_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_TroLyKeHoach'
CREATE INDEX [IX_FK_NhapBanMu_TroLyKeHoach]
ON [dbo].[NhapBanMus]
    ([TroLyKeHoach_EMP_Id]);
GO

-- Creating foreign key on [DonViGiamSatId] in table 'GiamSatThiCongs'
ALTER TABLE [dbo].[GiamSatThiCongs]
ADD CONSTRAINT [FK__GiamSatTh__DonVi__44952D46]
    FOREIGN KEY ([DonViGiamSatId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__GiamSatTh__DonVi__44952D46'
CREATE INDEX [IX_FK__GiamSatTh__DonVi__44952D46]
ON [dbo].[GiamSatThiCongs]
    ([DonViGiamSatId]);
GO

-- Creating foreign key on [ProjectID] in table 'GiamSatThiCongs'
ALTER TABLE [dbo].[GiamSatThiCongs]
ADD CONSTRAINT [FK__GiamSatTh__Proje__2022C2A6]
    FOREIGN KEY ([ProjectID])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__GiamSatTh__Proje__2022C2A6'
CREATE INDEX [IX_FK__GiamSatTh__Proje__2022C2A6]
ON [dbo].[GiamSatThiCongs]
    ([ProjectID]);
GO

-- Creating foreign key on [TinhTrangDuAn] in table 'GiamSatThiCongs'
ALTER TABLE [dbo].[GiamSatThiCongs]
ADD CONSTRAINT [FK__GiamSatTh__TinhT__2116E6DF]
    FOREIGN KEY ([TinhTrangDuAn])
    REFERENCES [dbo].[StatusProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__GiamSatTh__TinhT__2116E6DF'
CREATE INDEX [IX_FK__GiamSatTh__TinhT__2116E6DF]
ON [dbo].[GiamSatThiCongs]
    ([TinhTrangDuAn]);
GO

-- Creating foreign key on [UnitId] in table 'GiamSatThiCongs'
ALTER TABLE [dbo].[GiamSatThiCongs]
ADD CONSTRAINT [FK_GiamSatThiCongs_Units]
    FOREIGN KEY ([UnitId])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GiamSatThiCongs_Units'
CREATE INDEX [IX_FK_GiamSatThiCongs_Units]
ON [dbo].[GiamSatThiCongs]
    ([UnitId]);
GO

-- Creating foreign key on [ProjectID] in table 'HoSoPhapLys'
ALTER TABLE [dbo].[HoSoPhapLys]
ADD CONSTRAINT [FK__HoSoPhapL__Proje__345EC57D]
    FOREIGN KEY ([ProjectID])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__HoSoPhapL__Proje__345EC57D'
CREATE INDEX [IX_FK__HoSoPhapL__Proje__345EC57D]
ON [dbo].[HoSoPhapLys]
    ([ProjectID]);
GO

-- Creating foreign key on [DonViKhaoSatId] in table 'KhaoSats'
ALTER TABLE [dbo].[KhaoSats]
ADD CONSTRAINT [FK__KhaoSats__DonViK__3A179ED3]
    FOREIGN KEY ([DonViKhaoSatId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__KhaoSats__DonViK__3A179ED3'
CREATE INDEX [IX_FK__KhaoSats__DonViK__3A179ED3]
ON [dbo].[KhaoSats]
    ([DonViKhaoSatId]);
GO

-- Creating foreign key on [ProjectID] in table 'KhaoSats'
ALTER TABLE [dbo].[KhaoSats]
ADD CONSTRAINT [FK__KhaoSats__Projec__29AC2CE0]
    FOREIGN KEY ([ProjectID])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__KhaoSats__Projec__29AC2CE0'
CREATE INDEX [IX_FK__KhaoSats__Projec__29AC2CE0]
ON [dbo].[KhaoSats]
    ([ProjectID]);
GO

-- Creating foreign key on [TinhTrangDuAn] in table 'KhaoSats'
ALTER TABLE [dbo].[KhaoSats]
ADD CONSTRAINT [FK__KhaoSats__TinhTr__2AA05119]
    FOREIGN KEY ([TinhTrangDuAn])
    REFERENCES [dbo].[StatusProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__KhaoSats__TinhTr__2AA05119'
CREATE INDEX [IX_FK__KhaoSats__TinhTr__2AA05119]
ON [dbo].[KhaoSats]
    ([TinhTrangDuAn]);
GO

-- Creating foreign key on [UnitId] in table 'KhaoSats'
ALTER TABLE [dbo].[KhaoSats]
ADD CONSTRAINT [FK__KhaoSats__UnitId__2B947552]
    FOREIGN KEY ([UnitId])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__KhaoSats__UnitId__2B947552'
CREATE INDEX [IX_FK__KhaoSats__UnitId__2B947552]
ON [dbo].[KhaoSats]
    ([UnitId]);
GO

-- Creating foreign key on [LevelId] in table 'LevelPermissions'
ALTER TABLE [dbo].[LevelPermissions]
ADD CONSTRAINT [FK_LevelPermissions_ToLevels]
    FOREIGN KEY ([LevelId])
    REFERENCES [dbo].[Levels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LevelPermissions_ToLevels'
CREATE INDEX [IX_FK_LevelPermissions_ToLevels]
ON [dbo].[LevelPermissions]
    ([LevelId]);
GO

-- Creating foreign key on [Id] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [FK_News_NewsPictures]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[News]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SubMenuId] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [FK_News_ToSubMenus]
    FOREIGN KEY ([SubMenuId])
    REFERENCES [dbo].[SubMenus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_News_ToSubMenus'
CREATE INDEX [IX_FK_News_ToSubMenus]
ON [dbo].[News]
    ([SubMenuId]);
GO

-- Creating foreign key on [TopicId] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [FK_News_ToTopics]
    FOREIGN KEY ([TopicId])
    REFERENCES [dbo].[Topics]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_News_ToTopics'
CREATE INDEX [IX_FK_News_ToTopics]
ON [dbo].[News]
    ([TopicId]);
GO

-- Creating foreign key on [ProjectID] in table 'NghiemThus'
ALTER TABLE [dbo].[NghiemThus]
ADD CONSTRAINT [FK__NghiemThu__Proje__2E70E1FD]
    FOREIGN KEY ([ProjectID])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__NghiemThu__Proje__2E70E1FD'
CREATE INDEX [IX_FK__NghiemThu__Proje__2E70E1FD]
ON [dbo].[NghiemThus]
    ([ProjectID]);
GO

-- Creating foreign key on [PhaseId] in table 'NghiemThus'
ALTER TABLE [dbo].[NghiemThus]
ADD CONSTRAINT [FK_NghiemThus_Phases]
    FOREIGN KEY ([PhaseId])
    REFERENCES [dbo].[Phases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NghiemThus_Phases'
CREATE INDEX [IX_FK_NghiemThus_Phases]
ON [dbo].[NghiemThus]
    ([PhaseId]);
GO

-- Creating foreign key on [UnitId] in table 'NghiemThus'
ALTER TABLE [dbo].[NghiemThus]
ADD CONSTRAINT [FK_NghiemThus_Units]
    FOREIGN KEY ([UnitId])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NghiemThus_Units'
CREATE INDEX [IX_FK_NghiemThus_Units]
ON [dbo].[NghiemThus]
    ([UnitId]);
GO

-- Creating foreign key on [DanhGiaCLMu] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_CLMu]
    FOREIGN KEY ([DanhGiaCLMu])
    REFERENCES [dbo].[StatusProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_CLMu'
CREATE INDEX [IX_FK_NhapBanMu_CLMu]
ON [dbo].[NhapBanMus]
    ([DanhGiaCLMu]);
GO

-- Creating foreign key on [DonViTienTe_Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_DonViTienTe]
    FOREIGN KEY ([DonViTienTe_Id])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_DonViTienTe'
CREATE INDEX [IX_FK_NhapBanMu_DonViTienTe]
ON [dbo].[NhapBanMus]
    ([DonViTienTe_Id]);
GO

-- Creating foreign key on [LoaiTK] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_LoaiThongKe]
    FOREIGN KEY ([LoaiTK])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_LoaiThongKe'
CREATE INDEX [IX_FK_NhapBanMu_LoaiThongKe]
ON [dbo].[NhapBanMus]
    ([LoaiTK]);
GO

-- Creating foreign key on [Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_NhapBanMu]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[NhapBanMus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [DoiTac_Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_Suppliers]
    FOREIGN KEY ([DoiTac_Id])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_Suppliers'
CREATE INDEX [IX_FK_NhapBanMu_Suppliers]
ON [dbo].[NhapBanMus]
    ([DoiTac_Id]);
GO

-- Creating foreign key on [Team_Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_Teams]
    FOREIGN KEY ([Team_Id])
    REFERENCES [dbo].[Teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_Teams'
CREATE INDEX [IX_FK_NhapBanMu_Teams]
ON [dbo].[NhapBanMus]
    ([Team_Id]);
GO

-- Creating foreign key on [InvestorId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK__Projects__Invest__2AD55B43]
    FOREIGN KEY ([InvestorId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Projects__Invest__2AD55B43'
CREATE INDEX [IX_FK__Projects__Invest__2AD55B43]
ON [dbo].[Projects]
    ([InvestorId]);
GO

-- Creating foreign key on [TinhTrangDuAn] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK__Projects__TinhTr__351DDF8C]
    FOREIGN KEY ([TinhTrangDuAn])
    REFERENCES [dbo].[StatusProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Projects__TinhTr__351DDF8C'
CREATE INDEX [IX_FK__Projects__TinhTr__351DDF8C]
ON [dbo].[Projects]
    ([TinhTrangDuAn]);
GO

-- Creating foreign key on [ProjectID] in table 'ThiCongs'
ALTER TABLE [dbo].[ThiCongs]
ADD CONSTRAINT [FK__ThiCongs__Projec__76EBA2E9]
    FOREIGN KEY ([ProjectID])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ThiCongs__Projec__76EBA2E9'
CREATE INDEX [IX_FK__ThiCongs__Projec__76EBA2E9]
ON [dbo].[ThiCongs]
    ([ProjectID]);
GO

-- Creating foreign key on [TinhTrangDuAn] in table 'ThiCongs'
ALTER TABLE [dbo].[ThiCongs]
ADD CONSTRAINT [FK__ThiCongs__TinhTr__77DFC722]
    FOREIGN KEY ([TinhTrangDuAn])
    REFERENCES [dbo].[StatusProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ThiCongs__TinhTr__77DFC722'
CREATE INDEX [IX_FK__ThiCongs__TinhTr__77DFC722]
ON [dbo].[ThiCongs]
    ([TinhTrangDuAn]);
GO

-- Creating foreign key on [TopicId] in table 'SubMenus'
ALTER TABLE [dbo].[SubMenus]
ADD CONSTRAINT [FK_SubMenu_ToTopics]
    FOREIGN KEY ([TopicId])
    REFERENCES [dbo].[Topics]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubMenu_ToTopics'
CREATE INDEX [IX_FK_SubMenu_ToTopics]
ON [dbo].[SubMenus]
    ([TopicId]);
GO

-- Creating foreign key on [DonViThiCongId] in table 'ThiCongs'
ALTER TABLE [dbo].[ThiCongs]
ADD CONSTRAINT [FK__ThiCongs__DonVi__44952D46]
    FOREIGN KEY ([DonViThiCongId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ThiCongs__DonVi__44952D46'
CREATE INDEX [IX_FK__ThiCongs__DonVi__44952D46]
ON [dbo].[ThiCongs]
    ([DonViThiCongId]);
GO

-- Creating foreign key on [UnitId] in table 'ThiCongs'
ALTER TABLE [dbo].[ThiCongs]
ADD CONSTRAINT [FK_ThiCongs_Units]
    FOREIGN KEY ([UnitId])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThiCongs_Units'
CREATE INDEX [IX_FK_ThiCongs_Units]
ON [dbo].[ThiCongs]
    ([UnitId]);
GO

-- Creating foreign key on [TransportFilesId] in table 'TransportFileUrls'
ALTER TABLE [dbo].[TransportFileUrls]
ADD CONSTRAINT [FK_TransportFileUrls_TransportFiles]
    FOREIGN KEY ([TransportFilesId])
    REFERENCES [dbo].[TransportFiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransportFileUrls_TransportFiles'
CREATE INDEX [IX_FK_TransportFileUrls_TransportFiles]
ON [dbo].[TransportFileUrls]
    ([TransportFilesId]);
GO

-- Creating foreign key on [FileId] in table 'Transports'
ALTER TABLE [dbo].[Transports]
ADD CONSTRAINT [FK_Transports_TransportFiles]
    FOREIGN KEY ([FileId])
    REFERENCES [dbo].[TransportFiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Transports_TransportFiles'
CREATE INDEX [IX_FK_Transports_TransportFiles]
ON [dbo].[Transports]
    ([FileId]);
GO

-- Creating foreign key on [ModifiedAccount_Id] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_NguoiSua]
    FOREIGN KEY ([ModifiedAccount_Id])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_NguoiSua'
CREATE INDEX [IX_FK_NhapBanMu_NguoiSua]
ON [dbo].[NhapBanMus]
    ([ModifiedAccount_Id]);
GO

-- Creating foreign key on [TinhTrang] in table 'NhapBanMus'
ALTER TABLE [dbo].[NhapBanMus]
ADD CONSTRAINT [FK_NhapBanMu_TinhTrang]
    FOREIGN KEY ([TinhTrang])
    REFERENCES [dbo].[StatusProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhapBanMu_TinhTrang'
CREATE INDEX [IX_FK_NhapBanMu_TinhTrang]
ON [dbo].[NhapBanMus]
    ([TinhTrang]);
GO

-- Creating foreign key on [AccountId] in table 'UserPermissionGroups'
ALTER TABLE [dbo].[UserPermissionGroups]
ADD CONSTRAINT [FK__UserPermi__Accou__592635D8]
    FOREIGN KEY ([AccountId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__UserPermi__Accou__592635D8'
CREATE INDEX [IX_FK__UserPermi__Accou__592635D8]
ON [dbo].[UserPermissionGroups]
    ([AccountId]);
GO

-- Creating foreign key on [GroupId] in table 'GroupPermissions'
ALTER TABLE [dbo].[GroupPermissions]
ADD CONSTRAINT [FK__GroupPerm__Group__546180BB]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[PermissionGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__GroupPerm__Group__546180BB'
CREATE INDEX [IX_FK__GroupPerm__Group__546180BB]
ON [dbo].[GroupPermissions]
    ([GroupId]);
GO

-- Creating foreign key on [PermissionId] in table 'GroupPermissions'
ALTER TABLE [dbo].[GroupPermissions]
ADD CONSTRAINT [FK__GroupPerm__Permi__5555A4F4]
    FOREIGN KEY ([PermissionId])
    REFERENCES [dbo].[Permissions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__GroupPerm__Permi__5555A4F4'
CREATE INDEX [IX_FK__GroupPerm__Permi__5555A4F4]
ON [dbo].[GroupPermissions]
    ([PermissionId]);
GO

-- Creating foreign key on [GroupId] in table 'UserPermissionGroups'
ALTER TABLE [dbo].[UserPermissionGroups]
ADD CONSTRAINT [FK__UserPermi__Group__5A1A5A11]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[PermissionGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__UserPermi__Group__5A1A5A11'
CREATE INDEX [IX_FK__UserPermi__Group__5A1A5A11]
ON [dbo].[UserPermissionGroups]
    ([GroupId]);
GO

-- Creating foreign key on [ParentId] in table 'MenuItems'
ALTER TABLE [dbo].[MenuItems]
ADD CONSTRAINT [FK_MenuItem_Parent]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[MenuItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MenuItem_Parent'
CREATE INDEX [IX_FK_MenuItem_Parent]
ON [dbo].[MenuItems]
    ([ParentId]);
GO

-- Creating foreign key on [DonVi_Id] in table 'ThietBiXeMays'
ALTER TABLE [dbo].[ThietBiXeMays]
ADD CONSTRAINT [FK_ThietBiXeMay_DonVi]
    FOREIGN KEY ([DonVi_Id])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThietBiXeMay_DonVi'
CREATE INDEX [IX_FK_ThietBiXeMay_DonVi]
ON [dbo].[ThietBiXeMays]
    ([DonVi_Id]);
GO

-- Creating foreign key on [NguoiSuDung_Id] in table 'ThietBiXeMays'
ALTER TABLE [dbo].[ThietBiXeMays]
ADD CONSTRAINT [FK_ThietBiXeMay_NguoiSuDung]
    FOREIGN KEY ([NguoiSuDung_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThietBiXeMay_NguoiSuDung'
CREATE INDEX [IX_FK_ThietBiXeMay_NguoiSuDung]
ON [dbo].[ThietBiXeMays]
    ([NguoiSuDung_Id]);
GO

-- Creating foreign key on [ThietBi_Id] in table 'HoSoPhapLys'
ALTER TABLE [dbo].[HoSoPhapLys]
ADD CONSTRAINT [FK_HoSoPhapLys_ThietBis]
    FOREIGN KEY ([ThietBi_Id])
    REFERENCES [dbo].[ThietBiXeMays]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HoSoPhapLys_ThietBis'
CREATE INDEX [IX_FK_HoSoPhapLys_ThietBis]
ON [dbo].[HoSoPhapLys]
    ([ThietBi_Id]);
GO

-- Creating foreign key on [DepartmentId] in table 'MenuItems'
ALTER TABLE [dbo].[MenuItems]
ADD CONSTRAINT [FK_MenuItems_Donvis]
    FOREIGN KEY ([DepartmentId])
    REFERENCES [dbo].[DM_DonVis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MenuItems_Donvis'
CREATE INDEX [IX_FK_MenuItems_Donvis]
ON [dbo].[MenuItems]
    ([DepartmentId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------