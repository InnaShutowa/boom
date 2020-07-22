using BoombCats.Lib.Enums;
using BoombCats.Lib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoombCats.Lib.Models {
    // модель с основными данными по игре
    public class MainGameModel {
        public MainGameModel(SessionModel session) {
            
            var gamePack = session.MainPackModel.Cards.Where(a => a.Status == CardStatusEnum.Free).ToList();
            var sbros = session.MainPackModel.Cards.Where(a => a.Status == CardStatusEnum.Used).ToList();

            sbros.Add(new CardModel(TypeCardEnum.Skip));

            var usersPack = new List<CardsPackApiModel>();

            Sbros = new CardsPackApiModel(sbros.Count, sbros);
            MainGamePack = new CardsPackApiModel(gamePack.Count, gamePack);

            session.MainPackModel.Users.ForEach(user => {
                usersPack.Add(new CardsPackApiModel(user.Count, user.Cards.ToList(), user.IsLoser, user.IsConnected, user.UserId, user.IsActivated, user.IsWinner));
            });
            
           
            UsersPack = usersPack.ToArray();
            SessionId = session.SessionId;
            SelectedCardsIds = session.SelectedCardsIdsList.ToArray();
        }
        /// <summary>
        /// основная игровая колода
        /// </summary>
        [JsonProperty("pack")]
        public CardsPackApiModel MainGamePack { get; set; }
        /// <summary>
        /// сброс
        /// </summary>
        [JsonProperty("sbros")]
        public CardsPackApiModel Sbros { get; set; }
        /// <summary>
        /// колоды игроков
        /// </summary>
        [JsonProperty("users")]
        public CardsPackApiModel[] UsersPack { get; set; }
        /// <summary>
        /// id карт, которые лежат в центре поля
        /// </summary>
        [JsonProperty("selected_cards_ids")]
        public int[] SelectedCardsIds { get; set; }
        /// <summary>
        /// id игры
        /// </summary>
        [JsonProperty("id")]
        public int SessionId { get; set; }
    }
    // модель колоды карт
    public class CardsPackApiModel {
        public CardsPackApiModel(int count, 
            List<CardModel> cards, 
            bool loser = false, 
            bool isConnected = false, 
            int userId = 0,
            bool isActive = false,
            bool isWinner = false) {
            Count = count;
            var crds = new List<CardApiModel>();
            cards.ForEach(card => {
                crds.Add(new CardApiModel(card.Number, card.Type, card.Status));
            });
            Cards = crds.ToArray();
            Loser = loser;
            IsConnected = isConnected;
            UserId = userId;
            IsActiveUser = isActive;
            Winner = isWinner;
        }
        /// <summary>
        /// количество
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }
        /// <summary>
        /// карты        
        /// /// </summary>
        [JsonProperty("cards")]
        public CardApiModel[] Cards { get; set; }
        /// <summary>
        /// id игрока
        /// </summary>
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        /// <summary>
        /// проиграл игрок или нет
        /// </summary>
        [JsonProperty("loser")]
        public bool Loser { get; set; } = false;
        /// <summary>
        /// победил игрок или нет
        /// </summary>
        [JsonProperty("winner")]
        public bool Winner { get; set; } = false;
        /// <summary>
        /// подключен данный пользователь или нет
        /// </summary>
        [JsonProperty("is_connected")]
        public bool IsConnected { get; set; } = false;
        /// <summary>
        /// этот пользователь сейчас играет или нет
        /// </summary>
        [JsonProperty("is_active_user")]
        public bool IsActiveUser { get; set; } = false;
    }
    /// <summary>
    /// модель карты
    /// </summary>
    public class CardApiModel {
        public CardApiModel(int num, TypeCardEnum type, CardStatusEnum status) {
            IdCard = num;
            Type = type;
            CardStatus = status;
        }
        /// <summary>
        /// номер карты
        /// </summary>
        [JsonProperty("id")]
        public int IdCard { get; set; }
        /// <summary>
        /// "масть"
        /// </summary>
        [JsonProperty("type")]
        public TypeCardEnum Type { get; set; }
        /// <summary>
        /// где карта, на поле или нет
        /// </summary>
        [JsonProperty("card_status")]
        public CardStatusEnum CardStatus { get; set; }
    }
}