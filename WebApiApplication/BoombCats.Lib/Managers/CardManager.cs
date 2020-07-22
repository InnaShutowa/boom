using BoombCats.Lib.Enums;
using BoombCats.Lib.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoombCats.Lib.Managers {
    public static class CardManager {

        private static Logger Logger = LogManager.GetCurrentClassLogger();
        public static List<SessionModel> Sessions = new List<SessionModel>();
        
        /// <summary>
        /// отменить шаг
        /// </summary>
        public static MainGameModel InternalCanselShag(string data) {
            try {
                if (string.IsNullOrEmpty(data)) return null;
                var mdl = JsonConvert.DeserializeObject<ApiCanselShagModel>(data);
                var session = Sessions.FirstOrDefault(a => a.SessionId == mdl.SessionId);
                if (session == null) return null;

                mdl.CardIds.ToList().ForEach(id => {
                    session
                    .SelectedCardsIdsList
                    .Remove(id);
                });
                UpdateSession(session);
                var result = new MainGameModel(session);
                return result;
            } catch (Exception ex) {
                return null;
            }
        }
        /// <summary>
        /// делем что-то с картами, когда меняется активный пользователь
        /// </summary>
        public static void GetUserCard(int sessionId) {
            try {
                var session = Sessions
                           .FirstOrDefault(a => a.SessionId == sessionId);
                if (session == null)
                    return;
                var mainPack = session.MainPackModel;
                var oldActiveUserId = mainPack
                           .Users
                           .First(a => a.IsActivated).UserId;
                var minUserId = mainPack
                    .Users
                    .Select(a => a.UserId).Min();
                var maxUserId = mainPack
                    .Users
                    .Select(a => a.UserId).Max();

                if (oldActiveUserId < maxUserId) {
                    var selCards = mainPack
                   .Users
                   .First(a => a.UserId == maxUserId).Cards.First();

                    mainPack
                    .Users
                    .First(a => a.IsActivated).Cards.Add(selCards);

                    mainPack
                   .Users
                   .First(a => a.UserId == maxUserId)
                   .Cards
                   .Remove(selCards);
                } else {
                    var selCards = mainPack
                   .Users
                   .First(a => a.UserId == minUserId).Cards.First();

                    mainPack
                    .Users
                    .First(a => a.IsActivated).Cards.Add(selCards);

                    mainPack
                   .Users
                   .First(a => a.UserId == minUserId)
                   .Cards
                   .Remove(selCards);
                }
                session.MainPackModel = mainPack;
                UpdateSession(session);
            } catch (Exception ex) {
                Logger.Error($"GetUserCard {ex.Message}");
            }
        }

        /// <summary>
        /// после создания новой сессии, нужно заполнить ее данными
        /// </summary>
        public static void UpdateSession(SessionModel session) {
            if (Sessions.FirstOrDefault(a => a.SessionId == session.SessionId) != null) {
                Sessions.FirstOrDefault(a => a.SessionId == session.SessionId).ExpectedUserCount = session.ExpectedUserCount;
                Sessions.FirstOrDefault(a => a.SessionId == session.SessionId).MainPackModel = session.MainPackModel;
                Sessions.FirstOrDefault(a => a.SessionId == session.SessionId).SelectedCardsIdsList = session.SelectedCardsIdsList;
            }
        }
        /// <summary>
        /// формируем основную колоду
        /// </summary>
        public static int CreateMainPack() {
            try {
                var totalModel = new MainPackModel();
                var cards = new List<CardModel>();

                for (var i = 0; i < 4; i++) {
                    cards.Add(new CardModel(TypeCardEnum.Boomb));
                    cards.Add(new CardModel(TypeCardEnum.Attack));
                    cards.Add(new CardModel(TypeCardEnum.Skip));
                    cards.Add(new CardModel(TypeCardEnum.Lending));
                    cards.Add(new CardModel(TypeCardEnum.Mix));
                    cards.Add(new CardModel(TypeCardEnum.CatCardOne));
                    cards.Add(new CardModel(TypeCardEnum.CatCardTwo));
                    cards.Add(new CardModel(TypeCardEnum.CatCardThree));
                    cards.Add(new CardModel(TypeCardEnum.CatCardFour));
                    cards.Add(new CardModel(TypeCardEnum.CatCardFive));
                }

                for (var i = 0; i < 6; i++) {
                    cards.Add(new CardModel(TypeCardEnum.Neutralize));
                }

                for (var i = 0; i < 6; i++) {
                    cards.Add(new CardModel(TypeCardEnum.No));
                    cards.Add(new CardModel(TypeCardEnum.LookInFuture));
                }

                for (var i = 0; i < cards.Count; i++) {
                    cards.ForEach(a => {
                        a.Number = i;
                        i++;
                    });
                }

                totalModel.Cards = cards.ToArray();
                Sessions.Add(new SessionModel(totalModel));

                return Sessions.Max(a => a.SessionId);
            } catch (Exception ex) {
                Logger.Error(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// перемешиваем карты
        /// </summary>
        public static List<CardModel> MixCards(List<CardModel> cards) {
            try {
                var result = new List<CardModel>();
                var i = 0;
                while (i < cards.Count) {
                    var rand = new Random();
                    while (true) {
                        var num = rand.Next(0, cards.Count);
                        if (result.All(a => a.Number != cards[num].Number)) {
                            result.Add(cards[num]);
                            break;
                        }
                    }
                    i++;
                }
                return result;
            } catch (Exception ex) {
                Logger.Error(ex.Message);
                return null;
            }
        }
    }
}
