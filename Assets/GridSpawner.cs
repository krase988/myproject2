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

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 pos = new Vector3(x * spacing, 0f, z * spacing);
                GameObject gridObj = Instantiate(cellPrefab, pos, Quaternion.identity, this.transform);
                GridCube gridCube = gridObj.GetComponent<GridCube>();
                if (gridCube != null)
                {
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
}
