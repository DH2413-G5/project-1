using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Code.Scripts.Checkpoints
{   
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField] private List<Checkpoint> checkpoints;
        [SerializeField] private string nextSceneName;
        private Checkpoint _currentCheckpoint;
        
        private AsyncOperation _asyncOperation;
        public string NextSceneName => nextSceneName;
        
        private void Start()
        {
            Assert.IsTrue(checkpoints.Count >= 1);
            ValidateCheckpointOrder();
            InitializeCheckpoints();
            
            // Start scene preloading.
            StartCoroutine(LoadNextSceneAsyncProcess());

        }


        private void ValidateCheckpointOrder()
        {
            for (int i = 0; i < checkpoints.Count; i++)
            {
                if (checkpoints[i].CheckpointNumber != i)
                {
                    Debug.LogError("Error: Checkpoints not in chronological Order! Checkpoint: "+ checkpoints[i].CheckpointNumber + " Expected: " + i);
                }           
            }
        }

        private void InitializeCheckpoints()
        {
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.SetEnabled(false);
            }
            
            SetNextCheckpoint(checkpoints[0]);
        }

        private void SetNextCheckpoint()
        {
            // Check if we are at the last checkpoint
            if (_currentCheckpoint.CheckpointNumber+1 >= checkpoints.Count)
            {
                LastCheckpointReached();
                return;
            }
            SetNextCheckpoint(checkpoints[_currentCheckpoint.CheckpointNumber + 1]);
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
            // TODO Switch to game scene
            Debug.Log("LAST CHECKPOINT REACHED");
            
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