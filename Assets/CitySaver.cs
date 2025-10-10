using UnityEngine;
using System.IO;
using System;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CitySaver : MonoBehaviour
{
#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }

    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            SaveCity();
        }
    }
#else
    void OnApplicationQuit()
    {
        SaveCity();
    }
#endif

    public void SaveCity()
    {
        GameObject[] gridCubes = GameObject.FindGameObjectsWithTag("GridCube");

        if(gridCubes.Length == 0)
        {
            Debug.LogWarning("GridCube 오브젝트가 없습니다.");
            return;
        }
        Debug.Log("gridCubes.Length: " + gridCubes.Length);

        // 좌표 최소/최대 초기화
        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minZ = int.MaxValue;
        int maxZ = int.MinValue;

        // 좌표 범위 찾기
        foreach(var cube in gridCubes)
        {
            int x = Mathf.RoundToInt(cube.transform.position.x/1.1f);
            int z = Mathf.RoundToInt(cube.transform.position.z/1.1f);

            if(x < minX) minX = x;
            if(x > maxX) maxX = x;
            if(z < minZ) minZ = z;
            if(z > maxZ) maxZ = z;
            Debug.Log("x: " + x);
            Debug.Log("z: " + z);   
        }

        int width = maxX - minX + 1;
        int height = maxZ - minZ + 1;
        Debug.Log("width: " + width);
        Debug.Log("height: " + height);

        int[,] buildingCounts = new int[height, width];

        // Building 개수 배열에 채우기
        foreach(var cube in gridCubes)
        {
            int x = Mathf.RoundToInt(cube.transform.position.x/1.1f);
            int z = Mathf.RoundToInt(cube.transform.position.z/1.1f);
            
            int localX = x - minX;
            int localZ = z - minZ;

            int buildingCount = cube.transform.childCount;

            buildingCounts[localZ, localX] = buildingCount;
        }

        // CSV 문자열 생성
        StringBuilder sb = new StringBuilder();
        for(int row = 0; row < height; row++)
        {
            string[] rowData = new string[width];
            for(int col = 0; col < width; col++)
            {
                rowData[col] = buildingCounts[row, col].ToString();
            }
            sb.AppendLine(string.Join(",", rowData));
        }

        // 파일 이름 생성 (YYMMDD_hhmmss.city)
        string timestamp = DateTime.Now.ToString("yyMMdd_HHmmss");
        string filename = $"{timestamp}.city";

        // 저장 경로 설정
        string path = Path.Combine(Application.persistentDataPath, filename);

        // 파일 쓰기
        File.WriteAllText(path, sb.ToString());

        Debug.Log($"City data saved to: {path}");
    }
}
