using BoombCats.Lib.Enums;
using BoombCats.Lib.Managers;
using BoombCats.Lib.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MainGameModel = BoombCats.Lib.Models.MainGameModel;
using ResultModel = BoombCats.Lib.Models.ResultModel;

namespace WebApiApplication.Managers {
    public static class ApiCardManager {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// отменяем ход
        /// </summary>
        public static ResultModel CanselShag(string data) {
            try {
                var result = CardManager.InternalCanselShag(data);
                if (result == null)
                    return new ResultModel(false, "Something wrong");

                return new ResultModel(true, result);
            } catch (Exception ex) {
                return new ResultModel(false, ex.Message);
            }
        }

        /// <summary>
        /// удаляем сессию из списка, если она закончена
        /// </summary>
        public static bool DeleteSession(int sessionId) {
            try {
                var session = CardManager.Sessions.FirstOrDefault(a => a.SessionId == sessionId);
                if (session == null) return false;
                CardManager.Sessions.Remove(session);
                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        /// <summary>
        /// передаем ход другому пользователю
        /// </summary>
        public static ResultModel ChangeActiveUser(int sessionId) {
            try {
                var result = InternalPlayerManager.InternalChangeActiveUser(sessionId);
                if (result == null)
                    return new ResultModel(false, "Something wrong");
                return new ResultModel(true, result);
            } catch (Exception ex) {
                return new ResultModel(false, ex.Message);
            }
        }

        /// <summary>
        /// завершаем ход
        /// </summary>
        public static ResultModel FinishShag(string data) {
            try {
                if (string.IsNullOrEmpty(data))
                    return new ResultModel(false, "data is null");
                var model = JsonConvert.DeserializeObject<ApiFinishShagModel>(data);

                var session = CardManager.Sessions.FirstOrDefault(a => a.SessionId == model.SessionId);
                if (session == null) return new ResultModel(false, "session id is wrong");
                var user = session.MainPackModel?.Users?.FirstOrDefault(a => a.UserId == model.UserId);
                if (user == null) return new ResultModel(false, "user id in wrong");

                if (model.CardsId.Count() == 0)
                    return new ResultModel(false, "there are no ids");

                if (model.CardsId.Count() == 1) {
                    var card = session.MainPackModel.Cards.First(a => a.Number == model.CardsId.First());
                    if (card.Type == TypeCardEnum.Boomb ||
                        card.Type == TypeCardEnum.CatCardFour ||
                        card.Type == TypeCardEnum.CatCardFive ||
                        card.Type == TypeCardEnum.CatCardOne ||
                        card.Type == TypeCardEnum.CatCardThree ||
                        card.Type == TypeCardEnum.CatCardTwo ||
                        card.Type == TypeCardEnum.LookInFuture ||
                        card.Type == TypeCardEnum.No ||
                        card.Type == TypeCardEnum.Attack)
                        return new ResultModel(false);
                    if (user.IsLoser) {
                        if (card.Type == TypeCardEnum.Neutralize) {
                            var result = InternaCardFunctionsManager.NeutralizeShag(user.UserId, session.SessionId);
                            return new ResultModel(true, result);
                        }
                        return new ResultModel(false);
                    }
                    if (card.Type == TypeCardEnum.Mix) {
                        var result = InternaCardFunctionsManager.MixShag(user.UserId, session.SessionId);
                        return new ResultModel(true, result);
                    }
                    if (card.Type == TypeCardEnum.Skip) {
                        return ChangeActiveUser(session.SessionId);
                    }
                    if (card.Type == TypeCardEnum.Lending) {
                        CardManager.GetUserCard(model.SessionId);
                        var result = new MainGameModel(CardManager.Sessions.First(a => a.SessionId == session.SessionId));
                        return new ResultModel(true, result);
                    }
                    return new ResultModel(false);
                } else {
                    var cards = session.MainPackModel
                        .Cards
                        .Where(a => model.CardsId.Contains(a.Number))
                        .Select(a => a.Type)
                        .ToList();
                    if (!cards.All(a => a == cards.First())) {
                        return new ResultModel(false);
                    } else {
                        CardManager.GetUserCard(model.SessionId);
                        var result = new MainGameModel(CardManager.Sessions.First(a => a.SessionId == session.SessionId));
                        return new ResultModel(true, result);
                    }
                }
            } catch (Exception ex) {
                return new ResultModel(false, ex.Message);
            }
        }

        /// <summary>
        /// сохраняем в апишке данные о том, что пользователь сделал ходи и карта должна лежать в центре поля
        /// </summary>
        public static ResultModel DoShag(int sessionId, int userId, int cardNum) {
            try {
                var session = CardManager.Sessions.FirstOrDefault(a => a.SessionId == sessionId);
                if (session == null) return new ResultModel(false, "session id is wrong");
                var user = session.MainPackModel.Users.FirstOrDefault(a => a.UserId == userId);
                if (user == null) return new ResultModel(false, "user id is wrong");

                session.SelectedCardsIdsList.Add(cardNum);
                CardManager.UpdateSession(session);

                var result = new MainGameModel(session);
                return new ResultModel(true, result);
            } catch (Exception ex) {
                return new ResultModel(false, ex.Message);
            }
        }
        /// <summary>
        /// активируем пользователя при первом подключении
        /// </summary>
        public static ResultModel ActivateUser(int sessionId, int userId) {
            try {
                var result = InternalPlayerManager.InternalActivateUser(userId, sessionId);
                if (result == null)
                    return new ResultModel(false, "Something wrong");
                return new ResultModel(true, result);
            } catch (Exception ex) {
                Logger.Error($"ActivateUser {ex.Message}");
                return new ResultModel(false);
            }
        }
        /// <summary>
        /// получаем список всех игр, созданных на данный момент
        /// </summary>
        public static ResultModel ApiGetSessions() {
            try {
                var result = InternalSessionManager.InternalGetSessionsList();
                if (result == null)
                    return new ResultModel(false, "Something wrong");
                return new ResultModel(true, result?.ToArray());
            } catch (Exception ex) {
                return null;
            }
        }
        /// <summary>
        /// удаляем игрока из сессии
        /// </summary>
        public static ResultModel ApiDeleteUserFromGame(int userId, int sessionId) {
            try {
                var result = InternalSessionManager.InternalDeleteUserFromGame(userId, sessionId);
                if (result == null)
                    return new ResultModel(false, "Something wrong");
                return new ResultModel(true, result);
            } catch (Exception ex) {
                Logger.Error($"ApiDeleteUserFromGame {ex.Message}");
                return new ResultModel(false, ex.Message);
            }
        }
        /// <summary>
        /// отправляем карты определенного пользователя в сброс
        /// </summary>
        public static ResultModel ApiPutCardsInSbros(string data) {
            try {
                /// проверочки
                var model = JsonConvert.DeserializeObject<ApiPutCardsInSbros>(data);
                if (model == null) return new ResultModel(false, "model is null");
                var workModel = CardManager.Sessions.FirstOrDefault(a => a.SessionId == model.SessionId).MainPackModel;
                if (!workModel.Users.Any(a => a.UserId == model.UserId))
                    return new ResultModel(false, "пользователь вообще не найден");

                MainInternalManager.PutCardInSbros(model.UserId, model.CardsId, model.SessionId);

                var result = new MainGameModel(CardManager.Sessions.First(a => a.SessionId == model.SessionId));
                return new ResultModel(true, result);
            } catch (Exception ex) {
                Logger.Error($"ApiPutCardsInSbros {ex.Message}");
                return new ResultModel(false, ex.Message);
            }
        }

        /// <summary>
        /// получаем карты из колоды
        /// </summary>
        public static ResultModel ApiGetCardsFromPack(int count, int userId, int sessionId) {
            try {
                var workModel = CardManager.Sessions.FirstOrDefault(a => a.SessionId == sessionId)?.MainPackModel;

                if (workModel == null) return new ResultModel(false, "sessionId is wrong");
                if (workModel.Cards.Count(a => a.Status == CardStatusEnum.Free) > 0) {

                    MainInternalManager.GetCarFromPack(count, userId, sessionId);
                    var result = new MainGameModel(CardManager.Sessions.First(a => a.SessionId == sessionId));

                    return new ResultModel(true, result);
                }
                return new ResultModel(false, "Не осталось карт в колоде");
            } catch (Exception ex) {
                Logger.Error($"ApiGetCardsFromPack {ex.Message}");
                return new ResultModel(false, ex.Message);
            }
        }
        /// <summary>
        /// формируем колоду
        /// </summary>
        public static ResultModel ApiGeneratePack(int userCount) {
            try {
                var res = InternalDeckManager.InternalGeneratePack(userCount);
                return res;
            } catch (Exception ex) {
                Logger.Error($"ApiGeneratePack {ex.Message}");

                return new ResultModel(false, ex.Message);
            }
        }
        /// <summary>
        /// получаем текущее состояние игровой колоды
        /// </summary>
        public static ResultModel ApiGetCurrentPack(int sessionId) {
            return InternalDeckManager.InternalGetCurrentPack(sessionId);
        }

    }
}