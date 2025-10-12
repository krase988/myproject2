using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

public class MainToolButtonGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button mainButton;
    public Image mainIcon;
    public GameObject toolButtonsPanel; // Vertical Layout Group
    public List<Button> toolButtons = new List<Button>();
    public List<Image> toolIcons = new List<Image>();

    private bool isPointerOver = false;

    void Start()
    {
        toolButtonsPanel.SetActive(false);
    }

    void Update()
    {
        if (!isPointerOver && toolButtonsPanel.activeSelf)
            toolButtonsPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        toolButtonsPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        toolButtonsPanel.SetActive(false);
    }

    // 외부에서 메인 버튼/아이콘/이벤트/툴버튼/툴아이콘/툴이벤트를 쉽게 설정할 수 있도록 메서드 제공
    public void SetMainIcon(Sprite icon) => mainIcon.sprite = icon;
    public void SetMainButtonAction(UnityEngine.Events.UnityAction action)
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(action);
    }
    public void SetToolButton(int index, Sprite icon, UnityEngine.Events.UnityAction action)
    {
        if (index < toolButtons.Count && index < toolIcons.Count)
        {
            toolIcons[index].sprite = icon;
            toolButtons[index].onClick.RemoveAllListeners();
            toolButtons[index].onClick.AddListener(action);
        }
    }
}