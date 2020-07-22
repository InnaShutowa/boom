using Assets;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PutCardsInSbros : MonoBehaviour {

    public Transform realPlayer;
    public Transform realPlayerTwoRow;
    public Transform panel;
    public Transform selectedCard;
    public Button toSbros;
    public Button go;
    public Button cansel;
    public Button take;

    public Button ok;
    public Button lose;
    public Text koloda;
    public Text sbros;
    public Text message;


    public void TaskOnClick() {
        try {
            if (ApiClientScript.IsBlocked) return;

            var userId = ApiClientScript.GameModel.Users.First(user => user.UserId == ApiClientScript.CurrentUserId).UserId;

            if (ApiClientScript.GameModel.Users.First(a => a.UserId == userId).CountCards <= 0) {
                MessageManager.ErrorMessage(ok, 
                    lose,
                    message, 
                    panel,
                    $" Произошла внутренняя \n ошибка, " +
                                    $"\n попробуйте еще раз!",
                    Assets.Enums.TypeNotificationEnum.Error);
                return;
            }

            if (ApiClientScript.SelectedCardsIds.Count != 0) {
                var finResult = HttpQueryManager.FinishShag();
                var data = finResult.Data;
                if (!finResult.StatusShag) {
                    MessageManager.ErrorMessage(ok,
                    lose,
                    message,
                    panel,
                    $" Нельзя использовать карту!",
                    Assets.Enums.TypeNotificationEnum.Error);
                    return;
                }
                data = HttpQueryManager.PutCardsInSbros();
                var information = Manager.GetInfoAboutPlayers(data);
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

                InternalManager.SetRealUserCardsTwo(realPlayer, realPlayerTwoRow, information.RealUser.CardsModel, selectedCard);

                ApiClientScript.SelectedCard = "";
               

                ApiClientScript.SelectedCardsIds.Clear();


                koloda.text = "Колода: " + data.pack.count;
                sbros.text = "Сброс: " + data.sbros.count;

                go.interactable = true;
            }
        } catch (Exception ex) {
            Debug.Log(ex.Message);
            MessageManager.ErrorMessage(ok, 
                lose,
                message, 
                panel,
                $" Произошла внутренняя \n ошибка, " +
                          $"\n попробуйте еще раз! " +  
                          $"{ ex.Message}",
                Assets.Enums.TypeNotificationEnum.Error);
        }
    }

    // Start is called before the first frame update
    void Start() {
        toSbros.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update() {

    }
}
