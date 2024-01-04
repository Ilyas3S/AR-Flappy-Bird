using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPipes : MonoBehaviour
{
    public Transform parentPipes;

    public GameObject pipe;
    public GameObject controlPoint;

    public RuleGame ruleGame;

    List<Pipe> pipes = new List<Pipe>();
    List<GameObject> cpoints = new List<GameObject>();

    float timerSpawn;
    bool isPlay = false;

    public bool IsPlay
    {
        get { return isPlay; }
        private set
        {
            isPlay = value;
            pipes.ForEach(pipe => pipe.paused = !value);
        }
    }
    void Start()
    {
        // Установка таймера
        timerSpawn = ruleGame.timeBetweenSpawn;
        IsPlay = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerSpawn();
    }
    void UpdateTimerSpawn()
    {
        if (IsPlay)
        {
            timerSpawn -= Time.deltaTime;
            if (timerSpawn <= 0)
            {
                // Спавн по таймеру
                SpawnPortion();
                timerSpawn = ruleGame.timeBetweenSpawn;
            }
        }
    }
    void SpawnPortion()
    {
        // Позиция по Y центра пустого пространства между трубами
        float posCenterSpaceBetweenPipe = Random.value * 
            (ruleGame.heightGameZone - ruleGame.percent_minHeightPipe * ruleGame.heightGameZone * 2) 
            + ruleGame.percent_minHeightPipe * ruleGame.heightGameZone;
        // Спавн двух видов труб
        SpawnByType(TypePipe.Lower, posCenterSpaceBetweenPipe);
        Pipe upper = SpawnByType(TypePipe.Upper, posCenterSpaceBetweenPipe);
        SpawnControlPoint(posCenterSpaceBetweenPipe, upper.transform);
    }
    Pipe SpawnByType(TypePipe tpipe, float posSpace)
    {
        // Объявление переменных позиции и ротации
        Vector3 spawnPos;
        Quaternion spawnRot;
        //Если тип - верхняя труба
        if (tpipe == TypePipe.Upper)
        {
            // Позиция смещается вверх относительно пустого пространства
            spawnPos = transform.position + transform.up * (posSpace + (ruleGame.percent_spaceBetweenPipe * ruleGame.heightGameZone / 2));

            spawnRot = Quaternion.Euler(180, 0, 0);
        } else
        {
            // Позиция смещается вниз относительно пустого пространства
            spawnPos = transform.position + transform.up * (posSpace - (ruleGame.percent_spaceBetweenPipe * ruleGame.heightGameZone / 2));

            spawnRot = Quaternion.Euler(0, 0, 0);
        }
        
        Pipe new_pipe;
        if (pipes.Count != 0 && pipes.Find(pipe => !pipe.gameObject.activeSelf) != null)
        {
            // Получение старого элемента, установка параметров заново и включение в дело
            new_pipe = pipes.Find(pipe => !pipe.gameObject.activeSelf);

            new_pipe.gameObject.SetActive(true);
        }
        else
        {
            // Создание новой трубы и получение его скрипта
            if (parentPipes != null)
                new_pipe = Instantiate(pipe,  parentPipes).GetComponent<Pipe>();
            else
                new_pipe = Instantiate(pipe).GetComponent<Pipe>();
            
            new_pipe.trRelateMove = transform;
            new_pipe.ruleGame = ruleGame;
            pipes.Add(new_pipe);
        }
        new_pipe.transform.position = spawnPos;
        new_pipe.transform.localRotation = spawnRot;
        // Выравнивание размера тела трубы
        new_pipe.AutoSize(posSpace,tpipe);
        
        StartCoroutine(TimeoutDeactivate(new_pipe.gameObject));
        return new_pipe;
    }
    void SpawnControlPoint(float posY, Transform parent)
    {
        GameObject new_cpoint;
        
        if (cpoints.Count != 0 && cpoints.Find(cpoint => !cpoint.activeSelf) != null)
        {
            // Получение старого элемента, установка параметров заново и включение в дело
            new_cpoint = cpoints.Find(cpoint => !cpoint.activeSelf);
            new_cpoint.transform.position = transform.position + transform.up * posY;
            new_cpoint.transform.rotation = transform.rotation;
            new_cpoint.transform.parent = parent;
            new_cpoint.SetActive(true);
        }
        else
        {
            new_cpoint = Instantiate(controlPoint, transform.position + transform.up * posY, transform.rotation, parent);
            cpoints.Add(new_cpoint);
        }
    }
    IEnumerator TimeoutDeactivate(GameObject pipe)
    {
        float time = ruleGame.farDeathX / ruleGame.speedMovingPipe;
        yield return new WaitForSeconds(time);
        if (isPlay)
        {
            pipe.SetActive(false);
        }
    }
    public void PreStartGame()
    {
        pipes.ForEach(pipe => pipe.gameObject.SetActive(false));
    }
     public void StartGame()
    {
        IsPlay = true;
        timerSpawn = ruleGame.timeBetweenSpawn;
        StopAllCoroutines();
    }
    public void PauseGame()
    {
        IsPlay = false;
    }
    public void UnpauseGame()
    {
        IsPlay = true;
    }
    
}

public enum TypePipe
{
    // Верхний и нижний
    Upper = 0, Lower = 1
}
