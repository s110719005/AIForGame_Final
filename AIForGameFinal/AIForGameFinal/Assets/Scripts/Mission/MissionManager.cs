using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour // mission point manager
{
    [SerializeField] private List<Mission> missions;
    [SerializeField] private float possibleRange = 5;

    public List<Mission> GetPossibleMissionSpot()
    {
        List<Mission> possibleMissions = new List<Mission>();
        foreach (var mission in missions)
        {
            if(!mission.DoesPlayerHasMission()) { continue;}
            if(mission.IsMissionComplete()) { continue; }
            if(!mission.CanExecute()) { continue; }
            if(Vector3.Distance(SpyPlayer.Instance.transform.position, mission.transform.position) < possibleRange)
            {
                possibleMissions.Add(mission);
            }
        }
        return possibleMissions;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum MissionType
{
    gasStation = 0,
    hotdog = 1,
    chat = 2,
    collect = 3
}

[System.Serializable]
public class PlayerMission
{
    public MissionType missionType;
    public bool isComplete;

    public void CompleteMission()
    {
        isComplete = true;
    }
}

