using System.Collections;
using UnityEngine;

public class BoundaryChecker : MonoBehaviour
{

    [SerializeField] private GameObject boundaryWarningPopup;
    private Animator _boundaryWarningAnimator;
    private bool _popupIsActive;
    private float _animationTime = 3f; // Adjust if animation is changed
    private void Awake()
    {
        _popupIsActive = false;
        boundaryWarningPopup.SetActive(false);
        _boundaryWarningAnimator = boundaryWarningPopup.GetComponent<Animator>();
    }

    private void OnTriggerExit(Collider other)
    {
        // boundaryWarningPopup.SetActive(true);
        // _boundaryWarningAnimator.SetTrigger("Show");
        Debug.Log("Player Exited Area! " +" Active Popup:"+  _popupIsActive);
        if (_popupIsActive) return;
        StartCoroutine(ShowPopUp(_animationTime));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // boundaryWarningPopup.SetActive(false);
        Debug.Log("Player Entered Area!");
    }


    private IEnumerator ShowPopUp(float waitTime)
    {
        Debug.Log("Played");
        boundaryWarningPopup.SetActive(true);
        _popupIsActive = true;
        _boundaryWarningAnimator.SetTrigger("Show");
        yield return new WaitForSeconds(waitTime);
        boundaryWarningPopup.SetActive(false);
        _popupIsActive = false;
    }
}
