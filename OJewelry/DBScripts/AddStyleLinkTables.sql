
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object: Table [dbo].[StyleCastings] Script Date: 4/21/2018 9:57:34 AM ******/
CREATE TABLE [dbo].[StyleCastings] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [StyleId]   INT NULL,
    [CastingId] INT NULL
);


/****** Object: Table [dbo].[StyleLabor] Script Date: 4/21/2018 9:57:47 AM ******/
CREATE TABLE [dbo].[StyleLabor] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [StyleId] INT NOT NULL,
    [LaborId] INT NOT NULL
);

/****** Object: Table [dbo].[StyleMisc] Script Date: 4/21/2018 9:57:56 AM ******/
CREATE TABLE [dbo].[StyleMisc] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [StyleId] INT NOT NULL,
    [MiscId]  INT NOT NULL
);


