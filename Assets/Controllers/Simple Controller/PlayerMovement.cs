using StandardLogging;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : Tracker
{
    public float walkingSpeed = 7.5f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    public LayerMask IgnoreMe;
    public bool sameObject = false;
    public GameObject lookingAt = null;

    [SerializeField] bool replayMode = false;

    public override void StartReplayMode()
    {
        base.StartReplayMode();
        replayMode = true;
    }

    void Start()
    {
        if (replayMode)
        {
            return;
        }
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void ApplyValue(string type, string value)
    {
        base.ApplyValue(type, value);
        if (type == logtype.State.ToString())
        {
            playerCamera.transform.localEulerAngles = parseVector3(value);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f, ~IgnoreMe))
        {
            AvatarInstance avatarInstance = hit.collider.transform.parent.gameObject.GetComponent<AvatarInstance>();
            if (avatarInstance != null)
            {
                avatarInstance.SetDirectLook(true);
                if (lookingAt != null && lookingAt.GetInstanceID() != hit.collider.gameObject.GetInstanceID() && lookingAt.transform.parent.gameObject.GetComponent<AvatarInstance>() != null)
                {
                    lookingAt.transform.parent.gameObject.GetComponent<AvatarInstance>().SetDirectLook(false);
                }
                lookingAt = hit.collider.gameObject;
            }
            else
            {
                if (lookingAt != null && lookingAt.transform.parent.gameObject.GetComponent<AvatarInstance>() != null)
                {
                    lookingAt.transform.parent.gameObject.GetComponent<AvatarInstance>().SetDirectLook(false);
                }
                lookingAt = hit.collider.gameObject;
            }
        }
    }

    protected override void Update()
    {
        if (replayMode)
        {
            return;
        }
        base.Update();
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        float curSpeedX = canMove ? (walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        moveDirection.y = movementDirectionY;

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            Map.UpdateOrCreate(new KVPair<logtype, string>(logtype.Vision, playerCamera.transform.localEulerAngles.ToString()));
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}