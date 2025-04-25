using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMimic : ActionBase
{
    [SerializeField] private float mimicUpdateInterval = 0.5f;
    [SerializeField] private float mimicDuration = 4;
    private Queue<MovingData> movingDatas = new Queue<MovingData>();
    private Animator currentAnimator;
    private float currentSpeed;
    private bool isWalking;
    private float updateDataTimer;
    private float delayTimer;
    private float delay;
    private float activeTimer;
    private Coroutine toIdleCoroutine;
    private MovingData currentMovingData;



    public struct MovingData
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool isWalking;
    }

    
    public override void OnStart(Animator animator)
    {
        base.OnStart(animator);
        currentAnimator = animator;
        delay = Random.Range(0, 0.9f);
        ResetTimer();
    }

    private void ResetTimer()
    {
        updateDataTimer  = 0;
        delayTimer  = 0;
        activeTimer  = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        updateDataTimer += Time.deltaTime;
        delayTimer += Time.deltaTime;
        activeTimer += Time.deltaTime;
        if(activeTimer > mimicDuration)
        {
            npcMovement.MakeRandomDecision();
        }
        if(updateDataTimer > mimicUpdateInterval)
        {
            updateDataTimer = 0;

            //add rotation noise
            Quaternion originalRot = SpyPlayer.Instance.transform.rotation;
            float randomYOffset = Random.Range(-15f, 15f);
            Vector3 euler = originalRot.eulerAngles;
            euler.y += randomYOffset;

            RecordPlayerMovingData(new MovingData
            { 
                position = SpyPlayer.Instance.transform.position,
                rotation = Quaternion.Euler(euler),
                isWalking = SpyPlayer.Instance.CurrentSpeed > 0.9f
            });
        }
        if(delayTimer > delay)
        {
            delay = Random.Range(0, 0.9f);
            delayTimer = 0;
            if(movingDatas.Count <= 0) { return;}
            currentMovingData = movingDatas.Dequeue();
        }
        if(movingDatas.Count <= 0) { return;}

        Quaternion targetRotation = currentMovingData.rotation;
        currentAnimator.transform.rotation = Quaternion.Slerp(currentAnimator.transform.rotation, targetRotation, Time.deltaTime * 5f);
        if(isWalking && isWalking != currentMovingData.isWalking)
        {
            //agent should stop
            isWalking = false;
            if(toIdleCoroutine != null)
            {
                StopCoroutine(toIdleCoroutine);
                toIdleCoroutine = null;
            }
            StartCoroutine(ChangeToIdleCoroutine());
        }
        else if(!isWalking && isWalking != currentMovingData.isWalking)
        {
            //agent should start walking
            currentSpeed = 1;
            currentAnimator.SetFloat("Vert", currentSpeed);
            isWalking = true;
        }
    }

    private void RecordPlayerMovingData(MovingData movingData)
    {
        movingDatas.Enqueue(movingData);
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
        yield return null;
    }
}