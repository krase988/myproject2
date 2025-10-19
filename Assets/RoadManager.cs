using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    public static RoadManager Instance { get; private set; }

    // grid coord -> GridCube 제공(GridSpawner에서 세팅해야 함)
    public GridSpawner gridSpawner;

    // 위치 기반 관리
    HashSet<GridCube> streetCubes = new HashSet<GridCube>();
    HashSet<GridCube> highwayCubes = new HashSet<GridCube>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterStreet(StreetCube street, GridCube at)
    {
        streetCubes.Add(at);
        at.occupant = GridCube.OccupantType.Street;
        at.occupantObj = street.gameObject;
        RecalculateAround(at);
    }

    public void UnregisterStreet(StreetCube street, GridCube from)
    {
        streetCubes.Remove(from);
        from.occupant = GridCube.OccupantType.Empty;
        from.occupantObj = null;
        RecalculateAround(from);
    }

    public void RegisterHighway(HighwayCube highway, GridCube at)
    {
        highwayCubes.Add(at);
        at.occupant = GridCube.OccupantType.Highway;
        at.occupantObj = highway.gameObject;
        RecalculateAllHighwayEffects();
    }

    public void UnregisterHighway(HighwayCube highway, GridCube from)
    {
        highwayCubes.Remove(from);
        from.occupant = GridCube.OccupantType.Empty;
        from.occupantObj = null;
        RecalculateAllHighwayEffects();
    }

    // 도로가 놓인 근처 건물들만 재계산
    void RecalculateAround(GridCube tile)
    {
        var neighbours = gridSpawner.GetNeighbors(tile, 1); // 8방향 1칸
        foreach (var g in neighbours)
            UpdateBuildingInfluence(g);
        UpdateBuildingInfluence(tile);
        // 하이웨이 연쇄 영향도 재계산 필요
        RecalculateAllHighwayEffects();
    }

    void RecalculateAllHighwayEffects()
    {
        // 1) highway 체인 찾고 길이>=5인 체인 탐색
        // 2) 각 유효한 체인에서 BFS로 연결된 Street을 depth 10까지 찾아 'boost' 마킹
        // 3) 최종적으로 모든 빌딩에 대해 street 영향(스택, cap 6) + highway boost 적용하여 업데이트
        // (간단 구현: 모든 buildings에 대해 전체 street list를 검사 -> 성능 이슈시 지역화)
        ApplyInfluencesToAllBuildings();
    }

    void ApplyInfluencesToAllBuildings()
    {
        var allBuildings = gridSpawner.GetAllBuildings();
        foreach (var b in allBuildings)
        {
            // 1) find nearby street tiles within 1 tile (8-dir)
            var nearStreets = gridSpawner.GetNeighbors(b.gridCube, 1);
            List<StreetCube> affecting = new List<StreetCube>();
            foreach (var t in nearStreets)
            {
                if (t.occupant == GridCube.OccupantType.Street && t.occupantObj != null)
                    affecting.Add(t.occupantObj.GetComponent<StreetCube>());
            }

            // cap to maxStackPerBuilding (각 StreetCube의 규칙에서 동일한 값 사용)
            int cap = 6;
            if (affecting.Count > cap)
                affecting = affecting.GetRange(0, cap);

            // base multiplier
            float mult = 1f;
            foreach (var s in affecting)
                mult *= s.growthFactorPerStreet;

            // TODO: highway boost 적용(현재 예시에선 단순히 체크 플래그를 사용)
            if (IsBoostedByHighway(b.gridCube))
                mult *= 2f; // 스트리트의 효과가 2배가 됨 (요구사항에 따라 구현 조정)

            b.SetGrowthMultiplier(mult);
        }
    }

    bool IsBoostedByHighway(GridCube buildingTile)
    {
        // 실제 요구사항은 "HighwayChain이 있고 그 체인과 연결된 Street에서 거리 기준"인데
        // 간단 구현: 주변의 Street 중 하나라도 highway-boost 마킹이 있으면 true
        var near = gridSpawner.GetNeighbors(buildingTile, 1);
        foreach (var g in near)
        {
            if (g.occupant == GridCube.OccupantType.Street && g.occupantObj != null)
            {
                var street = g.occupantObj.GetComponent<StreetCube>();
                // 여기서 street가 highway에 의해 boost되었는지 확인하는 로직 필요
                // 예: street.HasHighwayBoost flag (RoadManager가 BFS로 세팅)
            }
        }
        return false;
    }
}