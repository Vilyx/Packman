using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {

	public GameLogic gameLogic;
	public void StartGame()
	{
		gameLogic.StartGame();
	}
}
