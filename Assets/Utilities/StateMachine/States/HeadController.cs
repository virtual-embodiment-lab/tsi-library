using UnityEngine;

[RequireComponent(typeof(Animator))]

public class HeadController : MonoBehaviour
{

    protected Animator animator;
    public bool ikActive = false;
    public Transform lookObj = null;
    public float lookWeight = 0.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetLookObject(Transform obj)
    {
        lookObj = obj;
    }

    public void SetActive(bool active)
    {
        ikActive = active;
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {

        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {
                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(lookWeight);
                    animator.SetLookAtPosition(lookObj.position);
                }

            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                animator.SetLookAtWeight(0);
            }
        }
    }
}