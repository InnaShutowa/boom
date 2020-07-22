using Assets.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static Assets.Models.ApiModels;

namespace Assets {
    public class HttpQueryManager {
        private static string Text = "";


        public static void LoadFromServer(string url) {
            var www = new WWW(url);
            while (!www.isDone) {
                continue;
            }

            if (!string.IsNullOrEmpty(www.error)) {
                Debug.Log(www.error);
            } else {
                Text = www.text;
            }
        }
        /// <summary>
        /// отменяем шаг
        /// </summary>
        public static ApiMainGameModel CanselShag() {
            try {
                var model = new CanselShagModel() {
                    card_ids = ApiClientScript.SelectedCardsIds.ToArray(),
                    session_id = ApiClientScript.SessionId
                };
                var data = JsonUtility.ToJson(model);

                var url = "http://localhost:61934/api/ApiLogicSecond/CanselShag?data=" + data;
                LoadFromServer(url);

                if (Text == "") Debug.Log("CanselShag Text = null");
                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) {
                    return null;
                }
                if (!string.IsNullOrEmpty(query.error)) {
                    return null;
                }
                var result = query.data;

                return result;
            } catch (Exception ex) {
                Debug.Log($"CanselShag {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// удаляем законченную сессию
        /// </summary>
        public static bool DeleteSession() {
            try {
                var url = "http://localhost:61934/api/ApiLogic/DeleteSession?session_id=" + ApiClientScript.SessionId;
                LoadFromServer(url);

                if (Text == "") Debug.Log("DeleteSession Text = null");

                if (Text == "true") return true;
                return false;
            } catch (Exception ex) {
                Debug.Log($"DeleteSession {ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// заканчиваем шаг
        /// </summary>
        public static AfterFinishShagModel FinishShag() {
            try {
                var model = new FinishShagModel() {
                    session_id = ApiClientScript.SessionId,
                    user_id = ApiClientScript.CurrentUserId,
                    cards_id = ApiClientScript.SelectedCardsIds?.ToArray()
                };

                var data = JsonUtility.ToJson(model);

                var url = "http://localhost:61934/api/ApiLogic/FinishShag?data=" + data;
                LoadFromServer(url);

                if (Text == "") Debug.Log("FinishShag Text = null");
                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) {
                    return null;
                }
                if (!string.IsNullOrEmpty(query.error)) {
                    Debug.Log(query.error);
                    return null;
                }

                var result = new AfterFinishShagModel() {
                    Data = query.data,
                    StatusShag = query.status
                };

                return result;
            } catch (Exception ex) {
                Debug.Log($"FinishShag {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// добавляем карты на стол
        /// </summary>
        public static ApiMainGameModel DoShag(int cardNumber) {
            try {
                var url = "http://localhost:61934/api/ApiLogic/DoShag?session_id="
                    + ApiClientScript.SessionId
                    + "&user_id=" + ApiClientScript.CurrentUserId
                    + "&card_number=" + cardNumber;
                LoadFromServer(url);

                if (Text == "") Debug.Log("DoShag Text = null");
                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) {
                    return null;
                }
                if (!string.IsNullOrEmpty(query.error)) {
                    Debug.Log(query.error);
                    return null;
                }
                var result = query.data;

                return result;
            } catch (Exception ex) {
                Debug.Log($"DoShag {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// активируем пользователя, который приоединяется к существующей игре
        /// </summary>
        public static ApiMainGameModel DeleteUserFromGame() {
            try {
                var url = "http://localhost:61934/api/DeleteUser?session_id="
                    + ApiClientScript.SessionId
                    + "&user_id="
                    + ApiClientScript.CurrentUserId;

                LoadFromServer(url);

                if (Text == "") Debug.Log("DeleteUserFromGame Text = null");
                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) {
                    return null;
                }
                if (!string.IsNullOrEmpty(query.error)) {
                    Debug.Log(query.error);
                    return null;
                }
                var result = query.data;

                return result;
            } catch (Exception ex) {
                Debug.Log($"DeleteUserFromGame {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// получаем список всех текущих игр, где не хватает игрока
        /// </summary>
        public static ApiResulSessionModel GetSessions() {
            try {
                var url = "http://localhost:61934/api/ApiSessions";
                LoadFromServer(url);
                if (Text == "") Debug.Log("GetSessions Text = null");
                var query = JsonUtility.FromJson<ApiResulSessionModel>(Text);
                Debug.Log(Text);
                if (query == null) {
                    Debug.Log("пришел null или нихера не распарсилось");
                    return null;
                }

                var result = query;
                return result;
            } catch (Exception ex) {
                Debug.Log($"GetSessions {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// активируем пользователя, который приоединяется к существующей игре
        /// </summary>
        public static ApiMainGameModel ChangeActiveUser(int sessionId) {
            try {
                var url = "http://localhost:61934/api/ApiLogicSecond/ChangeActiveUser?session_id=" + sessionId;
                LoadFromServer(url);

                if (Text == "") Debug.Log("ChangeActiveUser Text=null");
                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) {
                    return null;
                }
                if (!string.IsNullOrEmpty(query.error)) {
                    Debug.Log(query.error);
                    return null;
                }
                var result = query.data;
                return result;
            } catch (Exception ex) {
                Debug.Log($"ChangeActiveUser {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// активируем пользователя, который приоединяется к существующей игре
        /// </summary>
        public static ApiMainGameModel ActivateUser(int userId, int sessionId) {
            try {
                var url = "http://localhost:61934/api/ApiMain/ActivateUser?session_id=" + sessionId + "&user_id=" + userId;
                LoadFromServer(url);

                if (Text == "") Debug.Log("ActivateUser text=null");
                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) {
                    return null;
                }
                if (!string.IsNullOrEmpty(query.error)) {
                    Debug.Log(query.error);
                    return null;
                }
                var result = query.data;
                Debug.Log(result);
                return result;
            } catch (Exception ex) {
                Debug.Log($"ActivateUser {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// запрос к апишке на создание новой колоды на заданное количество игроков
        /// </summary>
        public static ApiMainGameModel GetCardsForPlayers(int userCount) {
            try {
                var result = new ApiMainGameModel();
                if (userCount < 0) return null;

                var url = "http://localhost:61934/api/ApiMain/GeneratePack?count_users=" + userCount;
                UnityWebRequest www = UnityWebRequest.Get(url);

                LoadFromServer(url);

                if (Text == "") Debug.Log("GetCardsForPlayers text = null");
                Debug.Log("fuckk");
                Debug.Log(Text);
                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) {
                    return null;
                }
                if (!string.IsNullOrEmpty(query.error)) {
                    Debug.Log(query.error);
                    return null;
                }
                result = query.data;

                Debug.Log("fuckk");
                Debug.Log(result);
                return result;
            } catch (Exception ex) {
                Debug.Log($"GetCardsForPlayers {ex.Message}");
                return null;
            }
        }
        // получаем модель игровой колоды на данный момент
        public static ApiMainGameModel GetCurrentPack(int session_id) {
            try {

                var data = new ApiResultModel();
                var result = new ApiMainGameModel();
                var url = "http://localhost:61934/api/ApiMain/GetCurrentPack?session_id=" + session_id;

                LoadFromServer(url);

                if (Text == "") Debug.Log("GetCurrentPack text = null");

                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) {
                    return null;
                }
                if (!string.IsNullOrEmpty(query.error)) {
                    Debug.Log(query.error);
                    return null;
                }
                result = query.data;

                return result;
            } catch (Exception ex) {
                Debug.Log($"GetCurrentPack {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// берем карты из колоды
        /// </summary>
        public static ApiMainGameModel GetCardsFromPack(int count, int userId, int sessionId) {
            try {
                var data = new ApiGetCardsFromPackModel(count, userId);
                var result = new ApiMainGameModel();
                var url = "http://localhost:61934/api/ApiMain/GetCardsFromPack?count=" + count + "&user_id=" + userId + "&session_id=" + sessionId;

                //var url = "http://localhost:61934/api/GeneratePack/GetCards?count="+count+"&user_id="+userId+"&session_id="+sessionId;
                var str = JsonUtility.ToJson(data);
                LoadFromServer(url);

                if (Text == "") Debug.Log("GetCardsFromPack text=null");
                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) return null;
                
                if (!string.IsNullOrEmpty(query.error)) {
                    Debug.Log(query.error);
                    return null;
                }
                result = query.data;

                return result;
            } catch (Exception ex) {
                Debug.Log($"GetCardsFromPack {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// отправляем карты в сброс
        /// </summary>
        public static ApiMainGameModel PutCardsInSbros() {
            try {
                var data = new ApiPutCardsInSbros(ApiClientScript.CurrentUserId, ApiClientScript.SelectedCardsIds.ToArray(), ApiClientScript.SessionId);
                var str = JsonUtility.ToJson(data);
                var result = new ApiMainGameModel();
                var url = "http://localhost:61934/api/ApiMain/PutCardsInSbros?data=" + str;
                LoadFromServer(url);

                if (Text == "") Debug.Log("PutCardsInSbros text=null");
                var query = JsonUtility.FromJson<ApiResultModel>(Text);
                if (query == null) return null;
                
                if (!string.IsNullOrEmpty(query.error)) {
                    Debug.Log(query.error);
                    return null;
                }
                result = query.data;

                return result;
            } catch (Exception ex) {
                Debug.Log($"PutCardsInSbros {ex.Message}");
                return null;
            }
        }
    }
}
