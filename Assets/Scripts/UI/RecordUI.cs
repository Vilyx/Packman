using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecordUI : MonoBehaviour {

	public Text RatingPositionText;
	public Text PlayerNameText;
	public Text ScoreText;

	public string RatingPosition
	{
		get { return ratingPosition; }
		set 
		{ 
			ratingPosition = value; 
			RatingPositionText.text = value; 
		}
	}

	public string PlayerName
	{
		get { return playerName; }
		set 
		{ 
			playerName = value; 
			PlayerNameText.text = value; 
		}
	}

	public string Score
	{
		get { return score; }
		set 
		{ 
			score = value; 
			ScoreText.text = value; 
		}
	}

	protected string ratingPosition = "0";
	protected string playerName = "Nobody";
	protected string score = "0";
}
