using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;
using Assets.Helper;

public class RenderMap : MonoBehaviour
{
    public int width = 30; // Chiều rộng mê cung
    public int height = 30; // Chiều cao mê cung
    public Tilemap tilemap; // Tilemap để render mê cung
    public Tilemap tilePathMap; // Tilemap để render đường đi
    public TileBase wallTile; // Tile cho tường
    public TileBase pathTile; // Tile cho đường đi
    public GameObject trapPrefab; // Prefab cho bẫy (thay thế trapTile)
    public GameObject exitPrefab; // Prefab cho cổng ra (thay thế exitTile)
    public int trapCount = 10; // Số lượng bẫy
    public GameObject playerPrefab; // Prefab của nhân vật
    public CinemachineVirtualCamera virtualCamera; // Tham chiếu đến Cinemachine Virtual Camera
    public GameObject mapBoundsPrefab; // Prefab chứa collider giới hạn map

    private int[,] maze;
    private Vector2Int exitPosition; // Vị trí cổng ra
    private GameObject playerInstance; // Tham chiếu đến nhân vật sau khi được tạo ra

    void Start()
    {
        maze = new int[width, height];
        GenerateMaze();
        AddTraps();
        PlaceExit();
        RenderMaze();
        SpawnPlayerRandomly();

        // Tạo collider giới hạn map
        CreateMapBounds();

        // Gán virtualCamera.Follow sau khi nhân vật được tạo ra
        if (playerInstance != null && virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform;
        }
        else
        {
            Debug.LogError("Player instance or virtual camera is missing!");
        }
        DoorData.DoorId = 7;
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
                    Instantiate(trapPrefab, trapPosition, Quaternion.identity);
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
        Vector3 spawnPosition = tilePathMap.GetCellCenterWorld(new Vector3Int(playerPosition.x, playerPosition.y, 0));
        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        playerInstance.SetActive(true);
    }
}