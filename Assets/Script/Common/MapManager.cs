// MapManager.cs
using Assets.Helper;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Assets.Helper.MapSaveData;

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

	public void LoadScene(string nextScene)
	{
		PlayerPrefs.SetString("NextScene", nextScene);
		SceneManager.LoadScene("LoadScene");
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
            enemyCounts = new int[] { 10, 11, 10 }
        };
    }

	public void SaveMapState(int width, int height, int[,] maze, GameObject playerInstance, Vector2Int exitPosition, int currentMap)
	{
 
		MapSaveData saveData = new MapSaveData
		{
			width = width,
			height = height,
			maze = ConvertArray.Convert2DTo1D(maze),
			playerPosition = playerInstance.transform.position,
			exitPosition = exitPosition,
			enemies = new List<EnemySaveData>(),
            traps = new List<TrapSaveData>()
		};

		var enemiesCanShoot = GameObject.FindGameObjectsWithTag(EnemyType.EnemyCanShoot);
		foreach (var enemy in enemiesCanShoot)
		{
			saveData.enemies.Add(new MapSaveData.EnemySaveData
			{
				enemyType = 0,
				position = enemy.transform.position
			});
		}

		var enemiesCanNotShoot = GameObject.FindGameObjectsWithTag(EnemyType.EnemyCanNotShoot);
		foreach (var enemy in enemiesCanNotShoot)
		{
			saveData.enemies.Add(new MapSaveData.EnemySaveData
			{
				enemyType = 1,
				position = enemy.transform.position
			});
		}

		var traps = GameObject.FindGameObjectsWithTag("Trap"); 
		foreach (var trap in traps)
		{
			saveData.traps.Add(new MapSaveData.TrapSaveData
			{
				position = trap.transform.position,
				isActive = true
			});
		}
		string json = JsonUtility.ToJson(saveData);
		string path = Path.Combine(Application.persistentDataPath, $"map_{currentMap}_save.json");
		File.WriteAllText(path, json);
	}
}