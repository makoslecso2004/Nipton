-- ==========================================
-- Nipton (Neptun-klón) Tesztadat Inicializáló (v2 - Nagy adathalmaz)
-- ==========================================

-- 1. MEGLÉVŐ ADATOK TÖRLÉSE (A külső kulcsok miatt a megfelelő sorrendben!)
DELETE FROM NotificationLogs;
DELETE FROM Schedules;
DELETE FROM CourseStudents;
DELETE FROM CourseTeachers;
DELETE FROM Courses;
DELETE FROM Subjects;
DELETE FROM Users;

-- 2. FELHASZNÁLÓK (Users)
-- 2 Admin, 6 Oktató, 16 Hallgató (10 Nappali, 6 Levelező)
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (Id, Username, Email, Password, Role, StudentStudyForm, IsActive) VALUES
-- Adminok
(1, 'Admin', 'admin@nipton.hu', 'admin123', 'Admin', NULL, 1),
(2, 'Szuper Admin', 'superadmin@nipton.hu', 'admin123', 'Admin', NULL, 1),
-- Oktatók
(3, 'Dr. Kovács Péter', 'kovacs.peter@nipton.hu', 'oktato123', 'Teacher', NULL, 1),
(4, 'Nagy Anna', 'nagy.anna@nipton.hu', 'oktato123', 'Teacher', NULL, 1),
(5, 'Prof. Szabó Gábor', 'szabo.gabor@nipton.hu', 'oktato123', 'Teacher', NULL, 1),
(6, 'Dr. Tóth Zoltán', 'toth.zoltan@nipton.hu', 'oktato123', 'Teacher', NULL, 1),
(7, 'Horváth Mária', 'horvath.maria@nipton.hu', 'oktato123', 'Teacher', NULL, 1),
(8, 'Kiss László', 'kiss.laszlo@nipton.hu', 'oktato123', 'Teacher', NULL, 0), -- Ő egy inaktív oktató
-- Nappalis Hallgatók (9-18)
(9, 'Kiss Gábor', 'kiss.gabor@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(10, 'Tóth Csilla', 'toth.csilla@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(11, 'Varga Balázs', 'varga.balazs@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(12, 'Farkas Bence', 'farkas.bence@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(13, 'Simon Petra', 'simon.petra@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(14, 'Nemes Dávid', 'nemes.david@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(15, 'Kocsis Réka', 'kocsis.reka@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(16, 'Balogh Máté', 'balogh.mate@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(17, 'Lukács Zsófia', 'lukacs.zsofia@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(18, 'Kelemen Ádám', 'kelemen.adam@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
-- Levelezős Hallgatók (19-24)
(19, 'Fehér Róbert', 'feher.robert@student.nipton.hu', 'diak123', 'Student', 'PartTime', 1),
(20, 'Gál Edina', 'gal.edina@student.nipton.hu', 'diak123', 'Student', 'PartTime', 1),
(21, 'Juhász Tamás', 'juhasz.tamas@student.nipton.hu', 'diak123', 'Student', 'PartTime', 1),
(22, 'Magyar Eszter', 'magyar.eszter@student.nipton.hu', 'diak123', 'Student', 'PartTime', 1),
(23, 'Papp Gergő', 'papp.gergo@student.nipton.hu', 'diak123', 'Student', 'PartTime', 1),
(24, 'Takács Orsolya', 'takacs.orsolya@student.nipton.hu', 'diak123', 'Student', 'PartTime', 1);
SET IDENTITY_INSERT Users OFF;

-- 3. TANTÁRGYAK (Subjects)
SET IDENTITY_INSERT Subjects ON;
INSERT INTO Subjects (Id, Code, Name, Credits, IsActive) VALUES
(1, 'INFT-HP2', 'Haladó programozás II.', 5, 1),
(2, 'INFT-ADAT', 'Adatbázisrendszerek', 4, 1),
(3, 'INFT-SZOFT', 'Szoftverfejlesztés', 6, 1),
(4, 'INFT-HALO', 'Számítógép-hálózatok', 4, 1),
(5, 'INFT-AI01', 'Mesterséges Intelligencia', 5, 1),
(6, 'INFT-OPRE', 'Operációs rendszerek', 4, 1),
(7, 'INFT-WEB1', 'Webfejlesztés I.', 5, 1),
(8, 'INFT-MOB1', 'Mobilalkalmazás-fejlesztés', 5, 1),
(9, 'INFT-ALGO', 'Algoritmusok és adatszerkezetek', 6, 1),
(10, 'INFT-BZT', 'IT Biztonság', 3, 1),
(11, 'INFT-PROJ', 'Projektmenedzsment', 3, 1),
(12, 'INFT-MAT3', 'Számításmélet', 4, 0); -- Inaktív tantárgy
SET IDENTITY_INSERT Subjects OFF;

-- 4. KURZUSOK (Courses)
SET IDENTITY_INSERT Courses ON;
INSERT INTO Courses (Id, CourseCode, Semester, MaxStudents, Type, Form, Hours, IsWeeklyHours, SubjectId) VALUES
(1, 'HP2-EA-01', '2023/24/2', 120, 'Lecture', 'FullTime', 2, 1, 1),
(2, 'HP2-GY-01', '2023/24/2', 20, 'Practice', 'FullTime', 2, 1, 1),
(3, 'HP2-LEV-01', '2023/24/2', 50, 'Lecture', 'PartTime', 10, 0, 1),
(4, 'ADAT-EA-01', '2023/24/2', 100, 'Lecture', 'FullTime', 2, 1, 2),
(5, 'ADAT-LAB-1', '2023/24/2', 15, 'Lab', 'FullTime', 2, 1, 2),
(6, 'ADAT-LEV-1', '2023/24/2', 40, 'Lecture', 'PartTime', 8, 0, 2),
(7, 'SZOFT-EA', '2023/24/2', 80, 'Lecture', 'Undefined', 3, 1, 3), -- Tagozatfüggetlen
(8, 'HALO-EA-01', '2023/24/2', 90, 'Lecture', 'FullTime', 2, 1, 4),
(9, 'HALO-LAB-1', '2023/24/2', 12, 'Lab', 'FullTime', 2, 1, 4),
(10, 'HALO-LEV-1', '2023/24/2', 30, 'Lab', 'PartTime', 8, 0, 4),
(11, 'AI-EA-01', '2023/24/2', 150, 'Lecture', 'FullTime', 2, 1, 5),
(12, 'OPRE-EA-01', '2023/24/2', 100, 'Lecture', 'FullTime', 2, 1, 6),
(13, 'WEB1-LAB-1', '2023/24/2', 20, 'Lab', 'FullTime', 4, 1, 7),
(14, 'WEB1-LEV-1', '2023/24/2', 40, 'Lab', 'PartTime', 12, 0, 7),
(15, 'MOB1-GY-01', '2023/24/2', 25, 'Practice', 'FullTime', 2, 1, 8),
(16, 'ALGO-EA-01', '2023/24/2', 150, 'Lecture', 'Undefined', 2, 1, 9), -- Tagozatfüggetlen
(17, 'BZT-EA-01', '2023/24/2', 80, 'Lecture', 'FullTime', 2, 1, 10),
(18, 'PROJ-GY-01', '2023/24/2', 30, 'Practice', 'FullTime', 2, 1, 11);
SET IDENTITY_INSERT Courses OFF;

-- 5. KURZUSOKHOZ RENDELT OKTATÓK (CourseTeachers)
INSERT INTO CourseTeachers (CourseId, TeacherId) VALUES
(1, 3), (2, 3), (3, 4), -- HP2: Kovács Péter (Nappali), Nagy Anna (Levelező)
(4, 4), (5, 4), (6, 5), -- Adatbázis: Nagy Anna, Szabó Gábor
(7, 5), -- Szoftverfejlesztés: Szabó Gábor
(8, 6), (9, 6), (10, 6), -- Hálózatok: Tóth Zoltán
(11, 5), -- AI: Szabó Gábor
(12, 7), -- Opre: Horváth Mária
(13, 3), (14, 3), -- Web1: Kovács Péter
(15, 7), -- Mobil: Horváth Mária
(16, 6), -- Algoritmusok: Tóth Zoltán
(17, 7), (18, 4); -- Biztonság és Projekt: Horváth Mária, Nagy Anna

-- 6. KURZUSOKRA FELIRATKOZOTT HALLGATÓK (CourseStudents)
INSERT INTO CourseStudents (CourseId, StudentId) VALUES
-- Nappalisok tárgyfelvételei (Id: 9-18)
(1, 9), (2, 9), (4, 9), (5, 9), (8, 9), (9, 9), (16, 9), -- Kiss Gábor (nagyon szorgalmas)
(1, 10), (2, 10), (4, 10), (7, 10), (11, 10), -- Tóth Csilla
(1, 11), (2, 11), (12, 11), (13, 11), (15, 11), -- Varga Balázs
(1, 12), (2, 12), (7, 12), (16, 12), (17, 12),
(1, 13), (2, 13), (8, 13), (9, 13), (18, 13),
(4, 14), (5, 14), (11, 14), (12, 14), (16, 14),
(4, 15), (5, 15), (7, 15), (13, 15), (15, 15),
(8, 16), (9, 16), (11, 16), (17, 16), (18, 16),
(12, 17), (13, 17), (15, 17), (16, 17),
(7, 18), (11, 18), (17, 18), (18, 18),

-- Levelezősök tárgyfelvételei (Id: 19-24)
(3, 19), (6, 19), (7, 19), (16, 19), -- Fehér Róbert (A 7 és 16 tagozatfüggetlen!)
(3, 20), (10, 20), (14, 20),
(6, 21), (10, 21), (16, 21),
(3, 22), (6, 22), (14, 22),
(10, 23), (14, 23), (7, 23),
(3, 24), (6, 24), (10, 24), (14, 24), (16, 24);

-- 7. ÓRAREND (Schedules)
SET IDENTITY_INSERT Schedules ON;
INSERT INTO Schedules (Id, StartTime, EndTime, CourseId) VALUES
-- HP2 Nappali (Heti rendszerességű)
(1, '2024-05-14 10:00:00', '2024-05-14 11:30:00', 1),
(2, '2024-05-16 14:00:00', '2024-05-16 15:30:00', 2),
-- Adatbázis Nappali
(3, '2024-05-13 08:00:00', '2024-05-13 09:30:00', 4),
(4, '2024-05-13 10:00:00', '2024-05-13 11:30:00', 5),
-- Szoftverfejlesztés (Tagozatfüggetlen)
(5, '2024-05-15 16:00:00', '2024-05-15 18:30:00', 7),
-- Hálózatok
(6, '2024-05-14 12:00:00', '2024-05-14 13:30:00', 8),
(7, '2024-05-17 08:00:00', '2024-05-17 09:30:00', 9),
-- Web és Mobil lab
(8, '2024-05-15 08:00:00', '2024-05-15 11:30:00', 13),
(9, '2024-05-17 12:00:00', '2024-05-17 13:30:00', 15),
-- Levelezős tömbösített órák (Hétvégék)
(10, '2024-05-18 08:00:00', '2024-05-18 18:00:00', 3), -- HP2 Lev
(11, '2024-05-19 08:00:00', '2024-05-19 16:00:00', 6), -- Adat Lev
(12, '2024-05-25 08:00:00', '2024-05-25 16:00:00', 10), -- Háló Lev
(13, '2024-05-26 08:00:00', '2024-05-26 20:00:00', 14); -- Web Lev
SET IDENTITY_INSERT Schedules OFF;

-- 8. ÉRTESÍTÉS LOGOK (NotificationLogs)
SET IDENTITY_INSERT NotificationLogs ON;
INSERT INTO NotificationLogs (Id, Message, SentAt, UserId, CourseId) VALUES
(1, 'Emlékeztető: A HP2-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-07 09:30:00', 9, 1),
(2, 'Emlékeztető: A HP2-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-07 09:30:00', 10, 1),
(3, 'Emlékeztető: A HP2-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-07 09:30:00', 11, 1),
(4, 'Emlékeztető: A HP2-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-07 09:30:00', 3, 1), -- Oktató is kapja
(5, 'Emlékeztető: A ADAT-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-06 07:30:00', 9, 4),
(6, 'Emlékeztető: A ADAT-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-06 07:30:00', 14, 4),
(7, 'Emlékeztető: A ADAT-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-06 07:30:00', 4, 4), -- Oktató
(8, 'Értesítés: Az ALGO-EA-01 kurzusra az oktató anyagot töltött fel.', '2024-05-05 14:20:00', 16, 16),
(9, 'Értesítés: Az ALGO-EA-01 kurzusra az oktató anyagot töltött fel.', '2024-05-05 14:20:00', 19, 16),
(10, 'Figyelem: A WEB1-LEV-1 hétvégi tömbösített óra terme megváltozott (IK-202).', '2024-05-10 11:00:00', 20, 14),
(11, 'Figyelem: A WEB1-LEV-1 hétvégi tömbösített óra terme megváltozott (IK-202).', '2024-05-10 11:00:00', 22, 14),
(12, 'Figyelem: A WEB1-LEV-1 hétvégi tömbösített óra terme megváltozott (IK-202).', '2024-05-10 11:00:00', 3, 14);
SET IDENTITY_INSERT NotificationLogs OFF;

PRINT 'Adatbázis sikeresen feltöltve NAGY tesztadat halmazzal!';