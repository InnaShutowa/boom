using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiApplication.Managers;

namespace WebApiApplication.Controllers {
    public class ApiLogicController : ApiController {

        [HttpGet]
        public bool DeleteSession(int session_id) {
            return ApiCardManager.DeleteSession(session_id);
        }
        [HttpGet]
        public object FinishShag(string data) {
            return ApiCardManager.FinishShag(data);
        }
        [HttpGet]
        public object DoShag(int session_id, int user_id, int card_number) {
            return ApiCardManager.DoShag(session_id, user_id, card_number);
        }
        [HttpOptions]
        public HttpResponseMessage Options() {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}