using UnityEngine;
using TMPro;




public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance { get; private set; }

    public GameObject messagePanel; // MessageBorder 또는 MessagePanel 오브젝트
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f; // 메시지 표시 시간(초)
    public float fadeDuration = 0.5f;  // 페이드아웃 시간(초)

    private Coroutine fadeCoroutine;
    //private float timer = 0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

            
        // 모든 MessagePanel 오브젝트를 찾아서 상태 출력
        var panels = GameObject.FindObjectsOfType<Transform>();
        foreach (var t in panels)
        {
            if (t.name.Contains("MessagePanel"))
            {
                Debug.Log($"[MessageManager] Found MessagePanel: {t.name}, activeSelf={t.gameObject.activeSelf}, path={GetTransformPath(t)}");
            }
        }
        // 게임 시작 시 패널을 숨김
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    // void Update()
    // {
    //     if (messageText != null && messageText.gameObject.activeSelf)
    //     {
    //         timer -= Time.deltaTime;
    //         if (timer <= 0f)
    //             messageText.gameObject.SetActive(false);
    //     }
    // }

    // 트랜스폼 경로를 출력하는 유틸 함수
    string GetTransformPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }

    public void ShowMessage(string msg, float duration = -1f)
    {
        // if (messageText == null) return;
        // messageText.text = msg;
        // messageText.gameObject.SetActive(true);
        // timer = (duration > 0f) ? duration : messageDuration;

        if (messageText == null) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
            Debug.Log($"[MessageManager] ShowMessage: messagePanel 활성화됨, name={messagePanel.name}, activeSelf={messagePanel.activeSelf}");
        }

        // 알파값 완전하게 복구
        var color = messageText.color;
        color.a = 1f;
        messageText.color = color;

        float showTime = (duration >= 0f) ? duration : messageDuration;

        if (showTime > 0f)
            fadeCoroutine = StartCoroutine(FadeOutRoutine(showTime));
        // duration == 0이면 자동으로 사라지지 않음
    }

    private System.Collections.IEnumerator FadeOutRoutine(float showTime)
    {
        yield return new WaitForSeconds(showTime);

        float t = 0f;
        Color startColor = messageText.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            messageText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        messageText.gameObject.SetActive(false);
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}