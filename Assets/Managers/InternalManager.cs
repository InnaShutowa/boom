using Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Models.ApiModels;

namespace Assets.Managers {
    public class InternalManager : MonoBehaviour {

        public static ApiResultModel TryParseMainGameModel(string text) {
            try {
                if (string.IsNullOrEmpty(text)) return null;
                Debug.Log(text);
                var query = JsonUtility.FromJson<ApiResultModel>(text);

                if (query == null) {
                    Debug.Log("пришел null или нихера не распарсилось");
                    return null;
                }
                var result = query;
                Debug.Log("должно сработать");

                return result;
            } catch (Exception ex) {
                Debug.Log("TryParseMainGameModel: " + ex.Message);
                return null;
            }
        }


        /// <summary>
        /// получаем информацию по сессиям, чтобы отобразить на страничке "присоединиться к игре"
        /// </summary>
        public static void SetSessionsInfo(Canvas canvas,
            Button play,
            Text gameName,
            Text userCount,
            ApiSessionModel[] sessions,
            ScrollRect scrollbar) {
            try {
                var buttons = new List<GetSessionsInfoModel>();

                if (sessions == null || sessions.Count() == 0) return;

                var sessionsList = sessions.ToList();
                /// получаем размеры объектов для клонирования
                var rectPlay = play.GetComponent<RectTransform>();
                var playHeight = rectPlay.rect.height;
                var playWidth = rectPlay.rect.width;

                var rectGameName = gameName.GetComponent<RectTransform>();
                var gameNameHeight = rectGameName.rect.height;
                var gameNameWidth = rectGameName.rect.width;

                var rectUserCount = userCount.GetComponent<RectTransform>();
                var userCountHeight = rectUserCount.rect.height;
                var userCountWidth = rectUserCount.rect.width;

                /// получаем координаты объектов для клонирования
                var xSizePlay = play.transform.position.x;
                var ySizePlay = play.transform.position.y;

                var xSizeGameName = gameName.transform.position.x;
                var ySizeGameName = gameName.transform.position.y;

                var xSizeUserCount = userCount.transform.position.x;
                var ySizeUserCount = userCount.transform.position.y;

                gameName.text = "Игра №" + sessions.First().session_id;
                userCount.text = sessions.First().user_count + " - " + sessions.First().expected_user_count;

                play.gameObject.name = sessions.First().session_id.ToString();

                PlayScript.Buttons.Add(play);

                buttons.Add(new GetSessionsInfoModel() {
                    Button = play,
                    SessionId = sessions.First().session_id
                });


                sessionsList.Remove(sessions.First());

                sessionsList.ForEach(session => {
                    play = Instantiate(play, new Vector2(xSizePlay, ySizePlay - 1), Quaternion.identity);
                    play.transform.SetParent(scrollbar.transform);

                    var rct = play.GetComponent<RectTransform>();
                    rct.sizeDelta = new Vector2(playWidth, playHeight);
                    rct.localScale = new Vector3(1, 1, 1);

                    play.gameObject.name = session.session_id.ToString();

                    gameName = Instantiate(gameName, new Vector2(xSizeGameName, ySizeGameName - 1), Quaternion.identity);
                    gameName.transform.SetParent(scrollbar.transform);

                    rct = gameName.GetComponent<RectTransform>();
                    rct.sizeDelta = new Vector2(gameNameWidth, gameNameHeight);
                    rct.localScale = new Vector3(1, 1, 1);

                    userCount = Instantiate(userCount, new Vector2(xSizeUserCount, ySizeUserCount - 1), Quaternion.identity);
                    userCount.transform.SetParent(scrollbar.transform);

                    rct = userCount.GetComponent<RectTransform>();
                    rct.sizeDelta = new Vector2(userCountWidth, userCountHeight);
                    rct.localScale = new Vector3(1, 1, 1);

                    gameName.text = "Игра №" + session.session_id;
                    userCount.text = session.user_count + " - " + session.expected_user_count;

                    /// получаем координаты объектов для клонирования
                    xSizePlay = play.transform.position.x;
                    ySizePlay = play.transform.position.y;

                    xSizeGameName = gameName.transform.position.x;
                    ySizeGameName = gameName.transform.position.y;

                    xSizeUserCount = userCount.transform.position.x;
                    ySizeUserCount = userCount.transform.position.y;

                    PlayScript.Buttons.Add(play);
                });
            } catch (Exception ex) {
                Debug.Log(ex.Message);
            }
        }
        /// <summary>
        /// метод нужен, чтобы расставить карты пользвателя (особенно если их больше 6)
        /// </summary>
        public static void SetRealUserCardsTwo(Transform realPlayer, Transform realPlayer2Row, List<CardModel> realUserCards, Transform selectedCards) {
            try {
                if (realUserCards.Count == 0) {
                    Debug.Log("SetRealUserCardsTwo realUserCards.Count ==0");
                    return;
                }
                var cards = realUserCards.ToList();
                if (cards.Count > 5) {
                    realPlayer.transform.localScale = new Vector2((float)0.055, (float)0.0536);
                    realPlayer2Row.transform.localScale = new Vector2((float)0.055, (float)0.0536);
                    SetRealUserCards(realPlayer, cards.Take(6).ToList(), (float)1.5, selectedCards);
                    SetRealUserCards(realPlayer2Row, cards.Skip(6).ToList(), (float)1.5, selectedCards);
                } else {
                    realPlayer.transform.localScale = new Vector2((float)0.068, (float)0.061);
                    SetRealUserCards(realPlayer, cards, 2, selectedCards);
                }
            } catch (Exception ex) {
                Debug.Log($"SetRealUserCardsTwo {ex.Message}");
            }
        }


