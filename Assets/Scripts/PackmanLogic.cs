using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PackmanLogic : MonoBehaviour {

	public PackmanUI ui;
	public Transform wallPrefab;
	public Transform foodPrefab;
	public Transform wallsHolder;
	public Transform pickableHolder;
	public Player player;
	public Ghost ghostPink;
	public Ghost ghostYellow;
	public Ghost ghostGreen;
	
	public event ChangedEventHandler changed;
	public event ChangedEventHandler gameOver;
	public delegate void ChangedEventHandler(object sender, EventArgs e);
	[HideInInspector]
	public bool paused = false;

	public bool Paused
	{
		get { return paused; }
		set { 
				paused = value;
				if (paused) ui.showPausedUI();
				else ui.hidePausedUI();
			}
	}

	public int Points
	{
		get { return points; }
		set { points = value; OnChanged(); }
	}
	public int Lives 
	{
		get { return lives; }
		set { lives = value; OnChanged(); } 
	}

	protected int points = 0;
	protected int lives = 0;
	protected Vector3 playerDefaultPos;
	protected Vector3 ghostPinkDefaultPos;
	protected Vector3 ghostYellowDefaultPos;
	protected Vector3 ghostGreenDefaultPos;
	protected string mapStr = "1111111111111111111\n" +
							  "1000000001000000001\n" +
							  "1011011101011101101\n" +
							  "1011011101011101101\n" +
							  "1000000000000000001\n" +
							  "1011010111110101101\n" +
							  "1000010001000100001\n" +
							  "1111011101011101111\n" +
							  "1111010000000101111\n" +
							  "1111010114110101111\n" +
							  "1444000156710004441\n" +
							  "1111010111110101111\n" +
							  "1111010002000101111\n" +
							  "1111010111110101111\n" +
							  "1000000001000000001\n" +
							  "1011011101011101101\n" +
							  "1001000000000001001\n" +
							  "1101010111110101011\n" +
							  "1000010001000100001\n" +
							  "1011111101011111101\n" +
							  "1000000000000000001\n" +
							  "1111111111111111111";
	protected int[,] map;
	


	void Start () {
		map = ParseMap(mapStr);
		BuildMap();
		ResetPositions();
		Player playerController = player.GetComponent<Player>();
		playerController.foodEaten += OnFoodEaten;
		playerController.ghostCollide += OnGhostCollidesPlayer;
		playerController.moveSpeed = 6;

		Points = 0;
		Lives = 1;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Paused = !Paused;
		}
	}

	public bool IsPositionWalkable(Vector3 possibleNextPosition)
	{
		int w = Mathf.RoundToInt(possibleNextPosition.x);
		int h = Mathf.RoundToInt(possibleNextPosition.z);
		if (map[w, h] != CellValues.WALL)
		{
			return true;
		}
		return false;
	}

	public Direction[] GetPossibleDirections(Vector3 initialPosition)
	{
		List<Direction> directions = new List<Direction>();
		if (IsWalkableNextPosInDirection(initialPosition, Direction.Up))
		{
			directions.Add(Direction.Up);
		}
		if (IsWalkableNextPosInDirection(initialPosition, Direction.Down))
		{
			directions.Add(Direction.Down);
		}
		if (IsWalkableNextPosInDirection(initialPosition, Direction.Left))
		{
			directions.Add(Direction.Left);
		}
		if (IsWalkableNextPosInDirection(initialPosition, Direction.Right))
		{
			directions.Add(Direction.Right);
		}
		return directions.ToArray();
	}

	protected bool IsWalkableNextPosInDirection(Vector3 initialPosition, Direction direction)
	{
		Vector3 nextPos = initialPosition;
		if (direction == Direction.Right)
		{
			nextPos.x -= 1;
		}
		else if (direction == Direction.Down)
		{
			nextPos.z += 1;
		}
		else if (direction == Direction.Left)
		{
			nextPos.x += 1;
		}
		else if (direction == Direction.Up)
		{
			nextPos.z -= 1;
		}
		return IsPositionWalkable(nextPos);
	}

	protected void OnChanged()
	{
		if (changed != null)
			changed(this, EventArgs.Empty);
	}

	protected void OnFoodEaten(object sender, System.EventArgs e)
	{
		Points += 100;
		if (FoodCount() == 1)
			GameOver();
	}

	protected int FoodCount()
	{
		int layer = LayerMask.NameToLayer("Food");
		GameObject[] goArray = GameObject.FindObjectsOfType<GameObject>();
		List<GameObject> goList = new System.Collections.Generic.List<GameObject>();
		for (var i = 0; i < goArray.Length; i++) {
			if (goArray[i].layer == layer) {
				goList.Add(goArray[i]);
			}
		}
		return goList.Count;
	}

	protected void OnGhostCollidesPlayer(object sender, EventArgs e)
	{
		Lives -= 1;
		if (Lives < 1)
		{
			GameOver();
		}
		else
		{ 
			ResetPositions(); 
		}
	}

	protected void GameOver()
	{
		Debug.Log("PackmanLogic::GameOver");
		if (gameOver != null)
			gameOver(this, EventArgs.Empty);
	}

	protected int[,] ParseMap(string lvlDescription)
	{
		int[,] map;
		string[] mapStrings = mapStr.Split('\n');
		int mapHeight = mapStrings.Length;
		int mapWidth = (mapStrings[0] as string).Length;
		map = new int[mapWidth,mapHeight];
		for (int i = 0; i < mapHeight; i++)
		{
			for (int j = 0; j < mapWidth; j++)
			{
				map[j, i] = int.Parse((mapStrings[i] as string).Substring(j, 1));
			}
		}
		return map;
	}

	protected void BuildMap()
	{
		for (int i = 0; i < map.GetLength(0); i++)
		{
			for (int j = 0; j < map.GetLength(1); j++)
			{
				int currentCellValue = map[i, j];
				if (currentCellValue == CellValues.WALL)
				{
					Transform wallPart = GameObject.Instantiate(wallPrefab);
					wallPart.transform.position = new Vector3(i, 0, j);
					wallPart.SetParent(wallsHolder);
				}
				else if (currentCellValue == CellValues.PLAYER)
				{
					playerDefaultPos = new Vector3(i,0,j);
				}
				else if (currentCellValue == CellValues.FOOD)
				{
					Transform foodPiece = GameObject.Instantiate(foodPrefab);
					foodPiece.transform.position = new Vector3(i, 0, j);
					foodPiece.SetParent(pickableHolder);
				}
				else if (currentCellValue == CellValues.GHOST_SPAWN_PINK) 
				{
					ghostPinkDefaultPos = new Vector3(i,0,j);
				}
				else if (currentCellValue == CellValues.GHOST_SPAWN_YELLOW)
				{
					ghostYellowDefaultPos = new Vector3(i, 0, j);
				}
				else if (currentCellValue == CellValues.GHOST_SPAWN_GREEN)
				{
					ghostGreenDefaultPos = new Vector3(i, 0, j);
				}
			}
		}
	}

	protected void ResetPositions()
	{
		player.ResetPosition(playerDefaultPos);
		if (ghostPink != null)
		{
			ghostPink.ResetPosition(ghostPinkDefaultPos);
		}
		if (ghostYellow != null)
		{
			ghostYellow.ResetPosition(ghostYellowDefaultPos);
		}
		if (ghostGreen != null)
		{
			ghostGreen.ResetPosition(ghostGreenDefaultPos);
		}
	}
}
