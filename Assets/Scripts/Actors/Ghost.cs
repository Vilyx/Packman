using UnityEngine;
using System.Collections;

public class Ghost : ControllableCharacter
{
	protected override void Update()
	{
		Think();
		base.Update();
	}

	protected void Think()
	{
		if (!isMoving)
		{ 
			Vector3 desirablePosition = NextPosInDirection(transform.position, desirableDirection);
			if (!packmanLogic.IsPositionWalkable(desirablePosition) || desirableDirection == Direction.None)
			{
				ChangeDesirableDirection();
			}
			else if (packmanLogic.GetPossibleDirections(transform.position).Length > 2)
			{
				ChangeDesirableDirection();
			}
		}
	}

	protected void ChangeDesirableDirection()
	{
		Direction[] directions = packmanLogic.GetPossibleDirections(transform.position);
		int randIndex;
		do{
			randIndex = Random.Range(0, directions.Length);
			desirableDirection = directions[randIndex];
		} while (directions.Length > 1 && isDirectionsContrary(beforeStoppingDirection,desirableDirection));
	}
}
