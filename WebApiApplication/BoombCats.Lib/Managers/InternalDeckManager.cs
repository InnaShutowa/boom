using BoombCats.Lib.Enums;
using BoombCats.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoombCats.Lib.Managers {
    /// <summary>
    /// менеджер для работы с колодой
    /// </summary>
    public static class InternalDeckManager {
        /// <summary>
        /// формируем колоду карт для пользователя
        /// </summary>
        public static MainUserModel CreateUserPack(int userId, int sessionId) {
            try {
                var model = new MainUserModel();
                var cards = new List<CardModel>();
                var rand = new Random();
                var count = 0;
                var workModel = CardManager.Sessions.FirstOrDefault(a => a.SessionId == sessionId);
                if (workModel == null) return null;
                while (count < 4) {
                    // берем карту из общей колоды и отмечаем, что она уже используется
                    while (true) {
                        var num = rand.Next(1, workModel.MainPackModel.Count);
                        var card = workModel.MainPackModel.Cards.First(a => a.Number == num);
                        if (card.Status == CardStatusEnum.Free && card.Type != TypeCardEnum.Boomb && card.Type != TypeCardEnum.Neutralize) {
                            cards.Add(card);
                            workModel.MainPackModel.Cards.First(a => a.Number == num).Status = CardStatusEnum.Active;
                            workModel.MainPackModel.Cards.First(a => a.Number == num).UserId = userId;
                            break;
                        }
                    }
                    count++;
                }
                cards.Add(workModel.MainPackModel.Cards.FirstOrDefault(a => a.Status == CardStatusEnum.Free && a.Type == TypeCardEnum.Neutralize));
                workModel.MainPackModel.Cards.FirstOrDefault(a => a.Status == CardStatusEnum.Free && a.Type == TypeCardEnum.Neutralize).Status = CardStatusEnum.Active;
                CardManager.Sessions.First(a => a.SessionId == sessionId).MainPackModel = workModel.MainPackModel;
                model.Cards = cards;
                model.Count = count + 1;
                model.UserId = userId;
                return model;
            } catch (Exception ex) {
                return null;
            }
        }
        /// <summary>
        /// генерация колоды
        /// </summary>
        public static ResultModel InternalGeneratePack(int userCount) {
            try {
                var sessionId = CardManager.CreateMainPack();
                var users = new List<MainUserModel>();

                var session = CardManager
                        .Sessions
                        .FirstOrDefault(a => a.SessionId == sessionId);
                if (session == null)
                    return new ResultModel(false, "sessionId is wrong");

                for (var j = 1; j <= userCount; j++) {
                    users.Add(CreateUserPack(j, sessionId));
                }

                session.MainPackModel.Users = users;
                session.MainPackModel.Users.First().IsActivated = true;
                session.MainPackModel.Users.First().IsConnected = true;
                session.ExpectedUserCount = users.Count;
                session.MainPackModel.Cards
                    = CardManager.MixCards(session.MainPackModel.Cards.ToList()).ToArray();

                CardManager.UpdateSession(session);
                var result = new MainGameModel(session);
                return new ResultModel(true, result);
            } catch (Exception ex) {
                return new ResultModel(false);
            }
        }

        /// <summary>
        /// получаем инфу по состоянию колоды в данный момент
        /// </summary>
        public static ResultModel InternalGetCurrentPack(int sessionId) {
            try {
                var workModel = CardManager.Sessions.FirstOrDefault(a => a.SessionId == sessionId)?.MainPackModel;

                if (workModel == null
                    || workModel.Count == 0
                    || workModel.Users.Count == 0)
                    return new ResultModel(false, "колода пуста");

                var session = CardManager.Sessions?.FirstOrDefault(a => a.SessionId == sessionId);
                if (session == null)
                    return new ResultModel(false, "SessionId is wrong!");

                var result = new MainGameModel(session);
                return new ResultModel(true, result);
            } catch (Exception ex) {
                return new ResultModel(false, ex.Message);
            }
        }
    }
}
