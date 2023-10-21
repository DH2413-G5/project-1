using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Code.Scripts.Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        public UnityAction OnCheckpointReached;

        [SerializeField] private int checkpointNumber;
        [SerializeField] protected GameObject visuals;
        
        public int CheckpointNumber => checkpointNumber;
        
        public virtual void SetEnabled(bool isEnabled)
        {
            SetVisuals(isEnabled);
        }

        
        protected void SetVisuals(bool isActive)
        {
            visuals.SetActive(isActive);
        }
        
    }
}