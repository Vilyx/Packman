using UnityEngine;
using System.Collections;
using System;

public class Player : ControllableCharacter {

	public SoundsGameScreen sounds;
	public event EventHandler foodEaten;
	public event EventHandler ghostCollide;
	public delegate void EventHandler(object sender, EventArgs e);
	protected float lastStopTime = 0;

	void Start () {
	
	}

	protected override void Update()
	{
		HandleInput();
		base.Update();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Food"))
		{
			OnFoodEaten();
			Destroy(other.gameObject);
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Ghost"))
		{
			OnGhostCollide();
		}
	}

	protected void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			desirableDirection = Direction.Left;
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			desirableDirection = Direction.Up;
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			desirableDirection = Direction.Right;
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			desirableDirection = Direction.Down;
		}
	}
	protected void OnFoodEaten()
	{
		if (foodEaten != null)
			foodEaten(this, EventArgs.Empty);
	}
	protected void OnGhostCollide()
	{
		if (ghostCollide != null)
			ghostCollide(this, EventArgs.Empty);
	}
}
