using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GoalieMovement : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs playerInput;
    [SerializeField] private Transform target;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goalie")
        {
            Vector3 direction = target.position - playerInput.transform.position;
            Vector2 moveDirection = new Vector2(direction.x, direction.z).normalized;
            playerInput.MoveInput(moveDirection);
        }
    }
}
