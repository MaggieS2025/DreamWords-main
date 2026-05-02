using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneMusicPair
    {
        public string sceneName;   // 场景文件名（不含 .unity）
        public AudioClip clip;     // 对应的背景音乐
    }

    public SceneMusicPair[] sceneMusicList;

    private AudioSource audioSource;
    public static BGMManager Instance { get; private set; }

    void Awake()
    {
        // 强制成为根物体，防止父物体被卸载时连带销毁
        transform.SetParent(null);

        // 单例：保证只有一个 BGMManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 设置 AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;    // 2D 音乐，全局统一音量
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // 监听场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
        // 播放当前场景的音乐
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        AudioClip clipToPlay = null;
        foreach (var pair in sceneMusicList)
        {
            if (pair.sceneName == sceneName)
            {
                clipToPlay = pair.clip;
                break;
            }
        }

        if (clipToPlay == null)
        {
            // 如果没有为该场景配置音乐，可以停止或保留上一首
            // audioSource.Stop();
            return;
        }

        // 如果已经在播放同一首，就不重新开始
        if (audioSource.clip == clipToPlay && audioSource.isPlaying)
            return;

        audioSource.clip = clipToPlay;
        audioSource.Play();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

