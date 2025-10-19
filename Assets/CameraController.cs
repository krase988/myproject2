using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;       // 이동 속도
    public float rotateSpeed = 100f;    // 회전 속도
    public float zoomSpeed = 10f;       // 줌 속도

    [Header("Pan (중간 버튼 드래그)")]
    public float panSpeed = 1f;             // 마우스 픽셀 -> 월드 이동 비율
    public bool blockWhenPointerOverUI = true; // UI 위에서 패닝 차단 여부
    public bool usePanSmooth = true;
    public float panSmoothTime = 0.05f;

    [Header("Edge Scroll (화면 가장자리 이동)")]
    public bool edgeScrollEnabled = true;   // 엣지 스크롤 사용 여부
    public float edgeScrollZone = 30f;      // 엣지 영역 크기(픽셀)
    public float edgeScrollSpeed = 10f;     // 엣지 스크롤 속도 (월드 단위/sec)
    public bool edgeScrollUseSmooth = true; // 부드럽게 이동 여부

    public float panPlaneDistance = 20f; // 카메라 앞의 가상 평면까지 거리(Inspector에서 조절)

    Camera cam;
    Vector3 panVelocity = Vector3.zero;
    Vector3 targetPosition;
    Vector3 lastMousePos;
    bool isPanning = false;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            cam = Camera.main;
        targetPosition = transform.position;
    }

    void Update()
    {
        // 키보드 WSAD 이동
        float h = Input.GetAxis("Horizontal");   // A/D or Left/Right
        float v = Input.GetAxis("Vertical");     // W/S or Up/Down
        Vector3 move = new Vector3(h, 0, v);
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        // --- 중간 버튼으로 팬(화면 이동) ---
        if (Input.GetMouseButtonDown(2))
        {
            isPanning = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(2))
        {
            isPanning = false;
            panVelocity = Vector3.zero;
        }

        if (isPanning && Input.GetMouseButton(2))
        {
            // UI 위에서 패닝을 막을지 체크
            // if (blockWhenPointerOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            // {
            //     // UI 위에서는 드래그 시작 위치만 갱신하여 갑작스런 점프 방지
            //     lastMousePos = Input.mousePosition;
            // }
            // else
            // {
            //     Vector3 delta = Input.mousePosition - lastMousePos;
            //     lastMousePos = Input.mousePosition;

            //     // 픽셀 단위 델타를 카메라의 오른쪽/위 방향으로 변환
            //     Vector3 worldDelta = (-cam.transform.right * delta.x + -cam.transform.up * delta.y) * (panSpeed * Time.deltaTime);

            //     targetPosition = transform.position + worldDelta;

            //     if (usePanSmooth)
            //         transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref panVelocity, panSmoothTime);
            //     else
            //         transform.position = targetPosition;
            // }
            
            // UI 위 체크는 기존 코드와 동일하게 유지
            if (blockWhenPointerOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                lastMousePos = Input.mousePosition;
            }
            else
            {
                // 이전 픽셀 델타 방식 대신, "카메라 전방에 수직인 평면"을 기준으로 스크린->월드 변환
                Vector3 prevMouse = lastMousePos;
                Vector3 currMouse = Input.mousePosition;
                lastMousePos = currMouse;

                // 카메라 앞에 있는 평면(법선 = camera.forward)
                Plane dragPlane = new Plane(cam.transform.forward, cam.transform.position + cam.transform.forward * panPlaneDistance);

                Ray rPrev = cam.ScreenPointToRay(prevMouse);
                Ray rCurr = cam.ScreenPointToRay(currMouse);

                if (dragPlane.Raycast(rPrev, out float enterPrev) && dragPlane.Raycast(rCurr, out float enterCurr))
                {
                    Vector3 worldPrev = rPrev.GetPoint(enterPrev);
                    Vector3 worldCurr = rCurr.GetPoint(enterCurr);

                    // 화면 상의 이동이 평면 위에서 동일하게 유지되도록 카메라를 worldPrev - worldCurr 만큼 이동
                    Vector3 worldDelta = worldPrev - worldCurr;
                    targetPosition = transform.position + worldDelta;

                    if (usePanSmooth)
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref panVelocity, panSmoothTime);
                    else
                        transform.position = targetPosition;
                }
            }
            
        }
        else
        {
            // --- Edge Scroll: 마우스가 화면 가장자리에 있으면 카메라 이동 ---
            if (edgeScrollEnabled)
            {
                // UI 위에서 엣지 스크롤 차단
                if (!(blockWhenPointerOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
                {
                    Vector2 mouse = Input.mousePosition;
                    float ex = 0f, ey = 0f;

                    if (mouse.x <= edgeScrollZone) ex = -1f;
                    else if (mouse.x >= Screen.width - edgeScrollZone) ex = 1f;

                    if (mouse.y <= edgeScrollZone) ey = -1f;
                    else if (mouse.y >= Screen.height - edgeScrollZone) ey = 1f;

                    Vector2 edgeDir = new Vector2(ex, ey);
                    if (edgeDir != Vector2.zero)
                    {
                        // 카메라의 오른쪽 방향과 전방 방향을 사용하되, 전방은 XZ 평면으로 투영
                        Vector3 right = cam.transform.right;
                        Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;

                        Vector3 worldDelta = (right * edgeDir.x + forward * edgeDir.y) * edgeScrollSpeed * Time.deltaTime;

                        targetPosition = transform.position + worldDelta;

                        if (edgeScrollUseSmooth || usePanSmooth)
                            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref panVelocity, panSmoothTime);
                        else
                            transform.position = targetPosition;
                    }
                }
            }
        }

        // 마우스 우클릭 + 드래그로 카메라 회전
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            transform.Rotate(Vector3.up, mouseX * rotateSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, -mouseY * rotateSpeed * Time.deltaTime, Space.Self);
        }

        // 마우스 휠로 줌 인/아웃
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scroll * zoomSpeed, Space.Self);
    }
}
