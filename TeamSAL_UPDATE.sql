-- numCheckins of Business --
UPDATE Business
SET numCheckins = Temp.countCheckIns
FROM 
(
    SELECT Checkins.businessID, COUNT(checkInTime) AS countCheckIns
    FROM Checkins
    GROUP BY Checkins.businessID
) AS Temp
WHERE Business.businessID = Temp.businessID;

-- numTips of Business --
UPDATE Business
SET numTips = Temp.countTips
FROM 
(
    SELECT Tip.businessID, COUNT(tipText) AS countTips
    FROM Tip
    GROUP BY Tip.businessID
) AS Temp
WHERE Business.businessID = Temp.businessID;


-- totalLikes of Users --
UPDATE Users
SET totalLikes = Temp.likeCount
FROM
(
    SELECT Tip.userID, SUM(likes) AS likeCount
    FROM Tip
    GROUP BY Tip.userID
) AS Temp
WHERE Users.userID = Temp.userID;


-- tipCount of Users --
UPDATE Users
SET tipCount = Temp.totalTips
FROM
(
    SELECT Tip.userID, COUNT(tipText) AS totalTips
    FROM Tip
    GROUP BY Tip.userID
) AS Temp
WHERE Users.userID = Temp.userID;