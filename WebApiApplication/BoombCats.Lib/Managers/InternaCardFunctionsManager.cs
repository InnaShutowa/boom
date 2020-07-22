using BoombCats.Lib.Enums;
using BoombCats.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoombCats.Lib.Managers {
    public static class InternaCardFunctionsManager {
        /// <summary>
        /// функционал карты Neutralize
        /// </summary>
        public static MainGameModel NeutralizeShag(int userId, int sessionId) {
            try {
                var session = CardManager.Sessions.FirstOrDefault(w => w.SessionId == sessionId);
                if (session == null)
                    return null;
                var mainPack = session.MainPackModel;
                var boomb = mainPack.Users
                        .First(w => w.UserId == userId)
                        .Cards.FirstOrDefault(a => a.Type == TypeCardEnum.Boomb);


                if (boomb != null) {
                    mainPack.Users
                         .First(w => w.UserId == userId)
                         .Cards
                         .Remove(boomb);
                }
                session.SelectedCardsIdsList.Clear();
                session.MainPackModel = mainPack;
                CardManager.UpdateSession(session);
                var result = new MainGameModel(session);
                return result;
            } catch (Exception ex) {
                return null;
            }
        }
        /// <summary>
        /// функционал карты mix
        /// </summary>
        public static MainGameModel MixShag(int userId, int sessionId) {
            var session = CardManager.Sessions.FirstOrDefault(w => w.SessionId == sessionId);
            if (session == null)
                return null;
            var mainPack = session.MainPackModel;
            mainPack.Cards = CardManager.MixCards(
                                mainPack.Cards
                                .ToList()).ToArray();
            session.SelectedCardsIdsList.Clear();
            session.MainPackModel = mainPack;
            CardManager.UpdateSession(session);
            var result = new MainGameModel(session);

            return result;
        }
    }
}
