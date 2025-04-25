using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject spyWinPanel;
    [SerializeField] private GameObject sniperWinPanel;
    [SerializeField] private TextMeshProUGUI timerText;
    private float gameTimer;
    private void Awake() 
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
        gameTimer = 180;
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer -= Time.deltaTime;
        timerText.text = "Time Left: " + Mathf.RoundToInt(gameTimer) + " sec";
        if(gameTimer <= 0)
        {
            EndGame(false);
        }
    }

    public void EndGame(bool isSpyWin)
    {
        endPanel.SetActive(true);
        if(isSpyWin)
        {
            spyWinPanel.SetActive(true);
        }
        else
        {
            sniperWinPanel.SetActive(true);
        }
    }
}
