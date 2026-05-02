using UnityEngine;

public class FlyAround : MonoBehaviour
{
    [Header("飞行速度")]
    public float speed = 2f;

    [Header("多久换一次方向")]
    public float changeDirInterval = 5f;

    [Header("上下浮动幅度")]
    public float floatStrength = 0.8f;

    private Vector3 currentDir;
    private float timer;

    void Start()
    {
        SetNewRandomDirection();
    }

    void Update()
    {
        // 乱飞
        transform.Translate(currentDir * speed * Time.deltaTime, Space.World);

        // 轻微上下漂
        Vector3 pos = transform.position;
        pos.y += Mathf.Sin(Time.time * 2f) * floatStrength * Time.deltaTime;
        transform.position = pos;

        // 定时换方向
        timer += Time.deltaTime;
        if (timer >= changeDirInterval)
        {
            SetNewRandomDirection();
            timer = 0;
        }
    }

    void SetNewRandomDirection()
    {
        currentDir = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-0.2f, 0.2f),  // Y小一点，不会疯狂上下飞
            Random.Range(-1f, 1f)
        ).normalized;
    }
}