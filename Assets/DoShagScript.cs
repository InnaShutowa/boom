using Assets;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Manager;

public class DoShagScript : MonoBehaviour {
    public Button go;
    public Button cancel;
    public Button ok;
    public Button lose;
    public Transform main;
    public Transform mainTwoRow;
    public Transform panel;
    public Transform selectedCard;
    public Text message;


    public void TaskOnClick() {
        try {
            if (!string.IsNullOrEmpty(ApiClientScript.SelectedCard)) {
                Int32.TryParse(ApiClientScript.SelectedCard, out var num);
                ApiClientScript.modell = HttpQueryManager.DoShag(num);
                ApiClientScript.SelectedCardsIds = ApiClientScript.modell.selected_cards_ids.ToList();
                ApiClientScript.buff = 110;
                ApiClientScript.SelectedCard = "";
            }
            

           
        } catch (Exception ex) {
            Debug.Log(ex.Message);
            MessageManager.ErrorMessage(ok,
                            lose,
                            message,
                            panel,
                            $" Произошла внутренняя \n ошибка, " +
                                      $"\n попробуйте еще раз! " +
                                      $"{ex.Message}",
                            Assets.Enums.TypeNotificationEnum.Error);
        }
    }

    public void CancelShag() {
        try {
            if (ApiClientScript.IsBlocked) return;

            go.interactable = true;
            // получаем данные с апишки
            var data = HttpQueryManager.CanselShag();
            var information = Manager.GetInfoAboutPlayers(data);
            if (information == null || information.RealUser == null) {
                return;
            }
            ApiClientScript.GameModel
                    .Users.First(a => a.UserId == ApiClientScript.CurrentUserId)
                    .CardsModel.ForEach(a => {
                        Destroy(a.Card.gameObject);
                    });

            ApiClientScript.GameModel.Users.First(a => a.UserId == ApiClientScript.CurrentUserId).CardsModel.Clear();
            ApiClientScript.SelectedCardsIds.Clear();

            ApiClientScript.GameModel.Users.First(a => a.UserId == ApiClientScript.CurrentUserId).CardsModel = information.RealUser.CardsModel;
            InternalManager.SetRealUserCardsTwo(main, mainTwoRow, information.RealUser.CardsModel, selectedCard);
        } catch (Exception ex) {
            Debug.Log("CancelShag" + ex.Message);
            MessageManager.ErrorMessage(ok,
                lose,
                message,
                panel,
                $" Произошла внутренняя \n ошибка, " +
                           $"\n попробуйте еще раз!" +
                           $" {ex.Message}",
                Assets.Enums.TypeNotificationEnum.Error);
        }
    }
    // Start is called before the first frame update
    void Start() {
        go.onClick.AddListener(TaskOnClick);
        cancel.onClick.AddListener(CancelShag);
    }

    // Update is called once per frame
    void Update() {

    }
}
