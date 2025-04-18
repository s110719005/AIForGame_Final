using System.Collections;
using UnityEngine;

public class ActionWander : ActionBase
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float waitTime = 2f;
    private Animator currentAnimator;
    private Vector3 targetPosition;
    private float waitTimer = 0f;
    private bool isWaiting = true;
    private float currentSpeed;
    private Coroutine toIdleCoroutine;

    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        currentAnimator = animator;
        PickNewDestination();
        isWaiting = false;
        currentAnimator.SetFloat("State", 0.7f);
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                isWaiting = false;
            }
        }
        else
        {
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
                isWaiting = true;
                if(toIdleCoroutine != null)
                {
                    StopCoroutine(toIdleCoroutine);
                    toIdleCoroutine = null;
                }
                toIdleCoroutine = StartCoroutine(ChangeToIdleCoroutine());
            }
        }

        //RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward) * 3, Color.yellow); 
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward), 3))
        { 
            PickNewDestination();
        }
    }

    void PickNewDestination()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        Vector3 randomOffset = new Vector3(randomCircle.x, 0, randomCircle.y);
        targetPosition = transform.position + randomOffset;
        currentSpeed = Random.Range(moveSpeed - 0.2f, moveSpeed);
    }

    private IEnumerator ChangeToIdleCoroutine()
    {
        for(int i = 0; i < 50; i++)
        {
            currentSpeed -= 0.1f;
            currentAnimator.SetFloat("Vert", currentSpeed);
            if(currentSpeed <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        PickNewDestination();
        yield return null;
    }
}
