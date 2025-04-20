using System.Collections.Generic;
using UnityEngine;

public class GEPCore : MonoBehaviour
{
    public static GEPCore Instance;

    [SerializeField] private Transform playerSpy;
    [SerializeField] private float checkInterval = 1f;
    [SerializeField] private float maxInterceptDistance = 0f;

    private List<NPCMovement> npcs = new List<NPCMovement>();
    private List<Vector3> playerPathHistory = new List<Vector3>();
    private float timer = 0f;
    private Vector3 predictedGoal;
    private NPCMovement activeElicitor;

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
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            UpdateGEP();
        }
    }

    private void UpdateGEP()
    {
        RecordPlayerPath();
        predictedGoal = PredictPlayerGoal();

        if (predictedGoal != Vector3.zero && activeElicitor == null)
        {
            AssignClosestElicitor(predictedGoal);
        }
    }

    private void RecordPlayerPath()
    {
        if (playerSpy != null)
        {
            playerPathHistory.Add(playerSpy.position);
            if (playerPathHistory.Count > 10)
            {
                playerPathHistory.RemoveAt(0);
            }
        }
    }

    private Vector3 PredictPlayerGoal()
    {
        if (playerPathHistory.Count < 3) return Vector3.zero;

        Vector3 direction = (playerPathHistory[playerPathHistory.Count - 1] - playerPathHistory[0]).normalized;
        float maxScore = 0f;
        Vector3 bestGoal = Vector3.zero;

        foreach (var npc in npcs)
        {
            if (npc == null) continue;

            float distanceToPlayer = Vector3.Distance(npc.transform.position, playerSpy.position);
            if (distanceToPlayer > maxInterceptDistance) continue;

            Vector3 toNPC = (npc.transform.position - playerPathHistory[0]).normalized;
            float score = Vector3.Dot(direction, toNPC);

            if (score > maxScore)
            {
                maxScore = score;
                bestGoal = npc.transform.position;
            }
        }

        return maxScore > 0.7f ? bestGoal : Vector3.zero;
    }

    private void AssignClosestElicitor(Vector3 goal)
    {
        NPCMovement bestElicitor = null;
        float minDistance = float.MaxValue;

        foreach (var npc in npcs)
        {
            if (npc == null || npc == activeElicitor) continue;

            float distanceToPlayer = Vector3.Distance(npc.transform.position, playerSpy.position);
            if (distanceToPlayer > maxInterceptDistance) continue;

            float distToPath = GetDistanceToPlayerPath(npc.transform.position);
            if (distToPath < minDistance)
            {
                minDistance = distToPath;
                bestElicitor = npc;
            }
        }

        if (bestElicitor != null)
        {
            activeElicitor = bestElicitor;
            bestElicitor.SetGEPAction(goal);
        }
    }

    public void OnElicitorComplete()
    {
        activeElicitor = null;
    }

    private float GetDistanceToPlayerPath(Vector3 point)
    {
        float minDist = float.MaxValue;
        for (int i = 1; i < playerPathHistory.Count; i++)
        {
            Vector3 closestPoint = GetClosestPointOnSegment(
                playerPathHistory[i - 1],
                playerPathHistory[i],
                point);
            float currentDist = Vector3.Distance(point, closestPoint);
            minDist = Mathf.Min(minDist, currentDist);
        }
        return minDist;
    }

    private Vector3 GetClosestPointOnSegment(Vector3 a, Vector3 b, Vector3 point)
    {
        Vector3 ab = b - a;
        float t = Vector3.Dot(point - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        return a + ab * t;
    }

    public Transform PlayerSpy
    {
        get { return playerSpy; }
    }
}