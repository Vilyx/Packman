using UnityEngine;
using System.Collections;
using System;

public class ControllableCharacter : MonoBehaviour
{

	public PackmanLogic packmanLogic;
	public float moveSpeed = 3;

	protected Direction desirableDirection = Direction.None;
	protected Direction moveDirection = Direction.None;
	protected Direction beforeStoppingDirection = Direction.None;
	protected Vector3 nextPosition;
	protected Vector3 prevPosition;
	protected bool isMoving = false;
	


	void Start()
	{

	}
	public void ResetPosition(Vector3 initialPosition)
	{
		isMoving = false;
		nextPosition = initialPosition;
		prevPosition = initialPosition;
		transform.position = initialPosition;
		desirableDirection = Direction.None;
		moveDirection = Direction.None;
		beforeStoppingDirection = Direction.None;
	}
	protected virtual void Update()
	{
		if (!packmanLogic.Paused)
		{
			UpdatePosition();
		}
	}

	protected void UpdatePosition()
	{

		if (desirableDirection != moveDirection)
		{
			if (!isMoving)
			{
				prevPosition = transform.position;
				Vector3 possibleNextPosition = NextPosInDirection(transform.position, desirableDirection);
				if (packmanLogic.IsPositionWalkable(possibleNextPosition))
				{
					moveDirection = desirableDirection;
					nextPosition = possibleNextPosition;
					isMoving = true;
				}
				else //продолжаем плыть по иннерции если не можем начать двигаться в желаемую сторону
				{
					Vector3 innertialNextPosition = NextPosInDirection(transform.position, beforeStoppingDirection);
					if (packmanLogic.IsPositionWalkable(innertialNextPosition))
					{
						moveDirection = beforeStoppingDirection;
						nextPosition = innertialNextPosition;
						isMoving = true;
					}
				}

			}
			else if (isDirectionsContrary(moveDirection, desirableDirection))
			{
				moveDirection = desirableDirection;
				Vector3 swapVec = prevPosition;
				prevPosition = nextPosition;
				nextPosition = swapVec;
			}
		}
		if (nextPosition != prevPosition)
		{
			moveInDirection(moveDirection);
			if (isReachedNextPos())
			{
				stopMoving();
			}
		}
	}
	protected void stopMoving()
	{
		prevPosition = nextPosition;
		transform.position = nextPosition; //устанавливаем целое значение
		if (moveDirection != Direction.None)
			beforeStoppingDirection = moveDirection;
		moveDirection = Direction.None;
		isMoving = false;
	}
	protected bool isReachedNextPos()
	{
		Vector3 curPos = transform.position;
		if (moveDirection == Direction.Up && curPos.z < nextPosition.z)
		{
			return true;
		}
		if (moveDirection == Direction.Down && curPos.z > nextPosition.z)
		{
			return true;
		}
		if (moveDirection == Direction.Left && curPos.x > nextPosition.x)
		{
			return true;
		}
		if (moveDirection == Direction.Right && curPos.x < nextPosition.x)
		{
			return true;
		}
		return false;
	}

	protected void moveInDirection(Direction direction)
	{
		Vector3 newPosition = transform.position;
		if (moveDirection == Direction.Up)
		{
			newPosition.z -= Time.deltaTime * moveSpeed;
		}
		if (moveDirection == Direction.Down)
		{
			newPosition.z += Time.deltaTime * moveSpeed;
		}
		if (moveDirection == Direction.Left)
		{
			newPosition.x += Time.deltaTime * moveSpeed;
		}
		if (moveDirection == Direction.Right)
		{
			newPosition.x -= Time.deltaTime * moveSpeed;
		}
		transform.position = newPosition;
	}

	protected Vector3 NextPosInDirection(Vector3 initialPosition, Direction direction)
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
		return nextPos;
	}

	protected bool isDirectionsContrary(Direction moveDirection, Direction desirableDirection)
	{
		if (moveDirection == desirableDirection) return false;

		if ((moveDirection      == Direction.Right || moveDirection      == Direction.Left) &&
			(desirableDirection == Direction.Right || desirableDirection == Direction.Left))
		{
			return true;
		}
		if ((moveDirection      == Direction.Up || moveDirection      == Direction.Down) &&
			(desirableDirection == Direction.Up || desirableDirection == Direction.Down))
		{
			return true;
		}

		return false;
	}
}
