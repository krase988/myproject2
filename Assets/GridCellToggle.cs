// using UnityEngine;
// using System.Collections;


// public class GridCellToggle : MonoBehaviour
// {
//     public GameObject buildingPrefab;        // Building 큐브 프리팹 연결
//     private Renderer rend;
//     private bool isSelected = false;
//     private bool isHovered = false;
//     private Coroutine buildCoroutine;

//     private float buildingHeight = 0.2f;    // Building 큐브 높이
//     private float baseY = 1f;              // GridCube 높이 기준 Y값

//     void Awake()
//     {
//         rend = GetComponent<Renderer>();
//         UpdateColor();
//     }

//     // 외부에서 선택 상태 변경 가능하게
//     public void SetSelected(bool selected)
//     {
//         isSelected = selected;
//         UpdateColor();

//         // 선택 상태에 따라 빌딩 코루틴 시작/중지
//         if (isSelected)
//             StartBuilding();
//         else
//             StopBuilding();
//     }

//     void OnMouseEnter()
//     {
//         isHovered = true;
//         UpdateColor();
//     }

//     void OnMouseExit()
//     {
//         isHovered = false;
//         UpdateColor();
//     }

//     void OnMouseDown()
//     {
//         isSelected = !isSelected;  // 클릭 시 선택 상태 토글
//         UpdateColor();
//     }

//     void UpdateColor()
//     {
//         if (isSelected)
//         {
//             rend.material.color = Color.red;  // 선택된 상태는 빨간색 유지
//             //StartBuilding();
//         }
//         else if (isHovered)
//         {
//             rend.material.color = Color.green;  // 마우스 오버 시 초록색
//         }
//         else
//         {
//             rend.material.color = Color.white;  // 기본 하얀색
//             //StopBuilding();
//         }
//     }

//     public static readonly Color DefaultColor = Color.white;
//     public static readonly Color BuildingColor = Color.red;
//     public static readonly Color HighlightColor = new Color(1f, 0.92f, 0.4f); // 연노란색
//     public static readonly Color HoverColor = new Color(1f, 0.82f, 0f); // 진노란색

//     public void SetHighlight(Color color)
//     {
//         if (rend == null) rend = GetComponent<Renderer>();
//         rend.material.color = color;
//         foreach (Transform child in transform)
//         {
//             var childRenderer = child.GetComponent<Renderer>();
//             if (childRenderer != null)
//                 childRenderer.material.color = color;
//         }
//     }


//        void StartBuilding()
//     {
//         if (buildCoroutine == null){
//             buildCoroutine = StartCoroutine(BuildingRoutine());
//             Debug.Log("StartBuilding");
//         }
            
//     }

//     void StopBuilding()
//     {
//         if (buildCoroutine != null)
//         {
//             StopCoroutine(buildCoroutine);
//             buildCoroutine = null;
//             Debug.Log("StopBuilding");
//         }
//     }

//     IEnumerator BuildingRoutine()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(1f);
//             BuildOne();
//             Debug.Log("BuildOne");
//         }
//     }

//     void BuildOne()
//     {
//         int count = transform.childCount; // 현재 건물 층 수
//         Debug.Log("BuildOne: " + count);
//         Vector3 pos = new Vector3(0f, baseY + buildingHeight * count, 0f);
//         GameObject newBuilding = Instantiate(buildingPrefab, transform);
//         newBuilding.transform.localPosition = pos;
//         Debug.Log("BuildOne: " + pos);
//         newBuilding.transform.localScale = new Vector3(0.9f, 0.2f, 0.9f);
//         Renderer rend = newBuilding.GetComponent<Renderer>();
//         if(rend != null)
//             rend.material.color = Color.red;
//     }
// }

using UnityEngine;

public class GridCellToggle : MonoBehaviour
{
    private GridCube gridCube;
    //private bool isSelected = false;
    //private bool isHovered = false;

    void Awake()
    {
        gridCube = GetComponent<GridCube>();
        //UpdateVisual();
    }

    // public void SetSelected(bool selected)
    // {
    //     // isSelected = selected;
    //     // if (isSelected)
    //     // {
    //     //     gridCube.StartBuildingCell();
    //     //     gridCube.SetHighlight(GridCube.BuildingColor);
    //     //     Debug.Log("StartBuilding on GridCellToggle");
    //     // }
    //     // else
    //     // {
    //     //     gridCube.StopBuildingCell();
    //     //     gridCube.SetHighlight(GridCube.DefaultColor);
    //     //     Debug.Log("StopBuilding on GridCellToggle");
    //     // }
    //      UpdateVisual();

    //     gridCube.SetSelected(selected)
    // }

    // public bool IsSelected()
    // {
    //     return gridCube.IsSelected();
    // }

    // public void SetHighlight(Color color)
    // {
    //     if (!IsSelected()) // 선택됐을 때는 빨강 유지
    //     {
    //         gridCube.SetHighlight(color);
    //     }
    // }

    // public void RemoveAllBuildings()
    // {
    //     gridCube.RemoveAllBuildings();
    // }

    // private void UpdateVisual()
    // {
    //     if (IsSelected())
    //     {
    //         gridCube.SetHighlight(GridCube.BuildingColor);
    //     }
    //     else if (isHovered)
    //     {
    //         gridCube.SetHighlight(Color.green); // 마우스 오버는 초록 등으로임의 적용
    //     }
    //     else
    //     {
    //         gridCube.SetHighlight(GridCube.DefaultColor);
    //     }
    // }

    void OnMouseEnter()
    {
        //isHovered = true;
        //UpdateVisual();
        gridCube.SetHovered(true, false, gridCube.HasBuilding());
    }

    void OnMouseExit()
    {
        //isHovered = false;
        //UpdateVisual();
        gridCube.SetHovered(false, false, gridCube.HasBuilding());
    }

    void OnMouseDown()
    {
        //SetSelected(!IsSelected());
        gridCube.SetSelected(!gridCube.IsSelected());
    }

    // public bool HasBuilding() => IsSelected();
}

