USE [master]
GO
/****** Object:  Database [OJewelryDB]    Script Date: 1/14/2018 12:10:11 PM ******/
/*
CREATE DATABASE [OJewelryDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OJewelryDBDeDev', FILENAME = N'D:\Data Sources\MSSQLLocalDB\OJewelryDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OJewelryDBDeDev_log', FILENAME = N'D:\Data Sources\MSSQLLocalDB\OJewelryDB.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
*/
ALTER DATABASE [OJewelryDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OJewelryDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OJewelryDB] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [OJewelryDB] SET ANSI_NULLS ON 
GO
ALTER DATABASE [OJewelryDB] SET ANSI_PADDING ON 
GO
ALTER DATABASE [OJewelryDB] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [OJewelryDB] SET ARITHABORT ON 
GO
ALTER DATABASE [OJewelryDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [OJewelryDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OJewelryDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OJewelryDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OJewelryDB] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [OJewelryDB] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [OJewelryDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OJewelryDB] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [OJewelryDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OJewelryDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [OJewelryDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OJewelryDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OJewelryDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OJewelryDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OJewelryDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OJewelryDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OJewelryDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OJewelryDB] SET RECOVERY FULL 
GO
ALTER DATABASE [OJewelryDB] SET  MULTI_USER 
GO
ALTER DATABASE [OJewelryDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OJewelryDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OJewelryDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OJewelryDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OJewelryDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OJewelryDB] SET QUERY_STORE = OFF
GO
USE [OJewelryDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [OJewelryDB]
GO

drop table dbo.acl
drop table dbo.Clients
drop table dbo.Memo
drop table dbo.Presenters
drop table dbo.SalesLedger
drop table dbo.StyleComponents
drop table dbo.Styles
drop table dbo.Buyers
drop table dbo.Collections
drop table dbo.Components
drop table dbo.ComponentTypes
drop table dbo.JewelryTypes
drop table dbo.Vendors
drop table dbo.Companies

/****** Object:  Table [dbo].[ACL]    Script Date: 1/14/2018 12:10:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACL](
	[Id] [int] NOT NULL,
	[UserId] [int] NULL,
	[CompanyId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Buyers]    Script Date: 1/14/2018 12:10:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buyers](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 1/14/2018 12:10:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Phone] [varchar](10) NULL,
	[Email] [varchar](50) NULL,
	[CompanyID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Collections]    Script Date: 1/14/2018 12:10:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Collections](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 1/14/2018 12:10:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Components]    Script Date: 1/14/2018 12:10:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Components](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ComponentTypeId] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[VendorId] [int] NULL,
	[Price] [money] NULL,
	[PricePerHour] [money] NULL,
	[PricePerPiece] [money] NULL,
	[MetalMetal] [varchar](50) NULL,
	[MetalLabor] [money] NULL,
	[StonesCtWt] [int] NULL,
	[StoneSize] [varchar](50) NULL,
	[StonePPC] [money] NULL,
	[FindingsMetal] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComponentTypes]    Script Date: 1/14/2018 12:10:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComponentTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Sequence] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JewelryTypes]    Script Date: 1/14/2018 12:10:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JewelryTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[Name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Memo]    Script Date: 1/14/2018 12:10:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Memo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PresenterID] [int] NOT NULL,
	[StyleID] [int] NULL,
	[Date] [datetime] NULL,
	[Quantity] [int] NOT NULL,
	[Notes] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Presenters]    Script Date: 1/14/2018 12:10:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Presenters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Phone] [nvarchar](10) NULL,
	[Email] [nvarchar](50) NULL,
	[CompanyId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesLedger]    Script Date: 1/14/2018 12:10:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesLedger](
	[Id] [int] NOT NULL,
	[StyleId] [int] NULL,
	[StyleInfo] [nvarchar](512) NULL,
	[UnitsSold] [int] NULL,
	[Date] [datetime] NULL,
	[Notes] [nchar](10) NULL,
	[BuyerId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StyleComponents]    Script Date: 1/14/2018 12:10:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StyleComponents](
	[Id] [int] NOT NULL,
	[StyleId] [int] NOT NULL,
	[ComponentId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Styles]    Script Date: 1/14/2018 12:10:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Styles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StyleNum] [varchar](15) NOT NULL,
	[StyleName] [varchar](50) NOT NULL,
	[Desc] [varchar](150) NULL,
	[JewelryTypeId] [int] NULL,
	[CollectionId] [int] NOT NULL,
	[MetalWeight] [money] NULL,
	[IntroDate] [date] NULL,
	[Image] [image] NULL,
	[Width] [decimal](8, 5) NULL,
	[Length] [decimal](8, 5) NULL,
	[ChainLength] [decimal](8, 5) NULL,
	[RetailRatio] [decimal](8, 5) NULL,
	[RedlineRatio] [decimal](8, 5) NULL,
	[Quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendors]    Script Date: 1/14/2018 12:10:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Phone] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ACL]  WITH CHECK ADD  CONSTRAINT [FK_ACL_ToCompanies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[ACL] CHECK CONSTRAINT [FK_ACL_ToCompanies]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_ToCompanies] FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_ToCompanies]
GO
ALTER TABLE [dbo].[Collections]  WITH CHECK ADD  CONSTRAINT [FK_Collections_ToCompanies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Collections] CHECK CONSTRAINT [FK_Collections_ToCompanies]
GO
ALTER TABLE [dbo].[Components]  WITH CHECK ADD  CONSTRAINT [FK_Components_ToCompanies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Components] CHECK CONSTRAINT [FK_Components_ToCompanies]
GO
ALTER TABLE [dbo].[Components]  WITH CHECK ADD  CONSTRAINT [FK_Components_ToComponentTypes] FOREIGN KEY([ComponentTypeId])
REFERENCES [dbo].[ComponentTypes] ([Id])
GO
ALTER TABLE [dbo].[Components] CHECK CONSTRAINT [FK_Components_ToComponentTypes]
GO
ALTER TABLE [dbo].[Components]  WITH CHECK ADD  CONSTRAINT [FK_Components_ToVendors] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendors] ([Id])
GO
ALTER TABLE [dbo].[Components] CHECK CONSTRAINT [FK_Components_ToVendors]
GO
ALTER TABLE [dbo].[JewelryTypes]  WITH CHECK ADD  CONSTRAINT [FK_JewelryTypes_ToTable] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[JewelryTypes] CHECK CONSTRAINT [FK_JewelryTypes_ToTable]
GO
ALTER TABLE [dbo].[Memo]  WITH CHECK ADD  CONSTRAINT [FK_Memo_ToPresenters] FOREIGN KEY([PresenterID])
REFERENCES [dbo].[Presenters] ([Id])
GO
ALTER TABLE [dbo].[Memo] CHECK CONSTRAINT [FK_Memo_ToPresenters]
GO
ALTER TABLE [dbo].[Memo]  WITH CHECK ADD  CONSTRAINT [FK_Memo_ToStyles] FOREIGN KEY([StyleID])
REFERENCES [dbo].[Styles] ([Id])
GO
ALTER TABLE [dbo].[Memo] CHECK CONSTRAINT [FK_Memo_ToStyles]
GO
ALTER TABLE [dbo].[Presenters]  WITH CHECK ADD  CONSTRAINT [FK_Presenters_ToCompanies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Presenters] CHECK CONSTRAINT [FK_Presenters_ToCompanies]
GO
ALTER TABLE [dbo].[SalesLedger]  WITH CHECK ADD  CONSTRAINT [FK_SalesLedger_ToBuyers] FOREIGN KEY([BuyerId])
REFERENCES [dbo].[Buyers] ([Id])
GO
ALTER TABLE [dbo].[SalesLedger] CHECK CONSTRAINT [FK_SalesLedger_ToBuyers]
GO
ALTER TABLE [dbo].[StyleComponents]  WITH CHECK ADD  CONSTRAINT [FK_StyleComponents_ToComponents] FOREIGN KEY([ComponentId])
REFERENCES [dbo].[Components] ([Id])
GO
ALTER TABLE [dbo].[StyleComponents] CHECK CONSTRAINT [FK_StyleComponents_ToComponents]
GO
ALTER TABLE [dbo].[StyleComponents]  WITH CHECK ADD  CONSTRAINT [FK_StyleComponents_ToStyles] FOREIGN KEY([StyleId])
REFERENCES [dbo].[Styles] ([Id])
GO
ALTER TABLE [dbo].[StyleComponents] CHECK CONSTRAINT [FK_StyleComponents_ToStyles]
GO
ALTER TABLE [dbo].[Styles]  WITH CHECK ADD  CONSTRAINT [FK_Style_ToCollections] FOREIGN KEY([CollectionId])
REFERENCES [dbo].[Collections] ([Id])
GO
ALTER TABLE [dbo].[Styles] CHECK CONSTRAINT [FK_Style_ToCollections]
GO
ALTER TABLE [dbo].[Styles]  WITH CHECK ADD  CONSTRAINT [FK_Style_TOJewelryDBTypes] FOREIGN KEY([JewelryTypeId])
REFERENCES [dbo].[JewelryTypes] ([Id])
GO
ALTER TABLE [dbo].[Styles] CHECK CONSTRAINT [FK_Style_TOJewelryDBTypes]
GO
USE [master]
GO
ALTER DATABASE [OJewelryDB] SET  READ_WRITE 
GO
