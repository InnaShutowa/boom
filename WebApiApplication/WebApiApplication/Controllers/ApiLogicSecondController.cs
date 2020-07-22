using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiApplication.Managers;

namespace WebApiApplication.Controllers
{
    public class ApiLogicSecondController : ApiController
    {
        [HttpGet]
        public object ChangeActiveUser(int session_id) {
            return ApiCardManager.ChangeActiveUser(session_id);
        }
        [HttpGet]
        public object CanselShag(string data) {
            return ApiCardManager.CanselShag(data);
        }
        [HttpOptions]
        public HttpResponseMessage Options() {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}