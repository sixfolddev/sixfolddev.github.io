using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using RoomAid.ManagerLayer;
using RoomAid.QueueConsumer;
using RoomAid.ServiceLayer;

namespace RoomAid.Messaging.Tests
{
    [TestClass]
    public class MessageServiceDatabaseTests
    {
        // DAOs
        private readonly MessageDAO _messageDAO = new MessageDAO();
        private readonly ISqlDAO _dao = new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
        private readonly ICreateAccountDAO _newAccountDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
        private readonly ICreateAccountDAO _newMappingDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
        private readonly IMapperDAO _mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
        private readonly ICreateAccountDAO _newUserDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
        
        // Global variables
        private string email;
        private SqlCommand cmd;
        private int receiverID;
        private const int _numMessages = 3; // For use when testing sending/receiving multiple messages
        private readonly MessageManager manager = new MessageManager();

        /// <summary>
        /// Executes before all tests to clean and setup necssary variables and objects
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            try
            {
                email = "Test@gmail.com";
                DeleteData(email); // Clean tables
                CreateAccount(email, "TestPassword", "TestSalt"); // Create user
                cmd = new SqlCommand("SELECT SysID FROM dbo.Users WHERE UserEmail = @email");
                cmd.Parameters.AddWithValue("@email", email);
                receiverID = (int)_messageDAO.RetrieveOneColumn(cmd); // Get SysID of user just created
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Executes after all tests to clean up any test data created
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            try
            {
                DeleteData(email);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        [TestMethod]
        public void GetMessageCount_Pass()
        {
            //Arrange
            bool expected = true;
            bool actual = false;
            IList<GeneralMessage> messages = new List<GeneralMessage>();

            //Act
            try
            {
                for (int i = 0; i < _numMessages; i++) // Creating 3 general messages to send to database
                {
                    messages.Add(new GeneralMessage(receiverID, i + 2, GetDateTime.GetUTCNow(), "Test message" + i));
                    RoomAid.QueueConsumer.QueueConsumer.SendToDB((IMessage)messages[i]);
                }
                if (_messageDAO.GetCount(receiverID, true) == _numMessages)
                {
                    actual = true;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Creates a user and sends a message to the database. If a message ID is created in both the InboxMessages and GeneralMessages tables, then 
        /// the test passes. Effectively tests QueueConsumer's SendToDB method, and sending one message and reading one column of data from the database.
        /// Also checks to ensure that an entry is created in the appropriate foreign-key constrained table (general messages).
        /// </summary>
        [TestMethod]
        public void SendGeneralMessageToDB_MessageIDCreated_Pass()
        {
            //Arrange
            IMessage message = new GeneralMessage(receiverID, 2, GetDateTime.GetUTCNow(), "Test message");
            bool isSuccess = false;

            //Act
            try
            {
                RoomAid.QueueConsumer.QueueConsumer.SendToDB(message); // using directive doesn't work for some reason
                // NOTE: All three commands below will return the first column of the first row of data, but since we cleared the database first there is only one entry
                int incomingSysID = (int)_messageDAO.RetrieveOneColumn(new SqlCommand("SELECT SysID FROM dbo.InboxMessages"));
                int incomingMessageID = (int)_messageDAO.RetrieveOneColumn(new SqlCommand("SELECT MessageID FROM dbo.InboxMessages"));
                int incomingGeneralMessageID = (int)_messageDAO.RetrieveOneColumn(new SqlCommand("SELECT MessageID FROM dbo.GeneralMessages"));
                if(incomingSysID == receiverID && incomingGeneralMessageID == incomingMessageID)
                {
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }

            //Assert
            Assert.IsTrue(isSuccess);
        }

        /// <summary>
        /// Creates a user and sends an invitation to the database. If a message ID is created in both the InboxMessages and Invitations tables,
        /// then the test passes. Effectively tests sending one invitation and reading one column of data from the database, and checks to ensure 
        /// that an entry is created in the appropriate foreign-key constrained table (invitations).
        /// </summary>
        [TestMethod]
        public void SendInvitationToDB_MessageIDCreated_Pass()
        {
            //Arrange
            IMessage invitation = new Invitation(receiverID, 2, GetDateTime.GetUTCNow());
            bool isSuccess = false;

            //Act
            try
            {
                RoomAid.QueueConsumer.QueueConsumer.SendToDB(invitation); // using directive doesn't work for some reason
                // NOTE: All three commands below will return the first column of the first row of data, but since we cleared the database first there is only one entry
                int incomingSysID = (int)_messageDAO.RetrieveOneColumn(new SqlCommand("SELECT SysID FROM dbo.InboxMessages"));
                int incomingMessageID = (int)_messageDAO.RetrieveOneColumn(new SqlCommand("SELECT MessageID FROM dbo.InboxMessages"));
                int incomingInvitationMessageID = (int)_messageDAO.RetrieveOneColumn(new SqlCommand("SELECT MessageID FROM dbo.Invitations"));
                if (incomingSysID == receiverID && incomingMessageID == incomingInvitationMessageID)
                {
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }

            //Assert
            Assert.IsTrue(isSuccess);
        }

        /// <summary>
        /// Sends a general message to the database and reads back the entire row of data. After parsing the info into a message, if the info
        /// is equal to the info of the message sent, then the test passes. Effectively tests retrieving one row of data and one column of
        /// data from the database, and checks to ensure that an entry is created in the appropriate foreign-key constrained table (general messages).
        /// </summary>
        [TestMethod]
        public void SendAndReadOneRow_GeneralMessage_Pass()
        {
            //Arrange
            GeneralMessage message = new GeneralMessage(receiverID, 2, GetDateTime.GetUTCNow(), "Test message");
            bool isSuccess = false;

            //Act
            try
            {
                RoomAid.QueueConsumer.QueueConsumer.SendToDB(message); // using directive doesn't work for some reason
                List<string> inboxContent = (List<string>)_messageDAO.RetrieveOneRow(new SqlCommand("SELECT * FROM dbo.InboxMessages"));
                string generalMessageContent = _messageDAO.RetrieveOneColumn(new SqlCommand("SELECT MessageBody FROM dbo.GeneralMessages")).ToString();

                // InboxMessage[0..6] = [ReceiverID, MessageID, PrevMessageID, SenderID, IsRead, SentDate, IsGeneral]
                GeneralMessage incomingMessage = new GeneralMessage(Int32.Parse(inboxContent[0]),
                    Int32.Parse(inboxContent[2]),
                    Int32.Parse(inboxContent[3]),
                    DateTime.Parse(inboxContent[5]),
                    generalMessageContent)
                {
                    IsRead = bool.Parse(inboxContent[4])
                };

                if (incomingMessage.SenderID == message.SenderID &&
                    incomingMessage.ReceiverID == message.ReceiverID &&
                    incomingMessage.PrevMessageID == message.PrevMessageID &&
                    incomingMessage.IsRead == message.IsRead &&
                    incomingMessage.SentDate.ToString().Equals(message.SentDate.ToString()) && // HACK: SQL & .NET DateTime precision is off, so comparing strings
                    incomingMessage.MessageBody.Equals(message.MessageBody) &&
                    incomingMessage.IsGeneral == true)
                {
                    isSuccess = true;
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }

            //Assert
            Assert.IsTrue(isSuccess);

        }

        /// <summary>
        /// Sends an invitation to the database and reads back the entire row of data. After parsing the info into an invitation, if the info
        /// is equal to the info of the invitation sent, then the test passes. Effectively tests retrieving one row of data from the database,
        /// and checks to ensure that an entry is created in the appropriate foreign-key constrained table (invitations).
        /// </summary>
        [TestMethod]
        public void SendAndReadOneRow_Invitation_Pass()
        {
            //Arrange
            Invitation invitation = new Invitation(receiverID, 2, GetDateTime.GetUTCNow());
            bool isSuccess = false;

            //Act
            try
            {
                RoomAid.QueueConsumer.QueueConsumer.SendToDB(invitation); // using directive doesn't work for some reason
                List<string> inboxContent = (List<string>)_messageDAO.RetrieveOneRow(new SqlCommand("SELECT * FROM dbo.InboxMessages"));
                bool invitationContent = bool.Parse(_messageDAO.RetrieveOneColumn(new SqlCommand("SELECT IsAccepted FROM dbo.Invitations")).ToString());

                // InboxMessage[0..6] = [ReceiverID, MessageID, PrevMessageID, SenderID, IsRead, SentDate, IsGeneral]
                Invitation incomingInvitation = new Invitation(Int32.Parse(inboxContent[0]),
                    Int32.Parse(inboxContent[2]),
                    Int32.Parse(inboxContent[3]),
                    DateTime.Parse(inboxContent[5]))
                {
                    IsRead = bool.Parse(inboxContent[4]),
                    IsAccepted = invitationContent
                };

                if (incomingInvitation.SenderID == invitation.SenderID &&
                    incomingInvitation.ReceiverID == invitation.ReceiverID &&
                    incomingInvitation.PrevMessageID == invitation.PrevMessageID &&
                    incomingInvitation.IsRead == invitation.IsRead &&
                    incomingInvitation.SentDate.ToString().Equals(invitation.SentDate.ToString()) && // HACK: SQL & .NET DateTime precision is off, so comparing strings
                    incomingInvitation.IsAccepted.Equals(invitation.IsAccepted) &&
                    incomingInvitation.IsGeneral == false)
                {
                    isSuccess = true;
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }

            //Assert
            Assert.IsTrue(isSuccess);

        }

        /// <summary>
        /// Sends multiple (3) messages to the database and retrieves those messages in the same order. If each message matches up to the original
        /// message in the same order, then the test passes. Otherwise, if any do not match, it will fail. Effectively tests retrieving multiple rows
        /// from the database, and checks to ensure that entries are created in the appropriate foreign-key constrained table (general messages).
        /// </summary>
        [TestMethod]
        public void SendAndReadMultipleRows_GeneralMessage_Pass()
        {
            //Arrange
            bool isSuccess = false;
            IList<GeneralMessage> messages = new List<GeneralMessage>();

            //Act
            try
            {
                for (int i = 0; i < _numMessages; i++) // Creating 3 general messages to send to database
                {
                    messages.Add(new GeneralMessage(receiverID, i + 2, GetDateTime.GetUTCNow(), "Test message" + i));
                    RoomAid.QueueConsumer.QueueConsumer.SendToDB((IMessage)messages[i]);
                }
                IList<IList<string>> incomingMessages = _messageDAO.RetrieveMultipleRows(new SqlCommand("SELECT * FROM dbo.InboxMessages"));
                IList<IList<string>> generalMessageContent = _messageDAO.RetrieveMultipleRows(new SqlCommand("SELECT MessageBody FROM dbo.GeneralMessages"));
                for (int i = 0; i < incomingMessages.Count; i++)
                {
                    // InboxMessage[0..6] = [ReceiverID, MessageID, PrevMessageID, SenderID, IsRead, SentDate, IsGeneral]
                    GeneralMessage incomingMessage = new GeneralMessage(Int32.Parse(incomingMessages[i][0]),
                        Int32.Parse(incomingMessages[i][2]),
                        Int32.Parse(incomingMessages[i][3]),
                        DateTime.Parse(incomingMessages[i][5]),
                        generalMessageContent[i][0])
                    {
                        IsRead = bool.Parse(incomingMessages[i][4]) // IsRead [4]
                    };
                    //incomingMessage.IsAccepted = bool.Parse(generalMessageContent[i][0]);

                    if (incomingMessage.SenderID == messages[i].SenderID &&
                        incomingMessage.ReceiverID == messages[i].ReceiverID &&
                        incomingMessage.PrevMessageID == messages[i].PrevMessageID &&
                        incomingMessage.IsRead == messages[i].IsRead &&
                        incomingMessage.SentDate.ToString().Equals(messages[i].SentDate.ToString()) && // HACK: SQL & .NET DateTime precision is off, so comparing strings
                        incomingMessage.MessageBody.Equals(messages[i].MessageBody) &&
                        incomingMessage.IsGeneral == true)
                    {
                        isSuccess = true;
                    } 
                    else
                    {
                        isSuccess = false; // Since there are multiple messages to be checked, must set false if any are not equal
                    }
                }
            } 
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            //Assert
            Assert.IsTrue(isSuccess);
        }

        /// <summary>
        /// Sends multiple (3) invitations to the database and retrieves those invitations in the same order. If each message matches up to the original
        /// invitation in the same order, then the test passes. Otherwise, if any do not match, it will fail. Effectively tests retrieving multiple rows
        /// from the database, and checks to ensure that an entry is created in the appropriate foreign-key constrained table (invitations).
        /// </summary>
        [TestMethod]
        public void SendAndReadMultipleRows_Invitation_Pass()
        {
            //Arrange
            bool isSuccess = false;
            IList<Invitation> invitations = new List<Invitation>();

            //Act
            try
            {
                for (int i = 0; i < _numMessages; i++) // Creating 3 general messages to send to database
                {
                    invitations.Add(new Invitation(receiverID, i + 2, GetDateTime.GetUTCNow()));
                    RoomAid.QueueConsumer.QueueConsumer.SendToDB((IMessage)invitations[i]);
                }
                IList<IList<string>> incomingInvitations = _messageDAO.RetrieveMultipleRows(new SqlCommand("SELECT * FROM dbo.InboxMessages"));
                IList<IList<string>> invitationContent = _messageDAO.RetrieveMultipleRows(new SqlCommand("SELECT IsAccepted FROM dbo.Invitations"));
                for (int i = 0; i < incomingInvitations.Count; i++)
                {
                    // InboxMessage[0..6] = [ReceiverID, MessageID, PrevMessageID, SenderID, IsRead, SentDate, IsGeneral]
                    Invitation incomingInvitation = new Invitation(Int32.Parse(incomingInvitations[i][0]),
                        Int32.Parse(incomingInvitations[i][2]),
                        Int32.Parse(incomingInvitations[i][3]),
                        DateTime.Parse(incomingInvitations[i][5]))
                    {
                        IsRead = bool.Parse(incomingInvitations[i][4]), // IsRead [4]
                        IsAccepted = bool.Parse(invitationContent[i][0])
                    };

                    if (incomingInvitation.SenderID == invitations[i].SenderID &&
                        incomingInvitation.ReceiverID == invitations[i].ReceiverID &&
                        incomingInvitation.PrevMessageID == invitations[i].PrevMessageID &&
                        incomingInvitation.IsRead == invitations[i].IsRead &&
                        incomingInvitation.SentDate.ToString().Equals(invitations[i].SentDate.ToString()) && // HACK: SQL & .NET DateTime precision is off, so comparing strings
                        incomingInvitation.IsAccepted == invitations[i].IsAccepted &&
                        incomingInvitation.IsGeneral == false)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false; // Since there are multiple invitations to be checked, must set false if any are not equal
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            //Assert
            Assert.IsTrue(isSuccess);
        }

        /// <summary>
        /// Sends to one user a general message from multiple (3) different users. The general message inbox is retrieved for the receiving user, and 
        /// if all listings of messages match the ones sent (correct messageID, sender first and last name), then the test passes. This test uses the
        /// MessageManager GetAllMessages method, which effectively tests the flow from the manager to the service to the data layer.
        /// </summary>
        [TestMethod]
        public void GetGeneralMessageInbox_Pass()
        {
            //Arrange
            bool isSuccess = false;
            IList<GeneralMessage> messages = new List<GeneralMessage>();
            IList<User> sendingUsers = new List<User>();
            try
            {
                CreateUser(receiverID, email, "Michell", "Kuang"); // Creating a user for the first account created in test initialization
                for (int i = 0; i < _numMessages; i++) // Creating the same number of users as the number of messages that will be sent
                {
                    // Create email
                    StringBuilder userEmail = new StringBuilder("email");
                    userEmail.Append(i);
                    userEmail.Append("@gmail.com");

                    // Create first name
                    StringBuilder firstName = new StringBuilder("FirstName");
                    firstName.Append(i);

                    // Create last name
                    StringBuilder lastName = new StringBuilder("LastName");
                    lastName.Append(i);

                    CreateAccount(userEmail.ToString(), "TestPassword", "TestSalt"); // Create user
                    var command = new SqlCommand("SELECT SysID FROM dbo.Users WHERE UserEmail = @email");
                    command.Parameters.AddWithValue("@email", userEmail.ToString());
                    int rcvid = (int)_messageDAO.RetrieveOneColumn(command); // Get SysID of user just created
                    var user = CreateUser(rcvid, userEmail.ToString(), firstName.ToString(), lastName.ToString());
                    sendingUsers.Add(user);
                    
                    messages.Add(new GeneralMessage(receiverID, rcvid, GetDateTime.GetUTCNow(), "Test message" + i)); // All messages will be sent to the first user created
                    RoomAid.QueueConsumer.QueueConsumer.SendToDB((IMessage)messages[i]);

                    command = new SqlCommand("SELECT MessageID FROM dbo.InboxMessages WHERE SenderID = @sendid");
                    command.Parameters.AddWithValue("@sendid", rcvid);
                    int incomingMessageID = (int)_messageDAO.RetrieveOneColumn(command);
                    messages[i].MessageID = incomingMessageID;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            
            //Act
            try
            {
                IList<MessageListing> inbox = manager.GetAllMessages(receiverID); // Retrieve all messages in inbox for user "Michell Kuang"
                for (int i = 0; i < inbox.Count; i++)
                {
                    if(inbox[i].MessageID == messages[i].MessageID &&
                       inbox[i].SentDate.ToString().Equals(messages[i].SentDate.ToString()) &&
                       inbox[i].FirstName.Equals(sendingUsers[i].FirstName) &&
                       inbox[i].LastName.Equals(sendingUsers[i].LastName))
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false; // Since there are multiple messages to be checked, must set false if any are not equal
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            //Cleanup other users created
            foreach(User u in sendingUsers)
            {
                DeleteAccounts(u.UserEmail);
            }

            //Assert
            Assert.IsTrue(isSuccess);
        }

        /// <summary>
        /// Sends to one user an invitation from multiple (3) different users. The invitation inbox is retrieved for the receiving user, and 
        /// if all listings of invitations match the ones sent (correct messageID, sender first and last name), then the test passes. This test uses the
        /// MessageManager GetAllInvitations method, which effectively tests the flow from the manager to the service to the data layer.
        /// </summary>
        [TestMethod]
        public void GetInvitationInbox_Pass()
        {
            //Arrange
            bool isSuccess = false;
            IList<Invitation> invitations = new List<Invitation>();
            IList<User> sendingUsers = new List<User>();
            try
            {
                CreateUser(receiverID, email, "Michell", "Kuang"); // Creating a user for the first account created in test initialization
                for (int i = 0; i < _numMessages; i++) // Creating the same number of users as the number of messages that will be sent
                {
                    // Create email
                    StringBuilder userEmail = new StringBuilder("email");
                    userEmail.Append(i);
                    userEmail.Append("@gmail.com");

                    // Create first name
                    StringBuilder firstName = new StringBuilder("FirstName");
                    firstName.Append(i);

                    // Create last name
                    StringBuilder lastName = new StringBuilder("LastName");
                    lastName.Append(i);

                    CreateAccount(userEmail.ToString(), "TestPassword", "TestSalt"); // Create user
                    var command = new SqlCommand("SELECT SysID FROM dbo.Users WHERE UserEmail = @email");
                    command.Parameters.AddWithValue("@email", userEmail.ToString());
                    int rcvid = (int)_messageDAO.RetrieveOneColumn(command); // Get SysID of user just created
                    var user = CreateUser(rcvid, userEmail.ToString(), firstName.ToString(), lastName.ToString());
                    sendingUsers.Add(user);

                    invitations.Add(new Invitation(receiverID, rcvid, GetDateTime.GetUTCNow())); // All messages will be sent to the first user created
                    RoomAid.QueueConsumer.QueueConsumer.SendToDB((IMessage)invitations[i]);

                    command = new SqlCommand("SELECT MessageID FROM dbo.InboxMessages WHERE SenderID = @sendid");
                    command.Parameters.AddWithValue("@sendid", rcvid);
                    int incomingMessageID = (int)_messageDAO.RetrieveOneColumn(command);
                    invitations[i].MessageID = incomingMessageID;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            //Act
            try
            {
                IList<MessageListing> inbox = manager.GetAllInvitations(receiverID); // Retrieve all messages in inbox for user "Michell Kuang"
                for (int i = 0; i < inbox.Count; i++)
                {
                    if (inbox[i].MessageID == invitations[i].MessageID &&
                       inbox[i].SentDate.ToString().Equals(invitations[i].SentDate.ToString()) &&
                       inbox[i].FirstName.Equals(sendingUsers[i].FirstName) &&
                       inbox[i].LastName.Equals(sendingUsers[i].LastName))
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false; // Since there are multiple messages to be checked, must set false if any are not equal
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            //Cleanup other users created
            foreach (User u in sendingUsers)
            {
                DeleteAccounts(u.UserEmail);
            }

            //Assert
            Assert.IsTrue(isSuccess);
        }

        /// <summary>
        /// Creates a user associated with an account for testing
        /// </summary>
        /// <param name="rcvid">SystemID of the user to be created</param>
        /// <param name="userEmail">Email of the user to be created</param>
        /// <param name="firstname">First name of user to be created</param>
        /// <param name="lastname">Last name of user to be created</param>
        /// <returns></returns>
        private User CreateUser(int rcvid, string userEmail, string firstname, string lastname)
        {
            try
            {
                var user = new User(rcvid, userEmail, firstname, lastname, "Enable", GetDateTime.GetUTCNow(), "Female");
                var update = new UpdateAccountSqlService(user, _dao);
                update.Update();
                return user;
            } 
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Creates an account for testing
        /// </summary>
        /// <param name="email">Email of account</param>
        /// <param name="password">Password of account</param>
        /// <param name="salt">Password salt of account</param>
        private void CreateAccount(string userEmail, string password, string salt)
        {
            Account testAccount = new Account(userEmail, password, salt);
            CreateAccountDAOs daos = new CreateAccountDAOs(_newAccountDAO, _newMappingDAO, _newUserDAO, _mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            cas.Create();
        }

        // Cleanup methods
        private void DeleteData(string userEmail)
        {
            // Delete message and data from all associated tables
            DeleteMessages();

            // Delete account and data from all associated tables
            DeleteAccounts(userEmail);
        }

        private void DeleteMessages()
        {
            List<SqlCommand> commands = new List<SqlCommand>
            {
                new SqlCommand("DELETE FROM dbo.GeneralMessages"),
                new SqlCommand("DELETE FROM dbo.Invitations"),
                new SqlCommand("DELETE FROM dbo.InboxMessages")
            };
            _messageDAO.RunCommand(commands);
        }

        private void DeleteAccounts(string userEmail)
        {
            DeleteAccount(userEmail, Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User), "dbo.Users");
            DeleteAccount(userEmail, Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User), "dbo.Mapping");
            DeleteAccount(userEmail, Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User), "dbo.Accounts");
        }

        public void DeleteAccount(string userEmail, string connectionString, string table)
        {
            StringBuilder sb = new StringBuilder("DELETE FROM ");
            sb.Append(table);
            sb.Append(" WHERE UserEmail = @userEmail");
            string commandText = sb.ToString();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
    }
}