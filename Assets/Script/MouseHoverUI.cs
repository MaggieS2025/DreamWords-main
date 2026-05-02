using UnityEngine;
using UnityEngine.UI;

public class MouseHoverShowImg : MonoBehaviour
{
    [Header("感应UI区域")]
    public RectTransform hoverArea;

    [Header("悬浮要显示的图片")]
    public GameObject showImage;

    void Start()
    {
        // 默认隐藏
        if (showImage != null)
            showImage.SetActive(false);
    }

    void Update()
    {
        // 判断鼠标是否在UI区域内
        if (RectTransformUtility.RectangleContainsScreenPoint(hoverArea, Input.mousePosition))
        {
            showImage.SetActive(true);
        }
        else
        {
            showImage.SetActive(false);
        }
    }
}