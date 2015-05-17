using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class HighscoreUI : MonoBehaviour {

	public Text scoreText;
	public InputField inputNameField;
	public Transform scoreListHolder;
	public Transform recordPrefab;

	public event EventHandler replay;
	public event EventHandler recordChange;
	public delegate void EventHandler(object sender, EventArgs e);

	void Start()
	{
		UpdateScoreList();
	}

	public HighscoreRecord CurrentRecord
	{
		get { return currentRecord; }
		set 
		{ 
			currentRecord = value; 
			scoreText.text = currentRecord.score.ToString(); 
		}
	}
	public List<HighscoreRecord> ScoreRecords
	{
		get { return scoreRecords; }
		set 
		{ 
			scoreRecords = value; 
			UpdateScoreList(); 
		}
	}
	protected HighscoreRecord currentRecord;
	protected List<HighscoreRecord> scoreRecords;



	public void Name_Change()
	{
		currentRecord.playerName = inputNameField.text;
		UpdateScoreList();
	}

	public void OnReplayClick()
	{
		if (replay != null)
			replay(this, EventArgs.Empty);
	}

	protected void UpdateScoreList()
	{
		var children = new List<GameObject>();
		foreach (Transform child in scoreListHolder) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));

		scoreRecords.Sort();
		scoreRecords.Reverse();
		for (int i = 0; i < scoreRecords.Count; i++)
		{
			Transform record = GameObject.Instantiate(recordPrefab);
			record.SetParent(scoreListHolder);
			RecordUI recUI = record.GetComponent<RecordUI>();
			recUI.RatingPosition = (i + 1).ToString() + ".";
			recUI.PlayerName = scoreRecords[i].playerName;
			recUI.Score = scoreRecords[i].score.ToString();
		}
		OnRecordChange();
	}
	
	protected void OnRecordChange()
	{
		if (recordChange != null)
			recordChange(this, EventArgs.Empty);
	}
}
