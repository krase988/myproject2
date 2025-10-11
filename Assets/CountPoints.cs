using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountPoints : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI pointsText;
    public Image backgroundImage;

    [Header("설정")]
    public int currentPoints = 0;

    void Start()
    {
        UpdateUI();
        // UI 스타일 적용
        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(0.2f, 0.8f, 0.2f, 1f); // 초록색
            backgroundImage.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 40);
        }
        if (pointsText != null)
        {
            pointsText.color = Color.white;
            pointsText.fontSize = 28;
            pointsText.alignment = TextAlignmentOptions.MidlineRight; // 오른쪽 정렬
            pointsText.margin = new Vector4(0, 0, 20, 0); // 오른쪽 패딩
        }
    }

    public void AddPoint(int amount = 1)
    {
        currentPoints += amount;
        UpdateUI();
    }

    public void RemovePoint(int amount = 1)
    {
        currentPoints = Mathf.Max(0, currentPoints - amount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (pointsText != null)
            pointsText.text = currentPoints.ToString();
    }
}