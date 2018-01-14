CREATE TABLE [dbo].[Clients] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [Phone]     VARCHAR (10) NULL,
    [Email]     VARCHAR (50) NULL,
    [CompanyID] INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Clients_ToCompanies] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Companies] ([Id])
);

