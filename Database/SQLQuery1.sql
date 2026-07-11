/* ============================================================
   LIBRARY MANAGEMENT DATABASE
   Generated from ERD (AUTHORS, CATEGORIES, PUBLISHERS, BOOKS,
   USERS, BORROW_RECORDS, BOOK_RESERVATIONS, FINES)
   Target: SQL Server (T-SQL)
   ============================================================ */

-- ============================================================
-- 1. CREATE DATABASE
-- ============================================================
IF DB_ID('LibraryManagementDB') IS NOT NULL
BEGIN
    ALTER DATABASE LibraryManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE LibraryManagementDB;
END
GO

CREATE DATABASE LibraryManagementDB;
GO

USE LibraryManagementDB;
GO

-- ============================================================
-- 2. TABLE: AUTHORS
-- ============================================================
CREATE TABLE dbo.AUTHORS
(
    Id          INT             IDENTITY(1,1)   NOT NULL,
    Name        NVARCHAR(150)                   NOT NULL,
    Biography   NVARCHAR(MAX)                   NULL,

    CONSTRAINT PK_AUTHORS PRIMARY KEY CLUSTERED (Id)
);
GO

-- ============================================================
-- 3. TABLE: CATEGORIES
-- ============================================================
CREATE TABLE dbo.CATEGORIES
(
    Id          INT             IDENTITY(1,1)   NOT NULL,
    Name        NVARCHAR(100)                   NOT NULL,
    Description NVARCHAR(MAX)                   NULL,

    CONSTRAINT PK_CATEGORIES PRIMARY KEY CLUSTERED (Id)
);
GO

-- ============================================================
-- 4. TABLE: PUBLISHERS
-- ============================================================
CREATE TABLE dbo.PUBLISHERS
(
    Id              INT             IDENTITY(1,1)   NOT NULL,
    Name            NVARCHAR(150)                   NOT NULL,
    Address         NVARCHAR(255)                   NULL,
    ContactNumber   NVARCHAR(30)                    NULL,

    CONSTRAINT PK_PUBLISHERS PRIMARY KEY CLUSTERED (Id)
);
GO

-- ============================================================
-- 5. TABLE: USERS
-- ============================================================
CREATE TABLE dbo.USERS
(
    Id              INT             IDENTITY(1,1)   NOT NULL,
    Name            NVARCHAR(150)                   NOT NULL,
    Email           NVARCHAR(255)                   NOT NULL,
    PasswordHash    NVARCHAR(255)                   NOT NULL,
    Role            NVARCHAR(20)                    NOT NULL CONSTRAINT DF_USERS_Role DEFAULT ('Member'),
    CreatedAt       DATETIME                        NOT NULL CONSTRAINT DF_USERS_CreatedAt DEFAULT (GETDATE()),

    CONSTRAINT PK_USERS PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_USERS_Email UNIQUE (Email),
    CONSTRAINT CK_USERS_Role CHECK (Role IN (N'Member', N'Librarian', N'Admin'))
);
GO

-- ============================================================
-- 6. TABLE: BOOKS
-- ============================================================
CREATE TABLE dbo.BOOKS
(
    Id                  INT             IDENTITY(1,1)   NOT NULL,
    Title               NVARCHAR(255)                   NOT NULL,
    ISBN                NVARCHAR(20)                    NOT NULL,
    PublicationYear     INT                             NULL,
    CopiesOwned         INT                             NOT NULL CONSTRAINT DF_BOOKS_CopiesOwned DEFAULT (0),
    AvailableCopies     INT                             NOT NULL CONSTRAINT DF_BOOKS_AvailableCopies DEFAULT (0),
    AuthorId            INT                             NOT NULL,
    CategoryId          INT                             NOT NULL,
    PublisherId         INT                             NOT NULL,

    CONSTRAINT PK_BOOKS PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_BOOKS_ISBN UNIQUE (ISBN),
    CONSTRAINT FK_BOOKS_AUTHORS FOREIGN KEY (AuthorId)
        REFERENCES dbo.AUTHORS (Id),
    CONSTRAINT FK_BOOKS_CATEGORIES FOREIGN KEY (CategoryId)
        REFERENCES dbo.CATEGORIES (Id),
    CONSTRAINT FK_BOOKS_PUBLISHERS FOREIGN KEY (PublisherId)
        REFERENCES dbo.PUBLISHERS (Id),
    CONSTRAINT CK_BOOKS_CopiesOwned CHECK (CopiesOwned >= 0),
    CONSTRAINT CK_BOOKS_AvailableCopies CHECK (AvailableCopies >= 0),
    CONSTRAINT CK_BOOKS_PublicationYear CHECK (PublicationYear IS NULL OR PublicationYear BETWEEN 1450 AND 2100)
);
GO

