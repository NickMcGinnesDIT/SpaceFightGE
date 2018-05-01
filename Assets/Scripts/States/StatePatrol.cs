﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrol : IState {

	public Path path;

	Vector3 nextWaypoint;

	
	public StatePatrol(Ship owningShip, Path thepath) : base(owningShip)
	{
		path = thepath;
	}

	public override void Enter()
	{	
	}

	public override void Exit()
	{
	}

	public override void Execute()
	{
		MyShip.GoalPosition = Calculate();
		ReachTarget();
		Rotate();
	}
	
	void ReachTarget()
	{
		Vector3 toTarget = MyShip.GoalPosition - MyShip.transform.position;
		float distance = toTarget.magnitude;
		
		MyShip.DesiredVector3 = toTarget / (distance*0.7f);
		MyShip.SteeringVector3 = MyShip.DesiredVector3 - MyShip.CurrentVelocity;
		
		float angle = Vector3.Angle(MyShip.CurrentVelocity, MyShip.SteeringVector3.normalized);
		MyShip.MainEnginePower = Mathf.Min(MyShip.SteeringVector3.magnitude * angle,MyShip.GForceLimit);
		
		float actualForce = MyShip.MainEnginePower * MyShip.ForceNeededForOneG;
		MyShip.MainDriveAcceleration = (actualForce / MyShip.ShipMass);
		
		Vector3 accelerationVector3 = MyShip.MainDriveAcceleration * MyShip.transform.forward;
		
		MyShip.CurrentVelocity += accelerationVector3 * Time.deltaTime;
		
	}

	void Rotate()
	{
		
		MyShip.RotQuat = Quaternion.LookRotation(MyShip.SteeringVector3);
		MyShip.transform.rotation = Quaternion.Slerp(MyShip.transform.rotation, MyShip.RotQuat, MyShip.RotationSpeed);
	}
	
	
	public Vector3 Calculate()
	{
		nextWaypoint = path.NextWaypoint();
		if (Vector3.Distance(MyShip.transform.position, nextWaypoint) < 30)
		{
			path.AdvanceToNext();
		}

		if (path.looped && path.IsLast())
		{
			return nextWaypoint;
		}
		else
		{
			return MyShip.GoalPosition;
		}
		
	}
}
