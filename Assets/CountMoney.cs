using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountMoney : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI moneyText;
    public Image backgroundImage;

    [Header("설정")]
    public int currentMoney = 0;

    void Start()
    {
        UpdateUI();
        // UI 스타일 적용
        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(1f, 0.82f, 0f, 1f); // 진노랑색
            backgroundImage.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 40);
        }
        if (moneyText != null)
        {
            moneyText.color = Color.white;
            moneyText.fontSize = 28;
            moneyText.alignment = TextAlignmentOptions.MidlineRight; // 오른쪽 정렬
            moneyText.margin = new Vector4(0, 0, 20, 0); // 오른쪽 패딩
        }
    }

    public void AddMoney(int amount = 1)
    {
        currentMoney += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = currentMoney.ToString();
    }
}