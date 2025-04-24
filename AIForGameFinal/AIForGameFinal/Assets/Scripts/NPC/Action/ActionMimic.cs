using UnityEngine;

public class ActionMimic : ActionBase
{
    [SerializeField] private float mimicUpdateInterval = 0.5f;
    private Transform playerSpy;
    private Vector3 lastPlayerPosition;
    private float timer;
    private Animator currentAnimator;

    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        currentAnimator = animator;
        //playerSpy = GEPCore.Instance.PlayerSpy;
        lastPlayerPosition = playerSpy.position;
        timer = 0f;
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= mimicUpdateInterval)
        {
            timer = 0f;
            UpdateMimicBehavior();
        }
    }

    private void UpdateMimicBehavior()
    {
        //Vector3 playerMovement = playerSpy.position - lastPlayerPosition;
        //if (playerMovement.magnitude > 0.1f)
        //{
        //    Vector3 targetPos = currentAnimator.transform.position + playerMovement.normalized;
        //    currentAnimator.SetFloat("Vert", 1f);
        //    currentAnimator.transform.LookAt(targetPos);
        //}
        //lastPlayerPosition = playerSpy.position;
    }
}