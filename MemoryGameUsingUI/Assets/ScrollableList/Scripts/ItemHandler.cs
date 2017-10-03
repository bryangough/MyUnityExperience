using UnityEngine.UI;
using UnityEngine;
public class ItemHandler : MonoBehaviour {

	public Text time;
	public Image image;

	public void setup(TimeModel model){
		time.text = model.time;
	}
	public void thisWasPressed()
	{
		Debug.Log("Pressed");
	}
}