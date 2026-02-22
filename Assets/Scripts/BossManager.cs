using UnityEngine;

public class BossManager : MonoBehaviour
{
    #region obj and comp

    Camera cam;

    #endregion

    public enum Bosses
    {
        InsectQueen,
        HeadAndHands,
        Plantera
    }

    [SerializeField] GameObject InsectQueenPrefab;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        

        switch (boss)
        {
            case Bosses.InsectQueen:

                GameObject spawnedBoss = Instantiate(InsectQueenPrefab,new Vector3(0,2.5f,1),Quaternion.identity,Camera.main.transform);
                spawnedBoss.GetComponent<Enemy>().OnEnemyDeath += OnBossFightEnd;

                break;
        }
    }

    private void OnBossFightEnd(object sender, Enemy boss)
    {
        boss.OnEnemyDeath -= OnBossFightEnd;
        GameManager.Instance.CurrentState = GameManager.GameStates.RegularGame;
        GameManager.Instance.StatTracker.TurnOffBoss(true);

        GameManager.Instance.SoundManager.PlayNonDiageticSound("MainThemeStart");


    }


}
