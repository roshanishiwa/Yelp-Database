CREATE OR REPLACE FUNCTION updateNumTips() RETURNS TRIGGER AS 
'
BEGIN
    UPDATE Business
    SET numTips = Business.numTips + 1
    WHERE Business.businessID = NEW.businessID;
    RETURN NEW;
END
'
LANGUAGE plpgsql;

CREATE TRIGGER updateNumberOfTips
AFTER INSERT
ON Tip
FOR EACH ROW
EXECUTE PROCEDURE updateNumTips();



CREATE OR REPLACE FUNCTION updateNumCheckins() RETURNS TRIGGER AS 
'
BEGIN
    UPDATE Business
    SET numCheckins = Business.numCheckins + 1
    WHERE Business.businessID = NEW.businessID;
    RETURN NEW;
END
'
LANGUAGE plpgsql;

CREATE TRIGGER updateNumberOfCheckins
AFTER INSERT
ON Checkins
FOR EACH ROW
EXECUTE PROCEDURE updateNumCheckins();


CREATE OR REPLACE FUNCTION updateTipCount() RETURNS TRIGGER AS 
'
BEGIN
    UPDATE Users
    SET tipCount = Users.tipCount + 1
    WHERE Users.userID = NEW.userID;
    RETURN NEW;
END
'
LANGUAGE plpgsql;

CREATE TRIGGER updateNumberOfTipCount
AFTER INSERT
ON Tip
FOR EACH ROW
EXECUTE PROCEDURE updateTipCount();


CREATE OR REPLACE FUNCTION updateTotalLikes() RETURNS TRIGGER AS 
'
BEGIN
    UPDATE Users
    SET totalLikes = Users.totalLikes + 1
    WHERE Users.userID = NEW.userID;
    RETURN NEW;
END
'
LANGUAGE plpgsql;

CREATE TRIGGER updateNumberLikes
AFTER INSERT
ON Tip
FOR EACH ROW
EXECUTE PROCEDURE updateTotalLikes();


-- Tests --
-- Check before insert:
SELECT totalLikes, tipCount
FROM Users
WHERE userID = 'jRyO2V1pA4CdVVqCIOPc1Q';

SELECT numCheckins, numTips
FROM Business
WHERE businessID = '--KQsXc-clkO7oHRqGzSzg';


-- Insert Statements:
INSERT INTO Tip
VALUES ('jRyO2V1pA4CdVVqCIOPc1Q', '--KQsXc-clkO7oHRqGzSzg', '2020-12-26 01:56:17', 'Good service.', 1);

INSERT INTO Checkins (businessID, year, month, day, checkinTime)
VALUES ('--KQsXc-clkO7oHRqGzSzg', '2020', '05', '09', '23:57:32');


-- Then recheck to see if the triggers work
SELECT totalLikes, tipCount
FROM Users
WHERE userID = 'jRyO2V1pA4CdVVqCIOPc1Q';

SELECT numCheckins, numTips
FROM Business
WHERE businessID = '--KQsXc-clkO7oHRqGzSzg';

-- Clean --
DROP TRIGGER updateNumberLikes ON Users;
DROP TRIGGER updateNumberOfTipCount ON Users;
DROP TRIGGER updateNumberOfTips ON Business;
DROP TRIGGER updateNumberOfCheckins ON Business;