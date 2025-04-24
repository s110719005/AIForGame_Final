using UnityEngine;

public class Mission : MonoBehaviour
{
    [SerializeField] private MissionType missionType;
    public MissionType Type => missionType;
    public int priority;
    public delegate void OnMissionTrigger(MissionType type);
    public static event OnMissionTrigger onMissionTrigger;
    //[SerializeField] private Vector3 position;
    //[SerializeField] private bool isComplete;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPrioirity()
    {
        priority = SpyPlayer.Instance.GetPriority(missionType);
    }

    public bool DoesPlayerHasMission()
    {
        return SpyPlayer.Instance.DoesPlayerHasMission(missionType);
    }

    public bool IsMissionComplete()
    {
        return SpyPlayer.Instance.IsMissionComplete(missionType);
    }

    public bool CanExecute()
    {
        switch(missionType)
        {
            case MissionType.gasStation:
                if(GasStationManager.instance.HasEmptySpot()) { return true;}
                return true;
                //break;
            case MissionType.hotdog:
                return true;
                //break;
            case MissionType.chat:
                return true;
                //break;
            case MissionType.collect:
                return true;
                //break;
        }
        return false;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "SpyPlayer")
        {
            onMissionTrigger?.Invoke(missionType);
        }
    }
}
