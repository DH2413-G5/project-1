using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Code.Scripts.Checkpoints
{
    [RequireComponent(typeof(Collider))]
    public class Checkpoint : MonoBehaviour
    {
        public UnityAction OnCheckpointReached;

        [SerializeField] private int checkpointNumber;
        [SerializeField] private GameObject visuals;
        private Collider _collider;
        
        public int CheckpointNumber => checkpointNumber;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            
            Assert.IsNotNull(_collider);
            Assert.IsTrue(_collider.isTrigger);
        }

        public void OnTriggerEnter(Collider other)
        {
            OnCheckpointReached?.Invoke();
        }

        public void SetEnabled(bool isEnabled)
        {
            SetTriggerActive(isEnabled);
            SetVisuals(isEnabled);
        }

        private void SetTriggerActive(bool isActive)
        {
            _collider.enabled = isActive;
        }

        private void SetVisuals(bool isActive)
        {
            visuals.SetActive(isActive);
        }
        
    }
}