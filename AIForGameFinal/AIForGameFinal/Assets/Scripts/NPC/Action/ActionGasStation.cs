using System;
using UnityEngine;

public class ActionGasStation : ActionArrivePlace
{
    private Animator currentAnimator;
    private GasStationSpot target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        SetDestination();
        if(target == null) { isArrived = true;}
    }

    private void SetDestination()
    {
        if(GasStationManager.instance.HasEmptySpot())
        {
            target = GasStationManager.instance.GetEmptySpot();
            targetPosition = target.transform.position;
        }
    }
}
