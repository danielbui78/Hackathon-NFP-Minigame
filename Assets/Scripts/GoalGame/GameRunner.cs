using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    [SerializeField] private BallLauncher ballLauncher;
    [SerializeField] private TargetSection targetSection;

    [SerializeField] private List<Transform> leftTargets;
    [SerializeField] private List<Transform> centerTargets;
    [SerializeField] private List<Transform> rightTargets;

    public enum TargetSection
    {
        None,
        Left,
        Center,
        Right
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ballLauncher.Launch(GetTarget());
        }
    }
}
