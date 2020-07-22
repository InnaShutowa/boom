using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoombCats.Lib.Models {
    /// <summary>
    /// модель с информацией по сессии
    /// </summary>
    public class SessionsInfoModel {
        /// <summary>
        /// количество подключенных пользователей
        /// </summary>
        [JsonProperty("user_count")]
        public int UserCount { get; set; }
        /// <summary>
        /// ожидаемое количество пользователей
        /// </summary>
        [JsonProperty("expected_user_count")]
        public int ExpectedUserCount { get; set; }
        /// <summary>
        /// id игры
        /// </summary>
        [JsonProperty("session_id")]
        public int SessionId { get; set; }
    }
}