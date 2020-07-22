using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets;
using System.Threading;
using System.Linq;
using Assets.Managers;

public class ComeInGameScript : MonoBehaviour {
    public Button comeIn;

    public void TaskOnClick() {
        SceneManager.LoadScene("SelectSession");
    }
    // Start is called before the first frame update
    void Start() {
        comeIn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update() {

    }
}
