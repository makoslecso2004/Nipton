-- ==========================================
-- Nipton (Neptun-klón) Tesztadat Inicializáló
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
-- Jelszó mindenhol egyszerűsítve teszteléshez, IsActive = 1 (True)
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (Id, Username, Email, Password, Role, StudentStudyForm, IsActive) VALUES
(1, 'Admin', 'admin@nipton.hu', 'admin123', 'Admin', NULL, 1),
(2, 'Dr. Kovács Péter', 'kovacs.peter@nipton.hu', 'oktato123', 'Teacher', NULL, 1),
(3, 'Nagy Anna', 'nagy.anna@nipton.hu', 'oktato123', 'Teacher', NULL, 1),
(4, 'Kiss Gábor', 'kiss.gabor@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(5, 'Tóth Csilla', 'toth.csilla@student.nipton.hu', 'diak123', 'Student', 'FullTime', 1),
(6, 'Varga Balázs', 'varga.balazs@student.nipton.hu', 'diak123', 'Student', 'PartTime', 1);
SET IDENTITY_INSERT Users OFF;

-- 3. TANTÁRGYAK (Subjects)
SET IDENTITY_INSERT Subjects ON;
INSERT INTO Subjects (Id, Code, Name, Credits, IsActive) VALUES
(1, 'INFT-HP2', 'Haladó programozás II.', 5, 1),
(2, 'INFT-ADAT', 'Adatbázisrendszerek', 4, 1),
(3, 'INFT-SZOFT', 'Szoftverfejlesztés', 6, 1);
SET IDENTITY_INSERT Subjects OFF;

-- 4. KURZUSOK (Courses)
-- Type: Lecture, Practice, Lab | Form: FullTime, PartTime, Undefined
SET IDENTITY_INSERT Courses ON;
INSERT INTO Courses (Id, CourseCode, Semester, MaxStudents, Type, Form, Hours, IsWeeklyHours, SubjectId) VALUES
(1, 'HP2-EA-01', '2023/24/2', 120, 'Lecture', 'FullTime', 2, 1, 1),  -- HP2 Nappali Elmélet (heti 2 óra)
(2, 'HP2-GY-01', '2023/24/2', 20, 'Practice', 'FullTime', 2, 1, 1), -- HP2 Nappali Gyakorlat (heti 2 óra)
(3, 'HP2-LEV-01', '2023/24/2', 50, 'Lecture', 'PartTime', 10, 0, 1), -- HP2 Levelező (tömbösített 10 óra)
(4, 'ADAT-EA-01', '2023/24/2', 100, 'Lecture', 'Undefined', 2, 1, 2); -- Adatbázis tagozatfüggetlen
SET IDENTITY_INSERT Courses OFF;

-- 5. KURZUSOKHOZ RENDELT OKTATÓK (CourseTeachers)
INSERT INTO CourseTeachers (CourseId, TeacherId) VALUES
(1, 2), -- Dr. Kovács Péter tartja a HP2 Elméletet
(2, 2), -- Ő tartja a gyakorlatot is
(3, 3), -- Nagy Anna tartja a levelezős HP2-t
(4, 3); -- Nagy Anna tartja az Adatbázisrendszereket is

-- 6. KURZUSOKRA FELIRATKOZOTT HALLGATÓK (CourseStudents)
INSERT INTO CourseStudents (CourseId, StudentId) VALUES
(1, 4), -- Kiss Gábor (Nappali) felvette a HP2 Elméletet
(2, 4), -- Kiss Gábor felvette a HP2 Gyakorlatot
(1, 5), -- Tóth Csilla (Nappali) felvette a HP2 Elméletet
(3, 6), -- Varga Balázs (Levelező) felvette a HP2 Levelezőt
(4, 4), -- Kiss Gábor felvette az Adatbázist
(4, 6); -- Varga Balázs is felvette az Adatbázist (mert Undefined tagozatú)

-- 7. ÓRAREND (Schedules)
SET IDENTITY_INSERT Schedules ON;
INSERT INTO Schedules (Id, StartTime, EndTime, CourseId) VALUES
(1, '2024-05-14 10:00:00', '2024-05-14 11:30:00', 1), -- Heti elmélet időpont
(2, '2024-05-16 14:00:00', '2024-05-16 15:30:00', 2), -- Heti gyakorlat időpont
(3, '2024-05-18 08:00:00', '2024-05-18 18:00:00', 3); -- Levelezős tömbösített időpont szombatra
SET IDENTITY_INSERT Schedules OFF;

-- 8. ÉRTESÍTÉS LOGOK (NotificationLogs)
SET IDENTITY_INSERT NotificationLogs ON;
INSERT INTO NotificationLogs (Id, Message, SentAt, UserId, CourseId) VALUES
(1, 'Emlékeztető: A HP2-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-07 09:30:00', 4, 1),
(2, 'Emlékeztető: A HP2-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-07 09:30:00', 5, 1),
(3, 'Emlékeztető: A HP2-EA-01 kurzus 30 perc múlva kezdődik.', '2024-05-07 09:30:00', 2, 1);
SET IDENTITY_INSERT NotificationLogs OFF;

PRINT 'Adatbázis sikeresen feltöltve tesztadatokkal!';