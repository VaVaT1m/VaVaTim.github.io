using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public Text highScore;


    private void Awake()
    {
        MenuManager.instance = this;
        instance = GetComponent<MenuManager>();
        PlayerPrefs.SetInt("HighScore",Container.scoreKeeper);
        highScore.text = "Рекорд: " + PlayerPrefs.GetInt("HighScore").ToString();
    }


    public void Load(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void StartRandomGame()
    {
        Container.isRandom = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Easy()
    {
        Container.mode = 0;
    }
    public void Medium()
    {
        Container.mode = 1;
    }
    public void Hard()
    {
        Container.mode = 2;
    }
}
