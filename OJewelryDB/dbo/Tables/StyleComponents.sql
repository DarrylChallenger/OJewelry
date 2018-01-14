CREATE TABLE [dbo].[StyleComponents] (
    [Id]          INT NOT NULL,
    [StyleId]     INT NOT NULL,
    [ComponentId] INT NOT NULL,
    [Quantity]    INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StyleComponents_ToComponents] FOREIGN KEY ([ComponentId]) REFERENCES [dbo].[Components] ([Id]),
    CONSTRAINT [FK_StyleComponents_ToStyles] FOREIGN KEY ([StyleId]) REFERENCES [dbo].[Styles] ([Id])
);

