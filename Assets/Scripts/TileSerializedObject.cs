using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileConfig : ScriptableObject
{
    public int Health = 10;

    [Range(0, 100)]

    public int SpawnChance = 25;

    public int TreasureDrop = 0;
    public int FuelDrop = 0;

    public bool ExplodeOnBreak = false;
    public LayerMask LayersDected;
    public float ExplosionRange = 0f;
    public int ExplosionDamage = 0;
    public float BlastForce = 1f;

    public GameObject[] SpawnedEnemies;
    public GameObject[] SpawnedPowerUps;

    public Sprite[] BrokenOverlays;

}
