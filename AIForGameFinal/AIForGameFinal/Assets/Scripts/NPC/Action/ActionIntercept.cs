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
        // 计算拦截点（在玩家和目标之间）
        Vector3 playerPos = GEPCore.Instance.PlayerSpy.position;
        interceptPoint = playerPos + (targetGoal - playerPos).normalized * interceptDistance;
    }

    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        currentAnimator = animator;
        currentAnimator.SetFloat("State", 0.9f); // 更快的行走状态
        currentSpeed = moveSpeed;
    }

    public override void OnUpdate()
    {
        Vector3 direction = (interceptPoint - currentAnimator.transform.position).normalized;

        // 移动
        currentAnimator.SetFloat("Vert", currentSpeed);

        // 旋转
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            currentAnimator.transform.rotation = Quaternion.Slerp(
                currentAnimator.transform.rotation,
                targetRotation,
                Time.deltaTime * 8f); // 更快的旋转
        }

        // 到达拦截点
        if (Vector3.Distance(currentAnimator.transform.position, interceptPoint) < 0.5f)
        {
            MakeNewDecision(); // 返回正常行为
        }
    }
}