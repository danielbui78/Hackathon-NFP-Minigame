using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GameRunner : MonoBehaviour
{
    [SerializeField] private BallLauncher ballLauncher;
    [SerializeField] private List<Transform> leftTargets;
    [SerializeField] private List<Transform> centerTargets;
    [SerializeField] private List<Transform> rightTargets;

    [SerializeField] private StarterAssetsInputs playerInput;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform kickPos;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject goalieObj;

    private TargetSection targetSection;
    private bool isLaunched;
    private bool isRunning;

    public enum TargetSection
    {
        None,
        Left,
        Center,
        Right
    }

    private void Start()
    {
        StopGame();
    }

    public void StartGame()
    {
        isRunning = true;

        playerObj.SetActive(true);
        goalieObj.SetActive(true);
    }

    public void StopGame()
    {
        isLaunched = false;
        isRunning = false;

        playerObj.SetActive(false);
        goalieObj.SetActive(false);
    }

    private Transform GetTarget()
    {
        targetSection = (TargetSection)Random.Range(1, 4);
        int index = 0;
        switch(targetSection)
        {
            case TargetSection.Left:
                if(leftTargets.Count == 0) break;
                index = Random.Range(0, leftTargets.Count);
                return leftTargets[index];
            case TargetSection.Center:
                if(centerTargets.Count == 0) break;
                index = Random.Range(0, centerTargets.Count);
                return centerTargets[index];
            case TargetSection.Right:
                if(rightTargets.Count == 0) break;
                index = Random.Range(0, rightTargets.Count);
                return rightTargets[index];
        }
        return null;
    }

    private void Update()
    {
        if (isRunning && Input.GetKeyDown(KeyCode.K))
        {
            if (!isLaunched) MovePlayerToKick();
        }
    }

    public void KickBall()
    {
        ballLauncher.Launch(GetTarget());
        isLaunched = true;
        StartCoroutine(WaitAndMovePlayerToStart());
    }

    private void MovePlayerToKick()
    {
        Vector3 direction = kickPos.position - playerInput.transform.position;
        Vector2 moveDirection = new Vector2(direction.x, direction.z).normalized;
        playerInput.MoveInput(moveDirection);
    }

    public void StopPlayer()
    {
        playerInput.MoveInput(Vector3.zero);
    }

    public void StopAndTurnPlayer()
    {
        StartCoroutine(StopPlayerCoroutine());
    }

    private IEnumerator StopPlayerCoroutine()
    {
        Vector3 direction = kickPos.position - playerInput.transform.position;
        Vector2 lookDirection = new Vector2(direction.x, direction.z).normalized;
        playerInput.MoveInput(lookDirection);

        yield return new WaitForSeconds(0.2f);

        playerInput.MoveInput(Vector2.zero);
        isLaunched = false;

        yield return null;
    }

    private IEnumerator WaitAndMovePlayerToStart()
    {
        yield return new WaitForSeconds(1.0f);

        Vector3 direction = startPos.position - playerInput.transform.position;
        Vector2 moveDirection = new Vector2(direction.x, direction.z).normalized;
        playerInput.MoveInput(moveDirection);

        yield return null;
    }

    public void ResetBallPosition()
    {
        ballLauncher.ResetBall();
    }
}
