using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour {

	private List<Item> database = new List<Item>();

	private JsonData ItemData;

	void Start(){
		ItemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
		ConstructDatabase ();
	}

	public Item FetchItemByID(int id){
		for (int i = 0; i < database.Count; i++) {
			if(database[i].ID == id){
				return database [i];
			}
		}
		return null;
	}

	void ConstructDatabase(){
		for(int i= 0; i < ItemData.Count; i++){
			database.Add(new Item((int)ItemData[i]["id"], ItemData[i]["title"].ToString(), (int)ItemData[i]["value"],
				(int)ItemData[i]["stats"]["power"], (int)ItemData[i]["stats"]["defense"], (int)ItemData[i]["stats"]["vitality"],
				ItemData[i]["description"].ToString(), (bool)ItemData[i]["stackable"], (int)ItemData[i]["rarity"],
				ItemData[i]["slug"].ToString()
			));
		}
	}
}
public class Item{
	
	public int ID { get; private set;}
	public string Title { get; set;}
	public int Value { get; set;}
	public int Power { get; set;}
	public int Defence { get; set;}
	public int Vitality { get; set;}
	public string Description { get; set;}
	public bool Stackable { get; set;}
	public int Rarity { get; set;}
	public string Slug { get; set;}
	public Sprite Sprite { get; set; }

	public Item(int id, string title, int value, int power, int defense, int vitality, string description, bool stackable, int rarity, string slug){
		this.ID = id;
		this.Title = title;
		this.Value = value;
		this.Power = power;
		this.Vitality = vitality;
		this.Defence = defense;
		this.Description = description;
		this.Stackable = stackable;
		this.Rarity = rarity;
		this.Slug = slug;
		this.Sprite = Resources.Load<Sprite> ("Sprites/Items/" + slug);
	}

	public Item(){
		this.ID = -1;
	}
}
