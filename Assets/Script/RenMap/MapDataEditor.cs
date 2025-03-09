// MapDataEditor.cs
#if UNITY_EDITOR
using Assets.Helper;
using UnityEditor;
using UnityEngine;

public class MapDataEditor : EditorWindow
{
    private MapData mapData = new MapData();

    [MenuItem("Window/Map Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<MapDataEditor>("Map Data Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Map Data Configuration", EditorStyles.boldLabel);

        mapData.level = EditorGUILayout.IntField("Level", mapData.level);
        mapData.width = EditorGUILayout.IntField("Width", mapData.width);
        mapData.height = EditorGUILayout.IntField("Height", mapData.height);
        mapData.steps = EditorGUILayout.IntField("Steps", mapData.steps);
        mapData.trapCount = EditorGUILayout.IntField("Trap Count", mapData.trapCount);
        mapData.chestCount = EditorGUILayout.IntField("Chest Count", mapData.chestCount);
        mapData.levelEnemy = EditorGUILayout.IntField("Level Enemy", mapData.levelEnemy);

        // Setup cho mảng enemyCounts
        int currentArraySize = (mapData.enemyCounts != null) ? mapData.enemyCounts.Length : 0;
        int newSize = EditorGUILayout.IntField("Enemy Array Size", currentArraySize);

        // Nếu kích thước thay đổi, cập nhật mảng
        if (newSize != currentArraySize)
        {
            int[] newEnemyCounts = new int[newSize];
            for (int i = 0; i < Mathf.Min(newSize, currentArraySize); i++)
            {
                newEnemyCounts[i] = mapData.enemyCounts[i];
            }
            mapData.enemyCounts = newEnemyCounts;
        }

        // Hiển thị các trường nhập cho từng phần tử của enemyCounts
        if (mapData.enemyCounts != null)
        {
            for (int i = 0; i < mapData.enemyCounts.Length; i++)
            {
                mapData.enemyCounts[i] = EditorGUILayout.IntField("Enemy Count [" + i + "]", mapData.enemyCounts[i]);
            }
        }

        if (GUILayout.Button("Save Map Data"))
        {
            SaveMapData();
        }
    }


    void SaveMapData()
    {
        string path = EditorUtility.SaveFilePanel("Save Map Data", "", "map_data", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string jsonData = JsonUtility.ToJson(mapData, true);
            System.IO.File.WriteAllText(path, jsonData);
            Debug.Log("Map data saved to: " + path);
        }
    }
}
#endif