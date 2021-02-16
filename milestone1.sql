CREATE TABLE User (
    UserID VARCHAR,
    UserName VARCHAR,
    latitude FLOAT,
    longitude FLOAT,
    fans INT,
    avgStar FLOAT,
    tipCount INT,
    tipLikes INT,
    joinDate DATETIME,
    voteCount INT,
    voteFunny INT,
    voteCool INT,
    voteFunny INT,
    PRIMARY KEY (UserID)
);

CREATE TABLE Review (
    reviewID VARCHAR,
    reviewDate DATETIME,
    likes INT,
    vote_Fun INT,
    vote_Cool INT,
    vote_Useful INT,
    PRIMARY KEY (reviewID)
);

CREATE TABLE Business (
    BusinessID VARCHAR,
    BusinessName VARCHAR,
    bLatitude FLOAT,
    bLongitude FLOAT,
    addr VARCHAR,
    city VARCHAR,
    bState VARCHAR,
    zip CHAR(5),
    rating FLOAT,
    reviewCount INT,
    totalCheckin INT,
    totalTips INT,
    priceRange INT,
    openHour VARCHAR,
    closeHour VARCHAR,
    Distance FLOAT,
    PRIMARY KEY (BusinessID)
);

CREATE TABLE Category (
    categoryName VARCHAR,
    PRIMARY KEY (categoryName)
);

CREATE TABLE Search (
    UserID VARCHAR,
    BusinessID VARCHAR,
    PRIMARY KEY (UserID, BusinessID),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID)
);

CREATE TABLE GiveReview (
    reviewID VARCHAR,
    BusinessID VARCHAR,
    UserID VARCHAR,
    PRIMARY KEY (reviewID, UserID, BusinessID),
    FOREIGN KEY (reviewID) REFERENCES Review(reviewID),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID)
);

CREATE TABLE CheckIn (
    BusinessID VARCHAR,
    UserID VARCHAR,
    PRIMARY KEY (UserID, BusinessID),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID)
);

CREATE TABLE LikeReview (
    reviewID VARCHAR,
    UserID VARCHAR,
    PRIMARY KEY (reviewID, UserID),
    FOREIGN KEY (reviewID) REFERENCES Review(reviewID),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
);

CREATE TABLE VoteReview (
    reviewID VARCHAR,
    UserID VARCHAR,
    PRIMARY KEY (reviewID, UserID),
    FOREIGN KEY (reviewID) REFERENCES Review(reviewID),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
);