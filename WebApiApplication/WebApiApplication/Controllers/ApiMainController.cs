using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiApplication.Managers;

namespace WebApiApplication.Controllers {
    public class ApiMainController : ApiController {
        /// <summary>
        /// получаем карты из колоды
        /// </summary>
        [HttpGet]
        public object GetCardsFromPack(int count, int user_id, int session_id) {
            return ApiCardManager.ApiGetCardsFromPack(count, user_id, session_id);
        }
        /// <summary>
        /// генерируем основную колоду при создании игры
        /// </summary>
        [HttpGet]
        public object GeneratePack(int count_users) {
            return ApiCardManager.ApiGeneratePack(count_users);
        }
        /// <summary>
        /// получаем колоду в ее текущем состоянии на данный момент
        /// </summary>
        [HttpGet]
        public object GetCurrentPack(int session_id) {
            return ApiCardManager.ApiGetCurrentPack(session_id);
        }
        /// <summary>
        /// отправляем карты в сброс
        /// </summary>
        [HttpGet]
        public object PutCardsInSbros(string data) {
            return ApiCardManager.ApiPutCardsInSbros(data);
        }
        /// <summary>
        /// добавляем в игру подключившегося пользователя
        /// </summary>
        [HttpGet]
        public object ActivateUser(int session_id, int user_id) {
            return ApiCardManager.ActivateUser(session_id, user_id);
        }
        [HttpOptions]
        public HttpResponseMessage Options() {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}