using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Scripts.Checkpoints {
    public class CheckpointHandInputTrigger : Checkpoint
    {
        [SerializeField] private SwimmerHandTracking playerHands;
        [SerializeField] private GameObject handPoseTutorial;
        [SerializeField] private GameObject swimmingMotionTutorial;

        private Collider _collider;
        private int _currentStep;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            Assert.IsNotNull(_collider);
            Assert.IsTrue(_collider.isTrigger);
            
            handPoseTutorial.SetActive(true);
            swimmingMotionTutorial.SetActive(false);
        }
        

        private bool IsSwimPose() {
            // Check if the Grab button on Oculus Touch controllers is pressed
            bool leftHandPose = playerHands.IsLeftHandSwim;
            bool rightHandPose = playerHands.IsRightHandSwim;

            return (leftHandPose && rightHandPose);
        }

        private void Update() {
            if (IsSwimPose()) {
                handPoseTutorial.SetActive(false);
                swimmingMotionTutorial.SetActive(true);
            } else {
                handPoseTutorial.SetActive(true);
                swimmingMotionTutorial.SetActive(false);
            }
        }

        public void SwimDetection()
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