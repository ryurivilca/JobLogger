USE [DbTest]
GO

/****** Object:  Table [dbo].[Log]    Script Date: 2/23/2016 10:08:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[message] [varchar](50) NULL,
	[type] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


