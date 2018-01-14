CREATE TABLE [dbo].[Collections] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [CompanyId] INT          NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Collections_ToCompanies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id])
);

