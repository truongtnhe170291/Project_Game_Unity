// MapData.cs
using System;

[Serializable]
public class MapData
{
    public int level;
    public int width;
    public int height;
    public int steps;
    public int trapCount;
    public int[] enemyCounts; // Số lượng quái vật theo loại
    // Thêm các trường dữ liệu khác tùy ý
}