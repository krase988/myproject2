/*
using UnityEngine;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
{
    public GameObject cellPrefab; // 프리팹 연결
    public int rows = 7;
    public int columns = 7;
    public float spacing = 1.1f; // 약간의 간격

    void Start()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 pos = new Vector3(x * spacing, 0, y * spacing);
                Instantiate(cellPrefab, pos, Quaternion.identity, this.transform);
            }
        }
    }
    // GridSpawner.cs 내에 모든 GridCube를 리스트에 담아두었다고 가정
    public List<GridCube> GetGridCubes()
    {
        return gridCubeList;  // 생성된 GridCube 리스트 반환 (예: List<GridCube> gridCubeList)
    }

}
*/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GridSpawner : MonoBehaviour
{
    public GameObject cellPrefab;  // GridCube 프리팹 연결
    public int rows = 7;
    public int columns = 7;
    public float spacing = 1.1f;  // grid 블록 사이 간격
    public BuildingDestroyer buildingDestroyer; // 인스펙터에서 연결
    private List<GridCube> gridCubeList = new List<GridCube>();
    public CountPoints countPoints;
    public CountMoney countMoney;
    public int cubeIndex = 0; // 각 GridCube에 고유 번호 부여용

    // 기존에 그리드 저장 구조가 있다면 그것을 사용하세요.
    // 없으면 이 두 컬렉션을 사용해 초기화(InitializeGridLookup)로 채웁니다.
    public Dictionary<Vector2Int, GridCube> gridLookup = new Dictionary<Vector2Int, GridCube>();
    public List<GridCube> allGridCubes = new List<GridCube>();

    void Start()
    {
        SpawnGrid();
    }

    // 씬에 있는 GridCube(또는 GridSpawner가 생성한 것)를 기반으로 lookup 생성
    public void InitializeGridLookup()
    {
        gridLookup.Clear();
        allGridCubes.Clear();
        var cubes = GetComponentsInChildren<GridCube>();
        foreach (var c in cubes)
        {
            allGridCubes.Add(c);
            gridLookup[c.coord] = c;
        }
    }

    void SpawnGrid()
    {
        int index = 0;
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 pos = new Vector3(x * spacing, 0f, z * spacing);
                GameObject gridObj = Instantiate(cellPrefab, pos, Quaternion.identity, this.transform);
                GridCube gridCube = gridObj.GetComponent<GridCube>();
                if (gridCube != null)
                {
                    gridCube.cubeIndex = index; // 고유 번호 할당
                    index++;
                    gridCubeList.Add(gridCube);

                    // EventTrigger 추가 및 이벤트 연결
                    EventTrigger trigger = gridObj.GetComponent<EventTrigger>();
                    if (trigger == null)
                        trigger = gridObj.AddComponent<EventTrigger>();

                    // PointerEnter
                    var entryEnter = new EventTrigger.Entry();
                    entryEnter.eventID = EventTriggerType.PointerEnter;
                    entryEnter.callback.AddListener((eventData) => {
                        buildingDestroyer.OnGridCubePointerEnter(gridCube);
                    });
                    trigger.triggers.Add(entryEnter);

                    // PointerExit
                    var entryExit = new EventTrigger.Entry();
                    entryExit.eventID = EventTriggerType.PointerExit;
                    entryExit.callback.AddListener((eventData) => {
                        buildingDestroyer.OnGridCubePointerExit(gridCube);
                    });
                    trigger.triggers.Add(entryExit);

                    // PointerClick (선택/삭제 등 필요시)
                    var entryClick = new EventTrigger.Entry();
                    entryClick.eventID = EventTriggerType.PointerClick;
                    entryClick.callback.AddListener((eventData) => {
                        buildingDestroyer.OnGridCubeClick(gridCube);
                    });
                    trigger.triggers.Add(entryClick);

                    // CountPoints 연결
                    gridCube.countPoints = countPoints;
                    // CountMoney 연결
                    gridCube.countMoney = countMoney; 
                }
                else
                {
                    Debug.LogError("GridCube 컴포넌트가 연결된 프리팹이 아닙니다.");
                }
            }
        }
    }

    /// <summary>
    /// 생성된 모든 GridCube 리스트 반환
    /// </summary>
    public List<GridCube> GetGridCubes()
    {
        if (gridCubeList == null)
            gridCubeList = new List<GridCube>();

        // 필요한 경우 리스트 초기화 및 갱신 코드 추가
        return gridCubeList;
    }

    // center 타일 주위 radius(칸 단위)의 이웃 타일을 반환 (center 제외)
    public List<GridCube> GetNeighbors(GridCube center, int radius)
    {
        var result = new List<GridCube>();
        if (center == null) return result;

        Vector2Int c = center.coord;
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dz = -radius; dz <= radius; dz++)
            {
                if (dx == 0 && dz == 0) continue; // 중심 제외 (필요하면 포함하도록 수정)
                Vector2Int key = new Vector2Int(c.x + dx, c.y + dz);
                if (gridLookup.TryGetValue(key, out var neighbor))
                    result.Add(neighbor);
            }
        }
        return result;
    }
    
    // 프로젝트에서 Building 컴포넌트가 존재하면 건물 목록을 반환 (RoadManager에서 사용)
    public List<Building> GetAllBuildings()
    {
        var list = new List<Building>();
        foreach (var gc in allGridCubes)
        {
            if (gc.occupant == GridCube.OccupantType.Building && gc.occupantObj != null)
            {
                var b = gc.occupantObj.GetComponent<Building>();
                if (b != null) list.Add(b);
            }
        }
        return list;
    }
}
