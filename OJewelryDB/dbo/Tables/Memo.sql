CREATE TABLE [dbo].[Memo] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [PresenterID] INT            NOT NULL,
    [StyleID]     INT            NULL,
    [Date]        DATETIME       NULL,
    [Quantity]    INT            NOT NULL,
    [Notes]       NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Memo_ToPresenters] FOREIGN KEY ([PresenterID]) REFERENCES [dbo].[Presenters] ([Id]),
    CONSTRAINT [FK_Memo_ToStyles] FOREIGN KEY ([StyleID]) REFERENCES [dbo].[Styles] ([Id])
);

