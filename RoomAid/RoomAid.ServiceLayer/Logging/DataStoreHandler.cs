using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace RoomAid.ServiceLayer
{
    public class DataStoreHandler : ILogHandler
    {
        MongoClient _client = new MongoClient("mongodb+srv://rwUser:4agLEh9JFz7P5QC4@roomaid-logs-s3nyt.gcp.mongodb.net/test?retryWrites=true&w=majority");
        IMongoDatabase _db;
        public IMongoCollection<BsonDocument> _collection;
        private readonly LogMessage _message;

        public DataStoreHandler(LogMessage msg)
        {
            string collectionName = "Mongo_" + DateTime.Now.ToString("yyyyMMdd");
            _db = _client.GetDatabase("test");
            _collection = _db.GetCollection<BsonDocument>(collectionName);
            _message = msg;
        }
        // <summary>
        // Connects to the database and the _collection specified by the time value stored within the log message
        // Creates a new document and writes it to the _collection
        // <\summary>

        // TODO: Convert to Asynchronous 
        public bool WriteLog()
        {
            try
            {
                var document = new BsonDocument
                    {
                        {"DateTime", BsonValue.Create(_message.Time) },
                        {"Class",BsonValue.Create(_message.CallingClass)},
                        {"Method",BsonValue.Create(_message.CallingMethod) },
                        {"Level",BsonValue.Create(_message.Level) },
                        {"UserID",BsonValue.Create(_message.UserID) },
                        {"SessionID",BsonValue.Create(_message.SessionID) },
                        {"Text",BsonValue.Create(_message.Text) }
                    };
                _collection.InsertOne(document);
                return true;
            }
            //TODO: Call error handling exception handler
            catch (Exception)
            {
                return false;
            }
        }
        // TODO: Convert to Asynchronous 
        public bool DeleteLog()
        {
            try
            {
                var deleteFilter = Builders<BsonDocument>.Filter.Eq("DateTime", _message.Time);
                _collection.FindOneAndDelete(deleteFilter);

                return true;
            }
            //TODO: Error Handling exception handler
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
