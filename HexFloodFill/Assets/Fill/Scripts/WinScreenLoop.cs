using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class WinScreenLoop : MonoBehaviour {
	public HexFieldBasicWithWater game;
	public Text bestOfLabel;
	public Text newLabel;

	private int currentLowClicks = 1000000;
	void Start()
	{
		if(PlayerPrefs.HasKey("currentLowClicks")){
			currentLowClicks = PlayerPrefs.GetInt("currentLowClicks");
		}
	}
	public void OnEnable()
	{
		Debug.Log ("on visible");
		if(game.clicks<currentLowClicks)
		{
			//newLabel.enabled = true;

			bestOfLabel.text = "Best of:"+game.clicks+"";

			PlayerPrefs.SetInt("currentLowClicks",game.clicks);
			currentLowClicks = game.clicks;
		}
		else
		{
			//newLabel.enabled = false;
			bestOfLabel.text = "This game:"+currentLowClicks+"";
		}
	}
}
