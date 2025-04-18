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

    private Vector3 targetPosition;
    private float waitTimer = 0f;
    private bool isWaiting = true;
    private float currentSpeed;
    private Coroutine toIdleCoroutine;
    private ActionBase currentAction;

    private void Start() 
    {
        MakeNewDecision();
    }

    public void OnUpdate()
    {
        if(currentAction != null)
        {
            currentAction.OnUpdate();
        }
    }

    public void MakeNewDecision()
    {
        //TODO: Change this to actual decision making script
        currentAction = actions[0];
        currentAction.OnStart(animator);
    }

    public void Kill()
    {
        animator.SetTrigger("Trigger_Die");
        characterController.enabled = false;
        transform.position -= new Vector3(0, 0.1f, 0);
    }
}
