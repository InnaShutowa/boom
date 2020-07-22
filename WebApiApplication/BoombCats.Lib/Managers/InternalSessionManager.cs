using BoombCats.Lib.Enums;
using BoombCats.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoombCats.Lib.Managers {
    /// <summary>
    /// менеджер для работы с сессиями
    /// </summary>
    public class InternalSessionManager {
        /// <summary>
        /// получаем список всех игр, созданных на данный момент
        /// </summary>
        public static List<SessionsInfoModel> InternalGetSessionsList() {
            try {
                var sessions = CardManager.Sessions.ToList();
                var result = new List<SessionsInfoModel>();
                sessions.Where(a => a.ExpectedUserCount > a.MainPackModel.Users.Count(w => w.IsActivated))?.ToList().ForEach(a => {
                    result.Add(new SessionsInfoModel() {
                        ExpectedUserCount = a.ExpectedUserCount,
                        SessionId = a.SessionId,
                        UserCount = a.MainPackModel.Users.Count(w => w.IsActivated)
                    });
                });
                return result;
            } catch (Exception ex) {
                return null;
            }
        }

        /// <summary>
        /// удаляем игрока из сессии
        /// </summary>
        public static MainGameModel InternalDeleteUserFromGame(int userId, int sessionId) {
            var workModel = CardManager.Sessions.FirstOrDefault(a => a.SessionId == sessionId);
            if (workModel == null)
                return null;
            var user = workModel.MainPackModel?.Users?.FirstOrDefault(a => a.UserId == userId);
            if (user == null)
                return null;
            DeleteUserFromSessionHelper(userId, sessionId);

            if (workModel.MainPackModel.Users.Count(a => a.IsLoser) == workModel.ExpectedUserCount - 1) {
                CardManager
                    .Sessions
                    .First(a => a.SessionId == sessionId)
                    .MainPackModel
                    .Users
                    .First(a => !a.IsLoser)
                    .IsWinner = true;
            }

            CardManager
                .Sessions
                .First(a => a.SessionId == sessionId)
                .SelectedCardsIdsList
                .Clear();

            return new MainGameModel(CardManager.Sessions.First(a => a.SessionId == sessionId));
        }



        /// <summary>
        /// удаляем пользователя из сессии (сдался или проиграл окончательно)
        /// </summary>
        private static void DeleteUserFromSessionHelper(int userId, int sessionId) {
            var session = CardManager
                .Sessions
                .FirstOrDefault(a => a.SessionId == sessionId);
            if (session == null)
                return;

            session.MainPackModel
                .Users
                .First(a => a.UserId == userId)
                .Cards.ForEach(a => {
                    a.Status = CardStatusEnum.Used;
                });

            session.MainPackModel
                .Users.ForEach(user => {
                    if (user.UserId == userId) {
                        user.IsLoser = true;
                    }
                });
            CardManager.UpdateSession(session);
        }
    }
}
