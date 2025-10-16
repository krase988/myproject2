using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniPopup : MonoBehaviour
{
    public RectTransform popupRect;
    public TextMeshProUGUI infoText;
    public Canvas canvas;
    public GridCube targetCube = null; // 현재 팝업이 표시 중인 GridCube

    private void Awake()
    {
        Hide();
    }

    void Update()
    {
        if (popupRect.gameObject.activeSelf)
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out mousePos
            );
            popupRect.anchoredPosition = mousePos + new Vector2(80, -40); // 마우스 오른쪽 아래로 약간 이동
            if (targetCube != null)
                UpdateInfo(); // 매 프레임 정보 갱신
        }
    }
    private void UpdateInfo()
    {
        if (targetCube != null)
        {
            infoText.text = $"No: {targetCube.cubeIndex}\nHeight: {targetCube.GetBuildingCount()}";
        }
    }

    //public void Show(string info)
    public void Show(GridCube cube)
    {
        targetCube = cube;
        UpdateInfo(); // 즉시 한 번 갱신
        popupRect.gameObject.SetActive(true);
    }
    public void Show(string info)
    {
        targetCube = null; // 타겟 없음
        infoText.text = info;
        popupRect.gameObject.SetActive(true);
    }

    public void Hide()
    {
        popupRect.gameObject.SetActive(false);
        targetCube = null;
    }
}