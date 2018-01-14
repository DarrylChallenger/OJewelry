CREATE TABLE [dbo].[SalesLedger] (
    [Id]        INT            NOT NULL,
    [StyleId]   INT            NULL,
    [StyleInfo] NVARCHAR (512) NULL,
    [UnitsSold] INT            NULL,
    [Date]      DATETIME       NULL,
    [Notes]     NCHAR (10)     NULL,
    [BuyerId]   INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalesLedger_ToBuyers] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Buyers] ([Id])
);

