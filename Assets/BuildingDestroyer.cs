using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BuildingDestroyer : MonoBehaviour
{
    public Button deleteButton;
    public GameObject confirmPopup;
    public TextMeshProUGUI popupText;
    //public TextMeshProUGUI messageText;
    public GridSpawner gridSpawner;  // 인스펙터에서 연결

    public List<GridCube> gridCubes;

    private bool isDeleteMode = false;
    private GridCube selectedGridCube = null;
    private List<GridCube> prevSelectedCubes = new List<GridCube>(); // 삭제모드 진입 전 선택 상태 저장
    public CountGrowingBuilding countGrowingBuilding; // 인스펙터에서 연결
    public MiniPopup miniPopup; // 인스펙터에서 연결
    public Button useMoneyButton; // 인스펙터에서 연결

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
        Debug.Log("GridCubes count: " + gridCubes.Count);
        deleteButton.onClick.AddListener(ToggleDeleteMode);
        confirmPopup.SetActive(false);
        //messageText.text = "";
        MessageManager.Instance.ShowMessage("");
        useMoneyButton.onClick.AddListener(UseMoneyButtonClicked);

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
        MessageManager.Instance.ShowMessage("Select Cell with Building.");
        confirmPopup.SetActive(false);
        selectedGridCube = null;
        //Debug.Log($"GridCubes count: {gridCubes.Count}");

        prevSelectedCubes.Clear();
        //Debug.Log("ActivateDeleteMode foreach: " + gridCubes.Count);
        foreach (var cube in gridCubes)
        {
            //Debug.Log($"Cube state: {cube.IsSelected()}");
            // 빨간색 상태면 먼저 선택 해제하고 코루틴 중지
            if (cube.IsSelected() == true)
            {
                prevSelectedCubes.Add(cube); // 선택 상태 저장
                cube.SetSelected(false, isDeleteMode, cube.HasBuilding()); // 선택 해제 및 성장 멈춤
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
        isDeleteMode = false; // 삭제모드 해제
        MessageManager.Instance.ShowMessage("");
        confirmPopup.SetActive(false);
        selectedGridCube = null;

        foreach (var cube in gridCubes)
        {
            cube.SetHighlight(GridCube.DefaultColor);
            // cube.StartBuildingCell();
            cube.SetSelected(false, isDeleteMode, cube.HasBuilding()); // 모든 성장 멈춤

        }
            // 삭제모드 진입 전 선택(빨간색)이었던 큐브만 다시 선택 및 성장 재개
        foreach (var cube in prevSelectedCubes)
        {
            Debug.Log("Restoring selection on: " + cube.name);
            Debug.Log("selectedGridCube: " + (selectedGridCube != null ? selectedGridCube.name : "null"));
            if (cube != selectedGridCube && cube.HasBuilding())
                cube.SetSelected(true);
        }
        prevSelectedCubes.Clear();
        UpdateDeleteButtonVisual(); // 버튼 색상도 원래대로
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

        // cube.SetHovered(isDeleteMode);
        // if (isDeleteMode && cube.HasBuilding())
        // {
        //     cube.SetHighlight(GridCube.HoverColor);
        // }
        bool hasBuilding = cube.HasBuilding();
        //Debug.Log("OnGridCubePointerEnter: " + cube.name + ", isDeleteMode: " + isDeleteMode + ", hasBuilding: " + hasBuilding);
        cube.SetHovered(true, isDeleteMode, hasBuilding);

        // 팝업 표시
        string info = $"No: {cube.cubeIndex}\nHeight: {cube.GetBuildingCount()}";
        miniPopup.Show(cube);
    }

    // 마우스 포인터가 GridCube에서 나갔을 때
    public void OnGridCubePointerExit(GridCube cube)
    {
        //cube.SetHovered(isDeleteMode);
        // if (isDeleteMode && cube.HasBuilding())
        // {
        //     cube.SetHighlight(GridCube.HighlightColor);
        // }
        bool hasBuilding = cube.HasBuilding();
        //Debug.Log("OnGridCubePointerExit: " + cube.name + ", isDeleteMode: " + isDeleteMode + ", hasBuilding: " + hasBuilding);
        cube.SetHovered(false, isDeleteMode, hasBuilding);
        // 팝업 숨김
        miniPopup.Hide();
    }

    // GridCube 클릭 시 호출
    public void OnGridCubeClick(GridCube cube)
    {
        if (isDeleteMode && cube.HasBuilding())
        {
            selectedGridCube = cube;
            MessageManager.Instance.ShowMessage("");
            popupText.text = "Destroy Building?";
            confirmPopup.SetActive(true);
        }
        else if (!isDeleteMode)
        {
            // 기존 전체 선택 해제 코드를 제거
            // 클릭한 GridCube만 토글
            // bool wasSelected = cube.IsSelected();
            // cube.SetSelected(!wasSelected, isDeleteMode, cube.HasBuilding());
            if (!cube.IsSelected())
            {
                if (countGrowingBuilding.CanSelect())
                {
                    cube.SetSelected(true, isDeleteMode, cube.HasBuilding());
                    countGrowingBuilding.OnSelect();
                }
            }
            else
            {
                cube.SetSelected(false, isDeleteMode, cube.HasBuilding());
                countGrowingBuilding.OnDeselect();
            }
        }
    }

    // 팝업에서 예 선택
    public void OnConfirmDestroy()
    {
        if (selectedGridCube != null)
        {
            // log for selectedGridCube.isSelected()
            //Debug.Log("Confirm destroy on " + selectedGridCube.name + ", isSelected: " + selectedGridCube.IsSelected() + ", hasBuilding: " + selectedGridCube.HasBuilding());
            // 만약 삭제된 GridCube가 선택(빨간색)이었다면 카운트 복원
            if (prevSelectedCubes.Contains(selectedGridCube))
            {
                countGrowingBuilding.OnDeselect();
                prevSelectedCubes.Remove(selectedGridCube); // ★ 이 줄 추가!
                selectedGridCube.SetSelected(false, isDeleteMode, selectedGridCube.HasBuilding());
            }
            selectedGridCube.RemoveAllBuildings();
            selectedGridCube.SetHighlight(GridCube.DefaultColor);
            Debug.Log("Building destroyed on " + selectedGridCube.name);
        }
        DeactivateDeleteMode();
        selectedGridCube = null; // DeactivateDeleteMode 이후에 null로 만듭니다.
    }

    // 팝업에서 아니오 선택
    public void OnCancelDestroy()
    {
        selectedGridCube = null;
        DeactivateDeleteMode();
    }

    public void UseMoneyButtonClicked()
    {
        // 여기에 재화 사용 로직 구현
        Debug.Log("Clicked Use Money Button");
        // 예시: 특정 금액 차감, 건물 생성 등
    }
}
