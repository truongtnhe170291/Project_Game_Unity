
namespace Assets.Helper
{
	public static class ConvertArray
	{
		public static int[] Convert2DTo1D(int[,] array2D)
		{
			int[] array1D = new int[array2D.GetLength(0) * array2D.GetLength(1)];
			for (int x = 0; x < array2D.GetLength(0); x++)
				for (int y = 0; y < array2D.GetLength(1); y++)
					array1D[x + y * array2D.GetLength(0)] = array2D[x, y];
			return array1D;
		}

		public static int[,] Convert1DTo2D(int[] array1D, int width, int height)
		{
			int[,] array2D = new int[width, height];
			for (int i = 0; i < array1D.Length; i++)
			{
				int x = i % width;
				int y = i / width;
				array2D[x, y] = array1D[i];
			}
			return array2D;
		}
	}
}
