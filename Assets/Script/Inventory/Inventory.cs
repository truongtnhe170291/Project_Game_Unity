using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {


    public int[] items;
    public GameObject[] slots;

    void Start()
    {
        // Tìm GameObject "Inventory" trong toàn bộ Scene
        GameObject inventoryObj = GameObject.Find("Inventory");

        if (inventoryObj == null)
        {
            return;
        }
        // Tìm GameObject "Bag" bên trong Inventory
        Transform bag = inventoryObj.transform.Find("Bag");

        if (bag == null)
        {

            return;
        }
        // Tạo danh sách lưu các Slot tìm được
        List<GameObject> slotList = new List<GameObject>();

        // Duyệt qua tất cả InventoryPlace bên trong Bag
        foreach (Transform inventoryPlace in bag)
        {
            Transform slot = inventoryPlace.Find("Slot");
            if (slot != null)
            {
        Debug.Log("vaoday3");

                slotList.Add(slot.gameObject);
            }
            else
            {
                Debug.Log("vaoday4");

            }
        }

        // Chuyển danh sách thành mảng và gán vào slots
        slots = slotList.ToArray();
    }
}
