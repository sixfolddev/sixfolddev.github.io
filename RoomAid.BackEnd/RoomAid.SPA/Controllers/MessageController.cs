using RoomAid.ManagerLayer;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Configuration;


namespace RoomAid.SPA.Controllers
{
    //[EnableCors(origins: "http://localhost:8080", headers: "*", methods: "GET, POST, PUT, DELETE")]
    [RoutePrefix("api/inbox")]
    public class MessageController : ApiController
    {
        private readonly MessageManager _messageManager;

        // CONSTRUCTORS
        public MessageController()
        {
            _messageManager = new MessageManager();
        }

        public MessageController(MessageManager messageManager)
        {
            _messageManager = messageManager;
        }

        // GET REQUESTS
        [HttpGet]
        [Route("{receiverID}/{isGeneral}/messages/count")]
        public IHttpActionResult GetNewMessagesCount(int receiverID, bool isGeneral)
        {
            try
            {
                return Ok(_messageManager.GetNewCount(receiverID, isGeneral));
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        /*        private IHttpActionResult ExecuteAction(IHttpActionResult act)
                {
                    try
                    {
                        return act;
                    }
                    catch (Exception e)
                    {
                        var newError = new Exception(e.Message, null);
                        return InternalServerError(newError);
                    }
                }

                [HttpGet]
                [Route("general/{receiverID}")]
                public IHttpActionResult GetAllMessages(int receiverID)
                {
                    return ExecuteAction(Ok(_messageManager.GetAllMessages(receiverID)));
                }*/
        [HttpGet]
        [Route("general/{receiverID}")]
        public IHttpActionResult GetAllMessages(int receiverID)
        {
            try
            {
                return Ok(_messageManager.GetAllMessages(receiverID));
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("invitation/{receiverID}")]
        public IHttpActionResult GetAllInvitations(int receiverID)
        {
            try
            {
                return Ok(_messageManager.GetAllInvitations(receiverID));
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        // TODO: implement
        [HttpGet]
        [Route("sent/{receiverID}")]
        public IHttpActionResult GetAllSent(int senderID)
        {
            try
            {
                return Ok();
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("general/{receiverID}/{messageID}")]
        public IHttpActionResult ReadMessage(int receiverID, int messageID)
        {
            try
            {
                return Ok(_messageManager.ReadMessage(receiverID, messageID));
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("invitation/{receiverID}/{messageID}")]
        public IHttpActionResult ReadInvitation(int receiverID, int messageID)
        {
            try
            {
                return Ok(_messageManager.ReadInvitation(receiverID, messageID));
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        // POST REQUESTS
        [HttpPost]
        [Route("{senderID}/send/general/{receiverID}")] //parameters and route... 1:1??
        public IHttpActionResult SendMessage(int senderID, int receiverID, string messageBody)
        {
            try
            {
                return Ok(_messageManager.SendMessage(receiverID, senderID, messageBody));
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{senderID}/send/invitation/{receiverID}")] //
        public IHttpActionResult SendInvitation(int receiverID, int senderID)
        {
            try
            {
                return Ok(_messageManager.SendInvitation(receiverID, senderID));
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{senderID}/{prevMessageID}/reply/general/{receiverID}")]
        public IHttpActionResult ReplyMessage(int receiverID, int prevMessageID, int senderID, string messageBody)
        {
            try
            {
                return Ok(_messageManager.ReplyMessage(receiverID, prevMessageID, senderID, messageBody));
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{senderID}/{prevMessageID}/reply/invitation/{receiverID}")]
        public IHttpActionResult ReplyInvitation(int receiverID, int prevMessageID, int senderID, bool accepted)
        {
            try
            {
                return Ok(_messageManager.ReplyInvitation(receiverID, prevMessageID, senderID, accepted));
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}
