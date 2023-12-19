CREATE TABLE [dbo].[Payments]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Status] tinyint NOT NULL,
	[Amount] Decimal(12,2) NOT NULL, 
    [FromAccount] NVARCHAR(20) NOT NULL, 
    [ToAccount] NVARCHAR(20) NOT NULL, 
    [Reference] NVARCHAR(20) NOT NULL, 
    [Date] DATE NOT NULL,
    [UpdatedAt] DateTimeOffset NOT NULL,

    CONSTRAINT [PK_Payments] PRIMARY KEY (Id), 
    CONSTRAINT [UK_Reference] UNIQUE(Reference), 
)
