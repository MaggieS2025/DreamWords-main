using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    public AudioClip uiClickSound;   // 拖入你的点击音效
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    /// <summary> 外部按钮调用：先播音效，再执行后续动作 </summary>
    /// <param name="onComplete">音效播完后会执行的事件</param>
    public void PlayClickAndThen(UnityAction onComplete)
    {
        if (uiClickSound == null)
        {
            Debug.LogWarning("UIAudioManager: 没有指定 UI 点击音效");
            onComplete?.Invoke();
            return;
        }

        StartCoroutine(PlayAndWait(onComplete));
    }

    IEnumerator PlayAndWait(UnityAction onComplete)
    {
        audioSource.PlayOneShot(uiClickSound);
        // 等待音效长度（确保完整播放）
        yield return new WaitForSeconds(uiClickSound.length);
        onComplete?.Invoke();
    }
}
