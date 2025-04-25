using System;
using UnityEngine;

public class ActionGasStation : ActionArrivePlace
{
    //private Animator currentAnimator;
    private GasStationSpot target;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        isArrived = false;
        SetDestination();
        if(target == null) { isArrived = true;}
        timer = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(timer > 5) { npcMovement.MakeRandomDecision(); }
        if(target == null) { return; }
        if(isArrived)
        {
            timer += Time.deltaTime;
        }
        else if(Vector3.Distance(target.transform.position, npcMovement.transform.position) < 0.3f )
        {
            isArrived = true;
            currentAnimator.SetFloat("Vert", 0);
            GasStationManager.instance.ReturnSpot(target);
        }
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
