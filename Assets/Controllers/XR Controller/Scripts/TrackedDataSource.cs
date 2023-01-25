using System.Text;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using StandardLogging;
using UnityEngine;

public class TrackedDataSource : Tracker
{

    public Transform head;
    public Transform headAnchor;
    public Transform leftHand;
    public Transform leftHandAnchor;
    public Transform leftControllerAnchor;

    public Transform rightHand;
    public Transform rightHandAnchor;
    public Transform rightControllerAnchor;

    public HandVisual handVisualLeft;
    public HandVisual handVisualRight;

    [SerializeField] private Transform rig = null;

    [SerializeField] private OVRCameraRig ovrRig = null;
    [SerializeField] private OVRCameraRigRef ovrRigRef = null;

    private bool useRecordedPoses = false;

    public string[] splitString;
    public string[] headSplit;
    public string[] leftHandSplit;
    public string[] leftControllerAnchorSplit;
    public string[] rightHandSplit;
    public string[] rightControllerAnchorSplit;

    public override void StartReplayMode()
    {
        base.StartReplayMode();
        useRecordedPoses = true;
    }


    private void Awake()
    {
        // prevent assert error when the Hand script is null in HandVisual start()
        handVisualLeft.enabled = false;
        handVisualRight.enabled = false;
    }

    private void OnEnable()
    {

    }

    private void init()
    {

        if (!useRecordedPoses)
        {
            ovrRig = FindObjectOfType<OVRCameraRig>(true); //both active and inactive objects
            if (ovrRig != null)
            {
                ovrRigRef = FindObjectOfType<OVRCameraRigRef>(true);

                Hand leftHandScript = ovrRigRef.gameObject.transform.Find("Hands/LeftHand").GetComponent<Hand>();
                handVisualLeft.Hand = leftHandScript;

                Hand rightHandScript = ovrRigRef.gameObject.transform.Find("Hands/RightHand").GetComponent<Hand>();
                handVisualRight.Hand = rightHandScript;

                handVisualLeft.enabled = true;
                handVisualRight.enabled = true;

            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    public override void ApplyValue(string type, string value)
    {
        base.ApplyValue(type, value);
        if (type == logtype.VRRig.ToString())
        {

            splitString = value.Split('_');

            headSplit = splitString[0].Split('|');
            leftHandSplit = splitString[1].Split('|');
            leftControllerAnchorSplit = splitString[2].Split('|');
            rightHandSplit = splitString[3].Split('|');
            rightControllerAnchorSplit = splitString[4].Split('|');

            rig.position = parseVector3(headSplit[0]);
            rig.rotation = parseQuaternion(headSplit[1]);
            rig.localScale = parseVector3(headSplit[2]);

            //ovrRig.centerEyeAnchor.position = parseVector3(headSplit[0]);
            ovrRig.centerEyeAnchor.rotation = parseQuaternion(headSplit[1]);
            //ovrRig.centerEyeAnchor.localScale = parseVector3(headSplit[2]);

            head.position = parseVector3(headSplit[0]);
            head.rotation = parseQuaternion(headSplit[1]);
            head.localScale = parseVector3(headSplit[2]);

            leftHand.position = parseVector3(leftHandSplit[0]);
            leftHand.rotation = parseQuaternion(leftHandSplit[1]);
            leftHand.localScale = parseVector3(leftHandSplit[2]);

            leftControllerAnchor.position = parseVector3(leftControllerAnchorSplit[0]);
            leftControllerAnchor.rotation = parseQuaternion(leftControllerAnchorSplit[1]);
            leftControllerAnchor.localScale = parseVector3(leftControllerAnchorSplit[2]);

            rightHand.position = parseVector3(rightHandSplit[0]);
            rightHand.rotation = parseQuaternion(rightHandSplit[1]);
            rightHand.localScale = parseVector3(rightHandSplit[2]);

            rightControllerAnchor.position = parseVector3(rightControllerAnchorSplit[0]);
            rightControllerAnchor.rotation = parseQuaternion(rightControllerAnchorSplit[1]);
            rightControllerAnchor.localScale = parseVector3(rightControllerAnchorSplit[2]);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!useRecordedPoses)
        {
            StringBuilder sb = new StringBuilder();
            Utils.CopyTransform(ovrRig.centerEyeAnchor, head);
            sb.Append($"{ovrRig.centerEyeAnchor.position}|{ovrRig.centerEyeAnchor.rotation}|{ovrRig.centerEyeAnchor.localScale}");
            Utils.CopyTransform(ovrRig.leftHandAnchor, leftHand);
            sb.Append($"_{ovrRig.leftHandAnchor.position}|{ovrRig.leftHandAnchor.rotation}|{ovrRig.leftHandAnchor.localScale}");
            Utils.CopyTransform(ovrRig.leftControllerAnchor, leftControllerAnchor);
            sb.Append($"_{ovrRig.leftControllerAnchor.position}|{ovrRig.leftControllerAnchor.rotation}|{ovrRig.leftControllerAnchor.localScale}");
            Utils.CopyTransform(ovrRig.rightHandAnchor, rightHand);
            sb.Append($"_{ovrRig.rightHandAnchor.position}|{ovrRig.rightHandAnchor.rotation}|{ovrRig.rightHandAnchor.localScale}");
            Utils.CopyTransform(ovrRig.rightControllerAnchor, rightControllerAnchor);
            sb.Append($"_{ovrRig.rightControllerAnchor.position}|{ovrRig.rightControllerAnchor.rotation}|{ovrRig.rightControllerAnchor.localScale}");
            Map.UpdateOrCreate(new KVPair<logtype, string>(logtype.VRRig, sb.ToString()));
        }
    }

    private void LateUpdate()
    {

    }
}
