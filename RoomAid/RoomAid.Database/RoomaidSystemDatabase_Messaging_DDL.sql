CREATE TABLE InboxMessages(
	SysID INT NOT NULL,
	MessageID INT NOT NULL,
	PrevMessageID INT,
	SenderID INT,
	IsRead BIT NOT NULL DEFAULT(0),
	SentDate DATETIME NOT NULL,
	CONSTRAINT pk_Messages PRIMARY KEY (SysID, MessageID),
	CONSTRAINT fk_Messages_Users FOREIGN KEY (SysID) REFERENCES Users(SysID)
);

CREATE TABLE GeneralMessages(
	SysID INT NOT NULL,
	MessageID INT NOT NULL,
	MessageBody VARCHAR(2000),
	CONSTRAINT pk_GeneralMessages PRIMARY KEY (SysID, MessageID),
	CONSTRAINT fk_GeneralMessages_InboxMessages FOREIGN KEY(SysID, MessageID) REFERENCES InboxMessages(SysID, MessageID)
);

CREATE TABLE Invitations(
	SysID INT NOT NULL,
	MessageID INT NOT NULL,
	Response VARCHAR(100),
	CONSTRAINT pk_Invitations PRIMARY KEY (SysID, MessageID),
	CONSTRAINT fk_Invitations_InboxMessages FOREIGN KEY(SysID, MessageID) REFERENCES InboxMessages(SysID, MessageID)
);