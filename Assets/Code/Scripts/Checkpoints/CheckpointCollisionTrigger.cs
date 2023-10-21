using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Scripts.Checkpoints
{
    [RequireComponent(typeof(Collider))]
    public class CheckpointCollisionTrigger : Checkpoint
    {
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            
            Assert.IsNotNull(_collider);
            Assert.IsTrue(_collider.isTrigger);
        }
        
        
        public override void SetEnabled(bool isEnabled)
        {
            SetTriggerActive(isEnabled);
            SetVisuals(isEnabled);
        }
        
        private void SetTriggerActive(bool isActive)
        {
            _collider.enabled = isActive;
        }

        public void OnTriggerEnter(Collider other)
        {
            OnCheckpointReached?.Invoke();
        }
        
    }
}