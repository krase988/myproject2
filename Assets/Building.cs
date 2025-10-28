using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [Header("공통 Building 정보")]
    public GridCube gridCube; // 이 빌딩이 위치한 그리드
    public float growthMultiplier = 1f; // 성장 배율(도로 등 영향)

    /// <summary>
    /// 성장 배율 등 외부 영향 적용
    /// </summary>
    public virtual void SetGrowthMultiplier(float mult)
    {
        growthMultiplier = mult;
        // 필요시 UI 갱신 등 추가
    }

    /// <summary>
    /// 건물 설치 시 호출(필요시 오버라이드)
    /// </summary>
    public virtual void OnPlaced(GridCube at)
    {
        gridCube = at;
        if (at != null)
        {
            at.occupant = GridCube.OccupantType.Building;
            at.occupantObj = this.gameObject;
        }
    }

    /// <summary>
    /// 건물 제거 시 호출(필요시 오버라이드)
    /// </summary>
    public virtual void OnRemoved(GridCube from)
    {
        if (from != null && from.occupantObj == this.gameObject)
        {
            from.occupant = GridCube.OccupantType.Empty;
            from.occupantObj = null;
        }
        gridCube = null;
    }
}