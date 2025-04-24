using UnityEngine;

public class ActionClobber : ActionBase
{
    private Animator currentAnimator;
    private Mission mission;
    private Vector3 targetPosition;
    private float timer;
    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        currentAnimator = animator;
        Debug.Log("START CLOBBER");
        currentAnimator.SetFloat("State", 1);
        timer = 0;
        switch(mission.Type)
        {
            case MissionType.gasStation:
                targetPosition = mission.transform.position * 0.6f + SpyPlayer.Instance.transform.position * 0.4f;
                break;
            case MissionType.hotdog:
                break;
            case MissionType.chat:
                break;
            case MissionType.collect:
                break;
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        timer += Time.deltaTime;
        if(timer >= 7) 
        { 
            npcMovement.MakeRandomDecision(); 
            Debug.Log("TIME OUT");
            GEPCore.Instance.ClearElicitor();
        }


        switch(mission.Type)
        {
            case MissionType.gasStation:
                Vector3 direction = (targetPosition - currentAnimator.transform.position).normalized;
                float distance = Vector3.Distance(targetPosition, currentAnimator.transform.position);
                if(distance < 1) 
                { 
                    currentAnimator.SetFloat("Vert", 0);
                }
                else
                {
                    //Move
                    currentAnimator.SetFloat("Vert", 1);
                    //Rotation
                    direction.y = 0f;
                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        currentAnimator.transform.rotation = Quaternion.Slerp(currentAnimator.transform.rotation, targetRotation, Time.deltaTime * 5f);
                    }
                }


                if(GEPCore.Instance.PredictedGoal != mission) 
                { 
                    currentAnimator.SetFloat("Vert", 0);
                    Debug.Log("PLAYER CHANGE THE PLAN");
                    GEPCore.Instance.ClearElicitor();
                    npcMovement.MakeRandomDecision(); 
                }
                break;
            case MissionType.hotdog:
                break;
            case MissionType.chat:
                break;
            case MissionType.collect:
                break;
        }
        
    }

    public void SetMission(Mission mission)
    {
        this.mission = mission;
    }
}
