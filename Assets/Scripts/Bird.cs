using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Bird : MonoBehaviour
{
    public DataGame dataGame;
    public RuleGame ruleGame;

    public float k_rotation;
    Rigidbody rb;
    Animator animator;
    Action UpdateFly = null;

    public GameObject vuBtnObject;

    bool accessVuButton = true;
    float durNonAccessVuButton = 0.3f;

    private static Bird _instance;
    public static Bird Instance
    {
        get => _instance;
        set
        {
            if (_instance == null)
                _instance = value;
            else
                Debug.LogError("This instance already exist: Bird");
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    [Obsolete]
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (dataGame.tapOnScreen && Input.touchSupported)
        {
            UpdateFly = UpdateTouch;
        }
        else if (dataGame.tapOnScreen && !Input.touchSupported)
        {
            UpdateFly = UpdateClick;
        }
        else
        {
            vuBtnObject.GetComponent<VirtualButtonBehaviour>().RegisterOnButtonPressed(UpdateVuButton);
        }
    }


    void Update()
    {
        if (ManagerGame.Instance.IsTargetFound)
        {
            UpdateFly?.Invoke();
            UpdateFall();
            UpdateRotation();
        }
    }
    void UpdateFall()
    {
        if (ManagerGame.Instance.stateGame == StateGame.isPlaying)
        {
            float curFall = ruleGame.gravityPower * Time.deltaTime;
            rb.velocity += curFall * -ManagerGame.Instance.transform.up;
        }
    }
    public void FlyUp()
    {
        Vector3 velocity = ManagerGame.Instance.transform.up * ruleGame.flyUpPower;
        rb.velocity = velocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pipe"))
        {
            ManagerGame.Instance.EndGame();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ControlPoint"))
        {
            ManagerGame.Instance.Score++;
            other.gameObject.SetActive(false);
            other.transform.parent = null;
        }
    }
    public void PreStartGame(Vector3 location)
    {
        transform.SetPositionAndRotation(location, ManagerGame.Instance.transform.rotation);
    }
    public void EndGame()
    {
        rb.isKinematic = true;
    }
    public void PauseGame()
    {
        rb.isKinematic = true;
    }
    public void UnpauseGame()
    {
        rb.isKinematic = false;
    }

    private void UpdateClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (ManagerGame.Instance.stateGame)
            {
                case StateGame.isPreStart:
                    ManagerGame.Instance.StartGame();
                    rb.isKinematic = false;
                    FlyUp();
                    break;
                case StateGame.isPlaying:
                    FlyUp();
                    break;
            }
        }
    }
    private void UpdateTouch()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            switch (ManagerGame.Instance.stateGame)
            {
                case StateGame.isPreStart:
                    ManagerGame.Instance.StartGame();
                    rb.isKinematic = false;
                    FlyUp();
                    break;
                case StateGame.isPlaying:
                    FlyUp();
                    break;
            }
        }
    }

    [Obsolete]
    private void UpdateVuButton(VirtualButtonBehaviour vb)
    {
        if (accessVuButton)
        {
            switch (ManagerGame.Instance.stateGame)
            {
                case StateGame.isPreStart:
                    ManagerGame.Instance.StartGame();
                    rb.isKinematic = false;
                    FlyUp();
                    break;
                case StateGame.isPlaying:
                    FlyUp();
                    break;
            }
            accessVuButton = false;
            StartCoroutine(BlockAccessVuButton());
        }
    }
    IEnumerator BlockAccessVuButton()
    {
        yield return new WaitForSeconds(durNonAccessVuButton);
        accessVuButton = true;
        Debug.Log(DateTime.Now.ToString() + " " + accessVuButton);
    }
    private void UpdateRotation()
    {
        if (!rb.isKinematic)
        {
            float velocityY = rb.velocity.z;
            Quaternion rotation = Quaternion.AngleAxis(velocityY * k_rotation, Vector3.forward);
            transform.localRotation = rotation;
        }
    }
}
