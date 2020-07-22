using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Models.ApiModels;

namespace Assets.Models {
    // основная игровая модель 
    public class MainModel {
        public MainModel() { }
        public MainModel(ApiMainGameModel apiModel) {
            Pack = new PackModel(apiModel.pack);
            Sbros = new PackModel(apiModel.sbros);
            Users = new List<UserModel>();
            apiModel.users.ToList().ForEach(usr => {
                Users.Add(new UserModel(usr));
            });
        }

        public PackModel Pack { get; set; }
        public PackModel Sbros { get; set; }
        public List<UserModel> Users { get; set; }
    }
    // модель колоды
    public class PackModel {
        public PackModel(ApiCardsPackModel packApiModel) {
            Count = packApiModel.count;
            Cards = new List<CardModel>();
            packApiModel.cards.ToList().ForEach(elem =>{
                Cards.Add(new CardModel(elem));
            });
        }
        public int Count { get; set; }
        public List<CardModel> Cards { get; set; }
    }
    // модель игрока
    public class UserModel {
        public UserModel() { }
        public UserModel(ApiCardsPackModel userApiModel) {
            UserId = userApiModel.user_id;
            CountCards = userApiModel.count;
            IsConnected = userApiModel.is_connected;
            CardsModel = new List<CardModel>();
            IsLoser = userApiModel.loser;
            userApiModel.cards.ToList().ForEach(card => {
                CardsModel.Add(new CardModel(card));
            });
            IsWinner = userApiModel.winner;
        }
        public TcpClient Client { get; set; }
        public bool IsLoser { get; set; }
        public int UserId { get; set; }
        public int CountCards { get; set; }
        //public bool IsCurrentUser { get; set; }
        public List<CardModel> CardsModel { get; set; }
        public Transform UserPack { get; set; }
        public TcpClient TcpClient { get; set; } = null;
        public bool IsConnected { get; set; } = false;
        public bool IsWinner { get; set; } = false;
    }
    // модель карты
    public class CardModel {
        public CardModel(ApiCardModel cardApiModel) {
            TypeCard = cardApiModel.type;
            CardNumber = cardApiModel.id;
            CardStatus = cardApiModel.card_status;
        }
        public TypeCardEnum TypeCard { get; set; }
        public int CardNumber { get; set; }
        public CardStatusEnum CardStatus { get; set; }
        public Transform Card { get; set; }
    }
}
