using UnityEngine;
using System.Collections;

public class SoundsGameScreen : MonoBehaviour {

	public Player player;
	public AudioSource death;
	public AudioSource eating;

	void Start()
	{
		player.foodEaten += OnFoodEaten;
		player.ghostCollide += OnGhostCollide;
	}

	protected void OnGhostCollide(object sender, System.EventArgs e)
	{
		death.Play();
	}

	protected void OnFoodEaten(object sender, System.EventArgs e)
	{
		eating.Play();
	}
}
