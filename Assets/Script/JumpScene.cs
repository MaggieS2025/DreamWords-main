using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpToScene : MonoBehaviour
{
    // 要跳转的场景名，统一设为 "Scene2"
    public string targetSceneName = "Scene2";

    // 点击按钮触发
    public void OnClickJump()
    {
        Debug.Log("✅ 点击跳转Scene2！");

        // 加载场景
        SceneManager.LoadScene(targetSceneName);
    }
}