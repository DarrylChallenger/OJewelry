USE [OJewelryProdDB]
GO

/****** Object: Table [dbo].[MetalWeightUnits] Script Date: 3/27/2018 4:47:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MetalWeightUnits] (
    [Id]   INT       IDENTITY (1, 1) NOT NULL,
    [Unit] NCHAR (4) NULL
	PRIMARY KEY CLUSTERED ([Id] ASC),
);


