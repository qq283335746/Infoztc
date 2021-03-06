SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductSizePicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductSizePicture](
	[ProductId] [uniqueidentifier] NOT NULL,
	[ProductItemId] [uniqueidentifier] NOT NULL,
	[Named] [nvarchar](20) NULL,
	[PictureId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ProductSizePicture] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[ProductItemId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductSizePicture', @level2type=N'COLUMN', @level2name=N'ProductId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductSize]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductSize](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProductSize_Id]  DEFAULT (newid()),
	[ProductId] [uniqueidentifier] NULL,
	[ProductItemId] [uniqueidentifier] NULL,
	[SizeName] [varchar](20) NULL,
 CONSTRAINT [PK_ProductSize] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductSize', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductPicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductPicture](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProductPicture_Id]  DEFAULT (newid()),
	[OriginalPicture] [varchar](100) NULL,
	[BPicture] [varchar](100) NULL,
	[MPicture] [varchar](100) NULL,
	[SPicture] [varchar](100) NULL,
	[OtherPicture] [varchar](100) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ProductPicture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductPicture', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductItem](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProductItem_Id]  DEFAULT (newid()),
	[ProductId] [uniqueidentifier] NULL,
	[Named] [nvarchar](50) NULL,
	[PictureId] [uniqueidentifier] NULL,
	[Sort] [int] NULL,
	[EnableStartTime] [datetime] NULL,
	[EnableEndTime] [datetime] NULL,
	[IsEnable] [bit] NULL,
	[IsDisable] [bit] NULL,
 CONSTRAINT [PK_ProductItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductItem', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductImage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductImage](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProductImage_Id]  DEFAULT (newid()),
	[ProductId] [uniqueidentifier] NOT NULL,
	[ProductItemId] [uniqueidentifier] NOT NULL,
	[PictureId] [uniqueidentifier] NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_ProductImage_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductImage', @level2type=N'COLUMN', @level2name=N'Id'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductImage', @level2type=N'COLUMN', @level2name=N'ProductId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductDetail](
	[ProductId] [uniqueidentifier] NOT NULL,
	[ProductItemId] [uniqueidentifier] NOT NULL,
	[OriginalPrice] [decimal](18, 2) NULL,
	[ProductPrice] [decimal](18, 2) NULL,
	[Discount] [float] NULL,
	[DiscountDescr] [nvarchar](20) NULL,
	[ContentText] [ntext] NULL,
	[PayOption] [nvarchar](100) NULL,
	[ViewCount] [int] NULL CONSTRAINT [DF_ProductDetail_ViewCount]  DEFAULT ((0)),
 CONSTRAINT [PK_ProductDetail] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[ProductItemId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductDetail', @level2type=N'COLUMN', @level2name=N'ProductId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductAttrTemplate]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductAttrTemplate](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProductAttrTemplate_Id]  DEFAULT (newid()),
	[TName] [nvarchar](100) NULL,
	[TValue] [nvarchar](1000) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ProductAttrTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductAttrTemplate', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductAttr]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductAttr](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProductAttr_Id]  DEFAULT (newid()),
	[ProductId] [uniqueidentifier] NOT NULL,
	[ProductItemId] [uniqueidentifier] NOT NULL,
	[AttrName] [nvarchar](30) NULL,
	[AttrValue] [nvarchar](50) NULL,
	[IsDisable] [bit] NULL,
 CONSTRAINT [PK_ProductAttr_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductAttr', @level2type=N'COLUMN', @level2name=N'Id'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductAttr', @level2type=N'COLUMN', @level2name=N'ProductId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Product](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Product_Id]  DEFAULT (newid()),
	[Named] [nvarchar](50) NULL,
	[PictureId] [uniqueidentifier] NULL,
	[Sort] [int] NULL,
	[EnableStartTime] [datetime] NULL,
	[EnableEndTime] [datetime] NULL,
	[IsEnable] [bit] NULL,
	[IsDisable] [bit] NULL,
	[UserId] [uniqueidentifier] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture_ProductSize]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture_ProductSize](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Picture_ProductSize_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Picture_ProductSize] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Picture_ProductSize', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture_Product]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture_Product](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Picture_Product_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Picture_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Picture_Product', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture_Category]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture_Category](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Picture_Category_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Picture_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Picture_Category', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture_Brand]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture_Brand](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Picture_Brand_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Picture_Brand] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Picture_Brand', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MenuProduct]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MenuProduct](
	[MenuId] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_MenuProduct] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC,
	[ProductId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MenuProduct', @level2type=N'COLUMN', @level2name=N'MenuId'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MenuProduct', @level2type=N'COLUMN', @level2name=N'ProductId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CategoryProduct]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CategoryProduct](
	[ProductId] [uniqueidentifier] NOT NULL,
	[CategoryId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CategoryProduct] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[CategoryId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'CategoryProduct', @level2type=N'COLUMN', @level2name=N'ProductId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CategoryPicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CategoryPicture](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_CategoryPicture_Id]  DEFAULT (newid()),
	[OriginalPicture] [varchar](100) NULL,
	[BPicture] [varchar](100) NULL,
	[MPicture] [varchar](100) NULL,
	[SPicture] [varchar](100) NULL,
	[OtherPicture] [varchar](100) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_CategoryPicture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'CategoryPicture', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CategoryBrand]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CategoryBrand](
	[BrandId] [uniqueidentifier] NOT NULL,
	[CategoryId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CategoryBrand] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC,
	[BrandId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'CategoryBrand', @level2type=N'COLUMN', @level2name=N'BrandId'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'CategoryBrand', @level2type=N'COLUMN', @level2name=N'CategoryId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Category](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Category_Id]  DEFAULT (newid()),
	[CategoryName] [nvarchar](50) NULL,
	[CategoryCode] [varchar](50) NULL,
	[ParentId] [uniqueidentifier] NULL,
	[PictureId] [uniqueidentifier] NULL,
	[Sort] [int] NULL CONSTRAINT [DF_Category_Sort]  DEFAULT ((0)),
	[Remark] [nvarchar](300) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cart]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Cart](
	[ProfileId] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NULL,
	[CategoryId] [uniqueidentifier] NULL,
	[Price] [decimal](18, 2) NULL,
	[Quantity] [int] NULL,
	[Named] [nvarchar](50) NULL,
	[IsShoppingCart] [bit] NULL,
	[LastUpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Cart', @level2type=N'COLUMN', @level2name=N'ProfileId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BrandProduct]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BrandProduct](
	[ProductId] [uniqueidentifier] NOT NULL,
	[BrandId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BrandProduct] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[BrandId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'BrandProduct', @level2type=N'COLUMN', @level2name=N'ProductId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Brand]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Brand](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Brand_Id]  DEFAULT (newid()),
	[BrandName] [nvarchar](30) NULL,
	[BrandCode] [varchar](50) NULL,
	[ParentId] [uniqueidentifier] NULL,
	[PictureId] [uniqueidentifier] NULL,
	[Sort] [int] NULL CONSTRAINT [DF_Brand_Sort]  DEFAULT ((0)),
	[Remark] [nvarchar](50) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Brand', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Supplier]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Supplier](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Supplier_Id]  DEFAULT (newid()),
	[SupplierName] [nvarchar](30) NULL,
	[Phone] [char](15) NULL,
	[ProvinceCityName] [nvarchar](20) NULL,
	[Address] [nvarchar](30) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Supplier', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SizePicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SizePicture](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SizePicture_Id]  DEFAULT (newid()),
	[OriginalPicture] [varchar](100) NULL,
	[BPicture] [varchar](100) NULL,
	[MPicture] [varchar](100) NULL,
	[SPicture] [varchar](100) NULL,
	[OtherPicture] [varchar](100) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_SizePicture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SizePicture', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Profiles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Profiles](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Profiles_Id]  DEFAULT (newid()),
	[Username] [nvarchar](50) NULL,
	[AppName] [nvarchar](50) NULL,
	[IsAnonymous] [bit] NULL,
	[LastActivityDate] [datetime] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Profiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Profiles', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductStock]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductStock](
	[ProductId] [uniqueidentifier] NOT NULL,
	[ProductItemId] [uniqueidentifier] NOT NULL,
	[ProductSizeId] [uniqueidentifier] NOT NULL,
	[StockNum] [int] NULL,
 CONSTRAINT [PK_ProductStock] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[ProductItemId] ASC,
	[ProductSizeId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ProductStock', @level2type=N'COLUMN', @level2name=N'ProductId'

