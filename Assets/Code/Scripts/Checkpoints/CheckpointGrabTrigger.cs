using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Checkpoints;
using UnityEngine;

/*
 * Used for the grabbing tutorial step. Checks whether an object was grabbed or not.
 */

public class CheckpointGrabTrigger : Checkpoint
{

    [SerializeField] private List<GameObject> grabbableGameObjects;

    private List<OVRGrabbable> _grabbableList;
    private bool _isEnabled;
    
    private void Start()
    {
        _grabbableList = new List<OVRGrabbable>(grabbableGameObjects.Count);
        foreach (var obj in grabbableGameObjects)
        {
            _grabbableList.Add(obj.GetComponent<OVRGrabbable>());
            obj.SetActive(false);
        }
        _isEnabled = false;
    }

    private void Update()
    {
        if (!_isEnabled) return;
        if (IsGrabbed())
        {
            OnCheckpointReached?.Invoke();   
        }
    }

    private bool IsGrabbed()
    {
        return _grabbableList.Any(grabbable => grabbable.isGrabbed);
    }

    public override void SetEnabled(bool isEnabled)
    {
        base.SetEnabled(isEnabled);
        if (!isEnabled) return;
        foreach (var obj in grabbableGameObjects)
        {
            obj.SetActive(true);
        }

        _isEnabled = true;
    }
}
