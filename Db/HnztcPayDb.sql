SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayCall]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayCall](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PayCall_Id]  DEFAULT (newid()),
	[Named] [nvarchar](50) NULL,
	[ReqUrl] [varchar](512) NULL,
	[ReqContent] [nvarchar](max) NULL,
	[ResStatusCode] [int] NULL,
	[ResResult] [nvarchar](max) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_PayCall] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PayCall', @level2type=N'COLUMN', @level2name=N'Id'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayLog](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PayLog_Id]  DEFAULT (newid()),
	[Named] [nvarchar](50) NULL,
	[ReqContent] [nvarchar](max) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_PayLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号ID' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PayLog', @level2type=N'COLUMN', @level2name=N'Id'

