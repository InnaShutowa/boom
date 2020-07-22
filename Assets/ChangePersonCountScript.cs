using Assets;
using Assets.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangePersonCountScript : MonoBehaviour {
    public Button button;
    public Dropdown dropdown;

    public void TaskOnClick() {
        ApiClientScript.modell = HttpQueryManager.GetCardsForPlayers(InfoFromAnotherScena.CountUser);
        ApiClientScript.CurrentUserId = ApiClientScript.modell.users.First(a => a.is_connected).user_id;
        ApiClientScript.SessionId = ApiClientScript.modell.id;

        SceneManager.LoadScene("SampleScene");
    }
    // Start is called before the first frame update
    void Start() {
        button.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update() {
        var val = dropdown.value;
        var text = dropdown.options[val].text;
        if (Int32.TryParse(text, out var count)) {
            InfoFromAnotherScena.CountUser = count;
        }

    }
}
