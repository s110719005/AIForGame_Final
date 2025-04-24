using UnityEngine;

public class ActionIntercept : ActionBase
{
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float stopDistance = 3f;
    [SerializeField] private float MaxDistance = 10f;

    private Vector3 interceptPoint;
    private Animator currentAnimator;
    private bool isIntercepting = false;

    public void SetTarget(Vector3 targetGoal)
    {
        //if (GEPCore.Instance == null || GEPCore.Instance.PlayerSpy == null) return;

        //Vector3 playerPos = GEPCore.Instance.PlayerSpy.position;
        //interceptPoint = playerPos + (targetGoal - playerPos).normalized * 2f;
        isIntercepting = true;
    }

    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        currentAnimator = animator;
        currentAnimator.SetFloat("Vert", 1f);
    }

    public override void OnUpdate()
    {
        if (!isIntercepting) return;

        Vector3 direction = (interceptPoint - currentAnimator.transform.position).normalized;
        currentAnimator.transform.position += direction * moveSpeed * Time.deltaTime;

        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            currentAnimator.transform.rotation = Quaternion.Slerp(
                currentAnimator.transform.rotation,
                targetRot,
                Time.deltaTime * 10f
            );
        }

        if (Vector3.Distance(currentAnimator.transform.position, interceptPoint) < stopDistance || Vector3.Distance(currentAnimator.transform.position, interceptPoint) > MaxDistance)
        {
            ReturnToNormalBehavior();
        }
    }

    private void ReturnToNormalBehavior()
    {
        isIntercepting = false;
        MakeNewDecision();
        GEPCore.Instance.OnElicitorComplete();
    }
}