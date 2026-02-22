using System;
using UnityEngine;

public class ExplodeBulletsOnDeath : MonoBehaviour
{

    public GameObject BulletPrefab;
    private TileBehavior behavior;

    public enum SpreadDirections
    {
        EightWays,
        FourWays,
        SideToSide
    }

    public SpreadDirections DirectionSetting;

    public float SpawnOffset = 0f;
    public Vector2 PositionOffset = Vector2.one/2;

    private void Start()
    {
        behavior = GetComponent<TileBehavior>();
        behavior.OnTileBreak += SpawnBullets;
    }

    

    void SpawnBullets(object sender, EventArgs e)
    {
        switch (DirectionSetting)
        {
            case SpreadDirections.EightWays:

                for (int i = 0;i < 8; i++)
                {
                    float radians = 45 * (float)i * Mathf.Deg2Rad;

                    Vector2 dir = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

                    Instantiate(BulletPrefab, ((Vector2)transform.position + PositionOffset) + (dir * SpawnOffset), Quaternion.LookRotation(dir,Vector3.forward) * Quaternion.Euler(90,0,0));
                }


                break;

        }
    }
}
