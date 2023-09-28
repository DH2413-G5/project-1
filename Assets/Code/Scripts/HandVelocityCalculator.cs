using Oculus.Interaction;
using UnityEngine;
using Oculus.Interaction.Input;
using Oculus.Interaction.PoseDetection;
using Unity.VisualScripting;

public class HandVelocityCalculator : MonoBehaviour
{
    [Tooltip("Left hand's IHand implementation.")] [SerializeField, Interface(typeof(IHand))]
    private UnityEngine.Object _leftHand;

    private IHand LeftHand;

    [Tooltip("Right hand's IHand implementation.")] [SerializeField, Interface(typeof(IHand))]
    private UnityEngine.Object _rightHand;

    private IHand RightHand;

    [Tooltip("Left hand's JointDeltaProvider for caching joint deltas.")]
    [SerializeField, Interface(typeof(IJointDeltaProvider))]
    private UnityEngine.Object _leftJointDeltaProvider;

    private IJointDeltaProvider LeftJointDeltaProvider;

    [Tooltip("Right hand's JointDeltaProvider for caching joint deltas.")]
    [SerializeField, Interface(typeof(IJointDeltaProvider))]
    private UnityEngine.Object _rightJointDeltaProvider;

    private IJointDeltaProvider RightJointDeltaProvider;

    private JointDeltaConfig _leftJointDeltaConfig;
    private JointDeltaConfig _rightJointDeltaConfig;

    public Rigidbody playerRigidbody;

    [Tooltip("Minimum velocity needed for the player to start swimming.")]
    public float minVelocity;

    private float _lastUpdateTime;
    private Vector3 _previousRootPosePosition = Vector3.zero;
    private bool _isPreviousRootPoseSet = false;

    private void Awake()
    {
        LeftHand = _leftHand as IHand;
        RightHand = _rightHand as IHand;

        LeftJointDeltaProvider = _leftJointDeltaProvider as IJointDeltaProvider;
        RightJointDeltaProvider = _rightJointDeltaProvider as IJointDeltaProvider;

        _lastUpdateTime = Time.time;
    }

    private void Start()
    {
        // Register joints for both hands
        _leftJointDeltaConfig = new JointDeltaConfig(GetInstanceID(), new[] { HandJointId.HandWristRoot });
        _rightJointDeltaConfig = new JointDeltaConfig(GetInstanceID(), new[] { HandJointId.HandWristRoot });

        LeftJointDeltaProvider.RegisterConfig(_leftJointDeltaConfig);
        RightJointDeltaProvider.RegisterConfig(_rightJointDeltaConfig);
    }

    private void OnEnable()
    {
        if (_leftJointDeltaConfig != null)
        {
            LeftJointDeltaProvider.RegisterConfig(_leftJointDeltaConfig);
        }

        if (_rightJointDeltaConfig != null)
        {
            RightJointDeltaProvider.RegisterConfig(_rightJointDeltaConfig);
        }
    }

    private void OnDisable()
    {
        if (_leftJointDeltaConfig != null)
        {
            LeftJointDeltaProvider.UnRegisterConfig(_leftJointDeltaConfig);
        }

        if (_rightJointDeltaConfig != null)
        {
            RightJointDeltaProvider.UnRegisterConfig(_rightJointDeltaConfig);
        }
    }

    public (Vector3 leftVelocityVector, Vector3 rightVelocityVector) GetWristVelocities()
    {
        float deltaTime = Time.time - _lastUpdateTime;

        // Calculate left hand's velocity
        Vector3 leftVelocityVector = ComputeHandWristRootVelocity(LeftHand, LeftJointDeltaProvider, deltaTime);

        // Calculate right hand's velocity
        /*Vector3 rightVelocityVector = ComputeHandWristRootVelocity(RightHand, RightJointDeltaProvider, deltaTime);*/
        Vector3 rightVelocityVector = Vector3.zero;

        Debug.Log("Left Hand Velocity: " + leftVelocityVector + ", Right Hand Velocity: " + rightVelocityVector);

        _lastUpdateTime = Time.time;

        return (leftVelocityVector, rightVelocityVector);
    }


    private Vector3 ComputeHandWristRootVelocity(IHand hand, IJointDeltaProvider jointDeltaProvider, float deltaTime)
    {
        if (hand.GetRootPose(out Pose rootPose))
        {
            Vector3 palmDirection;
            if (hand.Handedness == Handedness.Right)
            {
                palmDirection = -rootPose.up;
            }
            else
            {
                palmDirection = rootPose.up;
            }

            Vector3 worldVelocity = Vector3.zero;

            // Calculate worldVelocity if we have previous root pose position
            if (_isPreviousRootPoseSet)
            {
                worldVelocity = (rootPose.position - _previousRootPosePosition) / deltaTime;
            }
            _previousRootPosePosition = rootPose.position;
            _isPreviousRootPoseSet = true;
            
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
            Vector3 palmVelocity = component * palmDirection.normalized;

            // Debugging info
            Debug.Log(
                $"deltaTime: {deltaTime}, " +
                $"worldVelocity: ({worldVelocity.x:F7}, {worldVelocity.y:F7}, {worldVelocity.z:F5}), " +
                $"playerVelocity: ({playerVelocity.x:F5}, {playerVelocity.y:F5}, {playerVelocity.z:F5}), " +
                $"palmDirection: ({palmDirection.x:F5}, {palmDirection.y:F5}, {palmDirection.z:F5}), " +
                $"relativeHandVelocity: ({relativeHandVelocity.x:F5}, {relativeHandVelocity.y:F5}, {relativeHandVelocity.z:F5}), " +
                $"component: {component}, " +
                $"palmVelocity: {palmVelocity}"
            );


            return palmVelocity;
        }

        return Vector3.zero; // Return a zero vector if data is unavailable
    }
}