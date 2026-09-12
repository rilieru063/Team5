using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("玉")]
    [SerializeField] private GameObject ballPrefab;

    [Header("生成位置")]
    [SerializeField] private float spawnX = -7f;

    [SerializeField] private float spawnY = 0f;


    public GameObject SpawnBall(Transform player,int damage)
    {
        if (ballPrefab == null)
        {
            Debug.LogError("BallPrefabが設定されていません");
            return null;
        }

        Vector3 spawnPosition = new Vector3( spawnX, spawnY, 0f);

        GameObject ball = Instantiate( ballPrefab, spawnPosition, Quaternion.identity);

        Ball ballScript = ball.GetComponent<Ball>();

        if (ballScript == null)
        {
            Debug.LogError("BallコンポーネントがBallPrefabにありません");

            return ball;
        }

        ballScript.Initialize(player);
        ballScript.SetDamage(damage);

        return ball;

    }
}
