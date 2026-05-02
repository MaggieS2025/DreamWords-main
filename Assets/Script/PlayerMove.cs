using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移动设置")]
    public float speed = 3f;
    public float mouseSensitivity = 100f;
    public float gravity = 20f;

    private float xRotation = 0f;
    private CharacterController controller;
    private Vector3 velocity;

    [Header("镜子交互")]
    public Camera playerCamera;
    public GameObject mirrorUI;
    private bool isInMirrorArea = false;

    [Header("地图控制")]
    private bool isMapOpen = false;

    [Header("UI 音效")]
    public AudioSource uiAudioSource;    // 拖入用于播放 UI 音效的 AudioSource
    public AudioClip uiClickSound;      // 拖入 UI 点击音效

    [Header("脚步音效")]
    public AudioSource footstepAudioSource;   // 拖入用于播放脚步声的 AudioSource
    public AudioClip[] footstepClips;         // 拖入多个脚步声片段
    public float walkStepInterval = 0.5f;     // 走路步频间隔
    public float runStepInterval = 0.35f;     // 跑步步频间隔

    private float stepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        LockMouse();

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (mirrorUI != null)
            mirrorUI.SetActive(false);

        // 自动获取脚步 AudioSource（如果未拖入）
        if (footstepAudioSource == null)
            footstepAudioSource = GetComponent<AudioSource>();

        // 防止游戏开始时玩家已在镜子范围内误触发音效
        Collider[] startColliders = Physics.OverlapSphere(transform.position, 2.5f);
        foreach (var col in startColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Mirror"))
            {
                isInMirrorArea = true;
                mirrorUI.SetActive(true);
                UnlockMouse();
                break;
            }
        }
    }

    void Update()
    {
        if (!isInMirrorArea && !isMapOpen)
        {
            PlayerMoveAndLook();
        }

        CheckMirrorArea();
    }

    void PlayerMoveAndLook()
    {
        // 获取移动输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // 重力
        if (controller.isGrounded)
            velocity.y = -2f;
        else
            velocity.y -= gravity * Time.deltaTime;

        controller.Move(move * speed * Time.deltaTime + velocity * Time.deltaTime);

        // 鼠标视角
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        xRotation = Mathf.Clamp(xRotation - mouseY, -85f, 85f);
        playerCamera.transform.localEulerAngles = new Vector3(xRotation, 0, 0);

        // 处理脚步声
        HandleFootsteps(move);
    }

    void HandleFootsteps(Vector3 moveDirection)
    {
        // 只有在地面上且有移动输入时才播放脚步声
        if (!controller.isGrounded) return;
        if (moveDirection.magnitude < 0.1f) return;
        if (footstepAudioSource == null || footstepClips.Length == 0) return;

        // 检测是否在跑步（按左Shift）
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float interval = isRunning ? runStepInterval : walkStepInterval;

        stepTimer += Time.deltaTime;
        if (stepTimer >= interval)
        {
            stepTimer = 0f;
            // 随机选一个脚步声片段播放
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            footstepAudioSource.PlayOneShot(clip);
        }
    }

    void CheckMirrorArea()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.5f);
        bool nowInMirror = false;
        GameObject mirrorObject = null;

        foreach (var col in hitColliders)
        {
            // 通过 Layer 判断镜子（Tag 保留 NoTreeZone 防止树木遮挡）
            if (col.gameObject.layer == LayerMask.NameToLayer("Mirror"))
            {
                nowInMirror = true;
                mirrorObject = col.gameObject;
                break;
            }
        }

        // 进入镜子范围
        if (nowInMirror && !isInMirrorArea)
        {
            isInMirrorArea = true;
            mirrorUI.SetActive(true);
            UnlockMouse();

            // 播放镜子上的音效
            if (mirrorObject != null)
            {
                AudioSource mirrorAudio = mirrorObject.GetComponent<AudioSource>();
                if (mirrorAudio != null && mirrorAudio.clip != null)
                    mirrorAudio.PlayOneShot(mirrorAudio.clip);
            }
        }

        // 离开镜子范围
        if (!nowInMirror && isInMirrorArea)
        {
            isInMirrorArea = false;
            mirrorUI.SetActive(false);
            LockMouse();
        }
    }

    // 播放 UI 点击音效（内部调用）
    void PlayUIClickSound()
    {
        if (uiAudioSource != null && uiClickSound != null)
            uiAudioSource.PlayOneShot(uiClickSound);
    }

    /// <summary>“回去”按钮调用：关闭镜子 UI，返回游戏</summary>
    public void CloseMirrorUITotal()
    {
        PlayUIClickSound();
        isInMirrorArea = false;
        mirrorUI.SetActive(false);
        LockMouse();
    }

    public void CloseMirrorUI()
    {
        CloseMirrorUITotal();
    }

    /// <summary>“下一关”按钮调用：播放音效并跳转（你需要填充实际逻辑）</summary>
    public void GoToNextLevel()
    {
        PlayUIClickSound();

        // 在此处添加你的下一关跳转逻辑，例如：
        // SceneManager.LoadScene("NextLevel");
        Debug.Log("下一关点击，已播放音效");
    }

    // 地图 UI 控制
    public void OpenMapUI()
    {
        isMapOpen = true;
        UnlockMouse();
    }

    public void CloseMapUI()
    {
        isMapOpen = false;
        LockMouse();
    }

    void LockMouse()
    {
        // 暂时注释掉锁鼠标功能（如需启用，去掉注释即可）
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}