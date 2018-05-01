﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;

public class StateTarget : IState
{
	private Ship _tarShip;
	public StateTarget(Ship owningShip) : base(owningShip)
	{
	}

	public override void Enter()
	{
		FindTargets();
		MyShip.PrimaryTargetObject = MyShip.TargetObjects[0];
		_tarShip = MyShip.PrimaryTargetObject.GetComponent<Ship>();
	}

	public override void Exit()
	{
		
	}

	public override void Execute()
	{
		FindTargets();
		PredictTargetLocation();
	}

	void FindTargets()
	{
		Collider[] colliders = Physics.OverlapSphere(MyShip.transform.position, MyShip.ScannerRange,MyShip.TargetLayerMask);

		foreach (var col in colliders)
		{
			if (MyShip.TargetObjects.Count > 1)
			{
				foreach (var obj in MyShip.TargetObjects)
				{
					if (col.gameObject == obj)
						break;
				}
			}
			else
			{
				MyShip.TargetObjects.Add(col.gameObject);
			}
		}
		
	}
	
	void PredictTargetLocation()
	{
		
		Vector3 tposVector3 = MyShip.PrimaryTargetObject.transform.position;
		//Vector3 tposVector3 = MyShip.TargetPosition;
		Vector3 toTarget = tposVector3 - MyShip.transform.position;

		float timer = (toTarget.magnitude/ MyShip.CurrentVelocity.magnitude);
		
		MyShip.AimPosition = _tarShip.transform.position + _tarShip.CurrentVelocity * timer;

		MyShip.Sphere.transform.position = MyShip.AimPosition;


	}
	
}
