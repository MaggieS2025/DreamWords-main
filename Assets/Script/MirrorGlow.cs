using UnityEngine;

public class MirrorFrameGlowDynamic : MonoBehaviour
{
    public float minBright = 2.5f;
    public float maxBright = 6f;
    public float flowSpeed = 1.2f;

    private Material frameMat;

    void Start()
    {
        frameMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // 呼吸明暗
        float bright = Mathf.Lerp(minBright, maxBright,
            Mathf.PingPong(Time.time * flowSpeed, 1f));

        frameMat.SetColor("_EmissionColor",
            new Color(0.75f, 0.9f, 1f) * bright);
    }
}