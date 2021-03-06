SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service_Vote]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Service_Vote](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Service_Vote_Id]  DEFAULT (newid()),
	[ServiceItemId] [uniqueidentifier] NULL,
	[Named] [nvarchar](30) NULL,
	[HeadPictureId] [uniqueidentifier] NULL,
	[Descr] [nvarchar](300) NULL,
	[ContentText] [nvarchar](4000) NULL,
	[Sort] [int] NULL,
	[EnableStartTime] [datetime] NULL,
	[EnableEndTime] [datetime] NULL,
	[IsDisable] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Service_Vote] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Service_Vote', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service_UserVole]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Service_UserVole](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Service_UserVole_Id]  DEFAULT (newid()),
	[UserId] [uniqueidentifier] NULL,
	[ServiceItemId] [uniqueidentifier] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Service_UserVole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Service_UserVole', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service_UserPraise]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Service_UserPraise](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Service_UserPraise_Id]  DEFAULT (newid()),
	[UserId] [uniqueidentifier] NULL,
	[ServiceItemId] [uniqueidentifier] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Service_UserPraise] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Service_UserPraise', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service_Picture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Service_Picture](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Service_Picture_Id]  DEFAULT (newid()),
	[OriginalPicture] [varchar](100) NULL,
	[BPicture] [varchar](100) NULL,
	[MPicture] [varchar](100) NULL,
	[SPicture] [varchar](100) NULL,
	[OtherPicture] [varchar](100) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Service_Picture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Service_Picture', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service_Link]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Service_Link](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Service_Link_Id]  DEFAULT (newid()),
	[ServiceItemId] [uniqueidentifier] NULL,
	[PictureId] [uniqueidentifier] NULL,
	[Named] [nvarchar](30) NULL,
	[Url] [varchar](300) NULL,
	[Sort] [int] NULL,
	[EnableStartTime] [datetime] NULL,
	[EnableEndTime] [datetime] NULL,
	[IsDisable] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Service_Link] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Service_Link', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service_Item]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Service_Item](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Service_Item_Id]  DEFAULT (newid()),
	[Named] [nvarchar](30) NULL,
	[ParentId] [uniqueidentifier] NULL,
	[PictureId] [uniqueidentifier] NULL,
	[Sort] [int] NULL,
	[HasVote] [bit] NULL,
	[HasContent] [bit] NULL,
	[HasLink] [bit] NULL,
	[EnableStartTime] [datetime] NULL,
	[EnableEndTime] [datetime] NULL,
	[IsDisable] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Service_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Service_Item', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service_Content]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Service_Content](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Service_Content_Id]  DEFAULT (newid()),
	[ServiceItemId] [uniqueidentifier] NULL,
	[Named] [nvarchar](30) NULL,
	[PictureId] [uniqueidentifier] NULL,
	[Descr] [nvarchar](300) NULL,
	[ContentText] [nvarchar](4000) NULL,
	[Sort] [int] NULL,
	[EnableStartTime] [datetime] NULL,
	[EnableEndTime] [datetime] NULL,
	[IsDisable] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Service_Content] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Service_Content', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProvinceCity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProvinceCity](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProvinceCity_Id]  DEFAULT (newid()),
	[Named] [nvarchar](30) NULL,
	[Pinyin] [varchar](30) NULL,
	[FirstChar] [char](1) NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Sort] [int] NULL CONSTRAINT [DF_ProvinceCity_Sort]  DEFAULT ((0)),
	[Remark] [nvarchar](300) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ProvinceCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProvinceCity', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture_ServiceVote]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture_ServiceVote](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Picture_ServiceVote_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Picture_ServiceVote] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Picture_ServiceVote', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture_ServiceLink]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture_ServiceLink](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Picture_ServiceLink_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Picture_ServiceLink] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Picture_ServiceLink', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture_ServiceItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture_ServiceItem](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Picture_ServiceItem_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Picture_ServiceItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Picture_ServiceItem', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture_ServiceContent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture_ServiceContent](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Picture_ServiceContent_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Picture_ServiceContent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Picture_ServiceContent', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture_Content]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture_Content](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Picture_Content_Id]  DEFAULT (newid()),
	[UserId] [uniqueidentifier] NULL,
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Picture_Content] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Picture_Content', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Notice](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Notice_Id]  DEFAULT (newid()),
	[ContentTypeId] [uniqueidentifier] NULL,
	[Title] [nvarchar](100) NULL,
	[Descr] [nvarchar](300) NULL,
	[ContentText] [ntext] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Notice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Notice', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentType](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ContentType_Id]  DEFAULT (newid()),
	[TypeName] [nvarchar](50) NULL,
	[TypeCode] [varchar](50) NULL,
	[TypeValue] [nvarchar](256) NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Sort] [int] NULL CONSTRAINT [DF_ContentType_Sort]  DEFAULT ((0)),
	[PictureId] [uniqueidentifier] NULL,
	[HasChild] [bit] NULL,
	[IsSys] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ContentType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ContentType', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentPicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentPicture](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ContentPicture_Id]  DEFAULT (newid()),
	[OriginalPicture] [varchar](100) NULL,
	[BPicture] [varchar](100) NULL,
	[MPicture] [varchar](100) NULL,
	[SPicture] [varchar](100) NULL,
	[OtherPicture] [varchar](100) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ContentPicture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ContentPicture', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentDetail](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ContentDetail_Id]  DEFAULT (newid()),
	[ContentTypeId] [uniqueidentifier] NULL,
	[Title] [nvarchar](100) NULL,
	[PictureId] [uniqueidentifier] NULL,
	[Descr] [nvarchar](300) NULL,
	[ContentText] [ntext] NULL,
	[VirtualViewCount] [int] NULL,
	[ViewCount] [int] NULL,
	[Sort] [int] NULL,
	[IsDisable] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ContentDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ContentDetail', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Announcement]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Announcement](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Announcement_Id]  DEFAULT (newid()),
	[ContentTypeId] [uniqueidentifier] NULL,
	[Title] [nvarchar](100) NULL,
	[Descr] [nvarchar](300) NULL,
	[ContentText] [ntext] NULL,
	[VirtualViewCount] [int] NULL CONSTRAINT [DF_Announcement_VirtualViewCount]  DEFAULT ((0)),
	[ViewCount] [int] NULL CONSTRAINT [DF_Announcement_ViewCount]  DEFAULT ((0)),
	[Sort] [int] NULL,
	[IsDisable] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Announcement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Announcement', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdvertisementPicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AdvertisementPicture](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AdvertisementPicture_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_AdvertisementPicture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AdvertisementPicture', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdvertisementLink]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AdvertisementLink](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AdvertisementLink_Id]  DEFAULT (newid()),
	[AdvertisementId] [uniqueidentifier] NULL,
	[ActionTypeId] [uniqueidentifier] NULL,
	[ContentPictureId] [uniqueidentifier] NULL,
	[Url] [varchar](100) NULL,
	[Sort] [int] NULL,
	[IsDisable] [bit] NULL,
 CONSTRAINT [PK_AdvertisementLink] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AdvertisementLink', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdvertisementItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AdvertisementItem](
	[AdvertisementId] [uniqueidentifier] NOT NULL,
	[Descr] [nvarchar](300) NULL,
	[ContentText] [nvarchar](3000) NULL,
 CONSTRAINT [PK_AdvertisementItem] PRIMARY KEY CLUSTERED 
