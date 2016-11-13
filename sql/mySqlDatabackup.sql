INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (1, 'Jo', 'Copeland', 582, 0, 1, NULL, 1, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (2, 'David', 'Prozinski', 5500, 0, 1, NULL, 1, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (3, 'Max', 'McParland', 317, 0, 1, NULL, 2, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (4, 'Westy', 'Copeland', 900, 0, 1, NULL, 1, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (5, 'Paul', 'Fitch', 450, 0, 1, NULL, 2, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (6, 'Andrew', 'Long', 115, 0, 1, NULL, 2, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (7, 'Stef', 'Starsky', 320, 0, 1, NULL, 2, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (8, 'Kyle', 'Compto', 34, 0, 1, 'just off student status', 2, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (9, 'Constanti', 'Constantly', 410, 0, 1, NULL, 2, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (10, 'Jaso', 'Nederhoff', 590, 0, 1, NULL, 1, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (13, 'Kasey', 'Matejcek', 2300, 0, 1, NULL, 1, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (16, 'Andrew', 'Olso', 560, 0, 1, NULL, 1, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (17, 'Daw', 'Nederhoff', 132, 0, 1, NULL, 2, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (18, 'Kaleb', 'Lomme', 1164, 0, 1, NULL, 1, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (19, 'Josh', 'Hoyle', 500, 0, 1, NULL, 2, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (20, 'PIIA', 'PIIA', 1000, 0, 0, NULL, 1, 1);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (21, 'Joh', 'Shinnick', 1500, 1, 0, NULL, 1, NULL);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (22, 'Paul', 'Jursik', 3000, 0, 1, NULL, 1, NULL);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (23, 'Jay', 'Hilde', 800, 1, 0, NULL, 1, NULL);
INSERT INTO Jumper (JumperID, FirstName, LastName, NumberOfJumps, Organizer, Paid, Comment, RandomizedUpDown, JumpGroup) VALUES (24, 'Lisa', 'Shinnick', 1200, 1, 0, NULL, 1, NULL);

INSERT INTO Round (RoundID, RoundNumber, Formations) VALUES (1, 3, 'M,P,Q,A');
INSERT INTO Round (RoundID, RoundNumber, Formations) VALUES (2, 2, 'B,21,F');
INSERT INTO Round (RoundID, RoundNumber, Formations) VALUES (3, 1, 'H,B,J,O');
INSERT INTO Round (RoundID, RoundNumber, Formations) VALUES (4, 4, 'E,6,');
INSERT INTO Round (RoundID, RoundNumber, Formations) VALUES (5, 5, 'H,7,L');

INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (22, 3, 2, 16, 3, 7, 1, 7, 'Art', 'https://youtu.be/2z-ZhlkFmV0');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (23, 3, 18, 13, 6, 17, 1, 2, 'Art', 'https://youtu.be/jw9bK8Pl2R8');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (24, 3, 4, 22, 19, 8, 1, 2, 'Jimmy', 'https://youtu.be/xJlxpuGHPGg');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (25, 3, 1, 10, 9, 5, 1, 3, 'Jimmy', 'https://youtu.be/tXm196T4e0s');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (26, 2, 2, 10, 19, 17, 1, 6, 'Jimmy', 'https://youtu.be/ZMXQV_Qv9Lc');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (27, 2, 18, 16, 9, 8, 1, 2, 'Jimmy', 'https://youtu.be/dn1oFYHnyh8');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (28, 2, 4, 13, 3, 5, 1, 9, 'Art', 'https://youtu.be/oh_-xlrloi0');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (29, 2, 1, 22, 6, 7, 1, 2, 'Art', 'https://youtu.be/aLwIH2QTVJg');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (30, 1, 2, 21, 3, 8, 1, 2, 'Kare', 'https://youtu.be/hAaxTsXj77w');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (31, 1, 18, 10, 6, 5, 1, 7, 'Art', 'https://youtu.be/1Ff_k3tajXc');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (32, 1, 4, 16, 19, 7, 1, 4, 'Jimmy', 'https://youtu.be/HUuZuYbs9-s');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (33, 1, 1, 13, 9, 17, 1, 5, 'Jimmy', 'https://youtu.be/o3F6Av3wuMg');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (34, 4, 2, 13, 19, 5, 1, 6, 'Jimmy', 'https://youtu.be/RhesqNLsoT0');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (35, 4, 18, 22, 9, 7, 1, 4, 'Art', 'https://youtu.be/JD09JsJQReA');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (36, 4, 4, 10, 3, 17, 1, 2, 'Art', 'https://youtu.be/oHtZ4uQxFjc');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (37, 4, 1, 16, 6, 23, 1, 3, 'Jimmy', 'https://youtu.be/q_vvD8vARJ8');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (38, 5, 2, 22, 3, 8, 1, 2, 'Jimmy', 'https://youtu.be/nzoqDwWvO50');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (39, 5, 18, 10, 6, 5, 1, 0, 'Art', 'https://youtu.be/3QI7gtjS_4U');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (40, 5, 4, 16, 19, 7, 1, 5, 'Art', 'https://youtu.be/5W6cfQTMyy0');
INSERT INTO RoundJumperMap (ID, RoundID, UpJumper1ID, UpJumper2ID, DownJumper1ID, DownJumper2ID, JumpGroup, Score, Camera, VideoUrl) VALUES (41, 5, 1, 13, 9, 24, 1, 3, 'Jimmy', 'https://youtu.be/4rC5kVpCCgI');

UPDATE Configuration
SET ConfigurationValue = '$2a$10$pozCaWYKlckYtEpmGU4X4eRg.staPB24oQtFBvDB0HZrfmArgYmcS'
WHERE ConfigurationID = 2;
