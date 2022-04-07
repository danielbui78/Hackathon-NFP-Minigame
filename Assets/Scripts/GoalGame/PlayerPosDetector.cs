using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosDetector : MonoBehaviour
{
    [SerializeField] private GameRunner gameRunner;
    [SerializeField] private bool isKickPosition;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(isKickPosition)
            {
                gameRunner.KickBall();
                gameRunner.StopPlayer();
            }
            else
            {
                gameRunner.StopAndTurnPlayer();
            }
        }
    }
}
