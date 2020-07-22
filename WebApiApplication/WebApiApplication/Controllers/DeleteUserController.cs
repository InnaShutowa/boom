using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiApplication.Managers;

namespace WebApiApplication.Controllers {
    public class DeleteUserController : ApiController {
        /// <summary>
        /// удаляем пользователя из игры
        /// </summary>
        [HttpGet]
        public object Get(int user_id, int session_id) {
            return ApiCardManager.ApiDeleteUserFromGame(user_id, session_id); 
        }
        [HttpOptions]
        public HttpResponseMessage Options() {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}