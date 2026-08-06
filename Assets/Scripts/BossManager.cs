using UnityEngine;

public class BossManager : MonoBehaviour
{
    #region obj and comp

    Camera cam;
    BlockGenerator blockGenerator;

    #endregion

    public enum Bosses
    {
        InsectQueen,
        Viburnum
    }

    [SerializeField] GameObject RewardChest;
    [SerializeField] GameObject InsectQueenPrefab;
    [SerializeField] GameObject ViburnumPrefab;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blockGenerator = GetComponent<BlockGenerator>();    
        //StartBossFight(Bosses.InsectQueen);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartBossFight(Bosses boss)
    {
        GameManager.Instance.CurrentState = GameManager.GameStates.Boss;
        GameManager.Instance.StatTracker.TurnOnBoss();
        GameObject spawnedBoss;



        switch (boss)
        {
            case Bosses.InsectQueen:

                 spawnedBoss = Instantiate(InsectQueenPrefab,new Vector3(0,2.5f,1),Quaternion.identity,Camera.main.transform);
                spawnedBoss.GetComponent<Enemy>().OnEnemyDeath += OnBossFightEnd;

                break;

                case Bosses.Viburnum:

                 spawnedBoss = Instantiate(ViburnumPrefab, GameManager.Instance.Platform.transform.position, Quaternion.identity, Camera.main.transform);
                 spawnedBoss.GetComponentInChildren<Enemy>().OnEnemyDeath += OnBossFightEnd;


                break;
        }
    }

    private void OnBossFightEnd(object sender, Enemy boss)
    {
        boss.OnEnemyDeath -= OnBossFightEnd;
        GameManager.Instance.CurrentState = GameManager.GameStates.RegularGame;
        GameManager.Instance.StatTracker.TurnOffBoss(true);
        GameManager.Instance.Platform.IncreaseFuelRate(.1f);
        blockGenerator.NextLayer();
        GameManager.Instance.SoundManager.PlayNonDiageticSound("MainThemeStart");
        if (RewardChest != null) { Instantiate(RewardChest, this.transform.position, Quaternion.identity); }

    }


}
