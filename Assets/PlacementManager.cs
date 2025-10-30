using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; }
    public GameObject streetCubePrefab; // Inspector에서 연결
    public int playerMoney = 1000;      // 예시: 플레이어 재화

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 설치 모드 진입
    public void EnterStreetPlacementMode()
    {
        if (playerMoney < 100) // 가격은 StreetCube.price와 맞춰주세요
        {
            Debug.Log("재화가 부족합니다!");
            // 메시지 출력 등
            return;
        }
        // 설치 모드 진입(아래 단계에서 구현)
        Debug.Log("StreetCube 설치 모드 진입!");
        // 3번 단계로 이동
    }

    // 실제 설치 확정(클릭 시 호출)
    public void PlaceStreetCube(GridCube target)
    {
        if (playerMoney < 100) return;
        playerMoney -= 100;
        var obj = Instantiate(streetCubePrefab);
        var street = obj.GetComponent<StreetCube>();
        street.OnPlaced(target);
        // 설치 모드 종료(하이라이트/미리보기 제거 등)
    }
}