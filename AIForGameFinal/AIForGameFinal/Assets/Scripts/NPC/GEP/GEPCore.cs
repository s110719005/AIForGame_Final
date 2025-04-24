using System.Collections.Generic;
using UnityEngine;

public class GEPCore : MonoBehaviour
{
    public static GEPCore Instance;

    [SerializeField] private Transform playerSpy;
    [SerializeField] private float checkInterval = 1f;
    [SerializeField] private float maxInterceptDistance = 0f;
    [SerializeField] private MissionManager missionManager;

    private List<NPCMovement> npcs = new List<NPCMovement>();
    List<Mission> possibleMissions = new List<Mission>();
    List<Mission> priorityMissions = new List<Mission>();
    //private List<Vector3> playerPathHistory = new List<Vector3>();
    private float timer = 0f;
    private Mission predictedGoal;
    public Mission PredictedGoal => predictedGoal;
    private NPCMovement activeElicitor;
    private bool isGepOn = true;

    private void Awake()
    {
        Instance = this;
    }

    public void SetNPCs(List<NPCMovement> npcList)
    {
        npcs = npcList;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) { isGepOn = !isGepOn; }
        if(isGepOn)
        { 
            timer += Time.deltaTime;
            if (timer >= checkInterval)
            {
                timer = 0f;
                UpdateGEP();
            }
        }
        else
        {
            //USE MIMIC
        }
    }

    private void UpdateGEP()
    {
        //RecordPlayerPath();
        predictedGoal = PredictPlayerGoal();

        if (predictedGoal != null && activeElicitor == null)
        {
            AssignClosestElicitor(predictedGoal);
        }
    }

    // private void RecordPlayerPath()
    // {
    //     if (playerSpy != null)
    //     {
    //         playerPathHistory.Add(playerSpy.position);
    //         if (playerPathHistory.Count > 10)
    //         {
    //             playerPathHistory.RemoveAt(0);
    //         }
    //     }
    // }

    private Mission PredictPlayerGoal()
    {
        //if (playerPathHistory.Count < 3) return null;
        possibleMissions.Clear();
        possibleMissions = missionManager.GetPossibleMissionSpot();
        Mission bestGoal = null;
        float minDistance = Mathf.Infinity;
        int firstPriority = 10;
        float currentDistance = 0;

        //prority first
        foreach (Mission mission in possibleMissions)
        {
            mission.SetPrioirity();
            if(firstPriority > mission.priority)
            {
                firstPriority = mission.priority;
            }
        }
        priorityMissions.Clear();
        foreach (Mission mission in possibleMissions)
        {
            if(mission.priority == firstPriority) { priorityMissions.Add(mission); }
        }

        //distance
        foreach (Mission mission in priorityMissions)
        {
            currentDistance = Vector3.Distance(SpyPlayer.Instance.transform.position, mission.transform.position);
            if(currentDistance < minDistance)
            {
                bestGoal = mission;
                minDistance = currentDistance;
            }
        }

        return bestGoal;

    }

    private void AssignClosestElicitor(Mission mission)
    {
        NPCMovement bestElicitor = null;
        float minDistance = float.MaxValue;

        foreach (var npc in npcs)
        {
            if (npc == null || npc == activeElicitor) continue;

            float distanceToMission = Vector3.Distance(npc.transform.position, mission.transform.position);
            if (distanceToMission < minDistance) 
            { 
                bestElicitor = npc;
                minDistance = distanceToMission;
            }

            //TODO: check if they can interrupt in the future time

            // float distToPath = GetDistanceToPlayerPath(npc.transform.position);
            // if (distToPath < minDistance)
            // {
            //     minDistance = distToPath;
            //     bestElicitor = npc;
            // }
        }

        if (bestElicitor != null && bestElicitor != activeElicitor)
        {
            activeElicitor = bestElicitor;
            bestElicitor.SetGEPAction(mission);
        }
    }

    public void ClearElicitor()
    {
        activeElicitor = null;
    }

    public void OnElicitorComplete()
    {
        activeElicitor = null;
    }

    // private float GetDistanceToPlayerPath(Vector3 point)
    // {
    //     float minDist = float.MaxValue;
    //     for (int i = 1; i < playerPathHistory.Count; i++)
    //     {
    //         Vector3 closestPoint = GetClosestPointOnSegment(
    //             playerPathHistory[i - 1],
    //             playerPathHistory[i],
    //             point);
    //         float currentDist = Vector3.Distance(point, closestPoint);
    //         minDist = Mathf.Min(minDist, currentDist);
    //     }
    //     return minDist;
    // }

    // private Vector3 GetClosestPointOnSegment(Vector3 a, Vector3 b, Vector3 point)
    // {
    //     Vector3 ab = b - a;
    //     float t = Vector3.Dot(point - a, ab) / ab.sqrMagnitude;
    //     t = Mathf.Clamp01(t);
    //     return a + ab * t;
    // }

    // public Transform PlayerSpy
    // {
    //     get { return playerSpy; }
    // }
}