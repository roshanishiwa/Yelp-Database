CREATE TABLE Users (
    userID VARCHAR,
    username VARCHAR,
    fans INT,
    average_stars FLOAT,
    tipCount INT,
    funny INT,
    cool INT,
    useful INT,
    yelpSince TIMESTAMP,
    totalLikes INT,
    user_latitude FLOAT,
    user_longitude FLOAT,
    PRIMARY KEY (userID)
);

CREATE TABLE  Friend (
    friendForID VARCHAR,
    friendOfID VARCHAR,
    PRIMARY KEY (friendForID, friendOfID),
    FOREIGN KEY (friendForID) REFERENCES Users(UserID),
    FOREIGN KEY (friendOfID) REFERENCES Users(UserID)
);

CREATE TABLE Business (
    businessID VARCHAR,
    businessName VARCHAR,
    latitude FLOAT,
    longitude FLOAT,
    address VARCHAR,
    city VARCHAR,
    state VARCHAR,
    zip CHAR(5),
    numCheckins INT,
    numTips INT,
    isOpen BOOLEAN,
    stars FLOAT,
    PRIMARY KEY (businessID)
);

CREATE TABLE Tip (
    userID VARCHAR,
    businessID VARCHAR,
    tipDate TIMESTAMP,
    tipText VARCHAR,
    likes INT,
    PRIMARY KEY (userID, businessID, tipDate),
    FOREIGN KEY (userID) REFERENCES Users(userID),
    FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE Categories (
    businessID VARCHAR,
    categoryName VARCHAR,
    PRIMARY KEY (businessID, categoryName),
    FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE Attributes (
    businessID VARCHAR,
    attributeName VARCHAR,
    value BOOLEAN,
    PRIMARY KEY (businessID, attributeName),
    FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE Hours (
    businessID VARCHAR,
    dayOfWeek VARCHAR,
    open TIME,
    close TIME,
    PRIMARY KEY (businessID, dayOfWeek),
    FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE  Checkins (
    businessID VARCHAR,
    year CHAR(4),
    month VARCHAR,
    day VARCHAR,
    checkInTime TIME,
    PRIMARY KEY (businessID, year, month, day, checkInTime),
    FOREIGN KEY (businessID) REFERENCES Business(businessID)
);