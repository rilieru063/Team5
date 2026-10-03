using UnityEngine;
using System.Collections;

public class EnemyDisplayManager : MonoBehaviour
{
    [Header("Tutorial用")]
    public GameObject tutorialEnemyPrefab;

    [Header("Stage1用")]
    public GameObject stage1EnemyPrefab;

    [Header("Stage2用")]
    public GameObject stage2EnemyPrefab;

    [Header("Stage3用")]
    public GameObject stage3EnemyPrefab;

    [Header("消滅演出")]
    [SerializeField] private float fadeOutDuration = 1.0f;

    private GameObject currentEnemy;

    private SpriteRenderer currentSpriteRenderer;

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
                    currentEnemy = Instantiate( tutorialEnemyPrefab, transform.position, Quaternion.identity);
                }
                break;

            case 1:
                if (stage1EnemyPrefab != null)
                {
                    currentEnemy = Instantiate( stage1EnemyPrefab, transform.position, Quaternion.identity);
                }
                break;

            case 2:
                if (stage2EnemyPrefab != null)
                {
                    currentEnemy = Instantiate( stage2EnemyPrefab, transform.position, Quaternion.identity);
                }
                break;

            case 3:
                if (stage3EnemyPrefab != null)
                {
                    currentEnemy = Instantiate( stage3EnemyPrefab, transform.position, Quaternion.identity);
                }
                break;
        }

        if (currentEnemy != null)
        {
            currentSpriteRenderer = currentEnemy.GetComponentInChildren<SpriteRenderer>();
        }
    }


    public void HideEnemy()
    {
        if (currentEnemy == null)
            return;

        if (currentSpriteRenderer == null)
            return;

        StartCoroutine(HideEnemyCoroutine());
    }


    private IEnumerator HideEnemyCoroutine()
    {
        Material material = currentSpriteRenderer.material;

        float timer = 0f;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeOutDuration;

            material.SetFloat("_Dissolve", t);

            yield return null;
        }

        material.SetFloat("_Dissolve", 1f);

        Destroy(currentEnemy);
    }
}