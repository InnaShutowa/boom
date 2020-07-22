using Assets;
using Assets.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GetCardsFromPack : MonoBehaviour {
    public Button take;
    public Button toSbros;
    public Button go;
    public Button cansel;
    public Button ok;
    public Button lose;
    public Transform main;
    public Transform mainTwoRow;
    public Transform panel;
    public Transform selectedCard;
    public Text koloda;
    public Text sbros;
    public Text message;

    public void TaskOnClick() {
        if (ApiClientScript.IsBlocked) return;

        var userId = ApiClientScript.GameModel
                    .Users.First(a => a.UserId == ApiClientScript.CurrentUserId).UserId;

        if (ApiClientScript.GameModel.Users.First(a=>a.UserId==userId).CountCards < 12) {
            ApiClientScript.modell = HttpQueryManager.GetCardsFromPack(1, userId, ApiClientScript.SessionId);

            var information = Manager.GetInfoAboutPlayers(ApiClientScript.modell);
            if (information == null || information.RealUser == null) {
                MessageManager.ErrorMessage(ok,
                    lose,
                    message, 
                    panel,
                    $" Произошла внутренняя \n ошибка, " +
                               $"\n попробуйте еще раз!",
                    Assets.Enums.TypeNotificationEnum.Error);
                return;
            }
            ApiClientScript.GameModel
                       .Users.First(a => a.UserId == userId)
                       .CardsModel.ForEach(a => {
                           Destroy(a.Card.gameObject);
                       });
            ApiClientScript.GameModel.Users.First(a => a.UserId == userId).CardsModel.Clear();

            ApiClientScript.GameModel.Users.First(a => a.UserId == userId).CardsModel = information.RealUser.CardsModel;
            ApiClientScript.GameModel.Users.First(a => a.UserId == userId).CountCards = information.RealUser.CardsModel.Count();

            InternalManager.SetRealUserCardsTwo(main, mainTwoRow, information.RealUser.CardsModel, selectedCard);


            if (information.RealUser.IsLoser) {
                ApiClientScript.IsTryToStayAlive = false;
                MessageManager.ErrorMessage(ok,
                    lose,
                    message,
                    panel,
                    $"               Вы проиграли!",
                    Assets.Enums.TypeNotificationEnum.Boomb,
                    cansel,
                    toSbros,
                    take,
                    go
                    );
                ApiClientScript.IsBlocked = true;
            } else {
                ApiClientScript.modell = HttpQueryManager.ChangeActiveUser(ApiClientScript.SessionId);
                ApiClientScript.buff = 110;
            }
            koloda.text = "Колода: " + ApiClientScript.modell.pack.count;
            sbros.text = "Сброс: " + ApiClientScript.modell.sbros.count;
        }
    }
    // Start is called before the first frame update
    void Start() {
        take.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update() {

    }
}
