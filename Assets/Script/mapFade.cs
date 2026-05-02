using UnityEngine;
using UnityEngine.UI;

public class MapFadeUI : MonoBehaviour
{
    [Header("拖进对应UI")]
    public Image smallMap;
    public Image bigMap;
    public float fadeSpeed = 1f;

    private bool isOpen = false;

    // 给小地图按钮绑定这个方法
    public void OpenBigMap()
    {
        if (isOpen) return;
        isOpen = true;

        // 小地图直接变透明消失
        smallMap.color = new Color(smallMap.color.r, smallMap.color.g, smallMap.color.b, 0f);


        FindObjectOfType<PlayerMove>().OpenMapUI();
    }

    void Update()
    {
        if (isOpen)
        {
            // 大地图慢慢显示出来
            Color c = bigMap.color;
            if (c.a < 1f)
            {
                c.a += Time.deltaTime * fadeSpeed;
                bigMap.color = c;
            }
        }
    }
}