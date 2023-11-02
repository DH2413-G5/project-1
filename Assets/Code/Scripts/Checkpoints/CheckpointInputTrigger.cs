using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Scripts.Checkpoints {
    public class CheckpointInputTrigger : Checkpoint {
        [SerializeField] private GameObject buttonsPressedTutorial;
        [SerializeField] private GameObject swimmingMotionTutorial;

        private Collider _collider;
        private int _currentStep;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            Assert.IsNotNull(_collider);
            Assert.IsTrue(_collider.isTrigger);
            
            buttonsPressedTutorial.SetActive(true);
            swimmingMotionTutorial.SetActive(false);
        }
        

        private bool GripButtonsPressed() {
            // Check if the Grab button on Oculus Touch controllers is pressed
            bool leftHandGrabbed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
            bool rightHandGrabbed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);

            return (leftHandGrabbed && rightHandGrabbed);
        }

        private void Update() {
            if (GripButtonsPressed()) {
                buttonsPressedTutorial.SetActive(false);
                swimmingMotionTutorial.SetActive(true);
            } else {
                buttonsPressedTutorial.SetActive(true);
                swimmingMotionTutorial.SetActive(false);
            }
        }

        private void SwimDetection()
        {
            // ADD SWIMMING DETECTION LOGIC
            OnCheckpointReached?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            SwimDetection();
        }
    }
}