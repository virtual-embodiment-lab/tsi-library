using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

internal static class ExampleUtil
{
    public static bool isPresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances(xrDisplaySubsystems);
        foreach (var xrDisplay in xrDisplaySubsystems)
        {
            if (xrDisplay.running)
            {
                return true;
            }
        }
        return false;
    }
}

public class DetectVR : MonoBehaviour
{
    public GameObject XRController;
    public GameObject KMController;

    void Awake()
    {
        if (ExampleUtil.isPresent())
        {
            XRController.SetActive(true);
            KMController.SetActive(false);
        }
        else
        {
            XRController.SetActive(false);
            KMController.SetActive(true);
        }
    }
}
