using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Rigidbody ball;
    [SerializeField] private float launchHeight = 25;
    [SerializeField] private float gravity = -18;
    [SerializeField] private bool debugPath;

    private Transform ballTarget;

    private void Start()
    {
        ball.useGravity = false;
    }

    private void Update()
    {
        if (debugPath)
        {
            DrawPath();
        }
    }

    public void Launch(Transform target)
    {
        ballTarget = target;
        Physics.gravity = Vector3.up * gravity;
        ball.position = transform.position;
        ball.useGravity = true;
        ball.velocity = CalculateLaunchData().initialVelocity;
    }

    private LaunchData CalculateLaunchData()
    {
        float displacementY = ballTarget.position.y - ball.position.y;
        Vector3 displacementXZ = new Vector3(ballTarget.position.x - ball.position.x, 0, ballTarget.position.z - ball.position.z);
        float time = Mathf.Sqrt(-2 * launchHeight / gravity) + Mathf.Sqrt(2 * (displacementY - launchHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * launchHeight);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    private void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = ball.position;

        int resolution = 30;
        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = ball.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
        }
    }

    private struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}
