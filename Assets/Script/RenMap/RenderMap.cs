using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;
using Assets.Helper;
using System.IO;
using System.Collections.Generic;

public class RenderMap : MonoBehaviour
{
    public int width; // Chiều rộng mê cung
    public int height; // Chiều cao mê cung
    public Tilemap tilemap; // Tilemap để render mê cung
    public Tilemap tilePathMap; // Tilemap để render đường đi
    public TileBase wallTile; // Tile cho tường
    public TileBase pathTile; // Tile cho đường đi
    public GameObject trapPrefab; // Prefab cho bẫy (thay thế trapTile)
    public GameObject exitPrefab; // Prefab cho cổng ra (thay thế exitTile)
    public int trapCount; // Số lượng bẫy
    public GameObject[] playerPrefabs; // Mảng chứa các nhân vật có thể chọn
    private int selectedCharacterIndex; // Lưu index nhân vật đã chọn
    public CinemachineVirtualCamera virtualCamera; // Tham chiếu đến Cinemachine Virtual Camera
    public GameObject mapBoundsPrefab; // Prefab chứa collider giới hạn map
    public GameObject chestPrefab; // Prefab cho rương
    public int chestCount = 4;
    public int levelEnemy = 0;
    public GameObject[] enemyPrefabs;

    private int[,] maze;
    private Vector2Int exitPosition; // Vị trí cổng ra
    private GameObject playerInstance; // Tham chiếu đến nhân vật sau khi được tạo ra
    private int selectSkin;

    [Header("Pathfinding")]
    public AStarGridGenerator aStarPrefab;
    private AStarGridGenerator activeAStarGrid;

    private int currentLevel;

    void Start()
    {
        selectSkin = PlayerPrefs.GetInt(PlayerPrefsHelper.SelectSkin, 1);

        string isContinue = PlayerPrefs.GetString(PlayerPrefsHelper.IsContinue);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        if (string.IsNullOrEmpty(isContinue))
        {

            InitializeMap(currentLevel);

            CreateMapBounds();
            CreateAStarGrid();
        }
        else
            LoadMapState();
        PlayerPrefs.DeleteKey(PlayerPrefsHelper.IsContinue);
    }

    public void InitializeMap(int level)
    {
        // Load cấu hình map từ JSON dựa trên level
        MapManager.Instance.LoadMapData(level);
        width = MapManager.Instance.currentMapData.width;
        height = MapManager.Instance.currentMapData.height;
        trapCount = MapManager.Instance.currentMapData.trapCount;
        chestCount = MapManager.Instance.currentMapData.chestCount;
        levelEnemy = MapManager.Instance.currentMapData.levelEnemy;
        // Khởi tạo các thành phần map
        maze = new int[width, height];
        GenerateMaze();
        AddTraps();
        PlaceExit();
        RenderMaze();
        SpawnPlayerRandomly();
        SpawnEnemies();
        AddChests();
        if (playerInstance != null && virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform;
        }
        else
        {
            Debug.LogError("Player instance or virtual camera is missing!");
        }
    }

    public Vector2Int GetRandomPathPosition()
    {
        return FindRandomPathPosition();
    }

    public void ResetPlayer()
    {
        if (playerInstance != null)
        {
            Vector2Int newPos = FindRandomPathPosition();
            playerInstance.transform.position = tilePathMap.GetCellCenterWorld((Vector3Int)newPos);
        }
    }

    void CreateMapBounds()
    {
        // Tạo GameObject chứa collider giới hạn
        GameObject boundsObject = Instantiate(mapBoundsPrefab, Vector3.zero, Quaternion.identity);
        PolygonCollider2D polygonCollider = boundsObject.GetComponent<PolygonCollider2D>();
        boundsObject.SetActive(true);

        // Lấy vị trí góc dưới bên trái của tilemap (tọa độ thế giới)
        Vector3 bottomLeft = tilemap.CellToWorld(new Vector3Int(0, 0, 0));

        // Lấy vị trí góc trên bên phải của tilemap (tọa độ thế giới)
        Vector3 topRight = tilemap.CellToWorld(new Vector3Int(width, height, 0));

        // Tính toán kích thước map trong không gian thế giới
        Vector2[] boundsPoints = new Vector2[4];
        boundsPoints[0] = new Vector2(bottomLeft.x, bottomLeft.y); // Góc dưới trái
        boundsPoints[1] = new Vector2(topRight.x, bottomLeft.y);   // Góc dưới phải
        boundsPoints[2] = new Vector2(topRight.x, topRight.y);     // Góc trên phải
        boundsPoints[3] = new Vector2(bottomLeft.x, topRight.y);   // Góc trên trái

        // Đặt các điểm cho PolygonCollider2D
        polygonCollider.SetPath(0, boundsPoints);

        // Gán collider vào CinemachineConfiner
        CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();
        if (confiner != null)
        {
            confiner.m_BoundingShape2D = polygonCollider;
        }
        else
        {
            Debug.LogError("CinemachineConfiner component is missing on the virtual camera!");
        }
    }

