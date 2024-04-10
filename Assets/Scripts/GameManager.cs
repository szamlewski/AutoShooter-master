using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ta linijka powoduje, ¿e gamemanager bêdzie zawsze dostêpny nawet jeœli zmienicie scene
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //nowa gra
    public void NewGame()
    {
        SceneManager.LoadScene("Level1");
    }

    //wyjdz
    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
