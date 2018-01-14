CREATE TABLE [dbo].[JewelryTypes] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [CompanyId] INT          NULL,
    [Name]      VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JewelryTypes_ToTable] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id])
);

