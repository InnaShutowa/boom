using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CreateNewGameScript : MonoBehaviour
{
    public Button newGame;
    public void TaskOnClick() {
        
        SceneManager.LoadScene("ChangePlayerCount");
    }
    // Start is called before the first frame update
    void Start()
    {
        newGame.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
