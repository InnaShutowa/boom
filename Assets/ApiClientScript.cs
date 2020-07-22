using Assets.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Assets.Managers;
using Assets.Enums;
using static Assets.Models.ApiModels;
using Assets;
using UnityEngine.SceneManagement;

public class ApiClientScript : MonoBehaviour {
  
    /// <summary>
    /// элементы поля
    /// </summary>
    public Transform realPlayer;
    public Transform realPlayerTwoRow;
    public Transform selectedCard;
    public Transform player3;
    public Transform player4;
    public Transform player1;
    public Transform player2;
    public Transform panel;
    public Text koloda;
    public Text sbros;
    public Text message;
    public Text infoText;
    public Button ok;
    public Button lose;
    public Button fail;
    public Button goOut;
    public Button go;
    public Button cansel;
    public Button take;
    public Button toSbros;
    
    /// <summary>
    /// модель, которая приходит с апишки
    /// </summary>
    public static ApiMainGameModel modell;
    /// <summary>
    /// преобразованныя модель, которая приходит с апишки 
    /// - по сути то же самое, плюс объекты поля, 
    /// чтобы можно было их удалить в нужный момент
    /// </summary>
    public static MainModel GameModel;
    /// <summary>
    /// номер выбранной карты в строковом формате
    /// </summary>
    public static string SelectedCard = "";
    /// <summary>
    /// id выбранных карт
    /// </summary>
    public static List<int> SelectedCardsIds = new List<int>();
    /// <summary>
    /// id текущей игровой сессии
    /// </summary>
    public static int SessionId = 0;
    /// <summary>
    /// id текущего пользователя
    /// </summary>
    public static int CurrentUserId = -1;
    /// <summary>
    /// объекты для выбранных карт, 
    /// чтобы можн было соответственно 
    /// размещать их на поле и удалять
    /// </summary>
    public static List<Transform> SelectedCardsTransformList = new List<Transform>();

    public static bool IsBlocked = false;
    public static int buff = 0;
    public static bool IsTryToStayAlive = false;


    // Start is called before the first frame update
    void Start() {
        goOut.onClick.AddListener(GoOutTaskOnClick);

        modell = HttpQueryManager.GetCurrentPack(SessionId);
        
        if (CurrentUserId == -1) {
            CurrentUserId = modell?.users?.FirstOrDefault(a => !a.is_connected)?.user_id ?? 0;
            modell = HttpQueryManager.ActivateUser(CurrentUserId, SessionId);
        }
        SelectedCardsIds = modell.selected_cards_ids.ToList();
        SetSceneElements(modell);
        buff = 110;
    }

