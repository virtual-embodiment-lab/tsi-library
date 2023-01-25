using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScript : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        animator = GetComponent<Animator>();

        while(true) {
            yield return new WaitForSeconds(Random.Range(0, 10));
            // weights should follow state diagram construction (index 0 should be the weight for phone animation, etc.)
            int[] weights = new int[]{7, 5, 2, 1, 6, 4, 3, 7, 8, 3, 4};
            animator.SetInteger("ActionIndex", getRandomIndex(weights));
            animator.SetTrigger("Action");
        }
    }

    /**
    Returns a random index within [0, weights.Length) with weighted probabilities for each index given in the array.
    The probability index i is selected is weights[i] / weights.Sum().
    Requires: weights[i] >= 1
    **/
    private int getRandomIndex(int[] weights) {
        for(int i=1; i<weights.Length; i++) {
            weights[i] += weights[i-1];
        }
        int indexVal = Random.Range(1, weights[weights.Length-1]+1);
        for(int i=0; i<weights.Length; i++) {
            if(indexVal <= weights[i]) return i;
        }
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
