using UnityEngine;
using UnityEditor;

public class VoxelRoadGenerator : EditorWindow
{
    // --- 여기에 필드로 선언 ---
    Material bodyMat;
    Material centerMat;
    Material edgeMat;

    [MenuItem("Tools/Generate Voxel Road Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<VoxelRoadGenerator>("Voxel Road Generator");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Generate Voxel Road Prefabs"))
        {
            GenerateAll();
        }
    }

    void GenerateAll()
    {
        string folder = "Assets/VoxelRoadPrefabs";
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets", "VoxelRoadPrefabs");

        // --- 머티리얼 로드를 여기서 한 번만 ---
        bodyMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/VoxelRoadPrefabs/Materials/RoadBody.mat");
        centerMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/VoxelRoadPrefabs/Materials/RoadCenter.mat");
        edgeMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/VoxelRoadPrefabs/Materials/RoadEdge.mat");


        CreateVerticalRoad(folder);
        CreateHorizontalRoad(folder);
        CreateCrossRoad(folder);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("완료", "3종 도로 프리팹이 생성되었습니다!", "OK");
    }

    void CreateVerticalRoad(string folder)
    {
        GameObject go = new GameObject("Road_Vertical");
        go.transform.position = Vector3.zero;

        // 머티리얼 로드
        // Material bodyMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/VoxelRoadPrefabs/Materials/RoadBody.mat");
        // Material centerMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/VoxelRoadPrefabs/Materials/RoadCenter.mat");
        // Material edgeMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/VoxelRoadPrefabs/Materials/RoadEdge.mat");


        // 본체
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.SetParent(go.transform, false);
        body.transform.localScale = new Vector3(0.3f, 0.1f, 0.9f);
        //body.GetComponent<Renderer>().sharedMaterial.color = new Color(0.18f, 0.18f, 0.18f);
        body.GetComponent<Renderer>().sharedMaterial = bodyMat;

        // 중앙선(주황)
        GameObject center = GameObject.CreatePrimitive(PrimitiveType.Cube);
        center.transform.SetParent(go.transform, false);
        center.transform.localScale = new Vector3(0.05f, 0.11f, 0.8f);
        center.transform.localPosition = new Vector3(0, 0.06f, 0);
        center.GetComponent<Renderer>().sharedMaterial = centerMat;

        // 좌/우 흰색 실선
        GameObject left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.transform.SetParent(go.transform, false);
        left.transform.localScale = new Vector3(0.02f, 0.11f, 0.8f);
        left.transform.localPosition = new Vector3(-0.13f, 0.06f, 0);
        //left.GetComponent<Renderer>().sharedMaterial.color = Color.white;
        left.GetComponent<Renderer>().sharedMaterial = edgeMat;
        
        GameObject right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.transform.SetParent(go.transform, false);
        right.transform.localScale = new Vector3(0.02f, 0.11f, 0.8f);
        right.transform.localPosition = new Vector3(0.13f, 0.06f, 0);
        right.GetComponent<Renderer>().sharedMaterial = edgeMat;

        // 프리팹 저장
        SaveAsPrefab(go, folder + "/Road_Vertical.prefab");
        DestroyImmediate(go);
    }

    void CreateHorizontalRoad(string folder)
    {
        GameObject go = new GameObject("Road_Horizontal");
        go.transform.position = Vector3.zero;

        // 본체
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.SetParent(go.transform, false);
        body.transform.localScale = new Vector3(0.9f, 0.1f, 0.3f);
        body.GetComponent<Renderer>().sharedMaterial = bodyMat;

        // 중앙선(주황)
        GameObject center = GameObject.CreatePrimitive(PrimitiveType.Cube);
        center.transform.SetParent(go.transform, false);
        center.transform.localScale = new Vector3(0.8f, 0.11f, 0.05f);
        center.transform.localPosition = new Vector3(0, 0.06f, 0);
        center.GetComponent<Renderer>().sharedMaterial = centerMat;

        // 위/아래 흰색 실선
        GameObject top = GameObject.CreatePrimitive(PrimitiveType.Cube);
        top.transform.SetParent(go.transform, false);
        top.transform.localScale = new Vector3(0.8f, 0.11f, 0.02f);
        top.transform.localPosition = new Vector3(0, 0.06f, 0.13f);
        top.GetComponent<Renderer>().sharedMaterial = edgeMat;

        GameObject bottom = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bottom.transform.SetParent(go.transform, false);
        bottom.transform.localScale = new Vector3(0.8f, 0.11f, 0.02f);
        bottom.transform.localPosition = new Vector3(0, 0.06f, -0.13f);
        bottom.GetComponent<Renderer>().sharedMaterial = edgeMat;

        // 프리팹 저장
        SaveAsPrefab(go, folder + "/Road_Horizontal.prefab");
        DestroyImmediate(go);
    }

    void CreateCrossRoad(string folder)
    {
        GameObject go = new GameObject("Road_Cross");
        go.transform.position = Vector3.zero;

        // 본체
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.SetParent(go.transform, false);
        body.transform.localScale = new Vector3(0.9f, 0.1f, 0.9f);
        body.GetComponent<Renderer>().sharedMaterial = bodyMat;

        // 세로 중앙선(주황)
        GameObject centerV = GameObject.CreatePrimitive(PrimitiveType.Cube);
        centerV.transform.SetParent(go.transform, false);
        centerV.transform.localScale = new Vector3(0.05f, 0.11f, 0.8f);
        centerV.transform.localPosition = new Vector3(0, 0.06f, 0);
        centerV.GetComponent<Renderer>().sharedMaterial = centerMat;

        // 가로 중앙선(주황)
        GameObject centerH = GameObject.CreatePrimitive(PrimitiveType.Cube);
        centerH.transform.SetParent(go.transform, false);
        centerH.transform.localScale = new Vector3(0.8f, 0.11f, 0.05f);
        centerH.transform.localPosition = new Vector3(0, 0.06f, 0);
        centerH.GetComponent<Renderer>().sharedMaterial = centerMat;

        // 네 방향 흰색 실선
        float edge = 0.13f;
        for (int i = 0; i < 4; i++)
        {
            GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
            line.transform.SetParent(go.transform, false);
            if (i < 2)
            {
                // 위/아래
                line.transform.localScale = new Vector3(0.8f, 0.11f, 0.02f);
                line.transform.localPosition = new Vector3(0, 0.06f, (i == 0 ? edge : -edge));
            }
            else
            {
                // 좌/우
                line.transform.localScale = new Vector3(0.02f, 0.11f, 0.8f);
                line.transform.localPosition = new Vector3((i == 2 ? edge : -edge), 0.06f, 0);
            }
            line.GetComponent<Renderer>().sharedMaterial = edgeMat;
        }

        // 프리팹 저장
        SaveAsPrefab(go, folder + "/Road_Cross.prefab");
        DestroyImmediate(go);
    }

    void SaveAsPrefab(GameObject go, string path)
    {
        PrefabUtility.SaveAsPrefabAsset(go, path);
    }
}