USE [OJewelryDB]
GO

/****** Object: Table [dbo].[MetalCodes] Script Date: 3/26/2018 9:48:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MetalCodes] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Code] NCHAR (6)    NULL,
    [Desc] VARCHAR (50) NULL
);


