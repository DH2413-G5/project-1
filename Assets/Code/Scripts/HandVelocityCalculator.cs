using Oculus.Interaction;
using UnityEngine;
using Oculus.Interaction.Input;
using Oculus.Interaction.PoseDetection;
using Unity.VisualScripting;

public class HandVelocityCalculator : MonoBehaviour
{
    [Tooltip("Left hand's IHand implementation.")] [SerializeField, Interface(typeof(IHand))]
    private Object _leftHand;

    private IHand LeftHand;

    [Tooltip("Right hand's IHand implementation.")] [SerializeField, Interface(typeof(IHand))]
    private Object _rightHand;

    private IHand RightHand;

    public Rigidbody playerRigidbody;

    [Tooltip("Minimum velocity needed for the player to start swimming.")]
    public float minVelocity;

    private float _lastUpdateTime;
    private Vector3 _previousLeftHandPos = Vector3.zero;
    private Vector3 _previousRightHandPos = Vector3.zero;
    private bool _isPreviousLeftPos = false;
    private bool _isPreviousRightPos = false;

    private void Awake()
    {
        LeftHand = _leftHand as IHand;
        RightHand = _rightHand as IHand;

        _lastUpdateTime = Time.time;
    }

    public (Vector3 leftVelocityVector, Vector3 rightVelocityVector) GetWristVelocities()
    {
        float deltaTime = Time.time - _lastUpdateTime;

        // Calculate left hand's velocity
        Vector3 leftVelocityVector = ComputeHandWristRootVelocity(LeftHand, deltaTime);

        // Calculate right hand's velocity
        Vector3 rightVelocityVector = ComputeHandWristRootVelocity(RightHand, deltaTime);

        /*Debug.Log("Left Hand Velocity: " + leftVelocityVector + ", Right Hand Velocity: " + rightVelocityVector);*/

        _lastUpdateTime = Time.time;

        return (leftVelocityVector, rightVelocityVector);
    }


    private Vector3 ComputeHandWristRootVelocity(IHand hand, float deltaTime)
    {
        if (hand.GetRootPose(out Pose rootPose))
        {
            Vector3 palmDirection;
            Vector3 worldVelocity = Vector3.zero;
            // Calculate worldVelocity if we have previous root pose position
            if (hand.Handedness == Handedness.Right)
            {
                palmDirection = -rootPose.up;
                if (_isPreviousRightPos)
                {
                    worldVelocity = (rootPose.position - _previousRightHandPos) / deltaTime;
                }
                _previousRightHandPos = rootPose.position;
                _isPreviousRightPos = true;
            }
            else
            {
                palmDirection = rootPose.up;
                if (_isPreviousLeftPos)
                {
                    worldVelocity = (rootPose.position - _previousLeftHandPos) / deltaTime;
                }
                _previousLeftHandPos = rootPose.position;
                _isPreviousLeftPos = true;
            }
            
            Vector3 playerVelocity = playerRigidbody.velocity;
            Vector3 relativeHandVelocity = worldVelocity - playerVelocity;

            // If the magnitude of relativeHandVelocity is below the threshold, return zero vector
            if (relativeHandVelocity.magnitude < minVelocity)
            {
                return Vector3.zero;
            }

            // Project velocity onto the palm direction.
            float component = Vector3.Dot(relativeHandVelocity, palmDirection);

            if (component < 0)
            {
                component = 0;
            }

            // Get the velocity vector in the palm direction.
            Vector3 palmVelocity = component * relativeHandVelocity.normalized;

            // Debugging info
            /*Debug.Log(
                $"deltaTime: {deltaTime}, " +
                $"worldVelocity: ({worldVelocity.x:F7}, {worldVelocity.y:F7}, {worldVelocity.z:F5}), " +
                $"playerVelocity: ({playerVelocity.x:F5}, {playerVelocity.y:F5}, {playerVelocity.z:F5}), " +
                $"palmDirection: ({palmDirection.x:F5}, {palmDirection.y:F5}, {palmDirection.z:F5}), " +
                $"relativeHandVelocity: ({relativeHandVelocity.x:F5}, {relativeHandVelocity.y:F5}, {relativeHandVelocity.z:F5}), " +
                $"component: {component}, " +
                $"palmVelocity: {palmVelocity}"
            );*/


            return palmVelocity;
        }

        return Vector3.zero; // Return a zero vector if data is unavailable
    }
}