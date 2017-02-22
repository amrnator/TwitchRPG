using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour {
	GameObject inventoryPanel;
	GameObject slotPanel;
	ItemDatabase database;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	public int slotAmount;

	public List<Item> items = new List<Item> ();
	public List<GameObject> slots = new List<GameObject> (); 

	void Start(){
		database = GetComponent<ItemDatabase> ();

		slotAmount = 20;
		inventoryPanel = GameObject.Find ("Inventory Panel");

		slotPanel = inventoryPanel.transform.FindChild ("Slot Panel").gameObject;

		for (int i = 0; i < slotAmount; i++) {
			items.Add (new Item ());
			slots.Add (Instantiate (inventorySlot));
			slots [i].transform.SetParent (slotPanel.transform);

		}

		AddItem (0);
		AddItem (1);
		AddItem (1);
		AddItem (1);
	}

	public void AddItem(int id){
		//get item
		Item itemToAdd = database.FetchItemByID (id);

		if (itemToAdd.Stackable && CheckItemExists (itemToAdd)) {
			//item is already in inventory and stackable
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == id) {
					ItemData data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();
					data.amount++;
					data.transform.GetChild (0).GetComponent<Text> ().text = data.amount.ToString ();
					break;
				}
			}
		} else {
			//find an empty slot to add item
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == -1) {
					items [i] = itemToAdd;
					GameObject itemObj = Instantiate (inventoryItem);
					itemObj.transform.SetParent (slots [i].transform);
					itemObj.transform.position = Vector2.zero;
					itemObj.GetComponent<Image> ().sprite = itemToAdd.Sprite;
					itemObj.name = itemToAdd.Title;
					ItemData data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();
					data.amount = 1;
					break;
				}
			}
		}
	}

	bool CheckItemExists(Item item){
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == item.ID) {
				return true;
			}
		}
		return false;
	}
}
