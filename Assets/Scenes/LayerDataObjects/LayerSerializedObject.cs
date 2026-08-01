using UnityEngine;

[CreateAssetMenu]
public class LayerSerializedObject : ScriptableObject
{
    public Color[] LayerPallete = new Color[4] {Color.black,Color.red ,Color.blue ,Color.white};

    public GameObject[] BaseBlockPallet;

    public GameObject[] CommonFeatures;
    public int CommonFeatureSpawnRange = 15;

    public GameObject[] RareFeatures;
    public int RareFeatureSpawnRange = 5;

    public GameObject[] SpawnedEnemies;
    public int NumberOfEnemies = 4;

    public BossManager.Bosses LayerBoss;
}
