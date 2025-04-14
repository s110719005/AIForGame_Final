using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField]private Animator animator;
    [SerializeField]private float moveSpeed = 1;
    [SerializeField]private float wanderRadius = 5f;
    [SerializeField]private float waitTime = 2f;

    private Vector3 targetPosition;
    private float waitTimer = 0f;
    private bool isWaiting = true;
    private float currentSpeed;
    private Coroutine toIdleCoroutine;
    //LayerMask layerMask = LayerMask.GetMask("Default");

    private void Start() 
    {
        PickNewDestination();
        isWaiting = false;
    }

    public void OnUpdate()
    {
        Wander();
    }
    private void Wander()
    {
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
            Vector3 direction = (targetPosition - transform.position).normalized;
            //Move
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            animator.SetFloat("Vert", currentSpeed);

            //Rotation
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // Reach target
            //Debug.Log("DISTANCE: " + Vector3.Distance(transform.position, targetPosition));
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                isWaiting = true;
                if(toIdleCoroutine != null)
                {
                    StopCoroutine(toIdleCoroutine);
                    toIdleCoroutine = null;
                }
                toIdleCoroutine = StartCoroutine(ChangeToIdleCoroutine());
                //TODO: a slow down coroutine
                //animator.SetFloat("Hor", 0);
                //animator.SetFloat("Vert", 0);
            }
        }

        // 動畫控制
        if (animator != null)
        {
            bool isMoving = !isWaiting && Vector3.Distance(transform.position, targetPosition) > 0.1f;

            animator.SetFloat("State", 0.7f);

        }

        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward) * 3, Color.yellow); 
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward), out hit, 3))

        { 
            PickNewDestination();
            Debug.Log("Did Hit"); 
        }
        else
        { 
            //Debug.DrawRay(transform.position + new Vector3(0, 3, 0), transform.TransformDirection(Vector3.forward) * 1000, Color.white); 
            //Debug.Log("Did not Hit"); 
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
            animator.SetFloat("Vert", currentSpeed);
            if(currentSpeed <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        PickNewDestination();
        yield return null;
    }

    // private void OnTriggerEnter(Collider other) {
    //     PickNewDestination();
    //     Debug.Log("TRIGGER");
    // }
}
