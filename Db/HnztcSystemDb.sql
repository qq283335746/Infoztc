SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBase]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserBase](
	[UserId] [uniqueidentifier] NOT NULL,
	[Nickname] [nvarchar](30) NULL,
	[HeadPicture] [varchar](100) NULL,
	[Sex] [nchar](2) NULL,
	[MobilePhone] [varchar](20) NULL,
	[TotalGold] [int] NULL CONSTRAINT [DF_UserBase_TotalGold]  DEFAULT ((0)),
	[TotalSilver] [int] NULL CONSTRAINT [DF_UserBase_TotalSilver]  DEFAULT ((0)),
	[TotalIntegral] [int] NULL CONSTRAINT [DF_UserBase_TotalIntegral]  DEFAULT ((0)),
	[SilverLevel] [int] NULL CONSTRAINT [DF_UserBase_SilverLevel]  DEFAULT ((0)),
	[ColorLevel] [int] NULL CONSTRAINT [DF_UserBase_ColorLevel]  DEFAULT ((0)),
	[IntegralLevel] [int] NULL CONSTRAINT [DF_UserBase_IntegralLevel]  DEFAULT ((0)),
	[VIPLevel] [nvarchar](10) NULL,
 CONSTRAINT [PK_UserBase] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'UserBase', @level2type=N'COLUMN', @level2name=N'UserId'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sys_Log]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Sys_Log](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Sys_Log_Id]  DEFAULT (newid()),
	[AppName] [nvarchar](20) NULL,
	[MethodName] [varchar](100) NULL,
	[Message] [nvarchar](1000) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Sys_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Sys_Log', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserHeadPicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserHeadPicture](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UserHeadPicture_Id]  DEFAULT (newid()),
	[FileName] [nvarchar](100) NULL,
	[FileSize] [int] NULL,
	[FileExtension] [varchar](10) NULL,
	[FileDirectory] [nvarchar](100) NULL,
	[RandomFolder] [varchar](20) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_UserHeadPicture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'UserHeadPicture', @level2type=N'COLUMN', @level2name=N'Id'