-- ============================================================
-- 7. TABLE: BORROW_RECORDS
-- ============================================================
CREATE TABLE dbo.BORROW_RECORDS
(
    Id          INT             IDENTITY(1,1)   NOT NULL,
    BookId      INT                             NOT NULL,
    UserId      INT                             NOT NULL,
    BorrowDate  DATETIME                        NOT NULL CONSTRAINT DF_BORROW_RECORDS_BorrowDate DEFAULT (GETDATE()),
    DueDate     DATETIME                        NOT NULL,
    ReturnDate  DATETIME                        NULL,
    Status      NVARCHAR(20)                    NOT NULL CONSTRAINT DF_BORROW_RECORDS_Status DEFAULT (N'Borrowed'),

    CONSTRAINT PK_BORROW_RECORDS PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_BORROW_RECORDS_BOOKS FOREIGN KEY (BookId)
        REFERENCES dbo.BOOKS (Id),
    CONSTRAINT FK_BORROW_RECORDS_USERS FOREIGN KEY (UserId)
        REFERENCES dbo.USERS (Id),
    CONSTRAINT CK_BORROW_RECORDS_Status CHECK (Status IN (N'Borrowed', N'Returned', N'Overdue', N'Lost')),
    CONSTRAINT CK_BORROW_RECORDS_Dates CHECK (ReturnDate IS NULL OR ReturnDate >= BorrowDate)
);
GO

-- ============================================================
-- 8. TABLE: BOOK_RESERVATIONS
-- ============================================================
CREATE TABLE dbo.BOOK_RESERVATIONS
(
    Id              INT             IDENTITY(1,1)   NOT NULL,
    BookId          INT                             NOT NULL,
    UserId          INT                             NOT NULL,
    ReservationDate DATETIME                        NOT NULL CONSTRAINT DF_BOOK_RESERVATIONS_ReservationDate DEFAULT (GETDATE()),
    Status          NVARCHAR(20)                    NOT NULL CONSTRAINT DF_BOOK_RESERVATIONS_Status DEFAULT (N'Pending'),

    CONSTRAINT PK_BOOK_RESERVATIONS PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_BOOK_RESERVATIONS_BOOKS FOREIGN KEY (BookId)
        REFERENCES dbo.BOOKS (Id),
    CONSTRAINT FK_BOOK_RESERVATIONS_USERS FOREIGN KEY (UserId)
        REFERENCES dbo.USERS (Id),
    CONSTRAINT CK_BOOK_RESERVATIONS_Status CHECK (Status IN (N'Pending', N'Fulfilled', N'Cancelled', N'Expired'))
);
GO

-- ============================================================
-- 9. TABLE: FINES
-- ============================================================
CREATE TABLE dbo.FINES
(
    Id              INT             IDENTITY(1,1)   NOT NULL,
    BorrowRecordId  INT                             NOT NULL,
    Amount          DECIMAL(10,2)                   NOT NULL,
    IsPaid          BIT                             NOT NULL CONSTRAINT DF_FINES_IsPaid DEFAULT (0),
    PaidDate        DATETIME                        NULL,

    CONSTRAINT PK_FINES PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_FINES_BORROW_RECORDS FOREIGN KEY (BorrowRecordId)
        REFERENCES dbo.BORROW_RECORDS (Id),
    CONSTRAINT CK_FINES_Amount CHECK (Amount >= 0),
    CONSTRAINT CK_FINES_PaidDate CHECK (
        (IsPaid = 0 AND PaidDate IS NULL) OR
        (IsPaid = 1 AND PaidDate IS NOT NULL)
    )
);
GO

-- ============================================================
-- 10. INDEXES (non-key, to support common lookups)
-- ============================================================

-- BOOKS: frequently filtered/searched by title and by FK lookups
CREATE NONCLUSTERED INDEX IX_BOOKS_Title            ON dbo.BOOKS (Title);
CREATE NONCLUSTERED INDEX IX_BOOKS_AuthorId          ON dbo.BOOKS (AuthorId);
CREATE NONCLUSTERED INDEX IX_BOOKS_CategoryId        ON dbo.BOOKS (CategoryId);
CREATE NONCLUSTERED INDEX IX_BOOKS_PublisherId       ON dbo.BOOKS (PublisherId);

-- BORROW_RECORDS: frequently queried by user, by book, and by status
CREATE NONCLUSTERED INDEX IX_BORROW_RECORDS_UserId   ON dbo.BORROW_RECORDS (UserId);
CREATE NONCLUSTERED INDEX IX_BORROW_RECORDS_BookId   ON dbo.BORROW_RECORDS (BookId);
CREATE NONCLUSTERED INDEX IX_BORROW_RECORDS_Status   ON dbo.BORROW_RECORDS (Status);

-- BOOK_RESERVATIONS: frequently queried by user, by book, and by status
CREATE NONCLUSTERED INDEX IX_BOOK_RESERVATIONS_UserId ON dbo.BOOK_RESERVATIONS (UserId);
CREATE NONCLUSTERED INDEX IX_BOOK_RESERVATIONS_BookId ON dbo.BOOK_RESERVATIONS (BookId);
CREATE NONCLUSTERED INDEX IX_BOOK_RESERVATIONS_Status ON dbo.BOOK_RESERVATIONS (Status);

-- FINES: frequently queried by borrow record and by paid status
CREATE NONCLUSTERED INDEX IX_FINES_BorrowRecordId    ON dbo.FINES (BorrowRecordId);
CREATE NONCLUSTERED INDEX IX_FINES_IsPaid             ON dbo.FINES (IsPaid);
GO

/* ============================================================
   END OF SCRIPT
   ============================================================ */