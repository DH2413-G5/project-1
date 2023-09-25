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

    public (Vector3 leftVelocityVector, Vector3 rightVelocityVector) GetWristVelocities()
    {
        float deltaTime = Time.time - _lastUpdateTime;

        // Calculate left hand's velocity
        Vector3 leftVelocityVector = ComputeHandWristRootVelocity(LeftHand, LeftJointDeltaProvider, deltaTime);

        // Calculate right hand's velocity
        Vector3 rightVelocityVector = ComputeHandWristRootVelocity(RightHand, RightJointDeltaProvider, deltaTime);

        _lastUpdateTime = Time.time;

        return (leftVelocityVector, rightVelocityVector);
    }


    private Vector3 ComputeHandWristRootVelocity(IHand hand, IJointDeltaProvider jointDeltaProvider, float deltaTime)
    {
        if (hand.GetRootPose(out Pose rootPose) &&
            hand.GetJointPose(HandJointId.HandWristRoot, out Pose curPose) &&
            jointDeltaProvider.GetPositionDelta(HandJointId.HandWristRoot, out Vector3 worldDeltaDirection))
        {
            Vector3 palmDirection = rootPose.up;  // This assumes that the up direction of the root pose is the direction perpendicular to the palm.
        
            // Convert position delta to velocity by dividing by deltaTime.
            Vector3 worldVelocity = worldDeltaDirection / deltaTime;
            
            // Project velocity onto the palm direction.
            float component = Vector3.Dot(worldVelocity, palmDirection);
            /*Debug.Log("component:"+component);*/
            if (component<0)
            {
                component = Mathf.Sqrt(-component) * -1; 
            }
            // Get the velocity vector in the palm direction.
            Vector3 palmVelocity = component * palmDirection.normalized;

            // Return the velocity vector in the palm direction or a zero vector if its magnitude is less than 0.1.
            return palmVelocity;
        }

        return Vector3.zero;  // Return a zero vector if data is unavailable
    }

}
