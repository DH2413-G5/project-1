using UnityEngine;

namespace Code.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class SwimmerController : MonoBehaviour
    {
        [Header("Values")]
        [Tooltip("Determines the speed at which the player swims though the environment.")]
        [SerializeField] private float swimForce = 2f;
        [Tooltip("Determines the drag force of the water, slowing down the player.")]
        [SerializeField] private float dragForce = 1f;
        [Tooltip("Minimum force needed for the player to start swimming.")]
        [SerializeField] private float minForce;
        [Tooltip("Limits the amount of strokes per second.")]
        [SerializeField] private float minTimeBetweenStrokes;
        [Tooltip("Make the player drop naturally in the water.")]
        [SerializeField] private float gravity ;
        [Tooltip("Maximum force applied to rigibody at once.")]
        [SerializeField] private float maxForce;
    
        [Header("References")]
        [Tooltip("Determines which reference to use to apply the velocity to.")]
        [SerializeField] private Transform trackingReference;

        private Rigidbody _rigidbody;
        private float _cooldownTimer;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        
        private void FixedUpdate() {
        
            _cooldownTimer += Time.fixedDeltaTime;

            // Check if the Grab button on Oculus Touch controllers is pressed
            bool leftHandGrabbed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
            bool rightHandGrabbed = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.RTouch);
    
            Vector3 localVelocity = Vector3.zero;

            if (leftHandGrabbed) {
                // Get the left controller's velocity relative to the player
                Vector3 leftHandAbsVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
                localVelocity += leftHandAbsVelocity;
                Debug.Log("Left Hand Absolute Velocity: " + leftHandAbsVelocity);
            }
    
            if (rightHandGrabbed) {
                // Get the right controller's velocity relative to the player
                Vector3 rightHandAbsVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
                localVelocity += rightHandAbsVelocity;
                Debug.Log("Right Hand Absolute Velocity: " + rightHandAbsVelocity);
            }
            // Print the velocities to Unity Console
            Debug.Log("Local Velocity: " + localVelocity);
            localVelocity *= -1;

            if (_cooldownTimer > minTimeBetweenStrokes && localVelocity.sqrMagnitude > minForce * minForce) {
                Vector3 worldVelocity = trackingReference.TransformDirection(localVelocity);
                // Clamping the force to ensure it doesn't exceed maxForce
                Vector3 forceToAdd = worldVelocity  * swimForce;
                forceToAdd = Vector3.ClampMagnitude(forceToAdd, maxForce);
                _rigidbody.AddForce(forceToAdd, ForceMode.Acceleration);
                _cooldownTimer = 0f;
            }

            if (_rigidbody.velocity.sqrMagnitude > 0.01f) {
                _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
            }
            
            // Apply constant downward force for gravity.
            _rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
    }
}