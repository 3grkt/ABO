/* Delete old data */
DELETE tblRolePermission
DELETE tblPermission

/* Insert permissions */
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (1, N'Manage Profile Type', N'Định nghĩa các loại hồ sơ của NPP')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (2, N'Manage Profile Box', N'Quản lý thùng hồ sơ ')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (3, N'View Profile Scan Result', N'Kết quả xử lý hàng ngày')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (4, N'View Profile Box Details', N'Chi tiết một thùng hồ sơ')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (5, N'Manage Distributor''s Profile', N'Quản lý hồ sơ của một NPP')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (6, N'View Distributor Update', N'Xác định danh sách NPP có thông tin thay đổi chưa được scan trong 1 tháng gần đây')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (7, N'Manage User', N'Quản lý user')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (8, N'Purge Data', N'Xóa dữ liệu định kì')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (9, N'Print Distributor Letter', N'Chức năng in thư cho NPP gia nhập lại')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (10, N'View Distributor''s Profile', N'Xem hồ sơ của một NPP')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (11, N'Create Profile Box', N'Tạo thùng hồ sơ')


/* Insert roles */
IF NOT EXISTS (SELECT Id FROM tblRole WHERE Id = 1)
	INSERT [dbo].[tblRole] ([Id], [Name], [Description]) VALUES (1, N'Guess', N'Chỉ được xem thông tin hồ sơ của NPP')
IF NOT EXISTS (SELECT Id FROM tblRole WHERE Id = 2)
	INSERT [dbo].[tblRole] ([Id], [Name], [Description]) VALUES (2, N'Scanner', N'Bị hạn chế một số quyền')
IF NOT EXISTS (SELECT Id FROM tblRole WHERE Id = 3)
	INSERT [dbo].[tblRole] ([Id], [Name], [Description]) VALUES (3, N'Team Leader', N'Quyền cao nhất có thể làm mọi thứ')
IF NOT EXISTS (SELECT Id FROM tblRole WHERE Id = 4)
	INSERT [dbo].[tblRole] ([Id], [Name], [Description]) VALUES (4, N'IT', N'Có quyền quản lý user và xóa dữ liệu')

/* Insert role-permission link */
-- Guess
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (1, 10)
-- Scanner
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (2, 3)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (2, 4)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (2, 6)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (2, 9)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (2, 5)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (2, 10)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (2, 11)
-- Team Leader
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 1)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 2)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 3)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 4)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 5)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 6)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 9)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 10)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 11)
-- IT
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (4, 7)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (4, 8)