using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoombCats.Lib.Models {
    /// <summary>
    /// моделька, чтобы удалить карты из выбранных
    /// </summary>
    public class ApiCanselShagModel {
        [JsonProperty("card_ids")]
        public int[] CardIds { get; set; }
        [JsonProperty("session_id")]
        public int SessionId { get; set; }
    }
    /// <summary>
    /// модель для завершения хода
    /// </summary>
    public class ApiFinishShagModel {
        [JsonProperty("cards_id")]
        public int[] CardsId { get; set; }
        [JsonProperty("session_id")]
        public int SessionId { get; set; }
        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
    /// <summary>
    /// модель для post запроса на получение карты из колоды
    /// </summary>
    public class ApiGetCardsFromPackModel {
        /// <summary>
        /// id пользователя
        /// </summary>
        [JsonProperty("id_user")]
        public int IdUser { get; set; }
        /// <summary>
        /// количество карт 
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }
    }
    /// <summary>
    /// модель для post запроса на отправку карт в сброс 
    /// </summary>
    public class ApiPutCardsInSbros {
        /// <summary>
        /// id пользователя
        /// </summary>
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        /// <summary>
        /// список номеров карт, которые мы хотим отправить в сброс
        /// </summary>
        [JsonProperty("cards_id")]
        public int[] CardsId { get; set; }
        /// <summary>
        /// id игры
        /// </summary>
        [JsonProperty("session_id")]
        public int SessionId { get; set; }
    }

}