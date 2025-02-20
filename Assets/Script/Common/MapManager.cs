// MapManager.cs
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("Map Configuration")]
    public MapData currentMapData; // Dữ liệu map hiện tại

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Load dữ liệu từ JSON theo level
    public void LoadMapData(int level)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"MapData/map_level_{level}");
        if (jsonFile != null)
        {
            currentMapData = JsonUtility.FromJson<MapData>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Using default map data!");
            currentMapData = GenerateDefaultData(level);
        }
    }

    // Tạo dữ liệu mặc định nếu không tìm thấy file
    private MapData GenerateDefaultData(int level)
    {
        return new MapData
        {
            level = level,
            width = 30,
            height = 30,
            steps = 1000,
            trapCount = 10,
            enemyCounts = new int[] { 5, 3, 2 }
        };
    }
}