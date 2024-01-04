
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerGame : MonoBehaviour
{
    private bool isTargetFound = false;
    public bool IsTargetFound
    {
        get { return isTargetFound; }
        set {
            isTargetFound = value; 
            if (!value && stateGame == StateGame.isPlaying)
            {
                Pause();
            }
        }
    }

    public Transform parentPipes;

    public DataGame dataGame;
    public StateGame stateGame = StateGame.isPreStart;
    public ManagerPipes managerPipes;
    
    public Transform playerSpawn;
    
    public Transform gameOverPanel;
    public GameObject screenDie;
    public TMPro.TextMeshProUGUI tmpScore;
    public TMPro.TextMeshProUGUI scorePanel;
    public TMPro.TextMeshProUGUI bestScorePanel;
    public GameObject goPauseBtn;

    public Transform markerPipes;
    public Transform groundplane;
    public GameObject planeFinder;

    private int score = 0;
    public int Score {  get { return score; } set {  score = value; UpdateScore(); } }

    private static ManagerGame _instance;
    public static ManagerGame Instance
    {
        get => _instance;
        set
        {
            if (_instance == null)
                _instance = value;
            else
                Debug.LogError("This instance already exist: ManagerGame");
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InitVuforia();
        PreStartGame();
    }
    private void Update()
    {
        
    }

    void InitVuforia()
    {
        managerPipes.parentPipes = parentPipes;
        if (dataGame.outputMarker)
        {
            planeFinder.SetActive(false);
            groundplane.gameObject.SetActive(false);

            transform.parent.parent = markerPipes;
            transform.parent.localPosition = Vector3.up * 0.5f;
        }
        else
        {
            markerPipes.gameObject.SetActive(false);

            transform.parent.parent = groundplane;
            transform.parent.localPosition = Vector3.up * 0.5f;
        }
    }
    void UpdateScore()
    {
        tmpScore.text = score + "";
        PlayerSounds.Instance.PlayAudio(Audio.ScoreAdd);
    }
    public void PreStartGame ()
    {
        stateGame = StateGame.isPreStart;
        managerPipes.PreStartGame();
        Bird.Instance.PreStartGame(playerSpawn.position);
        gameOverPanel.gameObject.SetActive(false);
        
        score = 0;
        tmpScore.text = score + "";

        tmpScore.gameObject.SetActive(true);
    }
    public void StartGame () 
    {
        managerPipes.StartGame();
        stateGame = StateGame.isPlaying;
    }
    public void EndGame ()
    {
        stateGame = StateGame.isEndGame;
        managerPipes.PauseGame();
        PlayerSounds.Instance.PlayAudio(Audio.Die);

        tmpScore.gameObject.SetActive(false);
        Bird.Instance.EndGame();
        if (score > dataGame.bestScore)
        {
            dataGame.bestScore = score;
        }
        scorePanel.text = score + "";
        bestScorePanel.text = dataGame.bestScore + "";

        gameOverPanel.gameObject.SetActive(true);
        goPauseBtn.SetActive(false);
        screenDie.SetActive(true);
        
        StartCoroutine(HideScreenDie());
    }
    IEnumerator HideScreenDie()
    {
        Image panel = screenDie.transform.GetChild(0).GetComponent<Image>();
        Color prevColor = new(panel.color.r, panel.color.g, panel.color.b, panel.color.a);
        for (float i = 1; i <= 50; i++) { 
            yield return new WaitForSeconds(0.01f);
            panel.color = prevColor - new Color(0,0,0,i/50f);
            if (i >= 50) {
                screenDie.SetActive(false);
                panel.color = prevColor;
                break;
            }
        }
        
    }
    public void Pause()
    {
        stateGame = StateGame.isPaused;
        Bird.Instance.PauseGame();
        managerPipes.PauseGame();
        scorePanel.text = score + "";
        bestScorePanel.text = dataGame.bestScore + "";
        gameOverPanel.gameObject.SetActive(true);
        // Скрытие кнопки "Заново" и "Пауза"
        // И показ кнопки "Продолжить"
        // Осуществляется в ивентах клика на самой кнопке
    }
    public void Unpause()
    {
        stateGame = StateGame.isPlaying;
        managerPipes.UnpauseGame();
        Bird.Instance.UnpauseGame();
        gameOverPanel.gameObject.SetActive(false);
        // Скрытие кнопки "Продолжить"
        // И показ кнопки "Заново" и "Пауза"
        // Осуществляется в ивентах клика на самой кнопке
    }
    private void OnCollisionEnter(Collision collision)
    {
        {
            if (stateGame == StateGame.isPlaying && collision.gameObject.CompareTag("Player"))
            {
                EndGame();
            }
        }
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);

    }
}
 public enum StateGame
{
    isNoGame = 0,
    isPreStart = 1,
    isPlaying = 2,
    isPaused = 3,
    isEndGame = 4
}