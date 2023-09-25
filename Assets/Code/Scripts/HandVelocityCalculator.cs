using Oculus.Interaction;
using UnityEngine;
using Oculus.Interaction.Input;
using Oculus.Interaction.PoseDetection;

public class HandVelocityCalculator : MonoBehaviour
{
    [Tooltip("Left hand's IHand implementation.")]
    [SerializeField, Interface(typeof(IHand))]
    private UnityEngine.Object _leftHand;
    private IHand LeftHand;

    [Tooltip("Right hand's IHand implementation.")]
    [SerializeField, Interface(typeof(IHand))]
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

    private float _lastUpdateTime;

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

    public (float leftVelocity, float rightVelocity) GetWristVelocities()
    {
        float deltaTime = Time.time - _lastUpdateTime;

        // Calculate left hand's velocity
        float leftVelocity = ComputeHandWristRootVelocity(LeftHand, LeftJointDeltaProvider, deltaTime);

        // Calculate right hand's velocity
        float rightVelocity = ComputeHandWristRootVelocity(RightHand, RightJointDeltaProvider, deltaTime);

        _lastUpdateTime = Time.time;

        return (leftVelocity, rightVelocity);
    }

    private float ComputeHandWristRootVelocity(IHand hand, IJointDeltaProvider jointDeltaProvider, float deltaTime)
    {
        if (hand.GetRootPose(out Pose rootPose) &&
            hand.GetJointPose(HandJointId.HandWristRoot, out Pose curPose) &&
            jointDeltaProvider.GetPositionDelta(HandJointId.HandWristRoot, out Vector3 worldDeltaDirection))
        {
            Vector3 palmDirection = rootPose.up;  // This assumes that the up direction of the root pose is the direction perpendicular to the palm.
            float velocityMagnitude = worldDeltaDirection.magnitude / deltaTime;
        
            // Project velocity onto the palm direction.
            float component = Vector3.Dot(worldDeltaDirection.normalized, palmDirection);
            /*Debug.Log($"Vector3: {palmDirection}");*/
            
            // Multiply the component by the velocity magnitude.
            float palmComponentVelocity = velocityMagnitude * component;

            // Return the velocity component in the palm direction or 0 if its absolute value is less than 0.1.
            return palmComponentVelocity;
        }

        return 0.0f;  // Return 0 if data is unavailable
    }
}
