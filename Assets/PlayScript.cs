using Assets;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayScript : MonoBehaviour
{
    public Button play;
    public Text gameName;
    public Text userCount;
    public Canvas canvas;
    public ScrollRect scrollbar;

    public static int SessionId;
    public static List<Button> Buttons = new List<Button>();

    public void TaskOnClick() {
        try {
            var sessionIdString = EventSystem.current.currentSelectedGameObject.name;
            Int32.TryParse(sessionIdString, out var sessionId);
            ApiClientScript.SessionId = sessionId;

            ApiClientScript.modell = HttpQueryManager.GetCurrentPack(sessionId);

            if (ApiClientScript.modell.users.Any(a=>!a.is_connected)) {
                SceneManager.LoadScene("SampleScene");
            }

        } catch(Exception ex) {
            Debug.Log(ex.Message);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // кидаем запрос в апишку
        var sessions = HttpQueryManager.GetSessions();
        InternalManager.SetSessionsInfo(canvas, play, gameName, userCount, sessions.data, scrollbar);

        Buttons.ForEach(a => {
            a.onClick.AddListener(TaskOnClick);
        });
    }
}