    // Update is called once per frame
    void Update() {
        buff++;
        if (buff == 120) {
            GameModel
                       .Users.First(a => a.UserId == CurrentUserId)
                       .CardsModel.ForEach(a => {
                           Destroy(a.Card.gameObject);
                       });
            GameModel?
                       .Users?.Where(a => a.UserId != CurrentUserId && !a.IsConnected)
                       .ToList()?
                       .ForEach(usr => {
                           usr?.CardsModel?.ForEach(a => {
                               if (a?.Card?.gameObject != null) {
                                   Destroy(a.Card.gameObject);
                               }
                           });
                       });
            if (SelectedCardsTransformList.Any()) {
                SelectedCardsTransformList.ForEach(a => {
                    if (a?.gameObject != null)
                        Destroy(a.gameObject);
                });
                SelectedCardsTransformList.Clear();
            }

            modell = HttpQueryManager.GetCurrentPack(SessionId);
            SelectedCardsIds = modell.selected_cards_ids.ToList();

            SetSceneElements(modell);

            if (modell.users.Any(a => !a.is_connected && !a.loser)) {
                infoText.text = "Ждем игроков...";
                infoText.gameObject.SetActive(true);
                IsBlocked = true;
            } else {
                if (modell.users.First(a=>a.user_id==CurrentUserId).winner) {
                    MessageManager.ErrorMessage(ok,
                       lose,
                       message,
                       panel,
                       $"               Вы победили!",
                       TypeNotificationEnum.Win,
                       cansel,
                       toSbros,
                       take,
                       go);
                    IsBlocked = true;
                    HttpQueryManager.DeleteSession();
                    return;
                }

                IsBlocked = false;
                if (modell.users.First(a => a.user_id == CurrentUserId).is_active_user) {
                    infoText.text = "Ваш ход";
                    infoText.gameObject.SetActive(true);
                    if (modell.users.First(a => a.user_id == CurrentUserId).loser && !IsTryToStayAlive) {
                        MessageManager.ErrorMessage(ok,
                        lose,
                        message,
                        panel,
                        $"               Вы проиграли!",
                        TypeNotificationEnum.Boomb,
                        cansel,
                        toSbros,
                        take,
                        go);
                        return;
                    }
                } else {
                    infoText.gameObject.SetActive(false);
                    IsBlocked = true;
                    goOut.gameObject.SetActive(true);
                }
            }
            buff = 0;
        }

        if (Input.GetMouseButtonDown(0) && !IsBlocked) {
            Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);

            if (cubeHit) {
                if (SelectedCard == cubeHit.collider.name) {
                    SelectedCard = "";
                    var card = GameModel
                        .Users
                        .First(a => a.UserId == CurrentUserId)
                        .CardsModel
                        .First(a => a.CardNumber.ToString() == cubeHit.collider.name).Card;
                    card.transform.GetComponent<SpriteRenderer>().color = new Color(1F, 1F, 1F, 1);
                } else {
                    if (string.IsNullOrEmpty(SelectedCard)) {
                        SelectedCard = cubeHit.collider.name;
                        var card = GameModel
                            .Users
                            .First(a => a.UserId == CurrentUserId)
                            .CardsModel
                            .First(a => a.CardNumber.ToString() == cubeHit.collider.name).Card;
                        card.transform.GetComponent<SpriteRenderer>().color = new Color(0.2F, 0.3F, 0.4F, 0.5F);
                    }
                }
            }
        }
    }

    public void GoOutTaskOnClick() {
        HttpQueryManager.DeleteUserFromGame();
        SceneManager.LoadScene("Start");
    }

    public void SetSceneElements(ApiMainGameModel model) {
        try {
            MessageManager.TurnOffElements(ok, message, panel, lose, fail);
            if (CurrentUserId != -1) {
                var information = Manager.GetInfoAboutPlayers(model);
                if (information == null || information.RealUser == null) {
                    MessageManager.ErrorMessage(ok,
                                            lose,
                                            message,
                                            panel,
                                            $" Произошла внутренняя \n ошибка, " +
                                            $"\n попробуйте еще раз!",
                                            TypeNotificationEnum.Error);
                    return;
                }

                // создаем удобную модель данных
                GameModel = information.GameModel;

                // получаем реального пользователя
                var realUser = information.RealUser;

                take.gameObject.SetActive(true);

                // получаем всех фейковых пользователей
                var fakeUsers = information.FakeUsers;
                var players = new List<Transform>();


                // нужно, чтобы расположение вееров карт было правильным
                if (InfoFromAnotherScena.CountUser != 3) {
                    players.Add(player1);
                    players.Add(player3);
                    players.Add(player4);
                    players.Add(player2);
                } else {
                    players.Add(player1);
                    players.Add(player2);
                    players.Add(player3);
                    players.Add(player4);
                }
                // сопоставляем каждому пользователю свою колоду
                int buff = 0;
                fakeUsers.ForEach(a => {
                    a.UserPack = players[buff];
                    buff++;
                });


                koloda.text = "Колода: " + model.pack.count;
                sbros.text = "Сброс: " + model.sbros.count;

                Manager.SetCardInPole(fakeUsers,
                realUser.CardsModel,
                realPlayer,
                realPlayerTwoRow,
                player1,
                player3,
                player4,
                selectedCard,
                InfoFromAnotherScena.CountUser);
            }

        } catch (Exception ex) {
            Debug.Log(ex.Message);
            MessageManager.ErrorMessage(ok,
                      lose,
                      message,
                      panel,
                      $"               Ошибка!",
                      TypeNotificationEnum.Error,
                      cansel,
                      toSbros,
                      take,
                      go);
            IsBlocked = true;
        }
    }
}
