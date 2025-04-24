using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private List<ActionBase> actions;
    //[SerializeField] private ActionIntercept interceptAction;
    [SerializeField] private ActionClobber actionClobber;

    private ActionBase currentAction;

    private void Start() 
    {
        MakeRandomDecision();
    }

    public void OnUpdate()
    {
        if(currentAction != null)
        {
            currentAction.OnUpdate();
        }
    }

    public void MakeRandomDecision()
    {
        //TODO: Change this to actual decision making script
        int index = Random.Range(0, actions.Count);
        SwitchAction(actions[index]);
    }

    public void SwitchAction(ActionBase action)
    {
        if(currentAction != null) { currentAction.OnExit(); }
        currentAction = action;
        currentAction.OnStart(animator);
    }

    public void Kill()
    {
        animator.SetTrigger("Trigger_Die");
        characterController.enabled = false;
        transform.position -= new Vector3(0, 0.1f, 0);
    }

    public void SetGEPAction(Mission mission)
    {
        //switch the state to observer
        Debug.Log("SET GEP: " + mission.Type);
        actionClobber.SetMission(mission);
        SwitchAction(actionClobber);

        // if (interceptAction != null)
        // {
        //     interceptAction.SetTarget(targetPosition);
        //     currentAction = interceptAction;
        //     currentAction.OnStart(animator);
        // }
    }
}
