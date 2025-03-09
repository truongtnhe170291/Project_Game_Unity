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
    public int[] enemyCounts; 
    public int chestCount;
    public int levelEnemy;
}