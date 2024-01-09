using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class SwimmerController : MonoBehaviour
    {
        [Header("Values")]
        [Tooltip("Determines the speed at which the player swims though the environment.")]
        [SerializeField]
        private float swimForce;

        [Tooltip("Determines the drag force of the water, slowing down the player.")] [SerializeField]
        private float dragForce;

        [Tooltip("Minimum force needed for the player to start swimming.")] [SerializeField]
        private float minVelocity;

        [Tooltip("Make the player drop naturally in the water.")] [SerializeField]
        private float gravity;

        [Tooltip("Maximum force applied to rigibody at once.")] [SerializeField]
        private float maxForce;

        [Tooltip(
            "When the Angle between the direction of the motion speed and the horizontal direction of the world is less than this Angle, the speed in the vertical direction is reduced.")]
        [SerializeField]
        private float limitedAngle;

        // private Vector3 defaultPalmDirectionLeft = new Vector3(1, 0, 0);
        // private Vector3 defaultPalmDirectionRight = new Vector3(-1, 0, 0);
        private Vector3 defaultPalmDirectionLeft = new Vector3(1, 0, 0);
        private Vector3 defaultPalmDirectionRight = new Vector3(-1, 0, 0);
        private Transform playerTransform; // Or any other method to get it


        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            // Assume playerTransform is the Transform component of the player object
            playerTransform = this.transform;

            // Get the initial rotation of the player
            Quaternion initialPlayerRotation = playerTransform.rotation;

            // Apply the initial rotation to the default palm directions
            defaultPalmDirectionLeft = initialPlayerRotation * defaultPalmDirectionLeft;
            defaultPalmDirectionRight = initialPlayerRotation * defaultPalmDirectionRight;
        }

        private void FixedUpdate()
        {
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

                localVelocity += leftHandAbsVelocity.normalized * componentLeft;
            }

            if (rightHandGrabbed)
            {
                Quaternion currentRightRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
                Vector3 currentPalmDirectionRight = currentRightRotation * defaultPalmDirectionRight;

                Vector3 rightHandAbsVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
                float componentRight = Vector3.Dot(rightHandAbsVelocity, currentPalmDirectionRight);
                if (componentRight < 0)
                {
                    componentRight = 0;
                }


                localVelocity += rightHandAbsVelocity.normalized * componentRight;
            }

            Vector3 worldVelocity = playerTransform.TransformVector(localVelocity);

            // Calculate the angle between the velocity and the horizontal direction
            Vector3 horizontalDirection = new Vector3(worldVelocity.x, 0, worldVelocity.z);
            float angleBetween = Vector3.Angle(horizontalDirection, worldVelocity);
            worldVelocity *= -1;

            if (angleBetween < limitedAngle)
            {
                // Smoothly reduce vertical speed
                localVelocity.y *= (angleBetween / limitedAngle);
            }

            if (worldVelocity.sqrMagnitude > minVelocity)
            {
                // Print the values to the Unity console

                // Clamping the force to ensure it doesn't exceed maxForce
                Vector3 forceToAdd = worldVelocity * swimForce;
                forceToAdd = Vector3.ClampMagnitude(forceToAdd, maxForce);
                _rigidbody.AddForce(forceToAdd, ForceMode.Acceleration);
            }

            if (_rigidbody.velocity.sqrMagnitude > 0.01f)
            {
                _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
            }

            // Apply constant downward force for gravity.
            _rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
       
    }
}