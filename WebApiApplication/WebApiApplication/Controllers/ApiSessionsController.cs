using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiApplication.Managers;

namespace WebApiApplication.Controllers {
    public class ApiSessionsController : ApiController {
        [HttpGet]
        public object Get() {
            return ApiCardManager.ApiGetSessions();
        }

        [HttpOptions]
        public HttpResponseMessage Options() {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}