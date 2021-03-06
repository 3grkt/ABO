USE [ABO]
GO
/****** Object:  Table [dbo].[tblPermission]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPermission](
	[Id] [smallint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblPermission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (1, N'Manage Profile Type', N'Định nghĩa các loại hồ sơ của NPP')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (2, N'Manage Profile Box', N'Quản lý thùng hồ sơ ')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (3, N'View Profile Scan Result', N'Kết quả xử lý hàng ngày')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (4, N'View Profile Box Details', N'Chi tiết một thùng hồ sơ')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (5, N'Manage Distributor''s Profile', N'Quản lý hồ sơ của một NPP')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (6, N'View Distributor Update', N'Xác định danh sách NPP có thông tin thay đổi chưa được scan trong 1 tháng gần đây')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (7, N'Manage User', N'Quản lý user')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (8, N'Purge Data', N'Xóa dữ liệu định kì')
INSERT [dbo].[tblPermission] ([Id], [Name], [Description]) VALUES (9, N'Print Distributor Letter', N'Chức năng in thư cho NPP gia nhập lại')
/****** Object:  Table [dbo].[tblLocation]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLocation](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRole]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRole](
	[Id] [smallint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tblRole] ([Id], [Name], [Description]) VALUES (1, N'Guess', N'Chỉ được xem thông tin hồ sơ của NPP')
INSERT [dbo].[tblRole] ([Id], [Name], [Description]) VALUES (2, N'Scanner', N'Bị hạn chế một số quyền')
INSERT [dbo].[tblRole] ([Id], [Name], [Description]) VALUES (3, N'Team Leader', N'Quyền cao nhất có thể làm mọi thứ')
/****** Object:  Table [dbo].[tblProfileType]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProfileType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[StoredYears] [smallint] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Scanned] [bit] NOT NULL,
	[SystemType] [bit] NOT NULL,
	[Image] [bit] NOT NULL,
 CONSTRAINT [PK_tblProfileType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblProfileType] ON
INSERT [dbo].[tblProfileType] ([Id], [Name], [StoredYears], [Description], [Scanned], [SystemType], [Image]) VALUES (1, N'SA88', 1000, N'Lưu cho đến khi NPP expire', 1, 1, 0)
INSERT [dbo].[tblProfileType] ([Id], [Name], [StoredYears], [Description], [Scanned], [SystemType], [Image]) VALUES (2, N'Thay đổi', 1000, N'Lưu cho đến khi NPP expire', 1, 1, 0)
INSERT [dbo].[tblProfileType] ([Id], [Name], [StoredYears], [Description], [Scanned], [SystemType], [Image]) VALUES (3, N'Thay đổi 1 năm', 1, N'Lưu trong vòng 1 năm', 0, 1, 0)
INSERT [dbo].[tblProfileType] ([Id], [Name], [StoredYears], [Description], [Scanned], [SystemType], [Image]) VALUES (4, N'Hóa đơn', 1, N'Lưu trong vòng 1 năm', 0, 1, 0)
INSERT [dbo].[tblProfileType] ([Id], [Name], [StoredYears], [Description], [Scanned], [SystemType], [Image]) VALUES (6, N'Hình ảnh', 1000, N'Hình ảnh', 1, 1, 1)
INSERT [dbo].[tblProfileType] ([Id], [Name], [StoredYears], [Description], [Scanned], [SystemType], [Image]) VALUES (8, N'Ảnh cá nhân', 12, N'new type for test', 0, 0, 1)
SET IDENTITY_INSERT [dbo].[tblProfileType] OFF
/****** Object:  Table [dbo].[tblWarehouse]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblWarehouse](
	[WarehouseId] [nchar](2) NOT NULL,
	[WarehouseName] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_tblWarehouse] PRIMARY KEY CLUSTERED 
(
	[WarehouseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDataLog]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDataLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](100) NOT NULL,
	[FieldName] [nvarchar](200) NOT NULL,
	[OldValue] [nvarchar](1000) NULL,
	[NewValue] [nvarchar](1000) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](100) NULL,
 CONSTRAINT [PK_tblDataLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblDataLog] ON
INSERT [dbo].[tblDataLog] ([Id], [TableName], [FieldName], [OldValue], [NewValue], [LogDate], [LogUser]) VALUES (1, N'tblProfile', N'Status', N'1', N'2', CAST(0x0000A40300000000 AS DateTime), N'1')
INSERT [dbo].[tblDataLog] ([Id], [TableName], [FieldName], [OldValue], [NewValue], [LogDate], [LogUser]) VALUES (2, N'tblProfileBox', N'ScannedFolder', N'\\shared1', N'\\shared2', CAST(0x0000A40400000000 AS DateTime), N'1')
SET IDENTITY_INSERT [dbo].[tblDataLog] OFF
/****** Object:  Table [dbo].[tblStatus]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblStatus](
	[Id] [smallint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[StatusType] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_tblStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (101, N'Hiệu lực', N'Hiệu lực', N'Profile')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (102, N'Đã xóa', N'Đã xóa', N'Profile')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (103, N'Cần xóa', N'Cần xóa', N'Profile')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (201, N'Đang mở', N'Đang mở', N'ProfileBox')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (202, N'Đã đóng gói', N'Đã đóng gói', N'ProfileBox')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (203, N'Đã di chuyển', N'Đã di chuyển', N'ProfileBox')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (204, N'Lưu kho', N'Lưu kho', N'ProfileBox')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (205, N'Có thể hủy', N'Có thể hủy', N'ProfileBox')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (206, N'Cần hủy', N'Cần hủy', N'ProfileBox')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (207, N'Đã hủy', N'Đã hủy', N'ProfileBox')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (301, N'Chưa scan', N'Chưa scan', N'DistributorLog')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (302, N'Đã scan', N'Đã scan', N'DistributorLog')
INSERT [dbo].[tblStatus] ([Id], [Name], [Description], [StatusType]) VALUES (303, N'Không cần scan', N'Không cần scan', N'DistributorLog')
/****** Object:  Table [dbo].[tblRolePermission]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRolePermission](
	[RoleId] [smallint] NOT NULL,
	[PermissionId] [smallint] NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 1)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 2)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 3)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 4)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 5)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 6)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 7)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 8)
INSERT [dbo].[tblRolePermission] ([RoleId], [PermissionId]) VALUES (3, 9)
/****** Object:  Table [dbo].[tblUser]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](8) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[RoleId] [smallint] NOT NULL,
	[WarehouseId] [nchar](2) NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblUser] ON
INSERT [dbo].[tblUser] ([Id], [UserName], [FullName], [RoleId], [WarehouseId]) VALUES (1, N'VNM00000', N'VNM Default User', 3, NULL)
SET IDENTITY_INSERT [dbo].[tblUser] OFF
/****** Object:  Table [dbo].[tblDistributor]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDistributor](
	[DistNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address1] [nvarchar](50) NULL,
	[Address2] [nvarchar](50) NULL,
	[Address3] [nvarchar](50) NULL,
	[Address4] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[JoinDate] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[WarehouseId] [nchar](2) NULL,
 CONSTRAINT [PK_tblDistributor] PRIMARY KEY CLUSTERED 
(
	[DistNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblDistributor] ON
INSERT [dbo].[tblDistributor] ([DistNumber], [Name], [Address1], [Address2], [Address3], [Address4], [City], [JoinDate], [ExpiryDate], [WarehouseId]) VALUES (1, N'test dist', N'add1', N'', N'add3', N'add4', N'hcm', CAST(0x00009FCB00000000 AS DateTime), CAST(0x0000A57F00000000 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[tblDistributor] OFF
/****** Object:  Table [dbo].[tblDistributorUpdateType]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblDistributorUpdateType](
	[Type] [char](1) NOT NULL,
	[ProfileTypeId] [int] NOT NULL,
	[Description] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[tblDistributorUpdateType] ([Type], [ProfileTypeId], [Description]) VALUES (N'A', 1, N'Gia nhập')
INSERT [dbo].[tblDistributorUpdateType] ([Type], [ProfileTypeId], [Description]) VALUES (N'K', 2, N'Thay đổi thông tin')
INSERT [dbo].[tblDistributorUpdateType] ([Type], [ProfileTypeId], [Description]) VALUES (N'M', 2, N'Thay đổi địa chỉ')
INSERT [dbo].[tblDistributorUpdateType] ([Type], [ProfileTypeId], [Description]) VALUES (N'R', 3, N'Tái tục hợp đồng')
/****** Object:  Table [dbo].[tblDistributorUpdate]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDistributorUpdate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WarehouseId] [nchar](2) NULL,
	[DistNumber] [bigint] NOT NULL,
	[UpdatedType] [nvarchar](100) NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tblDistributorUpdate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDistributorLog]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblDistributorLog](
	[Id] [int] NOT NULL,
	[DistNumber] [bigint] NOT NULL,
	[UpdateType] [char](1) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[StatusId] [smallint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblDistributorLetter]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDistributorLetter](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LetterDate] [datetime] NOT NULL,
	[DistNumber] [bigint] NOT NULL,
	[DistName] [nvarchar](200) NOT NULL,
	[SponsorId] [bigint] NULL,
	[PlatiniumSponsorId] [bigint] NULL,
 CONSTRAINT [PK_tblDistributorLetter] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDataPurge]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDataPurge](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurgeDate] [datetime] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[UserID] [int] NOT NULL,
	[FileCount] [int] NULL,
	[PurgeLog] [nvarchar](4000) NULL,
 CONSTRAINT [PK_tblDataPurge] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProfileBox]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProfileBox](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[WarehouseId] [nchar](2) NULL,
	[OfficeId] [int] NULL,
	[LocationId] [int] NULL,
	[TypeId] [int] NOT NULL,
	[StatusId] [smallint] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[ADACount] [int] NOT NULL,
	[ScannedFolder] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblProfileBox] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProfileScan]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProfileScan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WarehouseId] [nchar](2) NULL,
	[BoxId] [int] NOT NULL,
	[ScannedDate] [datetime] NOT NULL,
	[FileCount] [int] NOT NULL,
	[Result] [smallint] NOT NULL,
	[Description] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tblProfileScan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProfile]    Script Date: 03/10/2015 23:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProfile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WarehouseId] [nchar](2) NULL,
	[TypeId] [int] NOT NULL,
	[BoxId] [int] NULL,
	[UserId] [int] NULL,
	[DistNumber] [bigint] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ScannedDate] [datetime] NULL,
	[Description] [nvarchar](1000) NULL,
	[StatusId] [smallint] NOT NULL,
 CONSTRAINT [PK_tblProfile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF__tblProfil__Image__1209AD79]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfileType] ADD  DEFAULT ((0)) FOR [Image]
GO
/****** Object:  Default [DF__tblProfil__ADACo__7755B73D]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfileBox] ADD  DEFAULT ((0)) FOR [ADACount]
GO
/****** Object:  ForeignKey [FK_tblRolePermission_Role]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblRolePermission]  WITH CHECK ADD  CONSTRAINT [FK_tblRolePermission_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[tblRole] ([Id])
GO
ALTER TABLE [dbo].[tblRolePermission] CHECK CONSTRAINT [FK_tblRolePermission_Role]
GO
/****** Object:  ForeignKey [FK_tblRolePermission_tblPermission]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblRolePermission]  WITH CHECK ADD  CONSTRAINT [FK_tblRolePermission_tblPermission] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[tblPermission] ([Id])
GO
ALTER TABLE [dbo].[tblRolePermission] CHECK CONSTRAINT [FK_tblRolePermission_tblPermission]
GO
/****** Object:  ForeignKey [FK_tblUser_tblRole]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblUser]  WITH CHECK ADD  CONSTRAINT [FK_tblUser_tblRole] FOREIGN KEY([RoleId])
REFERENCES [dbo].[tblRole] ([Id])
GO
ALTER TABLE [dbo].[tblUser] CHECK CONSTRAINT [FK_tblUser_tblRole]
GO
/****** Object:  ForeignKey [FK_tblUser_tblWarehouse]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblUser]  WITH CHECK ADD  CONSTRAINT [FK_tblUser_tblWarehouse] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[tblWarehouse] ([WarehouseId])
GO
ALTER TABLE [dbo].[tblUser] CHECK CONSTRAINT [FK_tblUser_tblWarehouse]
GO
/****** Object:  ForeignKey [FK_tblDistributor_tblWarehouse]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDistributor]  WITH CHECK ADD  CONSTRAINT [FK_tblDistributor_tblWarehouse] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[tblWarehouse] ([WarehouseId])
GO
ALTER TABLE [dbo].[tblDistributor] CHECK CONSTRAINT [FK_tblDistributor_tblWarehouse]
GO
/****** Object:  ForeignKey [FK_tblDistributorUpdateType_tblProfileType]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDistributorUpdateType]  WITH CHECK ADD  CONSTRAINT [FK_tblDistributorUpdateType_tblProfileType] FOREIGN KEY([ProfileTypeId])
REFERENCES [dbo].[tblProfileType] ([Id])
GO
ALTER TABLE [dbo].[tblDistributorUpdateType] CHECK CONSTRAINT [FK_tblDistributorUpdateType_tblProfileType]
GO
/****** Object:  ForeignKey [FK_tblDistributorUpdate_tblDistributor]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDistributorUpdate]  WITH CHECK ADD  CONSTRAINT [FK_tblDistributorUpdate_tblDistributor] FOREIGN KEY([DistNumber])
REFERENCES [dbo].[tblDistributor] ([DistNumber])
GO
ALTER TABLE [dbo].[tblDistributorUpdate] CHECK CONSTRAINT [FK_tblDistributorUpdate_tblDistributor]
GO
/****** Object:  ForeignKey [FK_tblDistributorUpdate_tblWarehouse]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDistributorUpdate]  WITH CHECK ADD  CONSTRAINT [FK_tblDistributorUpdate_tblWarehouse] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[tblWarehouse] ([WarehouseId])
GO
ALTER TABLE [dbo].[tblDistributorUpdate] CHECK CONSTRAINT [FK_tblDistributorUpdate_tblWarehouse]
GO
/****** Object:  ForeignKey [FK_tblDistributorLog_tblDistributor]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDistributorLog]  WITH CHECK ADD  CONSTRAINT [FK_tblDistributorLog_tblDistributor] FOREIGN KEY([DistNumber])
REFERENCES [dbo].[tblDistributor] ([DistNumber])
GO
ALTER TABLE [dbo].[tblDistributorLog] CHECK CONSTRAINT [FK_tblDistributorLog_tblDistributor]
GO
/****** Object:  ForeignKey [FK_tblDistributorLog_tblStatus]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDistributorLog]  WITH CHECK ADD  CONSTRAINT [FK_tblDistributorLog_tblStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[tblStatus] ([Id])
GO
ALTER TABLE [dbo].[tblDistributorLog] CHECK CONSTRAINT [FK_tblDistributorLog_tblStatus]
GO
/****** Object:  ForeignKey [FK_tblDistributorLetter_tblDistributor]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDistributorLetter]  WITH CHECK ADD  CONSTRAINT [FK_tblDistributorLetter_tblDistributor] FOREIGN KEY([DistNumber])
REFERENCES [dbo].[tblDistributor] ([DistNumber])
GO
ALTER TABLE [dbo].[tblDistributorLetter] CHECK CONSTRAINT [FK_tblDistributorLetter_tblDistributor]
GO
/****** Object:  ForeignKey [FK_tblDistributorLetter_tblDistributor1]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDistributorLetter]  WITH CHECK ADD  CONSTRAINT [FK_tblDistributorLetter_tblDistributor1] FOREIGN KEY([SponsorId])
REFERENCES [dbo].[tblDistributor] ([DistNumber])
GO
ALTER TABLE [dbo].[tblDistributorLetter] CHECK CONSTRAINT [FK_tblDistributorLetter_tblDistributor1]
GO
/****** Object:  ForeignKey [FK_tblDistributorLetter_tblDistributor2]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDistributorLetter]  WITH CHECK ADD  CONSTRAINT [FK_tblDistributorLetter_tblDistributor2] FOREIGN KEY([PlatiniumSponsorId])
REFERENCES [dbo].[tblDistributor] ([DistNumber])
GO
ALTER TABLE [dbo].[tblDistributorLetter] CHECK CONSTRAINT [FK_tblDistributorLetter_tblDistributor2]
GO
/****** Object:  ForeignKey [FK_tblDataPurge_tblUser]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblDataPurge]  WITH CHECK ADD  CONSTRAINT [FK_tblDataPurge_tblUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[tblUser] ([Id])
GO
ALTER TABLE [dbo].[tblDataPurge] CHECK CONSTRAINT [FK_tblDataPurge_tblUser]
GO
/****** Object:  ForeignKey [FK_tblProfileBox_tblLocation]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfileBox]  WITH CHECK ADD  CONSTRAINT [FK_tblProfileBox_tblLocation] FOREIGN KEY([LocationId])
REFERENCES [dbo].[tblLocation] ([Id])
GO
ALTER TABLE [dbo].[tblProfileBox] CHECK CONSTRAINT [FK_tblProfileBox_tblLocation]
GO
/****** Object:  ForeignKey [FK_tblProfileBox_tblProfileType]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfileBox]  WITH CHECK ADD  CONSTRAINT [FK_tblProfileBox_tblProfileType] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblProfileType] ([Id])
GO
ALTER TABLE [dbo].[tblProfileBox] CHECK CONSTRAINT [FK_tblProfileBox_tblProfileType]
GO
/****** Object:  ForeignKey [FK_tblProfileBox_tblStatus]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfileBox]  WITH CHECK ADD  CONSTRAINT [FK_tblProfileBox_tblStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[tblStatus] ([Id])
GO
ALTER TABLE [dbo].[tblProfileBox] CHECK CONSTRAINT [FK_tblProfileBox_tblStatus]
GO
/****** Object:  ForeignKey [FK_tblProfileBox_tblUser]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfileBox]  WITH CHECK ADD  CONSTRAINT [FK_tblProfileBox_tblUser] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[tblUser] ([Id])
GO
ALTER TABLE [dbo].[tblProfileBox] CHECK CONSTRAINT [FK_tblProfileBox_tblUser]
GO
/****** Object:  ForeignKey [FK_tblProfileBox_tblWarehouse]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfileBox]  WITH CHECK ADD  CONSTRAINT [FK_tblProfileBox_tblWarehouse] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[tblWarehouse] ([WarehouseId])
GO
ALTER TABLE [dbo].[tblProfileBox] CHECK CONSTRAINT [FK_tblProfileBox_tblWarehouse]
GO
/****** Object:  ForeignKey [FK_tblProfileScan_tblProfileBox]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfileScan]  WITH CHECK ADD  CONSTRAINT [FK_tblProfileScan_tblProfileBox] FOREIGN KEY([BoxId])
REFERENCES [dbo].[tblProfileBox] ([Id])
GO
ALTER TABLE [dbo].[tblProfileScan] CHECK CONSTRAINT [FK_tblProfileScan_tblProfileBox]
GO
/****** Object:  ForeignKey [FK_tblProfileScan_tblWarehouse]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfileScan]  WITH CHECK ADD  CONSTRAINT [FK_tblProfileScan_tblWarehouse] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[tblWarehouse] ([WarehouseId])
GO
ALTER TABLE [dbo].[tblProfileScan] CHECK CONSTRAINT [FK_tblProfileScan_tblWarehouse]
GO
/****** Object:  ForeignKey [FK_tblProfile_tblDistributor]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfile]  WITH CHECK ADD  CONSTRAINT [FK_tblProfile_tblDistributor] FOREIGN KEY([DistNumber])
REFERENCES [dbo].[tblDistributor] ([DistNumber])
GO
ALTER TABLE [dbo].[tblProfile] CHECK CONSTRAINT [FK_tblProfile_tblDistributor]
GO
/****** Object:  ForeignKey [FK_tblProfile_tblProfileBox]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfile]  WITH CHECK ADD  CONSTRAINT [FK_tblProfile_tblProfileBox] FOREIGN KEY([BoxId])
REFERENCES [dbo].[tblProfileBox] ([Id])
GO
ALTER TABLE [dbo].[tblProfile] CHECK CONSTRAINT [FK_tblProfile_tblProfileBox]
GO
/****** Object:  ForeignKey [FK_tblProfile_tblProfileType]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfile]  WITH CHECK ADD  CONSTRAINT [FK_tblProfile_tblProfileType] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblProfileType] ([Id])
GO
ALTER TABLE [dbo].[tblProfile] CHECK CONSTRAINT [FK_tblProfile_tblProfileType]
GO
/****** Object:  ForeignKey [FK_tblProfile_tblStatus]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfile]  WITH CHECK ADD  CONSTRAINT [FK_tblProfile_tblStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[tblStatus] ([Id])
GO
ALTER TABLE [dbo].[tblProfile] CHECK CONSTRAINT [FK_tblProfile_tblStatus]
GO
/****** Object:  ForeignKey [FK_tblProfile_tblUser]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfile]  WITH CHECK ADD  CONSTRAINT [FK_tblProfile_tblUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[tblUser] ([Id])
GO
ALTER TABLE [dbo].[tblProfile] CHECK CONSTRAINT [FK_tblProfile_tblUser]
GO
/****** Object:  ForeignKey [FK_tblProfile_tblWarehouse]    Script Date: 03/10/2015 23:33:25 ******/
ALTER TABLE [dbo].[tblProfile]  WITH CHECK ADD  CONSTRAINT [FK_tblProfile_tblWarehouse] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[tblWarehouse] ([WarehouseId])
GO
ALTER TABLE [dbo].[tblProfile] CHECK CONSTRAINT [FK_tblProfile_tblWarehouse]
GO
