using UnityEngine;

public class EnemyDisplayManager : MonoBehaviour
{
    [Header("Tutorial—p")]
    public GameObject tutorialEnemyPrefab;

    [Header("Stage1—p")]
    public GameObject stage1EnemyPrefab;

    [Header("Stage2—p")]
    public GameObject stage2EnemyPrefab;

    private GameObject currentEnemy;


    void Start()
    {
        ShowEnemy();
    }

    void ShowEnemy()
    {
        switch (StageManager.CurrentStage)
        {

            case 0:

                if (tutorialEnemyPrefab != null)
                {
                    currentEnemy = Instantiate(tutorialEnemyPrefab,transform.position,Quaternion.identity);
                }
                break;

            case 1:

                if (stage1EnemyPrefab != null)
                {
                    currentEnemy = Instantiate(stage1EnemyPrefab,transform.position,Quaternion.identity);
                }

                break;

            case 2:

                if (stage2EnemyPrefab != null)
                {
                    currentEnemy = Instantiate(stage2EnemyPrefab, transform.position, Quaternion.identity);
                }

                break;
        }
    }
}