using UnityEngine;
using UnityEngine.EventSystems;

public class ToolButton1Popup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MiniPopup miniPopup; // 인스펙터에서 연결
    public IncreaseBuildingCount increaseBuildingCount; // 인스펙터에서 연결

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (miniPopup != null && increaseBuildingCount != null)
        {
            string info = $"건설가능 빌딩수 증가\n필요 재화: {increaseBuildingCount.GetCurrentCost()}";
            miniPopup.Show(info);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (miniPopup != null)
            miniPopup.Hide();
    }
}