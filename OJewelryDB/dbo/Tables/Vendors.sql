CREATE TABLE [dbo].[Vendors] (
    [Id]    INT          IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR (50) NOT NULL,
    [Phone] VARCHAR (50) NOT NULL,
    [Email] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

