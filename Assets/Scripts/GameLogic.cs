using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

    public enum ScreenType { MainMenu, Game, Highscore};

	protected string highscoresKey = "highscores";
	protected ScreenType lvlToLoad = ScreenType.MainMenu;
	protected int lastHighscore;
	protected List<HighscoreRecord> scoreRecords = new List<HighscoreRecord>();

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		LoadHighscores();
	}

	void OnLevelWasLoaded(int level)
	{
		if (lvlToLoad == ScreenType.Game)
		{
			PackmanLogic packmanLogic = GameObject.FindObjectOfType<PackmanLogic>();
			packmanLogic.gameOver += OnGameOver;
		}
		if (lvlToLoad == ScreenType.Highscore) 
		{
			HighscoreUI highscoreUI = GameObject.FindObjectOfType<HighscoreUI>();
			HighscoreRecord rec = new HighscoreRecord();
			rec.playerName = "Noname";
			rec.score = lastHighscore;
			scoreRecords.Add(rec);
			highscoreUI.CurrentRecord = scoreRecords[scoreRecords.Count-1];
			highscoreUI.ScoreRecords = scoreRecords;
			highscoreUI.recordChange += OnHighscoreRecordChange;
			highscoreUI.replay += OnHighscoreReplayClick;
			SaveHighscores();
		}
	}
	
	public void StartGame()
	{
		SwitchScreen(ScreenType.Game);
	}

    protected void SwitchScreen(ScreenType newScreen)
    {
		lvlToLoad = newScreen;
		Application.LoadLevel(lvlToLoad.ToString());
    }

	protected void OnGameOver(object sender, System.EventArgs e)
	{
		PackmanLogic packmanLogic = sender as PackmanLogic;
		lastHighscore = packmanLogic.Points;
		SwitchScreen(ScreenType.Highscore);
		//добавить текущий рекорд в таблицу рекордов
		//Переключиться на экран highscore и отобразить текущий рекорд
	}

	protected void OnHighscoreRecordChange(object sender, System.EventArgs e)
	{
		SaveHighscores();
	}

	protected void LoadHighscores()
	{
		if (PlayerPrefs.HasKey(highscoresKey))
		{
			string highscoresStrFromPrefs = PlayerPrefs.GetString(highscoresKey);
			scoreRecords = ObjectToStoreStringUtil.StringToHighscore(highscoresStrFromPrefs);
		}
	}

	protected void SaveHighscores()
	{
		string highscoresString = ObjectToStoreStringUtil.HighscoreToString(scoreRecords);
		PlayerPrefs.SetString(highscoresKey, highscoresString);
	}

	protected void OnHighscoreReplayClick(object sender, System.EventArgs e)
	{
		StartGame();
	}
}
