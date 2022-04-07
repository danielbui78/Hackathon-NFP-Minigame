using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private GoalCounter goalCounter;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            goalCounter.UpdateScore();
        }
    }
}
