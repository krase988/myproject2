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
    private Coroutine hideCoroutine;

    void Start()
    {
        toolButtonsPanel.SetActive(false);

        // ToolButtonPanel에도 이벤트 트리거 추가
        EventTrigger trigger = toolButtonsPanel.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = toolButtonsPanel.AddComponent<EventTrigger>();

        // PointerEnter
        var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((data) => { OnPointerEnter(null); });
        trigger.triggers.Add(entryEnter);

        // PointerExit
        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((data) => { OnPointerExit(null); });
        trigger.triggers.Add(entryExit);
    }

    void Update()
    {
        // if (!isPointerOver && toolButtonsPanel.activeSelf)
        //     toolButtonsPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("[MainToolButtonGroup] OnPointerEnter");
        isPointerOver = true;
        toolButtonsPanel.SetActive(true);

        // 숨김 코루틴이 돌고 있다면 중지
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("[MainToolButtonGroup] OnPointerExit");
        isPointerOver = false;
        // toolButtonsPanel.SetActive(false);
        if (hideCoroutine == null)
            hideCoroutine = StartCoroutine(HidePanelWithDelay(1f));
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
    private System.Collections.IEnumerator HidePanelWithDelay(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     if (!isPointerOver)
    //         toolButtonsPanel.SetActive(false);
    //     hideCoroutine = null;
    // }
    {
        //Debug.Log($"[MainToolButtonGroup] HidePanelWithDelay 시작, delay={delay}");
        yield return new WaitForSeconds(delay);
        //Debug.Log($"[MainToolButtonGroup] HidePanelWithDelay 대기 후, isPointerOver={isPointerOver}");
        if (!isPointerOver)
        {
            toolButtonsPanel.SetActive(false);
            //Debug.Log("[MainToolButtonGroup] toolButtonsPanel 비활성화!");
        }
        hideCoroutine = null;
    }
}