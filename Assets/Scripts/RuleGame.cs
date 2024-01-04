using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RuleGame : ScriptableObject
{
    public float speedMovingPipe;
    public float timeBetweenSpawn;
    public float farDeathX;

    public float percent_minHeightPipe;
    public float percent_spaceBetweenPipe;

    public float heightGameZone;

    public float gravityPower;
    public float maxGravitySpeed;
    public float flyUpPower;
}
