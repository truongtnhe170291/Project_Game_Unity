using System.Collections.Generic;
using UnityEngine;

namespace Assets.Helper
{
	[System.Serializable]
	public class MapSaveData
	{
		public int width;
		public int height;
		public int[] maze;
		public Vector3 playerPosition;
		public Vector2Int exitPosition;
		public List<EnemySaveData> enemies;
		public List<TrapSaveData> traps;

		[System.Serializable]
		public class EnemySaveData
		{
			public int enemyType;
			public Vector3 position;
		}

		[System.Serializable]
		public class TrapSaveData
		{
			public Vector3 position;
			public bool isActive;
		}
	}
}
