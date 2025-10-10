using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BuildingDestroyer : MonoBehaviour
{
    public Button deleteButton;
    public GameObject confirmPopup;
    public TextMeshProUGUI popupText;
    public TextMeshProUGUI messageText;
    public GridSpawner gridSpawner;  // 인스펙터에서 연결

    public List<GridCube> gridCubes;

    private bool isDeleteMode = false;
    private GridCube selectedGridCube = null;

    void Start()
    {
        if (gridSpawner != null)
        {
            gridCubes = gridSpawner.GetGridCubes();  // 반드시 null 체크 후 호출
            //Debug.Log("GridCubes count: " + gridCubes.Count);
        }
        else
        {
            //Debug.LogError("GridSpawner is not assigned!");
            gridCubes = new List<GridCube>(FindObjectsOfType<GridCube>());
        }

        deleteButton.onClick.AddListener(ToggleDeleteMode);
        confirmPopup.SetActive(false);
        messageText.text = "";

        //Debug.Log("Grid cube count at Start: " + (gridCubes != null ? gridCubes.Count.ToString() : "null"));
    }

    // X 버튼 토글
    public void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode;

        if (isDeleteMode)
        {
            Debug.Log("DeleteMode ON");
            ActivateDeleteMode();
        }
        else
        {
            Debug.Log("DeleteMode OFF");
            DeactivateDeleteMode();
        }

        UpdateDeleteButtonVisual();
    }
    
    // 삭제 모드 활성화 : 연노랑 하이라이트, 빌딩 성장 중지
    public void ActivateDeleteMode()
    {
        messageText.text = "Select Cell with Building.";
        confirmPopup.SetActive(false);
        selectedGridCube = null;
        //Debug.Log($"GridCubes count: {gridCubes.Count}");

        foreach (var cube in gridCubes)
        {
            //Debug.Log($"Cube state: {cube.IsSelected()}");
            // 빨간색 상태면 먼저 선택 해제하고 코루틴 중지
            if (cube.IsSelected() == true)
            {
                cube.SetSelected(false);
            }//cube.StopBuildingCell();

            // buildingCube가 있으면 연노란색 하이라이트, 아니면 기본색
            if (cube.HasBuilding())
                cube.SetHighlight(GridCube.HighlightColor); // 연노란색
            else
                cube.SetHighlight(GridCube.DefaultColor);     // 하얀색 유지
        }
    }

    // 삭제 모드 비활성화 : 하이라이트 복원, 빌딩 성장 재개
    private void DeactivateDeleteMode()
    {
        messageText.text = "";
        confirmPopup.SetActive(false);
        selectedGridCube = null;

        foreach (var cube in gridCubes)
        {
            cube.SetHighlight(GridCube.DefaultColor);
            cube.StartBuildingCell();
        }
    }

    // X 버튼 눌림 여부에 따른 색상 변경
    private void UpdateDeleteButtonVisual()
    {
        ColorBlock colors = deleteButton.colors;
        colors.normalColor = isDeleteMode ? Color.yellow : Color.white;
        deleteButton.colors = colors;
    }

    // 마우스 포인터가 GridCube에 들어왔을 때
    public void OnGridCubePointerEnter(GridCube cube)
    {

        cube.SetHovered(isDeleteMode);
        // if (isDeleteMode && cube.HasBuilding())
        // {
        //     cube.SetHighlight(GridCube.HoverColor);
        // }
    }

    // 마우스 포인터가 GridCube에서 나갔을 때
    public void OnGridCubePointerExit(GridCube cube)
    {
        cube.SetHovered(isDeleteMode);
        // if (isDeleteMode && cube.HasBuilding())
        // {
        //     cube.SetHighlight(GridCube.HighlightColor);
        // }
    }

    // GridCube 클릭 시 호출
    public void OnGridCubeClick(GridCube cube)
    {
        if (isDeleteMode && cube.HasBuilding())
        {
            selectedGridCube = cube;
            popupText.text = "Destroy Building?";
            confirmPopup.SetActive(true);
        }
    }

    // 팝업에서 예 선택
    public void OnConfirmDestroy()
    {
        if (selectedGridCube != null)
        {
            selectedGridCube.RemoveAllBuildings();
            selectedGridCube.SetHighlight(GridCube.DefaultColor);
            selectedGridCube = null;
        }
        DeactivateDeleteMode();
    }

    // 팝업에서 아니오 선택
    public void OnCancelDestroy()
    {
        selectedGridCube = null;
        DeactivateDeleteMode();
    }
}
