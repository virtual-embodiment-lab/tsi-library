using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StandardLogging;
using UnityEngine;
using UtilityTypes;

public class Logger : Utility
{
    [Header("Switch")]
    [SerializeField] bool log = false;
    [SerializeField] bool replay = false;

    [Header("Shared")]
    [SerializeField] public int logFileSampleRate = 10;
    [SerializeField] List<Tracker> LoggedObjects = new List<Tracker>();

    [Header("Logger")]
    public string trialName = "test1";
    public bool logActive = false;
    private StreamWriter writer = null;
    public bool gameStarted = false;
    [SerializeField] protected ValueMap<string, string> Tracked = new ValueMap<string, string>();
    [SerializeField] private string tempLoggedString = null;

    [Header("Replay")]
    [SerializeField] protected string logFilePath = null;
    [SerializeField] protected string[] logFile = null;
    [SerializeField] protected string text = " ";
    [SerializeField] public string timeStamp = "";
    [SerializeField] public string[] colonSplit;
    [SerializeField] protected string[] logSplit;
    [SerializeField] protected int currentLine = 0;
    //protected StreamReader reader = null;

    [SerializeField] protected bool paused = false;
    [SerializeField] protected bool reversed = false;
    [SerializeField] protected int speed = 1;


    internal void Setup(LoggerData loggerData)
    {
        if (loggerData.replay)
        {
            replay = true;
            log = false;
        }
        else
        {
            replay = false;
            log = true;
        }
        logFileSampleRate = loggerData.logFileSampleRate;
        trialName = loggerData.trialName;
        logFilePath = loggerData.logFilePath;
    }

    public override void Setup(UtilityData utilityData)
    {
        base.Setup(utilityData);
    }

    void Start()
    {
        if (log)
        {
            trialName = UnityEngine.Random.Range(0, 1000000).ToString();
            gameStarted = true;

            foreach (var item in FindObjectsOfType<Tracker>())
            {
                LoggedObjects.Add(item);
            }

            // Logger
            if (!File.Exists(Application.persistentDataPath + "/trial_log_" + trialName + ".csv"))
            {
                Debug.Log(" " + Application.persistentDataPath + "/trial_log_" + trialName + ".csv");
                logActive = true;
                FileStream file = File.Open(Application.persistentDataPath + "/trial_log_" + trialName + ".csv", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                writer = new StreamWriter(file);
                StartCoroutine("Logging");
            }
        }
        if (replay)
        {

            //logFile = Resources.Load(logFileString) as TextAsset;
            foreach (var item in FindObjectsOfType<Tracker>())
            {
                LoggedObjects.Add(item);
                item.StartReplayMode();
            }

            logFile = File.ReadAllLines(logFilePath);
            //reader = new StreamReader(new FileStream(logFilePath, FileMode.Open, FileAccess.ReadWrite));
            StartCoroutine("Replaying");
        }

    }

    private void Update()
    {
        if (FindObjectsOfType<Tracker>().Length > LoggedObjects.Count)
        {
            foreach (var item in FindObjectsOfType<Tracker>())
            {
                if (!LoggedObjects.Contains(item))
                {
                    LoggedObjects.Add(item);
                }
            }
        }
    }

    IEnumerator Logging()
    {
        for (; ; )
        {
            tempLoggedString = "TimeStamp:" + Time.realtimeSinceStartup.ToString("0.00000") + "\t";

            for (int i = 0; i < LoggedObjects.Count(); i++)
            {
                foreach (KVPair<logtype, string> attributes in LoggedObjects[i].GetAttributes())
                {
                    Tracked.UpdateOrCreate(new KVPair<string, string>($"{i}:{attributes.Key}", attributes.Value));
                }
            }

            tempLoggedString += Tracked.ToLine('\t');

            writer.WriteLine(tempLoggedString);
            writer.Flush();
            yield return new WaitForSeconds((float)(1.0 / logFileSampleRate));
        }
    }

    public bool pausePlay()
    {
        return (paused = !paused);
    }

    public bool reversePlay()
    {
        return (reversed = !reversed);
    }

    public void resetPlay()
    {
        currentLine = 0;
    }

    public void endLinePlay()
    {
        currentLine = logFile.Length - 1;
    }

    public int setSpeed(int speed)
    {
        this.speed = speed;
        return speed;
    }

    IEnumerator Replaying()
    {
        while (true)
        {
            while (!paused) yield return null;

            logSplit = logFile[currentLine].Split('\t');
            timeStamp = logSplit[0].Substring(10);

            for (int i = 1; i < logSplit.Length; i++)
            {
                colonSplit = logSplit[i].Split(':');
                if (colonSplit.Length > 1)
                {
                    LoggedObjects[int.Parse(colonSplit[0])].ApplyValue(colonSplit[1], colonSplit[2]);
                }
            }
            currentLine = Math.Min(Math.Max(currentLine + (reversed ? -speed : speed), 0), logFile.Length - 1);
            yield return new WaitForSeconds((float)(1.0 / logFileSampleRate));
        }
    }

    void OnApplicationQuit()
    {
        if (logActive)
        {
            StopCoroutine("Logging");
            StopCoroutine("Replaying");
            logActive = false;
            writer.Close();
        }
    }
}
