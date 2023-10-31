using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Checkpoints
{   
    public class CheckpointController : MonoBehaviour
    {

        [SerializeField] private List<Checkpoint> checkpointsController;
        [SerializeField] private List<Checkpoint> checkpointsHands;
        private List<Checkpoint> _checkpoints;
        [SerializeField] private string nextSceneName;
        private Checkpoint _currentCheckpoint;
        [SerializeField] private Transform ControllerStartPosition;
        [SerializeField] private Transform HandsStartPosition;
        [SerializeField] private Transform PlayerTransform;
        [SerializeField] private TutorialUIManager _uiManager;
        
        private AsyncOperation _asyncOperation;
        public string NextSceneName => nextSceneName;
        
        private void Start()
        {
            Assert.IsTrue(checkpointsController.Count >= 1);
            Assert.IsTrue(checkpointsHands.Count >= 1);
            Assert.IsTrue(!ControllerStartPosition.IsUnityNull());
            Assert.IsTrue(!HandsStartPosition.IsUnityNull());
            Assert.IsTrue(!PlayerTransform.IsUnityNull());
            ValidateCheckpointOrder();
            DisableAllCheckpoints();
            
            // Start scene preloading.
            StartCoroutine(LoadNextSceneAsyncProcess());

        }


        private void ValidateCheckpointOrder()
        {
            for (int i = 0; i < checkpointsController.Count; i++)
            {
                if (checkpointsController[i].CheckpointNumber != i)
                {
                    Debug.LogError("Error: Checkpoints not in chronological Order! CheckpointController: "+ checkpointsController[i].CheckpointNumber + " Expected: " + i);
                }           
            }
            for (int i = 0; i < checkpointsHands.Count; i++)
            {
                 if (checkpointsHands[i].CheckpointNumber != i)
                 {
                     Debug.LogError("Error: Checkpoints not in chronological Order! CheckpointHands: "+ checkpointsHands[i].CheckpointNumber + " Expected: " + i);
                 }           
            }
        }

        private void DisableAllCheckpoints()
        {
            foreach (var checkpoint in checkpointsController)
            {
                checkpoint.SetEnabled(false);
            }

            foreach (var checkpoint in checkpointsHands)
            {
                checkpoint.SetEnabled(false);
            }
        }

        public void SetCheckpointController()
        {
            _checkpoints = checkpointsController;
            InitializeCheckpoints();
            PlayerTransform.position = ControllerStartPosition.position;
            _uiManager.HidePanels();
            _uiManager.SetControllerPanel();
        }

        public void SetCheckpointHands()
        {
            _checkpoints = checkpointsHands;
            InitializeCheckpoints();
            PlayerTransform.position = HandsStartPosition.position;
            _uiManager.HidePanels();
            _uiManager.SetHandPanel();
        }
        
        private void InitializeCheckpoints()
        {
            foreach (var checkpoint in _checkpoints)
            {
                checkpoint.SetEnabled(false);
            }
            
            SetNextCheckpoint(_checkpoints[0]);
        }

        private void SetNextCheckpoint()
        {
            // Check if we are at the last checkpoint
            if (_currentCheckpoint.CheckpointNumber+1 >= _checkpoints.Count)
            {
                _currentCheckpoint.SetEnabled(false);
                LastCheckpointReached();
                return;
            }
            SetNextCheckpoint(_checkpoints[_currentCheckpoint.CheckpointNumber + 1]);
        }

        private void SetNextCheckpoint(Checkpoint nextCheckpoint)
        {
            if (_currentCheckpoint is not null)
            {
                _currentCheckpoint.SetEnabled(false);
                _currentCheckpoint.OnCheckpointReached -= OnCurrentCheckpointReached;
            }

            _currentCheckpoint = nextCheckpoint;

            _currentCheckpoint.SetEnabled(true);
            _currentCheckpoint.OnCheckpointReached += OnCurrentCheckpointReached;
        }

        
        private void OnCurrentCheckpointReached()
        {
            SetNextCheckpoint();
        }

        private void LastCheckpointReached()
        {
            // Show popup UI
            _uiManager.ShowPanel();
        }

        public void ContinueNextScene()
        {
            // Activate next scene
            _asyncOperation.allowSceneActivation = true;
        }
        
        private IEnumerator LoadNextSceneAsyncProcess()
        {
            // Begin to load the Scene you have specified.
            _asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);

            // Don't let the Scene activate until you allow it to.
            _asyncOperation.allowSceneActivation = false;

            while (!_asyncOperation.isDone)
            {
                // Debug.Log($"[scene]:{nextScenePath} [load progress]: {_asyncOperation.progress}");

                yield return null;
            }
        }
    }
    
    
}