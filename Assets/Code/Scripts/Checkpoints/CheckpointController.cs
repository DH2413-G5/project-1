using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Code.Scripts.Checkpoints
{
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField] private List<Checkpoint> checkpoints;

        private Checkpoint _currentCheckpoint;

        
        private void Start()
        {
            Assert.IsTrue(checkpoints.Count >= 1);
            ValidateCheckpointOrder();
            InitializeCheckpoints();
        }


        private void ValidateCheckpointOrder()
        {
            const int startingCheckpoint = 0;
            bool startingCheckpointExists = false;
            foreach (var checkpoint in checkpoints.Where(checkpoint => checkpoint.CheckpointNumber == startingCheckpoint))
            {
                startingCheckpointExists = true;
            }

            if (!startingCheckpointExists)
            { 
                Debug.LogError("Error, no starting checkpoint. (checkpointNumber = 0)");
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
        }
        
    }
}