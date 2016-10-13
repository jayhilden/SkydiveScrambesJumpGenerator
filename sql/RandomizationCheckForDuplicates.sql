USE piiadb;

SELECT m.UpJumper1ID * m.UpJumper2ID * m.DownJumper1ID * m.DownJumper2ID, COUNT(1)
FROM dbo.RoundJumperMap m
GROUP BY m.UpJumper1ID * m.UpJumper2ID * m.DownJumper1ID * m.DownJumper2ID
HAVING count(1) > 1
ORDER BY COUNT(1) DESC


SELECT m.UpJumper1ID, m.UpJumper2ID, COUNT(1)
FROM dbo.RoundJumperMap m
GROUP BY m.UpJumper1ID, m.UpJumper2ID
HAVING COUNT(1) > 1

SELECT m.DownJumper1ID, m.DownJumper2ID, COUNT(1)
FROM dbo.RoundJumperMap m
GROUP BY m.DownJumper1ID, m.DownJumper2ID
HAVING COUNT(1) > 1
