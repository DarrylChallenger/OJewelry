CREATE TABLE [dbo].[Presenters] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NULL,
    [Phone]     NVARCHAR (10) NULL,
    [Email]     NVARCHAR (50) NULL,
    [CompanyId] INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Presenters_ToCompanies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id])
);

