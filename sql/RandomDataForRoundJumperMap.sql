USE piiadb

DECLARE @tmp TABLE (i INT NOT NULL);
INSERT INTO @tmp( i )
VALUES  (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)

DECLARE @id INT;
DECLARE c CURSOR FOR SELECT ID FROM dbo.RoundJumperMap;

OPEN C;
FETCH next FROM c INTO @id;

WHILE @@FETCH_STATUS = 0
BEGIN
UPDATE dbo.RoundJumperMap
SET score = (SELECT TOP 1 i
FROM @tmp
ORDER BY NEWID()
)
WHERE CURRENT OF c;
FETCH next FROM c INTO @id;
END;

SELECT *
FROM dbo.RoundJumperMap