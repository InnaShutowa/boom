using BoombCats.Lib.Enums;
using BoombCats.Lib.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoombCats.Lib.Models {
    /// <summary>
    /// модель всех игр, идущах на данный момент
    /// </summary>
    public class SessionModel {
        public SessionModel() { }
        public SessionModel(MainPackModel totalModel) {
            MainPackModel = totalModel;
            ExpectedUserCount = 0;
            if (CardManager.Sessions.Count == 0) SessionId = 1;
            else {
                SessionId = CardManager.Sessions.Select(a => a.SessionId).Max() + 1;
            }
        }
        /// <summary>
        /// id игры
        /// </summary>
        public int SessionId { get; set; }
        /// <summary>
        /// колода для конкретной игры
        /// </summary>
        public MainPackModel MainPackModel { get; set; }
        /// <summary>
        /// ожидаемое количество пользователей
        /// </summary>
        public int ExpectedUserCount { get; set; }

        public List<int> SelectedCardsIdsList { get; set; } = new List<int>();
    }
    /// <summary>
    /// модель пользователя
    /// </summary>
    public class MainUserModel {
        /// <summary>
        /// количество карт у пользователя
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// id пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// карты
        /// </summary>
        public List<CardModel> Cards { get; set; }
        /// <summary>
        /// проиграл пользователь или нет
        /// </summary>
        public bool IsLoser { get; set; } = false;
        /// <summary>
        /// выиграл игрок или нет
        /// </summary>
        public bool IsWinner { get; set; } = false;
        /// <summary>
        /// добавился этот пользователь уже или нет
        /// </summary>
        public bool IsConnected { get; set; } = false;
        /// <summary>
        /// этот пользователь играет или нет
        /// </summary>
        public bool IsActivated { get; set; } = false;
        public int NumberInGame { get; set; }


    }

    // модель колоды
    public class MainPackModel {
        /// <summary>
        /// количество карт в колоде
        /// </summary>
        public int Count { get; set; } = 56;
        /// <summary>
        /// карты
        /// </summary>
        public CardModel[] Cards { get; set; }
        /// <summary>
        /// данные по игрокам
        /// </summary>
        public List<MainUserModel> Users { get; set; }

    }
    // модель карты
    public class CardModel {
        /// <summary>
        ///  масть
        /// </summary>
        public TypeCardEnum Type { get; set; }
        /// <summary>
        /// используется уже карта или нет
        /// </summary>
        public CardStatusEnum Status { get; set; } = CardStatusEnum.Free;
        /// <summary>
        /// номер карты
        /// </summary>
        public int Number { get; set; } = 0;
        /// <summary>
        /// id игрока
        /// </summary>
        public int UserId { get; set; }
        public CardModel(TypeCardEnum type) {
            Type = type;
        }
    }
}
