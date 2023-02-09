using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class SOPlayerData : ScriptableObject
{


    public int level;
    public float speed;
    public int initialPlayerHealth;
    public float dashSpeed;
    public float dashTime;
    public int ammoPrimary;
    public int ammoSecondary;
    public float defaultSpeed;
    public int currentPlayerHealth;


    // [0] = triangle,
    // [1] = square,
    // [2] = circle/hex
    public int[] resource = { 0, 0, 0 };

}