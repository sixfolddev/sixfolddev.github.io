--List of possible status for users (Activated, Deactivated, Deleted)
CREATE TABLE UserStatus(
	AccountStatus VARCHAR(50) NOT NULL,
	CONSTRAINT pk_UserStatus PRIMARY KEY (AccountStatus)
);

-- User Information
CREATE TABLE Users(
	SysID INT NOT NULL,
	UserEmail VARCHAR(200) NOT NULL,
	FirstName VARCHAR(200),
	LastName VARCHAR(200),
	DateOfBirth DATE,
	Gender VARCHAR(30),
	AccountStatus VARCHAR(50) NOT NULL,
	CONSTRAINT pk_Users PRIMARY KEY (SysID),
	CONSTRAINT fk_Users_UserStatus FOREIGN KEY(AccountStatus) REFERENCES UserStatus(AccountStatus)
);
ALTER TABLE Users ALTER COLUMN SysID INT NOT NULL;
ALTER TABLE Users ALTER COLUMN FirstName VARCHAR(200);
ALTER TABLE Users ALTER COLUMN LastName VARCHAR(200);
ALTER TABLE Users ALTER COLUMN DateOfBirth DATE;
ALTER TABLE Users ALTER COLUMN Gender VARCHAR(30);
-- Optional User profile
CREATE TABLE UserProfiles(
	SysID INT NOT NULL,
	-- Profile Images will be stored in a separate folder and the paths to said images are stored here
	ProfileImage VARCHAR(260),
	UserDescription VARCHAR(2000),
	CONSTRAINT pk_UserProfiles PRIMARY KEY (SysID),
	CONSTRAINT fk_UserProfiles_Users FOREIGN KEY(SysID) REFERENCES Users(SysID)
);

--Permissions provided to each user as a list. Permission will not be listed with user if that user doesn't have that permission.
CREATE TABLE Authorizations(
	SysID INT NOT NULL,
	Permission VARCHAR(50),
	CONSTRAINT pk_Authorizations PRIMARY KEY (SysID, Permission),
	CONSTRAINT fk_Authorizations_Users FOREIGN KEY (SysID) REFERENCES Users(SysID)
);

-- List of static zipcodes
CREATE TABLE ZipLocations(
	ZipCode VARCHAR(5) NOT NULL,
	StateName VARCHAR(15) NOT NULL,
	City VARCHAR(50) NOT NULL,
	CONSTRAINT pk_ZipLocations PRIMARY KEY (ZipCode)
);

-- Household information
CREATE TABLE Households(
	HID INT NOT NULL IDENTITY,
	Rent INT NOT NULL,
	StreetAddress VARCHAR(200) NOT NULL,
	ZipCode VARCHAR(5) NOT NULL,
	-- BIT is used as boolean in SQL. '1' is equated to 'True' and '0' to 'False'
	IsAvailable BIT NOT NULL,
	CONSTRAINT pk_Households PRIMARY KEY (HID),
	CONSTRAINT fk_Households_ZipLocations FOREIGN KEY (ZipCode) REFERENCES ZipLocations(ZipCode)
);

-- Information required for a household to be listed in a search
CREATE TABLE HouseholdListings(
	HID INT NOT NULL,
	HouseHoldType VARCHAR(30),
	ListingDescription VARCHAR(2000),
	Price DECIMAL(19,4) NOT NULL,
	CONSTRAINT pk_HouseholdListings PRIMARY KEY (HID),
	CONSTRAINT fk_HouseholdListings_Households FOREIGN KEY (HID) REFERENCES Households(HID)
);

-- List of images provided by the HouseholdListing stored as paths
CREATE TABLE HouseholdImages(
	HID INT NOT NULL,
	ImagePath VARCHAR(260),
	CONSTRAINT pk_HouseholdImages PRIMARY KEY (HID),
	CONSTRAINT fk_HouseholdImages_HouseholdListings FOREIGN KEY (HID) REFERENCES HouseholdListings(HID)
);

-- List of roles that can be assigned to residents (Host, Co-host, Tenant)
CREATE TABLE UserRoles(
	HouseholdRole VARCHAR(30) NOT NULL,
	CONSTRAINT pk_UserRoles PRIMARY KEY (HouseholdRole)
);

-- List of users and their assigned households and their roles within them
CREATE TABLE Residents(
	SysID INT NOT NULL,
	HID INT NOT NULL,
	HouseholdRole VARCHAR(30) NOT NULL,
	CONSTRAINT pk_Residents PRIMARY KEY (SysID, HID),
	CONSTRAINT fk_Residents_UserRoles FOREIGN KEY (HouseholdRole) REFERENCES UserRoles(HouseholdRole),
	CONSTRAINT fk_Residents_Users FOREIGN KEY (SysID) REFERENCES Users(SysID),
	CONSTRAINT fk_Residents_Households FOREIGN KEY (HID) REFERENCES Households(HID)
);

-- Parent class for tasks, expenses, and supplies
CREATE TABLE Responsibilities(
	RID INT NOT NULL IDENTITY,
	CreatorSysID INT NOT NULL,
	CreatorHID INT NOT NULL,
	ResponsibilityName VARCHAR(200) NOT NULL,
	ResponsibilityType VARCHAR(30) NOT NULL,
	ResponsibilityDescription VARCHAR(1000) NOT NULL,
	CONSTRAINT pk_Responsibilities PRIMARY KEY (RID),
	CONSTRAINT fk_Responsibilities_Residents FOREIGN KEY (CreatorSysID,CreatorHID) REFERENCES Residents(SysID,HID)
);

-- List of residents responsible for the created Responsibilities
CREATE TABLE ResponsibleResidents(
	RID INT NOT NULL,
	ResponsibleSysID INT NOT NULL,
	ResponsibleHID INT NOT NULL,
	CONSTRAINT pk_ResponsibleResidents PRIMARY KEY (RID,ResponsibleSysID,ResponsibleHID),
	CONSTRAINT fk_ResponsibleResidents_Responsibilities FOREIGN KEY (RID) REFERENCES Responsibilities (RID),
	CONSTRAINT fk_ResponsibleResidents_Residents FOREIGN KEY (ResponsibleSysID,ResponsibleHID) REFERENCES Residents(SysID,HID)
);

CREATE TABLE Expenses(
	RID INT NOT NULL,
	DueDate DATE,
	Amount DECIMAL(19,4) NOT NULL,
	CreationDate DATE NOT NULL
	CONSTRAINT pk_Expenses PRIMARY KEY (RID),
	CONSTRAINT fk_Expenses_Responsibilities FOREIGN KEY (RID) REFERENCES Responsibilities (RID)
);

CREATE TABLE Supplies(
	RID INT NOT NULL,
	CreationDate DATE NOT NULL,
	CONSTRAINT pk_Supplies PRIMARY KEY (RID),
	CONSTRAINT fk_Supplies_Responsibilities FOREIGN KEY (RID) REFERENCES Responsibilities (RID)
);

CREATE TABLE Tasks(
	RID INT NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	IsRepeating BIT NOT NULL,
	-- Cycle period stored as number of days (as per the BRD)
	CyclePeriod INT,
	CONSTRAINT pk_Tasks PRIMARY KEY (RID),
	CONSTRAINT fk_Tasks_Responsibilities FOREIGN KEY (RID) REFERENCES Responsibilities (RID)
);