/*Initialization file for HouseholdListing Test DATA*/

BEGIN /*Deletes information from tables relevant to household listings*/
	DELETE FROM HouseholdListings;
	DELETE FROM Residents;
	DELETE FROM Households;
	DELETE FROM Users;
	DELETE FROM UserStatus;
	DELETE UserRoles;
END

  BEGIN /*Populates Enumeration Tables*/
  INSERT INTO UserStatus(AccountStatus) VALUES
  ('Activated'),
  ('Deactivated')
  INSERT INTO UserRoles(HouseholdRole) VALUES
  ('Host'),
  ('Tenant')
  END

  DECLARE @currentHID int
  DECLARE @insertedHouseholds table (HID int);
  BEGIN /*Populates Households and HouseholdListing Tables. Takes data from Jake's sql testing data.*/
	  /*Insert Household data from Jake*/
	  INSERT INTO Households(Rent, StreetAddress, ZipCode, IsAvailable)
	  OUTPUT INSERTED.HID INTO @insertedHouseHolds VALUES
	  (3000, '2934 Lincoln Ave', 90630, 1),
	  (500, '3241 Orange Ave', 90630, 1),
	  (700, '0542 Ball Rd', 90630, 0),
	  (1000, '32 Ivy Court', 95828, 1),
	  (400, '7718 Pendergast Ave', 92509, 1),
	  (1300, '7382 Marshall St', 90063, 0),
	  (950, '3241 Cleveland St', 90063, 1),
	  (300, '9392 Lake Dr', 90063, 1),
	  (550, '8523 Atlantic St', 90063, 1),
	  (700, '5919 Newport Ln', 90063, 1),
	  (1100, '58 Pawnee St', 90063, 1),
	  (1000, '22 Poplar St', 90063, 1),
	  (3200, '109 Sheffield St', 90063, 1),
	  (2200, '8029 Tanglewood Ave', 90063, 1),
	  (1400, '7045 Lookout Dr', 90063, 1),
	  (1450, '9955 Thorne Dr', 90063, 1),
	  (2200, '8259 St Louis Dr', 90063, 1),
	  (3221, '5252 Redwood Ln', 90063, 1),
	  (5200, '9873 Belmont Dr', 90063, 1),
	  (4000, '37 Briarwood St', 90063, 1),
	  (3000, '844 North Sheffield St', 90063,1),
	  (2300, '9782 Pheasant Ave', 90063,1),
	  (2500, '30 East 6th St', 90063, 1),
	  (3300, '268 Pin Oak Street', 90063,1),
	  (3500, '7511 Bridgeton Street', 90063,1),
	  (2200, '25 Spring St', 90063, 1),
	  (1800, '3 Boston Drive', 90063,1),
	  (4500, '637 Hartford St', 90063,1),
	  (2000, '9781 Primrose Street', 90063,1)
	  
	  /*Iterates through HouseholdID's and populates listing data for it.*/
	  DECLARE hidCursor CURSOR Scroll for (select * from @insertedHouseholds)
	  
	  Open hidCursor /*Populates cursor by using select statement.*/
	  FETCH First FROM hidCursor into @currentHID;
	  WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO HouseholdListings(HID, DatePosted, HouseHoldType, ListingDescription, Price) VALUES
			(@currentHID, SYSUTCDATETIME(), Rand(@currentHID),@currentHID, @currentHID)
			FETCH NEXT FROM hidCursor into @currentHID;
		END;
	  CLOSE hidCursor
	  DEALLOCATE hidCURSOR
  END


  DECLARE @currentSysID int
  DECLARE @insertedHosts table( SysID int);/*Created a variable Table to store SysID for later. */
  Begin /*Populates Hosts with first available houseHolds and residency.*/
	 INSERT INTO Users(SysID, UserEmail, FirstName, LastName, DateOfBirth, Gender, AccountStatus ) 
	  OUTPUT INSERTED.SysID INTO @insertedHosts VALUES
	  (1, 'alpha@gmail.com', 'alpha', '1', '1985-01-01', 'Male', 'Activated'),
	  (2, 'beta@gmail.com', 'beta', '2', '1986-02-02', 'Male', 'Activated'),
	  (3, 'gamma@gmail.com', 'gamma', '3', '1992-03-03', 'Female', 'Activated')

	  DECLARE hidCURSOR CURSOR for (SELECT HID from @insertedHouseholds) /*hidCursor will not be closed for future iteration down the line.*/
	  DECLARE sysIDCURSOR CURSOR for (SELECT SysID from @insertedHosts)
	  OPEN hidCURSOR
	  OPEN sysIDCURSOR;
	  FETCH NEXT FROM hidCursor into @currentHID;
	  FETCH NEXT FROM sysIDCURSOR INTO @currentSysID
	  WHILE @@FETCH_STATUS = 0 /*fetch status is global. based on last fetch call*/
		BEGIN
			INSERT INTO Residents(SysID, HID, HouseholdRole) VALUES
			(@currentSysID, @currentHID, 'Host')
			FETCH NEXT FROM hidCursor into @currentHID;
			FETCH NEXT FROM sysIDCURSOR into @currentSysID
		END;
	  CLOSE sysIDCURSOR;
	  DEALLOCATE sysIDCURSOR
  END