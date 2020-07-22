using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using Assets.Enums;

namespace Assets.Managers {
    public class MessageManager {
        /// <summary>
        /// отключаем основные кнопки сцены
        /// </summary>
        public static void TurnOffButtons(Button cansel,
                                        Button toSbros,
                                        Button take,
                                        Button go,
                                        Button fail) {
            try {
                cansel.GetComponent<Button>().gameObject.SetActive(false);
                toSbros.GetComponent<Button>().gameObject.SetActive(false);
                take.GetComponent<Button>().gameObject.SetActive(false);
                go.GetComponent<Button>().gameObject.SetActive(false);
                fail.GetComponent<Button>().gameObject.SetActive(false);
            } catch(Exception ex) {
                Debug.Log("TurnOffButtons:  " + ex.Message);
            }
        }
        /// <summary>
        /// возвращаем основные кнопки сцены
        /// </summary>
        public static void TurnOnButtons(Button cansel, 
                                        Button toSbros, 
                                        Button take, 
                                        Button go, 
                                        Button fail = null) {
            try {
                cansel.GetComponent<Button>().gameObject.SetActive(true);
                toSbros.GetComponent<Button>().gameObject.SetActive(true);
                take.GetComponent<Button>().gameObject.SetActive(true);
                go.GetComponent<Button>().gameObject.SetActive(true);
            } catch(Exception ex) {
                Debug.Log("TurnOnElements:  "+ ex.Message);
            }
        }
        /// <summary>
        ///  метод нужен, чтобы скрывать всплывашку с ошибкой или информацией
        /// </summary>
        public static void TurnOffElements(Button ok, Text message, Transform panel, Button lose, Button fail) {
            try {
                panel.GetComponent<SpriteRenderer>().enabled = false;
                message.gameObject.SetActive(false);
                ok.gameObject.SetActive(false);
                lose.gameObject.SetActive(false);
                fail.gameObject.SetActive(false);

            } catch (Exception ex) {
                Debug.Log($"TurnOffElements {ex.Message}");
            }
        }
        /// <summary>
        /// метод, который будет выводить всплывашку с ошибкой или инфой для пользователя
        /// </summary>
        public static void ErrorMessage(Button ok, 
            Button lose,
            Text message, 
            Transform panel, 
            string textMessage, 
            TypeNotificationEnum type,
            Button cansel = null,
            Button toSbros = null,
            Button take = null,
            Button go = null) {
            try {
                var pict = "vskl";
                switch ((int)type) {
                    case 1: {
                            pict = "vskl";
                            ok.GetComponent<Button>().gameObject.SetActive(true);
                            message.enabled = true;
                            ok.enabled = true;
                            break;
                        }
                    case 3: {
                            ok.GetComponentInChildren<Text>().text = "Продолжить игру";
                            pict = "booms";
                            lose.GetComponent<Button>().gameObject.SetActive(true);
                            TurnOnButtons(cansel, toSbros, take, go);
                            ok.GetComponent<Button>().gameObject.SetActive(true);
                            message.enabled = true;
                            ok.enabled = true;
                            break;
                        }
                    case 4: {
                            ok.GetComponent<Button>().gameObject.SetActive(false);
                            pict = "vskl";
                            lose.GetComponent<Button>().gameObject.SetActive(true);
                            lose.GetComponentInChildren<Text>().text = "Покинуть игру";
                            break;
                        }
                    default: {
                            break;
                        }
                }
                panel.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(pict);
                panel.GetComponent<SpriteRenderer>().enabled = true;
                message.GetComponent<Text>().gameObject.SetActive(true);

                message.text = textMessage;
            } catch (Exception ex) {
                Debug.Log($"ErrorMessage {ex.Message}");
            }
        }


        /// <summary>
        /// всплывашка, которая появляется когда кто-то ловит взрывного котенка
        /// </summary>
        public static void YouLoser(Transform panel) {
            try {
                panel.GetComponent<SpriteRenderer>().enabled = true;
            } catch (Exception ex) {
                Debug.Log($"YouLoser {ex.Message}");
            }
        }
    }
}
