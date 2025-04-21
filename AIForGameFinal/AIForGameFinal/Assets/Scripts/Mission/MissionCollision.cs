using TMPro;
using UnityEngine;

public class MissionCollision : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI missionText;
    void TriggerMission()
    {
        missionText.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "SpyPlayer")
        {
            TriggerMission();
        }
    }
}
