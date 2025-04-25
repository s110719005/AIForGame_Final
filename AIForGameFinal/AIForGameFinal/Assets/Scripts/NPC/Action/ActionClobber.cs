using UnityEngine;

public class ActionClobber : ActionBase
{
    private Animator currentAnimator;
    private Mission mission;
    private Vector3 targetPosition;
    private float timer;
    [SerializeField] private float collectClobberRadius = 3f;
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
                targetPosition = mission.transform.position * 0.3f + SpyPlayer.Instance.transform.position * 0.7f;
                break;
            case MissionType.hotdog:
            case MissionType.chat:
            case MissionType.collect:
                Vector2 randomCircle = Random.insideUnitCircle * collectClobberRadius;
                Vector3 randomOffset = new Vector3(randomCircle.x, 0, randomCircle.y);
                targetPosition = mission.transform.position + randomOffset;
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
        if(timer >= 5) 
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
            case MissionType.chat:
            case MissionType.collect:
                //break;
                direction = (targetPosition - currentAnimator.transform.position).normalized;
                distance = Vector3.Distance(targetPosition, currentAnimator.transform.position);
                //Debug.Log("DISTANCE:" + distance); 
                if(distance < collectClobberRadius) 
                { 
                    currentAnimator.SetFloat("Vert", 0);
                    //Rotation
                    Vector3 lookDirection = (mission.transform.position - currentAnimator.transform.position).normalized;
                    lookDirection.y = 0f;
                    if (lookDirection != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                        currentAnimator.transform.rotation = Quaternion.Slerp(currentAnimator.transform.rotation, targetRotation, Time.deltaTime * 5f);
                    }
                }
                else
                {
                    //Move
                    currentAnimator.SetFloat("Vert", 1);
                    //Rotation
                    Vector3 lookDirection = (mission.transform.position - currentAnimator.transform.position).normalized;
                    lookDirection.y = 0f;
                    if (lookDirection != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
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
            
        }
        
    }

    public void SetMission(Mission mission)
    {
        this.mission = mission;
    }
}
