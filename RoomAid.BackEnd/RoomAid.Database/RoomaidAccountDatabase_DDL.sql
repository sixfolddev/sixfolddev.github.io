CREATE TABLE Accounts(
	UserEmail VARCHAR(200) NOT NULL,
	HashPassword VARCHAR(64) NOT NULL,
	Salt VARCHAR(64) NOT NULL,
	CONSTRAINT pk_Accounts PRIMARY KEY (UserEmail)
);