    void GenerateMaze()
    {
        // Khởi tạo mê cung với tường
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1; // 1 là tường
            }
        }

        // Bắt đầu từ một điểm ngẫu nhiên
        int startX = Random.Range(1, width - 1);
        int startY = Random.Range(1, height - 1);
        maze[startX, startY] = 0; // 0 là đường đi

        // Thuật toán Random Walk
        int steps = 1000;
        int currentX = startX;
        int currentY = startY;
        for (int i = 0; i < steps; i++)
        {
            int direction = Random.Range(0, 4);
            switch (direction)
            {
                case 0: currentX++; break; // Phải
                case 1: currentX--; break; // Trái
                case 2: currentY++; break; // Lên
                case 3: currentY--; break; // Xuống
            }

            // Đảm bảo không đi ra khỏi biên
            currentX = Mathf.Clamp(currentX, 1, width - 2);
            currentY = Mathf.Clamp(currentY, 1, height - 2);

            maze[currentX, currentY] = 0;
        }
    }

    void AddTraps()
    {
        for (int i = 0; i < trapCount; i++)
        {
            int x, y;
            bool isValidPosition;
            do
            {
                x = Random.Range(1, width - 1);
                y = Random.Range(1, height - 1);
                isValidPosition = maze[x, y] == 0 && !IsNearWall(x, y); // Kiểm tra ô có phải là đường đi và không gần tường
            } while (!isValidPosition); // Đảm bảo bẫy được đặt trên đường đi và không gần tường

            maze[x, y] = 2; // 2 là bẫy
        }
    }

    bool IsNearWall(int x, int y)
    {
        // Kiểm tra xem ô (x, y) có gần tường hay không
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (maze[x + i, y + j] == 1) // Nếu có tường gần đó
                {
                    return true;
                }
            }
        }
        return false;
    }

    void PlaceExit()
    {
        // Đặt cổng ra
        exitPosition = FindRandomPathPosition();
        maze[exitPosition.x, exitPosition.y] = 4; // 4 là cổng ra
    }

    Vector2Int FindRandomPathPosition()
    {
        int x, y;
        do
        {
            x = Random.Range(1, width - 1);
            y = Random.Range(1, height - 1);
        } while (maze[x, y] != 0); // Đảm bảo vị trí là đường đi

        return new Vector2Int(x, y);
    }

    void RenderMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 0 || maze[x, y] == 2 || maze[x, y] == 4)
                {
                    // Đặt đường đi cho các ô có giá trị 0, 2, 4
                    tilePathMap.SetTile(new Vector3Int(x, y, 0), pathTile);
                }

                // Đặt tường cho các ô có giá trị 1
                if (maze[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }

                // Đặt bẫy (GameObject) cho các ô có giá trị 2
                if (maze[x, y] == 2)
                {
                    Vector3 trapPosition = tilePathMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    GameObject trapInstance = Instantiate(trapPrefab, trapPosition, Quaternion.identity);
                    SetSortingLayerRecursive(trapInstance, "Trap");
                }

                // Đặt cổng ra (GameObject) cho các ô có giá trị 4
                if (maze[x, y] == 4)
                {
                    Vector3 exitPositionWorld = tilePathMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    Instantiate(exitPrefab, exitPositionWorld, Quaternion.identity);
                }
            }
        }
    }

    void SetSortingLayerRecursive(GameObject parent, string layerName)
    {
        SpriteRenderer[] renderers = parent.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.sortingLayerName = layerName;
        }

        // Nếu có các loại renderer khác (TilemapRenderer,...)
        TilemapRenderer[] tileRenderers = parent.GetComponentsInChildren<TilemapRenderer>(true);
        foreach (TilemapRenderer renderer in tileRenderers)
        {
            renderer.sortingLayerName = layerName;
        }
    }
    void SpawnPlayerRandomly()
    {
        // Tìm vị trí ngẫu nhiên trên đường đi để đặt nhân vật
        Vector2Int playerPosition;
        bool isValidPosition;
        do
        {
            playerPosition = FindRandomPathPosition();
            isValidPosition = !IsNearWall(playerPosition.x, playerPosition.y); // Đảm bảo nhân vật không bị đặt sát tường
        } while (!isValidPosition);

        // Đặt nhân vật tại vị trí ngẫu nhiên
        GameObject selectedPlayerPrefab = playerPrefabs[selectedCharacterIndex];
        Vector3 spawnPosition = tilePathMap.GetCellCenterWorld(new Vector3Int(playerPosition.x, playerPosition.y, 0));
        playerInstance = Instantiate(selectedPlayerPrefab, spawnPosition, Quaternion.identity);
        playerInstance.SetActive(true);
    }



    // SPAWN ENEMIES
    void SpawnEnemies()
    {
        // Lấy dữ liệu từ MapManager
        int[] enemyCounts = MapManager.Instance.currentMapData.enemyCounts;

        // Kiểm tra dữ liệu hợp lệ
        if (enemyCounts == null || enemyCounts.Length != enemyPrefabs.Length)
        {
            Debug.LogError("Enemy configuration mismatch!");
            return;
        }

        // Spawn từng loại quái
        for (int enemyType = 0; enemyType < enemyCounts.Length; enemyType++)
        {
            // Spawn số lượng quái theo config
            for (int i = 0; i < enemyCounts[enemyType]; i++)
            {
                SpawnSingleEnemy(enemyType);
            }
        }
    }
    void SpawnSingleEnemy(int enemyType)
    {
        // Kiểm tra prefab hợp lệ
        if (enemyType >= enemyPrefabs.Length || enemyPrefabs[enemyType] == null)
        {
            Debug.LogError($"Missing prefab for enemy type {enemyType}");
            return;
        }

        // Tìm vị trí spawn hợp lệ
        Vector2Int spawnPosition = GetValidEnemySpawnPosition();
        Vector3 worldPosition = tilePathMap.GetCellCenterWorld((Vector3Int)spawnPosition);

        // Tạo quái vật
        GameObject enemy = Instantiate(enemyPrefabs[enemyType], worldPosition, Quaternion.identity);

        // Cấu hình thêm nếu cần
        // enemy.transform.parent = transform; // Đặt vào cùng parent
    }

    Vector2Int GetValidEnemySpawnPosition()
    {
        Vector2Int playerPosition = GetPlayerGridPosition();
        Vector2Int spawnPosition;
        bool isValidPosition;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            spawnPosition = FindRandomPathPosition();

            // Kiểm tra khoảng cách với player
            float distanceToPlayer = Vector2Int.Distance(spawnPosition, playerPosition);

            // Kiểm tra vị trí không có vật cản
            isValidPosition = distanceToPlayer > 5f && // Cách player ít nhất 5 ô
                             maze[spawnPosition.x, spawnPosition.y] == 0 && // Trên đường đi
                             !IsPositionOccupied(spawnPosition); // Không có vật thể khác

            attempts++;
        } while (!isValidPosition && attempts < maxAttempts);

        return isValidPosition ? spawnPosition : Vector2Int.zero;
    }

    Vector2Int GetPlayerGridPosition()
    {
        if (playerInstance == null) return Vector2Int.zero;

        Vector3 playerWorldPos = playerInstance.transform.position;
        return (Vector2Int)tilePathMap.WorldToCell(playerWorldPos);
    }

    bool IsPositionOccupied(Vector2Int gridPosition)
    {
        Vector3 worldPos = tilePathMap.GetCellCenterWorld((Vector3Int)gridPosition);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos, 0.4f);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player") ||
                col.CompareTag("Enemy") ||
                col.CompareTag("Trap") ||
                col.CompareTag("Chest"))
            {
                return true;
            }
        }
        return false;
    }

    void CreateAStarGrid()
    {
        if (activeAStarGrid != null)
        {
            Destroy(activeAStarGrid.gameObject);
        }

        activeAStarGrid = Instantiate(aStarPrefab, transform);
        activeAStarGrid.InitializeGrid(maze, width, height);
    }
    private void OnApplicationQuit()
    {
        SaveMap();
    }

    public void SaveMap()
    {
        GameObject[] exit = GameObject.FindGameObjectsWithTag("Exit");
        var exitVetor3 = exit[0].transform.position;
        Vector3Int exitVetor3Int = tilePathMap.WorldToCell(exitVetor3);
        List<Vector2Int> chestPositions = new List<Vector2Int>();
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        foreach (GameObject chest in chests)
        {
            Vector3Int cellPosition = tilePathMap.WorldToCell(chest.transform.position);
            chestPositions.Add(new Vector2Int(cellPosition.x, cellPosition.y));
        }
        FindObjectOfType<MapManager>().SaveMapState(width, height, maze, playerInstance, new Vector2Int(exitVetor3Int.x, exitVetor3Int.y), currentLevel, chestPositions);
    }
    public void ClearMap()
    {
        tilemap.ClearAllTiles();
        tilePathMap.ClearAllTiles();

        var traps = GameObject.FindGameObjectsWithTag("Trap");
        foreach (var trap in traps)
        {
            Destroy(trap.gameObject);
        }

        var exits = GameObject.FindGameObjectsWithTag("Exit");
        foreach (var exit in exits)
        {
            Destroy(exit.gameObject);
        }

        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        foreach (GameObject chest in chests)
        {
            Destroy(chest.gameObject);
        }

        var enemiesCanShoot = GameObject.FindGameObjectsWithTag(EnemyType.EnemyCanShoot);
        foreach (var enemy in enemiesCanShoot)
        {
            Destroy(enemy.gameObject);
        }

        var enemiesCanNotShoot = GameObject.FindGameObjectsWithTag(EnemyType.EnemyCanNotShoot);
        foreach (var enemy in enemiesCanNotShoot)
        {
            Destroy(enemy.gameObject);
        }

        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }
    }

    public void LoadMapState()
    {
        string path = Path.Combine(Application.persistentDataPath, $"map_{currentLevel}_save.json");
        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                MapSaveData saveData = JsonUtility.FromJson<MapSaveData>(json);

                maze = ConvertArray.Convert1DTo2D(saveData.maze, saveData.width, saveData.height);
                width = saveData.width;
                height = saveData.height;
                exitPosition = saveData.exitPosition;

                levelEnemy = saveData.levelEnemy;

                ClearMap();
                RenderMaze();

                playerInstance = Instantiate(playerPrefabs[selectSkin], saveData.playerPosition, Quaternion.identity);

                foreach (var enemyData in saveData.enemies)
                {
                    var enemyPrefab = enemyPrefabs[enemyData.enemyType];
                    var enemy = Instantiate(enemyPrefab, enemyData.position, Quaternion.identity);
                }

                foreach (Vector2Int chestPos in saveData.chestPositions)
                {
                    Vector3 worldPosition = tilePathMap.GetCellCenterWorld((Vector3Int)chestPos);
                    Instantiate(chestPrefab, worldPosition, Quaternion.identity);
                }

                CreateMapBounds();
                CreateAStarGrid();

                if (virtualCamera != null)
                {
                    virtualCamera.Follow = playerInstance.transform;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading map state: " + e.Message);
                InitializeMap(PlayerPrefs.GetInt("CurrentLevel", 1));
            }
        }
        else
        {
            InitializeMap(PlayerPrefs.GetInt("CurrentLevel", 1));
        }
    }


    //gen chest
    Vector2Int GetValidChestPosition()
    {
        Vector2Int chestPosition;
        bool isValidPosition;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            chestPosition = FindRandomPathPosition();
            isValidPosition = maze[chestPosition.x, chestPosition.y] == 0 && // Đảm bảo là đường đi
                              !IsPositionOccupied(chestPosition); // Không có vật thể khác
            attempts++;
        } while (!isValidPosition && attempts < maxAttempts);

        return isValidPosition ? chestPosition : Vector2Int.zero;
    }
    void AddChests()
    {
        for (int i = 0; i < chestCount; i++)
        {
            Vector2Int chestPosition = GetValidChestPosition();
            if (chestPosition != Vector2Int.zero)
            {
                Vector3 worldPosition = tilePathMap.GetCellCenterWorld((Vector3Int)chestPosition);
                GameObject chestInstance = Instantiate(chestPrefab, worldPosition, Quaternion.identity);
                SetSortingLayerRecursive(chestInstance, "Chest"); // Đặt sorting layer nếu cần
                Debug.Log($"Add Chest {chestCount}");
            }
            else
            {
                Debug.LogWarning("Không tìm được vị trí hợp lệ cho rương sau nhiều lần thử.");
            }
        }
    }
}