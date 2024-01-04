using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public DataGame dataGame;

    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Exitgame()
    {
        Application.Quit();
    }
    public void OnConfigOpen()
    {
        if (dataGame.tapOnScreen)
        {
            toggle3.isOn = true;
            toggle1.isOn = false;
            toggle2.isOn = false;
        }
        else if (dataGame.outputMarker)
        {
            toggle1.isOn = true;
            toggle2.isOn = false;
            toggle3.isOn = false;
        }
        else
        {
            toggle2.isOn = true;
            toggle1.isOn = false;
            toggle3.isOn = false;
        }
    }
    public void SetToggle1(bool val)
    {
        dataGame.outputMarker = val;
        dataGame.tapOnScreen = false;
    }
    public void SetToggle2(bool val)
    {
        dataGame.outputMarker = false;
        dataGame.tapOnScreen = !val;
    }
    public void SetToggle3(bool val)
    {
        dataGame.outputMarker = false;
        dataGame.tapOnScreen = val;
    }
}
