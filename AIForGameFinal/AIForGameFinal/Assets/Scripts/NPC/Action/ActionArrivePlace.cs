using System.Collections;
using UnityEngine;

public class ActionArrivePlace : ActionBase
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] protected Vector3 targetPosition;
    private Animator currentAnimator;
    protected bool isArrived;
    private Coroutine toIdleCoroutine;
    private float currentSpeed;
    
    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        currentAnimator = animator;
        currentAnimator.SetFloat("State", 0.7f);
        isArrived = false;
        currentSpeed = moveSpeed;
    }

    public override void OnUpdate()
    {
        if(isArrived) { return; }
        Vector3 direction = (targetPosition - currentAnimator.transform.position).normalized;
        //Move
        currentAnimator.SetFloat("Vert", currentSpeed);

        //Rotation
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            currentAnimator.transform.rotation = Quaternion.Slerp(currentAnimator.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Reach target
        //Debug.Log("DISTANCE: " + Vector3.Distance(transform.position, targetPosition));
        if (Vector3.Distance(currentAnimator.transform.position, targetPosition) < 0.5f)
        {
            isArrived = true;
            if(toIdleCoroutine != null)
            {
                StopCoroutine(toIdleCoroutine);
                toIdleCoroutine = null;
            }
            toIdleCoroutine = StartCoroutine(ChangeToIdleCoroutine());
        }
    }

    private IEnumerator ChangeToIdleCoroutine()
    {
        for(int i = 0; i < 50; i++)
        {
            currentSpeed -= 0.1f;
            currentAnimator.SetFloat("Vert", currentSpeed);
            if(currentSpeed <= 0)
            {
                currentSpeed = 0;
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        //npcCMovement.MakeNewDecision();
        yield return null;
    }
}
