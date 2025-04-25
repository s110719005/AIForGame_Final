using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spyPlayer;
    [SerializeField] private float horizontalSpeed = 1;
    [SerializeField] private int rotationSpeed = 5;

    private Coroutine toIdleCoroutine;

    private List<PlayerMission> playerMissions = new List<PlayerMission>();

    public static SpyPlayer Instance;
    public float CurrentSpeed => animator.GetFloat("Vert");
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AssignMission();
        Mission.onMissionTrigger += OnMissionTrigger;
    }

    private void OnMissionTrigger(MissionType type)
    {
       foreach (var mission in playerMissions)
       {
            if(mission.isComplete) { continue; }
            if(mission.missionType == type)
            {
                mission.CompleteMission();
            }
       }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            //sniperPlayer.transform.Rotate(Vector3.up * Time.deltaTime);
            spyPlayer.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            spyPlayer.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            //spyPlayer.transform.position -= spyPlayer.transform.forward * horizontalSpeed * Time.deltaTime;
            animator.SetFloat("Vert", 1);
        }
        else
        {
            if(animator.GetFloat("Vert") > 0)
            {
                if(toIdleCoroutine != null)
                {
                    StopCoroutine(toIdleCoroutine);
                    toIdleCoroutine = null;
                }
                StartCoroutine(ChangeToIdleCoroutine());
            }
        }
    }

    private void AssignMission()
    {
        int first = UnityEngine.Random.Range(0, 4);

        int second;
        do
        {
            second = UnityEngine.Random.Range(0, 4);
        } while (second == first);
        PlayerMission playerMission1 = new PlayerMission
        {
            missionType = (MissionType)first,
            isComplete = false
        };

        PlayerMission playerMission2 = new PlayerMission
        {
            missionType = (MissionType)second,
            isComplete = false
        };

        playerMissions.Add(playerMission1);
        playerMissions.Add(playerMission2);

        //TODO: Update UI
    }

    private IEnumerator ChangeToIdleCoroutine()
    {
        float currentSpeed = 1;
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
        animator.SetFloat("Vert", 0);
        yield return null;
    }

    internal bool DoesPlayerHasMission(MissionType missionType)
    {
        foreach (var mission in playerMissions)
        {
            if(mission.missionType == missionType) { return true;}
        }
        return false;
    }

    internal bool IsMissionComplete(MissionType missionType)
    {
        foreach (var mission in playerMissions)
        {
            if(mission.missionType == missionType && mission.isComplete) { return true;}
        }
        return false;
    }

    internal int GetPriority(MissionType missionType)
    {
        int index = 0;
        foreach (var mission in playerMissions)
        {
            if(mission.missionType == missionType) { return index;}
            index++;
        }
        return -1;
    }
}
