using UnityEngine;
using Pathfinding;

public class AStarGridGenerator : MonoBehaviour
{
    public GridGraph gridGraph;
    public int[,] mazeData;
    public int width;
    public int height;

    public void InitializeGrid(int[,] maze, int gridWidth, int gridHeight)
    {
        if (maze == null)
        {
            Debug.LogError("maze is null!");
            return;
        }
        if (gridWidth <= 0 || gridHeight <= 0)
        {
            Debug.LogError("Invalid grid dimensions!");
            return;
        }
        mazeData = maze;
        width = gridWidth;
        height = gridHeight;
        Debug.Log($"Initializing grid with size: {width}x{height}");

        AstarPath astar = AstarPath.active;
        if (astar == null)
        {
            Debug.LogError("AstarPath.active is null. Please add AstarPath component to the scene.");
            return;
        }

        gridGraph = astar.data.AddGraph(typeof(GridGraph)) as GridGraph;
        if (gridGraph == null)
        {
            Debug.LogError("Failed to create GridGraph");
            return;
        }
        Debug.Log("GridGraph created successfully");

        // Cấu hình GridGraph với tọa độ chính xác
        gridGraph.is2D = true;
        gridGraph.SetDimensions(width, height, 1); // Kiểm tra Node Size trong Inspector
        gridGraph.center = new Vector3((width - 1) / 2f, (height - 1) / 2f, 0); // Trung điểm của Tilemap
        gridGraph.collision.use2D = true;
        gridGraph.collision.mask = LayerMask.GetMask("Obstacle");
        gridGraph.collision.type = ColliderType.Capsule; // Phù hợp với Tilemap 2D
        gridGraph.collision.diameter = 0.9f;// Điều chỉnh để phù hợp với ô Tilemap

        Debug.Log($"GridGraph center: {gridGraph.center}, Dimensions: {width}x{height}");

        // Scan GridGraph trước
        AstarPath.active.Scan(gridGraph);
        Debug.Log("Grid Graph scanned automatically.");

        // Cập nhật node walkability sau khi scan
        UpdateGridGraph();
        Debug.Log("Node walkability updated based on maze data.");
    }

    void UpdateGridGraph()
    {
        Debug.Log("Updating Grid Graph...");
        if (gridGraph == null)
        {
            Debug.LogError("gridGraph is null!");
            return;
        }
        int nodeCount = 0;
        gridGraph.GetNodes(node => {
            nodeCount++;
            Debug.Log($"Processing node at position: {node.position} - Count: {nodeCount}");
            Vector3 pos = (Vector3) node.position;
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            Debug.Log($"Converting to grid coordinates: ({x}, {y})");
            node.Walkable = IsWalkable(x, y);
        });
        Debug.Log($"Finished updating Grid Graph. Total nodes processed: {nodeCount}");
    }

    bool IsWalkable(int x, int y)
    {
        if (mazeData == null)
        {
            Debug.LogError("mazeData is null!");
            return false;
        }
        Debug.Log($"Checking walkability at ({x}, {y}) - Width: {width}, Height: {height}");
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.Log($"Out of bounds: ({x}, {y})");
            return false;
        }

        Debug.Log($"mazeData[{x},{y}] = {mazeData[x, y]}");
        return mazeData[x, y] != 1; // 1 is wall
    }
}