using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private float minVelocity;
        [Tooltip("Make the player drop naturally in the water.")]
        [SerializeField] private float gravity ;
        [Tooltip("Maximum force applied to rigibody at once.")]
        [SerializeField] private float maxForce;

        private Vector3 defaultPalmDirectionLeft = new Vector3(1, 0, 0);
        private Vector3 defaultPalmDirectionRight = new Vector3(-1, 0, 0);

        
        private Rigidbody _rigidbody;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        
        private void FixedUpdate() {

            // Check if the Grab button on Oculus Touch controllers is pressed
            bool leftHandGrabbed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
            bool rightHandGrabbed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
    
            Vector3 localVelocity = Vector3.zero;

            if (leftHandGrabbed) 
            {
                Quaternion currentLeftRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
                Vector3 currentPalmDirectionLeft = currentLeftRotation * defaultPalmDirectionLeft;
                
                Vector3 leftHandAbsVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
                float componentLeft = Vector3.Dot(leftHandAbsVelocity, currentPalmDirectionLeft);
                if (componentLeft < 0)
                {
                    componentLeft = 0;
                }
                localVelocity += currentPalmDirectionLeft.normalized * componentLeft;
            }

            Debug.Log("rightHandGrabbed"+rightHandGrabbed);
            if (rightHandGrabbed) 
            {
                Quaternion currentRightRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
                Vector3 currentPalmDirectionRight = currentRightRotation * defaultPalmDirectionRight;

                Vector3 rightHandAbsVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
                float componentRight = Vector3.Dot(rightHandAbsVelocity, currentPalmDirectionRight);
                Debug.Log("componentRight: " + componentRight);
                if (componentRight < 0)
                {
                    componentRight = 0;
                }
                localVelocity += currentPalmDirectionRight.normalized * componentRight;
            }
            // Print the velocities to Unity Console
            Debug.Log("Local Velocity: " + localVelocity);
            localVelocity *= -1;

            if (localVelocity.sqrMagnitude > minVelocity) {
                
                // Print the values to the Unity console
                Debug.Log("Local Velocity: " + localVelocity.ToString());
                
                // Clamping the force to ensure it doesn't exceed maxForce
                Vector3 forceToAdd = localVelocity  * swimForce;
                forceToAdd = Vector3.ClampMagnitude(forceToAdd, maxForce);
                _rigidbody.AddForce(forceToAdd, ForceMode.Acceleration);
            }

            if (_rigidbody.velocity.sqrMagnitude > 0.01f) {
                _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
            }
            
            // Apply constant downward force for gravity.
            _rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }

    }
    
    
}