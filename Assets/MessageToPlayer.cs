using Assets;
using Assets.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MessageToPlayer : MonoBehaviour {
    
    public Text message;
    public Button ok;
    public Button lose;
    public Button cansel;
    public Button go;
    public Button toSbros;
    public Button take;
    public Button fail;
    public Transform panel;

    /// <summary>
    /// продолжить игру, если есть обезвредить
    /// </summary>
    public void TaskOnClick() {
        ApiClientScript.IsBlocked = false;
        ApiClientScript.IsTryToStayAlive = true;
        MessageManager.TurnOffElements(ok, message, panel, lose, fail);
        MessageManager.TurnOnButtons(cansel, toSbros, take, go, fail);
    }
    /// <summary>
    /// сдаться
    /// </summary>
    public void TaskOnLose() {
        ApiClientScript.IsTryToStayAlive = false;
        ApiClientScript.GameModel
                    .Users.First(a => a.UserId == ApiClientScript.CurrentUserId)
                    .CardsModel.ForEach(a => {
                        Destroy(a.Card.gameObject);
                    });

        ApiClientScript.GameModel.Users.First(a => a.UserId == ApiClientScript.CurrentUserId).CardsModel.Clear();
        ApiClientScript.modell = HttpQueryManager.DeleteUserFromGame();
        MessageManager.TurnOffElements(ok, message, panel, lose, fail);
        MessageManager.TurnOffButtons(cansel, toSbros, take, go, fail);
    }

    
    // Start is called before the first frame update
    void Start() {
        ok.onClick.AddListener(TaskOnClick);
        lose.onClick.AddListener(TaskOnLose);
        fail.onClick.AddListener(TaskOnLose);
    }

    // Update is called once per frame
    void Update() {
    }
}
