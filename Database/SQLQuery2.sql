/* ============================================================
   LIBRARY MANAGEMENT DATABASE — SEED / MOCK DATA
   Run against LibraryManagementDB (schema created previously)
   Order: AUTHORS -> CATEGORIES -> PUBLISHERS -> USERS
          -> BOOKS -> BORROW_RECORDS -> FINES -> BOOK_RESERVATIONS
   ============================================================ */

USE LibraryManagementDB;
GO

-- ============================================================
-- 1. AUTHORS
-- ============================================================
SET IDENTITY_INSERT dbo.AUTHORS ON;

INSERT INTO dbo.AUTHORS (Id, Name, Biography) VALUES
(1, N'J.K. Rowling',        N'British author best known for the Harry Potter fantasy series, one of the best-selling book series in history.'),
(2, N'George Orwell',       N'English novelist and essayist known for dystopian and political works including 1984 and Animal Farm.'),
(3, N'Isaac Asimov',        N'American author and biochemist, a prolific writer of science fiction and popular science, best known for the Foundation series.'),
(4, N'Agatha Christie',     N'English writer known as the "Queen of Mystery," creator of detectives Hercule Poirot and Miss Marple.'),
(5, N'Yuval Noah Harari',   N'Israeli historian and author of popular non-fiction works exploring human history and the future of the species.'),
(6, N'Martin Kleppmann',    N'Software engineer and researcher writing on distributed systems and data-intensive application design.'),
(7, N'Robert C. Martin',    N'American software engineer and author, widely known for his writing on software craftsmanship and clean code practices.');

SET IDENTITY_INSERT dbo.AUTHORS OFF;
GO

-- ============================================================
-- 2. CATEGORIES
-- ============================================================
SET IDENTITY_INSERT dbo.CATEGORIES ON;

INSERT INTO dbo.CATEGORIES (Id, Name, Description) VALUES
(1, N'Fiction',            N'Narrative literary works of imaginative storytelling.'),
(2, N'Science Fiction',    N'Speculative fiction exploring futuristic concepts, technology, and space.'),
(3, N'Technology',         N'Non-fiction works on software, engineering, and computing practices.'),
(4, N'History',            N'Non-fiction works examining historical events and human civilization.'),
(5, N'Mystery',            N'Fiction centered on the solving of a crime or puzzle.');

SET IDENTITY_INSERT dbo.CATEGORIES OFF;
GO

-- ============================================================
-- 3. PUBLISHERS
-- ============================================================
SET IDENTITY_INSERT dbo.PUBLISHERS ON;

INSERT INTO dbo.PUBLISHERS (Id, Name, Address, ContactNumber) VALUES
(1, N'Penguin Random House', N'1745 Broadway, New York, NY, USA',       N'+1-212-555-0101'),
(2, N'HarperCollins',        N'195 Broadway, New York, NY, USA',        N'+1-212-555-0102'),
(3, N'Simon & Schuster',     N'1230 Avenue of the Americas, NY, USA',   N'+1-212-555-0103'),
(4, N'O''Reilly Media',      N'1005 Gravenstein Hwy N, Sebastopol, CA', N'+1-707-555-0104');

SET IDENTITY_INSERT dbo.PUBLISHERS OFF;
GO

-- ============================================================
-- 4. USERS  (2 Admins, 5 Members)
-- ============================================================
SET IDENTITY_INSERT dbo.USERS ON;

INSERT INTO dbo.USERS (Id, Name, Email, PasswordHash, Role, CreatedAt) VALUES
(1, N'Sarah Mitchell', N'sarah.mitchell@athenaeum.com',  N'$2a$10$hash.placeholder.admin01', N'Admin',  '2025-01-10T09:00:00'),
(2, N'James Carter',   N'james.carter@athenaeum.com',    N'$2a$10$hash.placeholder.admin02', N'Admin',  '2025-02-15T09:00:00'),
(3, N'Emily Davis',    N'emily.davis@example.com',       N'$2a$10$hash.placeholder.mem01',   N'Member', '2025-03-05T14:20:00'),
(4, N'Michael Chen',   N'michael.chen@example.com',      N'$2a$10$hash.placeholder.mem02',   N'Member', '2025-04-18T11:45:00'),
(5, N'Olivia Martinez',N'olivia.martinez@example.com',   N'$2a$10$hash.placeholder.mem03',   N'Member', '2025-05-22T16:10:00'),
(6, N'Daniel Brown',   N'daniel.brown@example.com',      N'$2a$10$hash.placeholder.mem04',   N'Member', '2025-06-30T10:05:00'),
(7, N'Sophia Wilson',  N'sophia.wilson@example.com',     N'$2a$10$hash.placeholder.mem05',   N'Member', '2025-08-14T13:35:00');

SET IDENTITY_INSERT dbo.USERS OFF;
GO

-- ============================================================
-- 5. BOOKS (linked to Authors, Categories, Publishers)
-- ============================================================
SET IDENTITY_INSERT dbo.BOOKS ON;

