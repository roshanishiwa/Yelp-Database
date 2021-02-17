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
    BusinessID VARCHAR,
    UserID VARCHAR,
    PRIMARY KEY (reviewID),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID)
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
    FOREIGN KEY (UserID) REFERENCES User(UserID)
);

CREATE TABLE VoteReview (
    reviewID VARCHAR,
    UserID VARCHAR,
    PRIMARY KEY (reviewID, UserID),
    FOREIGN KEY (reviewID) REFERENCES Review(reviewID),
    FOREIGN KEY (UserID) REFERENCES User(UserID)
);

CREATE TABLE Has (
    BusinessID VARCHAR,
    categoryName VARCHAR,
    PRIMARY KEY (BusinessID, categoryName),
    FOREIGN KEY (BusinessID) REFERENCES Business (BusinessID),
    FOREIGN KEY (categoryName) REFERENCES Category(categoryName)
);

CREATE TABLE Friend (
    friendUserID VARCHAR,
    friendName VARCHAR,
    totalLikes INT,
    stars FLOAT,
    yelpSince DATETIME,
    PRIMARY KEY (friendUserID)
);

CREATE TABLE isFriend (
    UserID VARCHAR,
    friendUserID VARCHAR,
    PRIMARY KEY(UserID, friendUserID),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
    FOREIGN KEY (friendUserID) REFERENCES Friend(friendUserID)
);

CREATE TABLE latest (
    friendUserID VARCHAR,
    reviewID VARCHAR,
    PRIMARY KEY (friendUserID, reviewID),
    FOREIGN KEY (friendUserID) REFERENCES Friend(friendUserID),
    FOREIGN KEY (reviewID) REFERENCES Review(reviewID)
);

CREATE TABLE Fan (
    hasUserID VARCHAR,
    becomeUserID VARCHAR,
    PRIMARY KEY (hasUserID, becomeUserID),
    FOREIGN KEY (hasUserID) REFERENCES User(UserID),
    FOREIGN KEY (becomeUserID) REFERENCES User(UserID)
);

CREATE TABLE VoteUser (
    VoterID VARCHAR,
    VoteeID VARCHAR,
    PRIMARY KEY (VoterID, VoteeID),
    FOREIGN KEY (VoterID) REFERENCES User(UserID),
    FOREIGN KEY (VoteeID) REFERENCES User(UserID)
);

CREATE TABLE Rate (
    raterID VARCHAR,
    getRatedID VARCHAR,
    PRIMARY KEY (raterID, getRatedID),
    FOREIGN KEY (raterID) REFERENCES User(UserID)
    FOREIGN KEY (getRatedID) REFERENCES User(UserID)
);