using UnityEngine;

public class DirectLook : MonoBehaviour
{
    public LayerMask IgnoreMe;
    public bool sameObject = false;
    public GameObject lookingAt = null;

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
}
