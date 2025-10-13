using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance { get; private set; }

    public TextMeshProUGUI messageText;
    public float messageDuration = 2f; // 메시지 표시 시간(초)

    private float timer = 0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (messageText != null && messageText.gameObject.activeSelf)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                messageText.gameObject.SetActive(false);
        }
    }

    public void ShowMessage(string msg, float duration = -1f)
    {
        if (messageText == null) return;
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        timer = (duration > 0f) ? duration : messageDuration;
    }
}