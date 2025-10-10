using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;       // 이동 속도
    public float rotateSpeed = 100f;    // 회전 속도
    public float zoomSpeed = 10f;       // 줌 속도

    void Update()
    {
        // 키보드 WSAD 이동
        float h = Input.GetAxis("Horizontal");   // A/D or Left/Right
        float v = Input.GetAxis("Vertical");     // W/S or Up/Down
        Vector3 move = new Vector3(h, 0, v);
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

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
