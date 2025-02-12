using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class RenderMap : MonoBehaviour
{
    public int width = 30; // Chiều rộng mê cung
    public int height = 30; // Chiều cao mê cung
    public Tilemap tilemap; // Tilemap để render mê cung
    public TileBase wallTile; // Tile cho tường
    public TileBase pathTile; // Tile cho đường đi
    public TileBase trapTile; // Tile cho bẫy
    public TileBase entranceTile; // Tile cho cổng vào
    public TileBase exitTile; // Tile cho cổng ra
    public int trapCount = 10; // Số lượng bẫy
    public GameObject playerPrefab; // Prefab của nhân vật
    public CinemachineVirtualCamera virtualCamera; // Tham chiếu đến Cinemachine Virtual Camera

    private int[,] maze;
    private Vector2Int entrancePosition; // Vị trí cổng vào
    private Vector2Int exitPosition; // Vị trí cổng ra
    private GameObject playerInstance; // Tham chiếu đến nhân vật sau khi được tạo ra

    void Start()
    {
        maze = new int[width, height];
        GenerateMaze();
        AddTraps();
        PlaceEntranceAndExit();
        RenderMaze();
        SpawnPlayerNearEntrance();

        // Gán virtualCamera.Follow sau khi nhân vật được tạo ra
        if (playerInstance != null && virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform;
        }
        else
        {
            Debug.LogError("Player instance or virtual camera is missing!");
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
            do
            {
                x = Random.Range(1, width - 1);
                y = Random.Range(1, height - 1);
            } while (maze[x, y] != 0); // Đảm bảo bẫy được đặt trên đường đi

            maze[x, y] = 2; // 2 là bẫy
        }
    }

    void PlaceEntranceAndExit()
    {
        // Đặt cổng vào
        entrancePosition = FindRandomPathPosition();
        maze[entrancePosition.x, entrancePosition.y] = 3; // 3 là cổng vào

        // Đặt cổng ra
        do
        {
            exitPosition = FindRandomPathPosition();
        } while (exitPosition == entrancePosition); // Đảm bảo cổng ra khác cổng vào
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
                TileBase tile = maze[x, y] switch
                {
                    0 => pathTile, // Đường đi
                    1 => wallTile, // Tường
                    2 => trapTile, // Bẫy
                    3 => entranceTile, // Cổng vào
                    4 => exitTile, // Cổng ra
                    _ => wallTile
                };
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    void SpawnPlayerNearEntrance()
    {
        // Tìm vị trí gần cổng vào để đặt nhân vật
        Vector3 spawnPosition = tilemap.GetCellCenterWorld(new Vector3Int(entrancePosition.x, entrancePosition.y, 0));
        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
}