CREATE TABLE [dbo].[PaymentHistory]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[PaymentId] UNIQUEIDENTIFIER NOT NULL,
	[Type] tinyint NOT NULL,
	[User] NVARCHAR(256) NOT NULL,
	[Error] NVARCHAR(1024) NULL,
    [CreatedAt] DateTimeOffset NOT NULL,

    CONSTRAINT [PK_PaymentHistorys] PRIMARY KEY (Id), 
    CONSTRAINT [FK_PaymentHistory_Payments] FOREIGN KEY ([PaymentId]) REFERENCES [Payments]([Id]), 
)
