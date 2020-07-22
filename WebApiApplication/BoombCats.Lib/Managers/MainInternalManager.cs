using BoombCats.Lib.Enums;
using BoombCats.Lib.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoombCats.Lib.Managers {
    public static class MainInternalManager {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

     

        /// <summary>
        /// отправляем карты в сброс, обновляем модельки
        /// </summary>
        /// <param name="data"></param>
        public static void PutCardInSbros(int userId, int[] cardsId, int sessionId) {
            try {
                var userCards = CardManager
                    .Sessions
                    .FirstOrDefault(a=>a.SessionId==sessionId)?
                    .MainPackModel
                    .Users
                    .First(a => a.UserId == userId)
                    .Cards;
                if (userCards == null) return;
                cardsId.ToList().ForEach(id => {
                    var cardModel = userCards.FirstOrDefault(a => a.Number == id);
                    if (cardModel != null) {
                        userCards.Remove(cardModel);

                        userCards.ToList().ForEach(card => {
                            if (cardsId.Contains(card.Number)) {
                                card.Status = CardStatusEnum.Used;
                            }
                        });
                    }
                });
                CardManager
                    .Sessions
                    .First(a=>a.SessionId==sessionId)
                    .MainPackModel
                    .Users
                    .First(a => a.UserId == userId)
                    .Count = userCards.Count();
                CardManager
                    .Sessions
                    .First(a => a.SessionId == sessionId)
                    .MainPackModel
                    .Users
                    .First(a => a.UserId == userId)
                    .Cards = userCards;
            } catch (Exception ex) {
                Logger.Error($"PutCardsInSbros {ex.Message}");
            }
        }
        /// <summary>
        /// получаем нужное количество карт из колоды
        /// </summary>
        /// <param name="count"></param>
        /// <param name="userId"></param>
        public static void GetCarFromPack(int count, int userId, int sessionId) {
            try {
                var buff = 0;
                var isLoser = false;
                var user = CardManager
                    .Sessions
                    .FirstOrDefault(a => a.SessionId == sessionId)?
                    .MainPackModel
                    .Users
                    .FirstOrDefault(a=>a.UserId==userId);

                if (user == null) return;

                var selectedCard = new List<CardModel>();
                foreach (var card in 
                    CardManager
                    .Sessions
                    .FirstOrDefault(a => a.SessionId == sessionId)?
                    .MainPackModel
                    .Cards
                    .Where(a => a.Status == CardStatusEnum.Free)) {
                    if (buff == count) break;
                    buff++;
                    card.Status = CardStatusEnum.Active;
                    card.UserId = userId;
                    selectedCard.Add(card);
                    if (card.Type == TypeCardEnum.Boomb) isLoser = true;
                }

                CardManager.Sessions.First(a => a.SessionId == sessionId).MainPackModel.Users.First(a=>a.UserId==userId).Count += buff;
                CardManager.Sessions.First(a => a.SessionId == sessionId).MainPackModel.Users.First(a=>a.UserId==userId).Cards.AddRange(selectedCard);
                CardManager.Sessions.First(a => a.SessionId == sessionId).SelectedCardsIdsList = new List<int>();
                CardManager
                    .Sessions
                    .FirstOrDefault(a => a.SessionId == sessionId)
                    .MainPackModel
                    .Users
                    .First(a=>a.UserId==userId)
                    .IsLoser = isLoser;

            } catch (Exception ex) {
                Logger.Error($"PutCardsInSbros {ex.Message}");
            }
        }
    }
}
