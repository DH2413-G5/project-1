using UnityEngine;
using UnityEngine.Assertions;

/*
 * Used for the first hand-tracking checkpoint. A two-step checkpoint, to first teach the swimming pose and then the
 * swimming motion to the player.
 */

namespace Code.Scripts.Checkpoints {
    public class CheckpointHandInputTrigger : Checkpoint
    {
        [SerializeField] private SwimmerHandTracking playerHands;
        // Visuals to show the player which pose to strike to get into swimming mode.
        [SerializeField] private GameObject handPoseTutorial;
        // Visuals to show the player how to swim forward.
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
            // Check if hands are in the SwimmingPose
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
            OnCheckpointReached?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            SwimDetection();
        }
    }
}