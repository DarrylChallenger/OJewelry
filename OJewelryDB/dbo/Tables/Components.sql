CREATE TABLE [dbo].[Components] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [CompanyId]       INT          NOT NULL,
    [ComponentTypeId] INT          NOT NULL,
    [Name]            VARCHAR (50) NULL,
    [VendorId]        INT          NULL,
    [Price]           MONEY        NULL,
    [PricePerHour]    MONEY        NULL,
    [PricePerPiece]   MONEY        NULL,
    [MetalMetal]      VARCHAR (50) NULL,
    [MetalLabor]      MONEY        NULL,
    [StonesCtWt]      INT          NULL,
    [StoneSize]       VARCHAR (50) NULL,
    [StonePPC]        MONEY        NULL,
    [FindingsMetal]   VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Components_ToCompanies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]),
    CONSTRAINT [FK_Components_ToComponentTypes] FOREIGN KEY ([ComponentTypeId]) REFERENCES [dbo].[ComponentTypes] ([Id]),
    CONSTRAINT [FK_Components_ToVendors] FOREIGN KEY ([VendorId]) REFERENCES [dbo].[Vendors] ([Id])
);

