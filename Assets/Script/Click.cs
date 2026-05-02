using UnityEngine;

public class UISwitchButton : MonoBehaviour
{
    public GameObject panelToHide; // 拖入 bg-white
    public GameObject panelToShow; // 拖入 bg-white2

    public void OnClickSwitch()
    {
        Debug.Log("✅ Button 点击成功！");

        if (panelToHide != null)
            panelToHide.SetActive(false);

        if (panelToShow != null)
            panelToShow.SetActive(true);
    }
}