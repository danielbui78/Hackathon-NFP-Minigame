using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalieFollow : MonoBehaviour
{
    [SerializeField] private Transform goalie;
    [SerializeField] private GoalCounter goalCounter;

    private void Update()
    {
        var curPos = transform.position;
        curPos.x = goalie.position.x;
        transform.position = curPos;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            goalCounter.UpdateTurns();
        }
    }
}
