using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListHandler : MonoBehaviour {

	public GameObject contentPanel;
	public GameObject itemPrefab;

	ArrayList times;

	void Start () {

		// 1. Get the data to be displayed
		times = new ArrayList (){
			new TimeModel("9:00am"),
			new TimeModel("10:00am"),
			new TimeModel("11:00am"),
			new TimeModel("12:00pm"),
			new TimeModel("1:00pm"),
			new TimeModel("2:00pm"),
			new TimeModel("3:00pm"),
			new TimeModel("4:00pm"),
			new TimeModel("5:00pm"),
			new TimeModel("6:00pm")
		};

		foreach(TimeModel time in times){
			GameObject newListItem = Instantiate(itemPrefab) as GameObject;
			ItemHandler controller = newListItem.GetComponent<ItemHandler>();
			controller.setup(time);
			newListItem.transform.parent = contentPanel.transform;
			newListItem.transform.localScale = Vector3.one;
		}
	}	
}
