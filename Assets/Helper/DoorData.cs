using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Helper
{
    public static class DoorData
    {
        public static int DoorId { get; set; }
        public static List<int> StatusDoors { get; set; } = new List<int>() { 0, -1, -1, -1, -1, -1, -1, -1, -1 };

        public static List<int> GetListDoorStatic() { return StatusDoors; }

        public static List<int> GetListDoorInJsonFile()
        {
			TextAsset jsonFile = Resources.Load<TextAsset>($"MapData/list_map");
			if (jsonFile != null)
			{
				ListDoor listDoor = JsonUtility.FromJson<ListDoor>(jsonFile.text);
				return listDoor.StatusDoors;
			}
			else
			{
				Debug.LogError("Using default list map!");
				return StatusDoors;
			}
		}

		public static void UpdateListDoorInJsonFile(List<int> newStatusDoors)
		{
			string path = Path.Combine(Application.dataPath, "Resources/MapData/list_map.json");

			ListDoor listDoor = new ListDoor();
			listDoor.StatusDoors = newStatusDoors;

			string json = JsonUtility.ToJson(listDoor, true);

			File.WriteAllText(path, json);

			Debug.Log("Updated list_map.json with new StatusDoors data.");
		}

	}
}
