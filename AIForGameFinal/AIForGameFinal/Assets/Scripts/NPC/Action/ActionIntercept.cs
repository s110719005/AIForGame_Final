using UnityEngine;
public class ActionIntercept : ActionBase
{
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float interceptDistance = 2f;

    private Vector3 interceptPoint;
    private Animator currentAnimator;
    private float currentSpeed;

    public void SetTarget(Vector3 targetGoal)
    {
        Vector3 playerPos = GEPCore.Instance.PlayerSpy.position;
        interceptPoint = playerPos + (targetGoal - playerPos).normalized * interceptDistance;
    }

    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        currentAnimator = animator;
        currentAnimator.SetFloat("State", 0.9f);
        currentSpeed = moveSpeed;
    }

    public override void OnUpdate()
    {
        Vector3 direction = (interceptPoint - currentAnimator.transform.position).normalized;

        currentAnimator.SetFloat("Vert", currentSpeed);

        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            currentAnimator.transform.rotation = Quaternion.Slerp(
                currentAnimator.transform.rotation,
                targetRotation,
                Time.deltaTime * 8f);
        }

        if (Vector3.Distance(currentAnimator.transform.position, interceptPoint) < 0.5f)
        {
            MakeNewDecision();
        }
    }
}