INSERT INTO dbo.BOOKS (Id, Title, ISBN, PublicationYear, CopiesOwned, AvailableCopies, AuthorId, CategoryId, PublisherId) VALUES
(1,  N'Harry Potter and the Sorcerer''s Stone',       N'9780439708180', 1997, 5, 3, 1, 1, 1),
(2,  N'1984',                                          N'9780451524935', 1949, 4, 2, 2, 1, 1),
(3,  N'Animal Farm',                                   N'9780451526342', 1945, 3, 3, 2, 1, 1),
(4,  N'Foundation',                                    N'9780553293357', 1951, 4, 1, 3, 2, 2),
(5,  N'I, Robot',                                      N'9780553294385', 1950, 3, 0, 3, 2, 2),
(6,  N'Murder on the Orient Express',                  N'9780062693662', 1934, 5, 4, 4, 5, 2),
(7,  N'And Then There Were None',                      N'9780062073488', 1939, 4, 2, 4, 5, 3),
(8,  N'Sapiens: A Brief History of Humankind',         N'9780062316097', 2011, 6, 3, 5, 4, 1),
(9,  N'Homo Deus: A Brief History of Tomorrow',        N'9780062464316', 2016, 3, 3, 5, 4, 1),
(10, N'Designing Data-Intensive Applications',         N'9781449373320', 2017, 3, 1, 6, 3, 4),
(11, N'Clean Code: A Handbook of Agile Craftsmanship', N'9780132350884', 2008, 4, 2, 7, 3, 4);

SET IDENTITY_INSERT dbo.BOOKS OFF;
GO

-- ============================================================
-- 6. BORROW_RECORDS
--    Mix of: active/current, on-time returned, and overdue
-- ============================================================
SET IDENTITY_INSERT dbo.BORROW_RECORDS ON;

INSERT INTO dbo.BORROW_RECORDS (Id, BookId, UserId, BorrowDate, DueDate, ReturnDate, Status) VALUES
(1, 1,  3, '2026-06-20T10:00:00', '2026-07-04T10:00:00', NULL,                    N'Overdue'),   -- Emily Davis, Harry Potter, past due, not returned
(2, 2,  4, '2026-07-01T09:30:00', '2026-07-15T09:30:00', NULL,                    N'Borrowed'),  -- Michael Chen, 1984, currently active
(3, 4,  5, '2026-06-01T12:00:00', '2026-06-15T12:00:00', '2026-06-14T15:45:00',   N'Returned'),  -- Olivia Martinez, Foundation, returned on time
(4, 5,  6, '2026-06-25T14:15:00', '2026-07-09T14:15:00', NULL,                    N'Overdue'),   -- Daniel Brown, I Robot, past due, not returned
(5, 6,  7, '2026-05-20T11:00:00', '2026-06-03T11:00:00', '2026-06-01T09:20:00',   N'Returned'),  -- Sophia Wilson, Murder on the Orient Express, returned early
(6, 8,  3, '2026-07-05T16:30:00', '2026-07-19T16:30:00', NULL,                    N'Borrowed'),  -- Emily Davis, Sapiens, currently active
(7, 10, 4, '2026-06-28T13:00:00', '2026-07-12T13:00:00', NULL,                    N'Borrowed'),  -- Michael Chen, Designing Data-Intensive Applications, active, due soon
(8, 11, 5, '2026-06-10T10:45:00', '2026-06-24T10:45:00', NULL,                    N'Overdue');   -- Olivia Martinez, Clean Code, past due, not returned

SET IDENTITY_INSERT dbo.BORROW_RECORDS OFF;
GO

-- ============================================================
-- 7. FINES  (linked to the overdue borrow records above)
-- ============================================================
SET IDENTITY_INSERT dbo.FINES ON;

INSERT INTO dbo.FINES (Id, BorrowRecordId, Amount, IsPaid, PaidDate) VALUES
(1, 1, 5.00, 0, NULL),                       -- Emily Davis, Harry Potter overdue fine, unpaid
(2, 4, 7.50, 0, NULL),                       -- Daniel Brown, I Robot overdue fine, unpaid
(3, 8, 3.25, 1, '2026-07-10T17:00:00');      -- Olivia Martinez, Clean Code overdue fine, paid

SET IDENTITY_INSERT dbo.FINES OFF;
GO

-- ============================================================
-- 8. BOOK_RESERVATIONS  (pending and fulfilled examples)
-- ============================================================
SET IDENTITY_INSERT dbo.BOOK_RESERVATIONS ON;

INSERT INTO dbo.BOOK_RESERVATIONS (Id, BookId, UserId, ReservationDate, Status) VALUES
(1, 5,  3, '2026-07-08T10:00:00', N'Pending'),    -- Emily Davis waiting on I, Robot (currently 0 available)
(2, 1,  7, '2026-07-09T09:15:00', N'Pending'),    -- Sophia Wilson waiting on Harry Potter
(3, 11, 6, '2026-06-20T11:30:00', N'Fulfilled'),  -- Daniel Brown's reservation on Clean Code was fulfilled
(4, 9,  4, '2026-06-15T14:00:00', N'Fulfilled');  -- Michael Chen's reservation on Homo Deus was fulfilled

SET IDENTITY_INSERT dbo.BOOK_RESERVATIONS OFF;
GO

/* ============================================================
   END OF SEED DATA SCRIPT
   ============================================================ */