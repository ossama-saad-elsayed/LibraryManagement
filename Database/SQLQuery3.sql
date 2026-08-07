/* ============================================================
   ADD REFRESH_TOKENS TABLE FOR JWT REFRESH + LOGOUT
   Run this against your existing LibraryManagementDB
   ============================================================ */

USE LibraryManagementDB;
GO

CREATE TABLE dbo.REFRESH_TOKENS
(
    Id              INT             IDENTITY(1,1)   NOT NULL,
    UserId          INT                             NOT NULL,
    TokenHash       NVARCHAR(500)                   NOT NULL,
    JwtId           NVARCHAR(100)                   NOT NULL,
    IsUsed          BIT                             NOT NULL CONSTRAINT DF_REFRESH_TOKENS_IsUsed DEFAULT (0),
    IsRevoked       BIT                             NOT NULL CONSTRAINT DF_REFRESH_TOKENS_IsRevoked DEFAULT (0),
    CreatedAt       DATETIME                        NOT NULL CONSTRAINT DF_REFRESH_TOKENS_CreatedAt DEFAULT (GETDATE()),
    ExpiresAt       DATETIME                        NOT NULL,

    CONSTRAINT PK_REFRESH_TOKENS PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_REFRESH_TOKENS_USERS FOREIGN KEY (UserId)
        REFERENCES dbo.USERS (Id)
);
GO

CREATE NONCLUSTERED INDEX IX_REFRESH_TOKENS_TokenHash ON dbo.REFRESH_TOKENS (TokenHash);
CREATE NONCLUSTERED INDEX IX_REFRESH_TOKENS_UserId     ON dbo.REFRESH_TOKENS (UserId);
GO
