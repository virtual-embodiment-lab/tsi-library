using System;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityTypes
{
    [Serializable]
    public abstract class UtilityData
    {
        public UtilityData()
        {
        }
    }

    [Serializable]
    public class AvatarCreatorData
    {
        [SerializeField] public float density;
        [SerializeField] public Vector3 positionOffset;
        [SerializeField] public Quaternion rotation;
        [SerializeField] public GameObject parent;
        [SerializeField] public bool createOne;
        [SerializeField] public List<GameObject> Avatar;

        public AvatarCreatorData()
        {
        }
    }

    [Serializable]
    public class LoggerData
    {
        [SerializeField] public bool replay;
        [SerializeField] public int logFileSampleRate;
        [Header("Logger")]
        [SerializeField] public string trialName;
        [Header("Replay")]
        [SerializeField] public string logFilePath;


        public LoggerData()
        {
        }
    }
}