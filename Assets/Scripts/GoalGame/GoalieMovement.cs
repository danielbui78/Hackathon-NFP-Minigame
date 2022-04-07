using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GoalieMovement : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs playerInput;
    [SerializeField] private float waitTime = 3.0f;

    private void Start()
    {
        StartCoroutine(MovementRoutine());
    }

    private IEnumerator MovementRoutine()
    {
        playerInput.MoveInput(new Vector2(1, 0));
        yield return new WaitForSeconds(waitTime / 2);

        while(true)
        {
            playerInput.MoveInput(new Vector2(-1, 0));
            yield return new WaitForSeconds(waitTime);
            playerInput.MoveInput(new Vector2(1, 0));
            yield return new WaitForSeconds(waitTime);
        }
    }
}
