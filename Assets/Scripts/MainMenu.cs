using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public DataGame dataGame;

    public Toggle marker0;
    public Toggle marker1;
    public Toggle action0;
    public Toggle action1;
    void Start()
    {
        OnConfigOpen();
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
        if (dataGame.outputMarker)
        {
            marker0.isOn = true;
            marker1.isOn = false;
        }
        else
        {
            marker0.isOn = false;
            marker1.isOn = true;
        }
        if (dataGame.tapOnScreen)
        {
            action0.isOn = true;
            action1.isOn = false;
        }
        else
        {
            action0.isOn = false;
            action1.isOn = true;
        }
    }
    public void SetMarkerGame0(bool val)
    {
        dataGame.outputMarker = val;
        marker1.isOn = !val;
    }
    public void SetMarkerGame1(bool val)
    {
        dataGame.outputMarker = !val;
        marker1.isOn = val;
    }
    public void SetActionGame0(bool val)
    {
        dataGame.tapOnScreen = val;
        action1.isOn = !val;
    }
    public void SetActionGame1(bool val)
    {
        dataGame.tapOnScreen = !val;
        action0.isOn = !val;
    }
}
