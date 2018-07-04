USE [master]
GO
/****** Object:  Database [CustomerManagement]    Script Date: 3/15/2016 1:53:22 PM ******/
CREATE DATABASE [CustomerManagement]
GO
USE [CustomerManagement]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 3/15/2016 1:53:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [bigint] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[PrimaryEmail] [nvarchar](256) NOT NULL,
	[SecondaryEmail] [nvarchar](256) NULL,
	[IndustryID] [bigint] NOT NULL,
	[EmailingIsDisabled] [bit] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ids]    Script Date: 3/15/2016 1:53:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ids](
	[EntityName] [nvarchar](100) NOT NULL,
	[NextHigh] [int] NOT NULL,
 CONSTRAINT [PK_Ids] PRIMARY KEY CLUSTERED 
(
	[EntityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Industry]    Script Date: 3/15/2016 1:53:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Industry](
	[IndustryID] [bigint] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Industry] PRIMARY KEY CLUSTERED 
(
	[IndustryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Ids] ([EntityName], [NextHigh]) VALUES (N'Customer', 1)
GO
INSERT [dbo].[Ids] ([EntityName], [NextHigh]) VALUES (N'Industry', 0)
GO
INSERT [dbo].[Industry] ([IndustryID], [Name]) VALUES (1, N'Cars')
GO
INSERT [dbo].[Industry] ([IndustryID], [Name]) VALUES (2, N'Pharmacy')
GO
INSERT [dbo].[Industry] ([IndustryID], [Name]) VALUES (3, N'Other')
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Customer_Name]    Script Date: 3/15/2016 1:53:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Customer_Name] ON [dbo].[Customer]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
