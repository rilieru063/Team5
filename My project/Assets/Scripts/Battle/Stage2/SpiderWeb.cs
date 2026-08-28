using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    private float shrinkTime;
    private float stayTime;

    private float startScale;
    private float endScale;

    private float timer;

    private bool isShrinking = false;
    private bool isStaying = false;

    public void Initialize(float shrinkDuration,float stayDuration,float initialScale,float finalScale)

    {
        shrinkTime = shrinkDuration;
        stayTime = stayDuration;

        startScale = initialScale;
        endScale = finalScale;

        timer = 0f;

        transform.localScale =Vector3.one * startScale;

        isShrinking = true;
    }

    void Update()
    {
        // ‘å‚«‚¢ó‘Ô‚©‚çk¬
        if (isShrinking)
        {
            timer += Time.deltaTime;

            float t = timer / shrinkTime;
            t = Mathf.Clamp01(t);

            transform.localScale = Vector3.Lerp(Vector3.one * startScale,Vector3.one * endScale,t);

            if (t >= 1f)
            {
                isShrinking = false;
                isStaying = true;

                timer = 0f;
            }

            return;
        }

        // k¬Œã

        if (isStaying)
        {
            timer += Time.deltaTime;

            if (timer >= stayTime)
            {
                isStaying = false;

                Destroy(gameObject);
            }
        }
    }
}
