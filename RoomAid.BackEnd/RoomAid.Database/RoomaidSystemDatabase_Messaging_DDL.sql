DROP TABLE dbo.GeneralMessages;
DROP TABLE dbo.Invitations;
DROP TABLE dbo.InboxMessages;

CREATE TABLE InboxMessages(
	SysID INT NOT NULL,
	MessageID INT NOT NULL IDENTITY,
	PrevMessageID INT,
	SenderID INT NOT NULL,
	IsRead BIT NOT NULL DEFAULT(0),
	SentDate DATETIME NOT NULL,
	IsGeneral BIT NOT NULL,
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
	-- IsAccepted used to be Response VARCHAR, but unnecessary to store actual invitation msg; only need yes/no
	IsAccepted BIT NOT NULL DEFAULT(0),
	CONSTRAINT pk_Invitations PRIMARY KEY (SysID, MessageID),
	CONSTRAINT fk_Invitations_InboxMessages FOREIGN KEY(SysID, MessageID) REFERENCES InboxMessages(SysID, MessageID)
);