﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateEngage : IState
{
    public bool canEngage;

    public StateEngage(Ship owningShip) : base(owningShip)
    {
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        ControlGuns();
        CheckFire();
    }

    void ControlGuns()
    {
        foreach (GameObject gun in MyShip.myGuns)
        {
            gun.transform.LookAt(MyShip.AimPosition);
        }
    }

    void CheckFire()
    {
        if (MyShip.PrimaryTargetObject == null)
        {
            canEngage = false;
        }
        else
        {
            float distance = (MyShip.AimPosition - MyShip.PrimaryTargetObject.transform.position).magnitude;
            canEngage = distance < 100;
        }

        Debug.Log(canEngage);
        foreach (GameObject gun in MyShip.myGuns)
        {
            Gun theGun = gun.GetComponent<Gun>();
            //theGun.SetVel(MyShip.CurrentVelocity);
            theGun.canFire = canEngage;
        }
    }
}