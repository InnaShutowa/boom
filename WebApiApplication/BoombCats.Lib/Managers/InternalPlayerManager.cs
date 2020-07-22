using BoombCats.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoombCats.Lib.Managers {
    public static class InternalPlayerManager {
        /// <summary>
        /// активируем пользователя
        /// </summary>
        public static MainGameModel InternalActivateUser(int userId, int sessionId) {
            try {
                var session = CardManager.Sessions.FirstOrDefault(a => a.SessionId == sessionId);
                if (session == null) return null;
                if (session.ExpectedUserCount <= session.MainPackModel.Users.Count(a => a.IsConnected))
                    return null;

                session.MainPackModel.Users
                    .First(a => a.UserId == userId).IsConnected = true;
                CardManager.UpdateSession(session);
                var result = new MainGameModel(session);
                return result;
            } catch (Exception ex) {
                return null;
            }
        }
        /// <summary>
        /// передаем ход другому пользователю
        /// </summary>
        public static MainGameModel InternalChangeActiveUser(int sessionId) {
            try {
                var session = CardManager.Sessions.FirstOrDefault(a => a.SessionId == sessionId);
                
                if (session == null) return null;
                var mainPack = session.MainPackModel;

                var oldActiveUserId = mainPack.Users.First(a => a.IsActivated).UserId;
                var minUserId = mainPack.Users.Select(a => a.UserId).Min();
                var maxUserId = mainPack.Users.Select(a => a.UserId).Max();

                mainPack.Users.First(a => a.IsActivated).IsActivated = false;

                if (maxUserId > oldActiveUserId) {
                    mainPack
                    .Users.First(a => a.UserId == maxUserId).IsActivated = true;
                } else {
                    mainPack
                    .Users.First(a => a.UserId == minUserId).IsActivated = true;
                }

                // удаляем карты из списка выбранных, чтобы "сбежать" работало корректно
                session.SelectedCardsIdsList.Clear();
                session.MainPackModel = mainPack;
                CardManager.UpdateSession(session);

                var result = new MainGameModel(session);
                return result;
            } catch (Exception ex) {
                return null;
            }
        }
    }
}
