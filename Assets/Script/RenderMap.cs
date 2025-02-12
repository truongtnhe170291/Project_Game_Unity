using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RenderMap : MonoBehaviour
{
    public int width = 30; // Chiều rộng mê cung
    public int height = 30; // Chiều cao mê cung
    public Tilemap tilemap; // Tilemap để render mê cung
    public TileBase wallTile; // Tile cho tường
    public TileBase pathTile; // Tile cho đường đi
    public TileBase trapTile; // Tile cho bẫy
    public int trapCount = 10; // Số lượng bẫy

    private int[,] maze;

    void Start()
    {
        maze = new int[width, height];
        GenerateMaze();
        AddTraps();
        RenderMaze();
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
                    _ => wallTile
                };
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}
