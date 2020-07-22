using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BackToStartScript : MonoBehaviour
{
    public Button back;

    public void TaskOnClick() {
        SceneManager.LoadScene("Start");
    }
    // Start is called before the first frame update
    void Start() {
        back.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