        /// <summary>
        /// располагаем на поле карты реального пользователя 
        /// </summary>
        public static void SetRealUserCards(Transform realPlayer, List<CardModel> realUserCards, float margin, Transform selectedCards) {
            try {
                var xPos = 0;
                float buff = 0;
                var xSize = realPlayer.transform.localPosition.x;
                var ySize = realPlayer.transform.localPosition.y;


                realUserCards.ForEach(w => {
                    var cardName = ChangePicture(w.TypeCard);
                    Debug.Log(cardName);
                    var vect = new Vector3(xSize + buff, ySize, 0);

                    if (ApiClientScript.SelectedCardsIds.Contains(w.CardNumber)) {
                        vect = new Vector3(xPos, 1, 0);
                        xPos += 2;
                    }

                    realPlayer = Instantiate(realPlayer, vect, Quaternion.identity);
                    realPlayer.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(cardName);

                    realPlayer.name = w.CardNumber.ToString();

                    w.Card = realPlayer;
                    buff += margin;
                });
                var realUserCardsNums = realUserCards.Select(a => a.CardNumber).ToList();
                var selectedUniqueCards = new List<int>();


                var vectNew = new Vector3(xPos, 1, 0);
                ApiClientScript.SelectedCardsIds.ForEach(a => {
                    if (!realUserCardsNums.Contains(a)) {
                        var card = ApiClientScript.modell.users.First(w => w.is_active_user).cards.FirstOrDefault(e => e.id == a);
                        if (card == null) return;
                        vectNew = new Vector3(xPos, 1, 0);
                        xPos += 2;

                        selectedCards = Instantiate(selectedCards, vectNew, Quaternion.identity);
                        var cardName = ChangePicture(card.type);
                        selectedCards.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(cardName);

                        selectedCards.name = card.id.ToString();


                        ApiClientScript.SelectedCardsTransformList.Add(selectedCards);
                    }
                });

            } catch (Exception ex) {
                Debug.Log("SetRealUserCards" + ex.Message);
            }
        }



