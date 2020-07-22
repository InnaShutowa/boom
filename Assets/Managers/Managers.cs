using Assets.Managers;
using Assets.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Models.ApiModels;
using Random = System.Random;

public class Manager : MonoBehaviour {
    /// <summary>
    /// закидываем карту в центр поля
    /// </summary>
    public static void TransportInCenter(Transform card, Random rand) {
        try {
            var smeschX = rand.Next(-2, 2);
            var smeschY = rand.Next(-1, 1);
            card.transform.position = new Vector3(0 + smeschX, 0 + smeschY, 0);
            card.Rotate(0, 0, smeschX * 10);
        } catch (Exception ex) {
            Debug.Log(ex.Message);
        }
    }
    
    /// <summary>
    /// устаревшая хреновина, теперь не используется
    /// </summary>
    public static void CreateSbrosPack(int count, Transform sbros) {
        try {
            var buff = 0;
            var xSize = sbros.position.x;
            var ySize = sbros.position.y;
            sbros.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("back");
            var rand = new Random();
            while (buff < count) {
                var smeschX = rand.Next(-2, 2);
                var smeschY = rand.Next(-2, 2);
                var rott = rand.Next(-30, 30);
                var newPlayer = Instantiate(sbros,
                       new Vector3(xSize + smeschX, ySize + smeschY, 0),
                       Quaternion.identity);
                newPlayer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("back");

                newPlayer.Rotate(0, 0, rott);
                buff++;
            }
        } catch (Exception ex) {
            Debug.Log(ex.Message);
        }
    }


    
    /// <summary>
    ///  расставляем все карты на игровом поле
    /// </summary>
    public static void SetCardInPole(List<UserModel> fakeUsers,
                                     List<CardModel> realUserCards,
                                     Transform realPlayer,
                                     Transform realPlayer2Row,
                                     Transform player1,
                                     Transform player3,
                                     Transform player4,
                                     Transform selectedCards,
                                     int count) {
        try {

            var xSize = realPlayer.transform.localPosition.x;
            var ySize = realPlayer.transform.localPosition.y;

            InternalManager.SetRealUserCardsTwo(realPlayer, realPlayer2Row, realUserCards, selectedCards);

            InternalManager.SetFakeUserCards(fakeUsers,
                                             count,
                                             xSize,
                                             ySize,
                                             player1,
                                             player3,
                                             player4);

        } catch (Exception ex) {
            Debug.Log(ex.Message);
        }
    }

    
    /// <summary>
    /// получаем данные о распределении карт между реальным и фейковыми игроками
    /// </summary>
    public static GettingMainInfoModel GetInfoAboutPlayers(ApiMainGameModel data) {
        try {
            // создаем удобную модель данных
            var gameModel = new MainModel(data);
            if (gameModel == null || gameModel.Users.Count() != InfoFromAnotherScena.CountUser)
                return null;

            // получаем реального пользователя
            var realUser = gameModel.Users.FirstOrDefault(a => a.UserId == ApiClientScript.CurrentUserId);
            if (realUser == null) return null;
            // получаем всех фейковых пользователей
            var fakeUsers = gameModel.Users.Where(a => a.UserId != ApiClientScript.CurrentUserId).ToList();

            return new GettingMainInfoModel() {
                RealUser = realUser,
                FakeUsers = fakeUsers,
                GameModel = gameModel
            };
        } catch (Exception ex) {
            Debug.Log(ex.Message);
            return new GettingMainInfoModel();
        }

    }

}
