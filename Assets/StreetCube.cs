using UnityEngine;

public class StreetCube : Building
{
    [Header("도로 설정")]
    public int price = 100;
    public float growthFactorPerStreet = 1.03228f; // 각 스트리트당 성장 배율
    public int maxStackPerBuilding = 6;

    [Header("비주얼")]
    public Vector3 placedLocalScale = new Vector3(0.9f, 0.2f, 0.9f);
    public Color roadColor = new Color(0.18f, 0.18f, 0.18f); // 어두운 회색(도로색)
    public bool highwayBoosted = false; // 하이웨이 부스트 여부

    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
            rend = GetComponentInChildren<Renderer>();
    }

    public override void OnPlaced(GridCube at)
    {
        base.OnPlaced(at);
        transform.SetParent(at.transform, worldPositionStays: false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = placedLocalScale;

        ApplyVisual();

        // RoadManager에 등록
        if (RoadManager.Instance != null)
            RoadManager.Instance.RegisterStreet(this, at);
    }

    public override void OnRemoved(GridCube from)
    {
        base.OnRemoved(from);

        // RoadManager에서 제거
        if (RoadManager.Instance != null)
            RoadManager.Instance.UnregisterStreet(this, from);
    }

    // 비주얼 업데이트(도로 색상 / 하이웨이 부스트 시 색상 변경)
    public void ApplyVisual()
    {
        if (rend != null)
        {
            Color c = roadColor;
            if (highwayBoosted)
            {
                // 하이웨이 부스트 시 색을 살짝 밝게 표시
                c = Color.Lerp(roadColor, Color.yellow, 0.18f);
            }
            rend.material.color = c;
        }
    }

    // 외부에서 하이웨이 부스트 플래그를 세팅하고 시각 갱신
    public void SetHighwayBoost(bool boosted)
    {
        highwayBoosted = boosted;
        ApplyVisual();
    }
}