        public static void SetFakeUserCards(List<UserModel> fakeUsers,
                                            int count,
                                            float xSize,
                                            float ySize,
                                            Transform player1,
                                            Transform player3,
                                            Transform player4) {
            try {


                // расставляем веера ботов
                switch (count) {
                    case 2: {
                            fakeUsers.ForEach(fake => {
                                if (fake.IsConnected) {
                                    ySize = fake.UserPack.transform.localPosition.y;
                                    fake.UserPack.position = new Vector3(0, ySize, 1);
                                    CreateVeer(fake, false, false);
                                } else {
                                    ySize = fake.UserPack.transform.localPosition.y;
                                    fake.UserPack.position = new Vector3(0, ySize, 1);
                                    if (fake.IsLoser) {
                                        SetWaitingUser(fake.UserPack, "boomb");
                                    } else {
                                        SetWaitingUser(fake.UserPack, "preloader");

                                    }
                                }
                            });

                            break;
                        }
                    case 3: {
                            fakeUsers.ForEach(fake => {
                                if (fake.IsConnected) {
                                    CreateVeer(fake, false, false);
                                } else {
                                    if (fake.IsLoser) {
                                        SetWaitingUser(fake.UserPack, "boomb");
                                    } else {
                                        SetWaitingUser(fake.UserPack, "preloader");

                                    }
                                }
                            });
                            break;
                        }
                    case 4: {
                            fakeUsers.ForEach(fake => {
                                if (fake.IsConnected) {
                                    ySize = fake.UserPack.transform.localPosition.y;
                                    if (fake.UserPack == player1)
                                        fake.UserPack.position = new Vector3(0, ySize, 0);

                                    if (fake.UserPack == player3) {
                                        CreateVeer(fake, true, true);
                                    } else if (fake.UserPack == player4) {
                                        CreateVeer(fake, true, false);
                                    } else {
                                        CreateVeer(fake, false, false);
                                    }
                                } else {
                                    ySize = fake.UserPack.transform.localPosition.y;
                                    if (fake.UserPack == player1)
                                        fake.UserPack.position = new Vector3(0, ySize, 0);

                                    if (fake.IsLoser) {
                                        SetWaitingUser(fake.UserPack, "boomb");
                                    } else {
                                        SetWaitingUser(fake.UserPack, "preloader");

                                    }
                                }
                            });

                            break;
                        }
                    case 5: {
                            fakeUsers.ForEach(fake => {
                                if (fake.IsConnected) {
                                    if (fake.UserPack == player3) {
                                        CreateVeer(fake, true, true);
                                    } else if (fake.UserPack == player4) {
                                        CreateVeer(fake, true, false);
                                    } else {
                                        CreateVeer(fake, false, false);
                                    }
                                } else {
                                    if (fake.IsLoser) {
                                        SetWaitingUser(fake.UserPack, "boomb");
                                    } else {
                                        SetWaitingUser(fake.UserPack, "preloader");

                                    }
                                }
                            });
                            break;
                        }
                }
            } catch (Exception ex) {
                Debug.Log(ex.Message);
            }
        }


        /// <summary>
        /// получаем название картинки в зависимости от масти карты
        /// </summary>
        public static string ChangePicture(TypeCardEnum type) {
            var result = type.ToString();
            try {
                var rand = new System.Random();
                if (type == TypeCardEnum.Attack
                    || type == TypeCardEnum.Lending
                    || type == TypeCardEnum.LookInFuture
                    || type == TypeCardEnum.Mix
                    || type == TypeCardEnum.No
                    || type == TypeCardEnum.Skip
                    || type == TypeCardEnum.Neutralize) {
                    result += rand.Next(1, 2);
                } else if (type == TypeCardEnum.Boomb) {
                    result += rand.Next(1, 4);
                }
                return result;

            } catch (Exception ex) {
                Debug.Log(ex.Message);
                return null;
            }
        }


      
        public static void SetWaitingUser(Transform fakeUser, string pictName) {
            try {
                var ySize = fakeUser.transform.localPosition.y;
                var xSize = fakeUser.transform.localPosition.x;
                fakeUser.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(pictName);
            } catch (Exception ex) {
                Debug.Log("SetSecondUser " + ex.Message);
            }
        }

        // формируем веер карт
        public static void CreateVeer(UserModel usr, bool rotate, bool min) {
            try {
                var cnt = 0;
                var xSize = usr.UserPack.transform.localPosition.x;
                var ySize = usr.UserPack.transform.localPosition.y;
                while (cnt < usr.CountCards) {
                    if (rotate) {
                        var smesch = cnt - usr.CountCards / 2;
                        var newPlayer = Instantiate(usr.UserPack,
                            new Vector3(xSize, ySize + smesch, 0),
                            Quaternion.identity);
                        if (usr.IsLoser) {
                            newPlayer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("boomb");
                        } else {
                            newPlayer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("back");
                        }
                        if (min) {
                            newPlayer.Rotate(0, 0, -10 * (smesch) + 90);
                        } else {
                            newPlayer.Rotate(0, 0, 10 * (smesch) + 90);
                        }
                        usr.CardsModel[cnt].Card = newPlayer;
                    } else {
                        var smesch = cnt - usr.CountCards / 2;
                        var newPlayer = Instantiate(usr.UserPack,
                            new Vector3(xSize + smesch, ySize, 0),
                            Quaternion.identity);

                        if (usr.IsLoser) {
                            newPlayer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("boomb");
                        } else {
                            newPlayer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("back");
                        }
                        newPlayer.Rotate(0, 0, 10 * (smesch));
                        usr.CardsModel[cnt].Card = newPlayer;
                    }
                    cnt++;
                }
            } catch (Exception ex) {
                Debug.Log(ex.Message);
            }
        }
    }

}
