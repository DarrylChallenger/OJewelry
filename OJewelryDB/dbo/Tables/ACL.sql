CREATE TABLE [dbo].[ACL] (
    [Id]        INT NOT NULL,
    [UserId]    INT NULL,
    [CompanyId] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ACL_ToCompanies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id])
);

