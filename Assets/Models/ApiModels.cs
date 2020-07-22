using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Models {
    public class ApiModels : MonoBehaviour {
        public class AfterFinishShagModel {
            public bool StatusShag { get; set; }
            public ApiMainGameModel Data { get; set; }
        }

        [Serializable]
        public class CanselShagModel {
            public int[] card_ids;
            public int session_id;
        }

        [Serializable]
        public class FinishShagModel {
            public int[] cards_id;
            public int session_id;
            public int user_id;
        }
        
        [Serializable]
        public class MessageToClientModel {
            public ApiMainGameModel main_game_model;
            public int current_user_id;
        }

        /// <summary>
        /// модель для результатов операций с картами
        /// </summary>
        [Serializable]
        public class ApiResulSessionModel {
            public string error;
            public bool status;
            public ApiSessionModel[] data;
        }
        /// <summary>
        /// модель для получения инфы по сессии
        /// </summary>
        [Serializable]
        public class ApiSessionModel {
            public int user_count;
            public int expected_user_count;
            public int session_id;
        }
        /// <summary>
        /// модель для результатов операций с картами
        /// </summary>
        [Serializable]
        public class ApiResultModel {
            public string error;
            public bool status;
            public ApiMainGameModel data;
        }
        [Serializable]
        public class ApiMainGameModel {
            /// <summary>
            /// основная игровая колода
            /// </summary>
            public ApiCardsPackModel pack;
            /// <summary>
            /// сброс
            /// </summary>
            public ApiCardsPackModel sbros;
            /// <summary>
            /// колоды игроков
            /// </summary>
            public ApiCardsPackModel[] users;
            public int[] selected_cards_ids;
            public int id;
        
        }
        [Serializable]
        public class ApiCardsPackModel {
            /// <summary>
            /// id 
            /// </summary>
            public int user_id;
            /// <summary>
            /// количество
            /// </summary>
            public int count;
            /// <summary>
            /// карты        
            /// </summary>
            public ApiCardModel[] cards;
            ///// <summary>
            ///// текущий пользователь или нет
            ///// </summary>
            //public bool is_current_user;
            /// <summary>
            /// проиграл пользователь или нет
            /// </summary>
            public bool loser;
            /// <summary>
            /// подключен пользователь или нет
            /// </summary>
            public bool is_connected;
            /// <summary>
            /// играет пользователь или нет
            /// </summary>
            public bool is_active_user;
            /// <summary>
            /// победил пользователь или нет
            /// </summary>
            public bool winner;
        }
        [Serializable]
        public class ApiCardModel {
            /// <summary>
            /// номер карты
            /// </summary>
            public int id;
            /// <summary>
            /// "масть"
            /// </summary>
            public TypeCardEnum type;
            public CardStatusEnum card_status;
        }
        [Serializable]
        public class ApiGetCardsFromPackModel {
            public ApiGetCardsFromPackModel() { }
            public ApiGetCardsFromPackModel(int cnt, int userId) {
                count = cnt;
                id_user = userId;
            }
            /// <summary>
            /// id пользователя
            /// </summary>
            public int id_user;
            /// <summary>
            /// количество карт 
            /// </summary>
            public int count { get; set; }
        }
        /// <summary>
        /// модель для post запроса на отправку карт в сброс 
        /// </summary>
        [Serializable]
        public class ApiPutCardsInSbros {
            public ApiPutCardsInSbros() { }
            public ApiPutCardsInSbros(int userId, int[] cardIds, int sessionId) {
                user_id = userId;
                cards_id = cardIds;
                session_id = sessionId;
            }
            /// <summary>
            /// id пользователя
            /// </summary>
            public int user_id;
            /// <summary>
            /// список номеров карт, которые мы хотим отправить в сброс
            /// </summary>
            public int[] cards_id;
            /// <summary>
            /// id текущей игры
            /// </summary>
            public int session_id;
        }
    }
}
