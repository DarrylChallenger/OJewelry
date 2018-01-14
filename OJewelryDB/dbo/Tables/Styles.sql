CREATE TABLE [dbo].[Styles] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [StyleNum]      VARCHAR (15)   NOT NULL,
    [StyleName]     VARCHAR (50)   NOT NULL,
    [Desc]          VARCHAR (150)  NULL,
    [JewelryTypeId] INT            NULL,
    [CollectionId]  INT            NOT NULL,
    [MetalWeight]   MONEY          NULL,
    [IntroDate]     DATE           NULL,
    [Image]         IMAGE          NULL,
    [Width]         DECIMAL (8, 5) NULL,
    [Length]        DECIMAL (8, 5) NULL,
    [ChainLength]   DECIMAL (8, 5) NULL,
    [RetailRatio]   DECIMAL (8, 5) NULL,
    [RedlineRatio]  DECIMAL (8, 5) NULL,
    [Quantity]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Style_ToCollections] FOREIGN KEY ([CollectionId]) REFERENCES [dbo].[Collections] ([Id]),
    CONSTRAINT [FK_Style_TOJewelryDBTypes] FOREIGN KEY ([JewelryTypeId]) REFERENCES [dbo].[JewelryTypes] ([Id])
);

