using UnityEngine;
using System.Collections;

public class GridCube : MonoBehaviour
{
    private bool isSelected = false;
    private bool isHovered = false;
    private bool isDeleteMode = false;

    public GameObject buildingPrefab;        // Building 큐브 프리팹 연결
    private Renderer rend;
    //private bool isSelected = false;         // 빨간색 토글 상태
    private Coroutine buildCoroutine;

    private float buildingHeight = 0.2f;    // Building 큐브 높이
    private float baseY = 0.1f;              // GridCube 높이 기준 Y값

    public static readonly Color DefaultColor = Color.white;
    public static readonly Color BuildingColor = Color.red;
    public static readonly Color HighlightColor = new Color(1f, 0.92f, 0.4f); // 연노란색
    public static readonly Color HoverColorDeleteMode = new Color(1f, 0.82f, 0f); // 진노란색
    public static readonly Color HoverColorDefault = Color.green;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        //SetColor(DefaultColor);
        UpdateVisual(false);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        Debug.Log($"SetSelected called on {gameObject.name}");

        if (isSelected)
        {
            StartBuildingCell();
            //SetColor(BuildingColor);
            
        }
        else
        {
            StopBuildingCell();
            //SetColor(DefaultColor);
        }
        UpdateVisual(false);
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void SetDeleteMode(bool isDM)
    {
        isDeleteMode = isDM;
    }

    public void SetHovered(bool hovered)
    {
        isHovered = hovered;
        UpdateVisual(isDeleteMode);
    }

    private void UpdateVisual(bool isDeleteMode)
    {
        // if (isSelected)
        //     SetColor(BuildingColor);
        // else if (isHovered)
        //     SetColor(SelectColor);
        // else
        //     SetColor(DefaultColor);
        if (isSelected)
        {
            if(isDeleteMode)
                SetColor(HighlightColor);
            else
                SetColor(BuildingColor);
        }
        else if (isHovered)
        {
            if (isDeleteMode)
                SetColor(HoverColorDeleteMode);
            else
                SetColor(HoverColorDefault);
        }
        else
        {
            SetColor(DefaultColor);
        }
    }
    
    void SetColor(Color color)
    {
        if (rend == null) return;
        rend.material.color = color;
        // 자식 building 큐브들도 색상 변경
        foreach (Transform child in transform)
        {
            Renderer childRend = child.GetComponent<Renderer>();
            if (childRend != null)
                childRend.material.color = color;
        }
    }

    public void StartBuildingCell()
    {
        Debug.Log($"StartBuildingCell called on {gameObject.name}");
        StartBuilding();
    }

    void StartBuilding()
    {
        if (buildCoroutine == null)
            buildCoroutine = StartCoroutine(BuildingRoutine());
    }

    public void StopBuildingCell()
    {
        Debug.Log($"StopBuildingCell called on {gameObject.name}");
        StopBuilding();
    }
    
    void StopBuilding()
    {
        if (buildCoroutine != null)
        {
            Debug.Log($"Stopping coroutine on {gameObject.name}");
            StopCoroutine(buildCoroutine);
            buildCoroutine = null;
        }
    }

    IEnumerator BuildingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            BuildOne();
            //Debug.Log("BuildOne");
        }
    }

    void BuildOne()
    {
        int count = transform.childCount; // 현재 건물 층 수
        //Debug.Log("BuildOne: " + count);
        Vector3 pos = new Vector3(0f, baseY + buildingHeight * count, 0f);
        GameObject newBuilding = Instantiate(buildingPrefab, transform);
        newBuilding.transform.localPosition = pos;
        //Debug.Log("BuildOne: " + pos);
        newBuilding.transform.localScale = new Vector3(0.9f, 0.2f, 0.9f);
        Renderer rend = newBuilding.GetComponent<Renderer>();
        if(rend != null)
            rend.material.color = Color.red;
    }

    public bool HasBuilding()
    {
        // buildingPrefab으로 생성된 큐브들이 자식 오브젝트로 존재하면 true 반환 (0층 이상이면)
        return transform.childCount > 0;
    }



    public void SetHighlight(Color color)
    {
        // if (rend == null) rend = GetComponent<Renderer>();
        // rend.material.color = color;
        // foreach (Transform child in transform)
        // {
        //     var childRenderer = child.GetComponent<Renderer>();
        //     if (childRenderer != null)
        //         childRenderer.material.color = color;
        // }
        SetColor(color);
    }

    public void HighlightBuildings(bool highlight, Color color)
    {
        SetColor(color);
    }

    public void RemoveAllBuildings()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

}
