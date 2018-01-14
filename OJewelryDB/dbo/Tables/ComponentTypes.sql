CREATE TABLE [dbo].[ComponentTypes] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [Sequence] INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

