using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;       // 点击音效文件
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D 音效

        // 给场景中所有已有的按钮绑定事件
        Button[] allButtons = FindObjectsOfType<Button>(true);
        foreach (Button btn in allButtons)
        {
            AddClickSound(btn);
        }
    }

    void AddClickSound(Button button)
    {
        // 移除重复绑定，防止多次添加
        button.onClick.RemoveListener(PlayClickSound);
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    // 如果运行后有新按钮动态生成，可以外部调用这个公开方法绑定
    public void RegisterButton(Button newButton)
    {
        AddClickSound(newButton);
    }
}