(
	[AdvertisementId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AdvertisementItem', @level2type=N'COLUMN', @level2name=N'AdvertisementId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Advertisement]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Advertisement](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Advertisement_Id]  DEFAULT (newid()),
	[Title] [nvarchar](100) NULL,
	[SiteFunId] [uniqueidentifier] NULL,
	[LayoutPositionId] [uniqueidentifier] NULL,
	[Timeout] [int] NULL CONSTRAINT [DF_Advertisement_Timeout]  DEFAULT ((0)),
	[Sort] [int] NULL CONSTRAINT [DF_Advertisement_Sort]  DEFAULT ((0)),
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[VirtualViewCount] [int] NULL CONSTRAINT [DF_Advertisement_VirtualViewCount]  DEFAULT ((0)),
	[ViewCount] [int] NULL CONSTRAINT [DF_Advertisement_ViewCount]  DEFAULT ((0)),
	[IsDisable] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Advertisement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Advertisement', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdItemLink]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AdItemLink](
	[AdItemId] [uniqueidentifier] NOT NULL,
	[Url] [varchar](300) NULL,
	[ProductId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AdItemLink] PRIMARY KEY CLUSTERED 
(
	[AdItemId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AdItemLink', @level2type=N'COLUMN', @level2name=N'AdItemId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdItemContent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AdItemContent](
	[AdItemId] [uniqueidentifier] NOT NULL,
	[Descr] [nvarchar](300) NULL,
	[ContentText] [nvarchar](4000) NULL,
 CONSTRAINT [PK_AdItemContent] PRIMARY KEY CLUSTERED 
(
	[AdItemId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AdItemContent', @level2type=N'COLUMN', @level2name=N'AdItemId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AdItem](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AdItem_Id]  DEFAULT (newid()),
	[AdvertisementId] [uniqueidentifier] NULL,
	[PictureId] [uniqueidentifier] NULL,
	[ActionTypeId] [uniqueidentifier] NULL,
	[Sort] [int] NULL,
	[IsDisable] [bit] NULL,
 CONSTRAINT [PK_AdItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AdItem', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdBase]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AdBase](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AdBase_Id]  DEFAULT (newid()),
	[Title] [nvarchar](100) NULL,
	[SiteFunId] [uniqueidentifier] NULL,
	[LayoutPositionId] [uniqueidentifier] NULL,
	[Timeout] [int] NULL CONSTRAINT [DF_AdBase_Timeout]  DEFAULT ((0)),
	[Sort] [int] NULL CONSTRAINT [DF_AdBase_Sort]  DEFAULT ((0)),
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[VirtualViewCount] [int] NULL,
	[ViewCount] [int] NULL CONSTRAINT [DF_AdBase_ViewCount]  DEFAULT ((0)),
	[IsDisable] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_AdBase] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AdBase', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderRandom]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OrderRandom](
	[OrderCode] [varchar](20) NOT NULL,
	[Prefix] [varchar](10) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_OrderRandom] PRIMARY KEY CLUSTERED 
(
	[OrderCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'OrderRandom', @level2type=N'COLUMN', @level2name=N'OrderCode'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSignIn]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserSignIn](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UserSignIn_Id]  DEFAULT (newid()),
	[UserId] [uniqueidentifier] NULL,
	[SignInXml] [varchar](max) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_UserSignIn] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'UserSignIn', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserLevelView]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserLevelView](
	[UserId] [uniqueidentifier] NOT NULL,
	[FunCode] [int] NOT NULL,
	[EnumSource] [int] NOT NULL,
	[TotalGold] [int] NULL CONSTRAINT [DF_UserLevelView_TotalGold]  DEFAULT ((0)),
	[TotalSilver] [int] NULL CONSTRAINT [DF_UserLevelView_TotalSilver]  DEFAULT ((0)),
	[TotalIntegral] [int] NULL CONSTRAINT [DF_UserLevelView_TotalIntegral]  DEFAULT ((0)),
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_UserLevelView] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[FunCode] ASC,
	[EnumSource] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'UserLevelView', @level2type=N'COLUMN', @level2name=N'UserId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserLevelProduce]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserLevelProduce](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UserLevelProduce_Id]  DEFAULT (newid()),
	[UserId] [uniqueidentifier] NULL,
	[FunCode] [int] NULL,
	[EnumSource] [int] NULL,
	[TotalGold] [int] NULL,
	[TotalSilver] [int] NULL,
	[TotalIntegral] [int] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_UserLevelProduce] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'UserLevelProduce', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sys_Enum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Sys_Enum](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Sys_Enum_Id]  DEFAULT (newid()),
	[EnumCode] [nvarchar](50) NULL,
	[EnumName] [nvarchar](50) NULL,
	[EnumValue] [nvarchar](256) NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Sort] [int] NULL CONSTRAINT [DF_Sys_Enum_Sort]  DEFAULT ((0)),
	[Remark] [nvarchar](256) NULL,
 CONSTRAINT [PK_Sys_Enum] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Sys_Enum', @level2type=N'COLUMN', @level2name=N'Id'

