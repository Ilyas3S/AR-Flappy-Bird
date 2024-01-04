using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Vector3 vectorMove = Vector3.right;
    public Transform trRelateMove;

    public RuleGame ruleGame;
    public Transform bodyPipe;
    [SerializeField] float k_autoSize;
    public bool paused = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }
    void UpdateMove()
    {
        if (!paused && ManagerGame.Instance.IsTargetFound)
        {
            transform.Translate(ruleGame.speedMovingPipe * Time.deltaTime * trRelateMove.right, Space.World);
        }
    }
    public void AutoSize(float pos, TypePipe type)
    {
        Vector3 scale = bodyPipe.localScale;
        float new_pos = type == TypePipe.Upper ? ruleGame.heightGameZone - pos : pos;
        scale.z = 76.37997f * new_pos * k_autoSize;
        bodyPipe.localScale = scale;
    }
}
