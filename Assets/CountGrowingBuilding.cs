using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountGrowingBuilding : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI countText;
    public Image backgroundImage;

    [Header("설정")]
    public int maxCount = 3;

    private int currentCount;

    void Start()
    {
        currentCount = maxCount;
        UpdateUI();
        // UI 스타일 적용
        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(0.5f, 0.2f, 0.8f, 1f); // 보라색
            //backgroundImage.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 40);
            backgroundImage.GetComponent<Image>().raycastTarget = false;
            backgroundImage.GetComponent<Image>().maskable = false;
            backgroundImage.GetComponent<Image>().type = Image.Type.Sliced;
        }
        if (countText != null)
        {
            countText.color = Color.white;
            countText.fontSize = 28;
        }
    }

    public bool CanSelect()
    {
        return currentCount > 0;
    }

    public void OnSelect()
    {
        if (currentCount > 0)
        {
            currentCount--;
            UpdateUI();
        }
    }

    public void OnDeselect()
    {
        //Debug.Log("OnDeselect called");
        if (currentCount < maxCount)
        {
            currentCount++;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (countText != null)
            countText.text = $"{currentCount}/{maxCount}";
    }
}