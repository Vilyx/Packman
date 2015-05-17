using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {

	public GameLogic gameLogic;
	public GameObject difficultyLvlSelect;
	public void StartGame()
	{
		difficultyLvlSelect.SetActive(true);
	}
	public void DifficultySelectedEasy()
	{
		gameLogic.StartGame(0);
	}
	public void DifficultySelectedNormal()
	{
		gameLogic.StartGame(1);
	}
	public void DifficultySelectedHard()
	{
		gameLogic.StartGame(2);
	}
}
