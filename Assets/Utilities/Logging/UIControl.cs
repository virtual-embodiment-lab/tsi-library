using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] Logger logger = null;
    public TextMeshProUGUI TimeStamp;
    public Button pauseButton;
    public Button reverseButton;
    public Button resetButton;
    public Button lastButton;
    public Button speedOneButton;
    public Button speedTwoButton;
    public Button speedFourButton;

    // Start is called before the first frame update
    void Start()
    {
        logger = FindObjectOfType<RoomManager>().logger;
        pauseButton.onClick.AddListener(ClickPauseButton);
        reverseButton.onClick.AddListener(ClickReverseButton);
        resetButton.onClick.AddListener(ClickResetButton);
        lastButton.onClick.AddListener(ClickLastButton);
        speedOneButton.onClick.AddListener(ClickSpeedOneButton);
        speedTwoButton.onClick.AddListener(ClickSpeedTwoButton);
        speedFourButton.onClick.AddListener(ClickSpeedFourButton);
    }

    private void Update()
    {
        TimeStamp.text = logger.timeStamp == "" ? "00:00" : logger.timeStamp;
    }

    void ClickPauseButton()
    {
        pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = logger.pausePlay() ? "Pause" : "Resume";
    }

    void ClickReverseButton()
    {
        reverseButton.GetComponentInChildren<TextMeshProUGUI>().text = logger.reversePlay() ? "Reverse" : "Foward";
    }

    void ClickResetButton()
    {
        logger.resetPlay();
    }

    void ClickLastButton()
    {
        logger.endLinePlay();
    }

    void ClickSpeedOneButton()
    {
        logger.setSpeed(1);
    }

    void ClickSpeedTwoButton()
    {
        logger.setSpeed(2);
    }

    void ClickSpeedFourButton()
    {
        logger.setSpeed(4);
    }
}
