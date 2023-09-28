using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    public class PlayerBoundaryController : MonoBehaviour
    {

        [SerializeField] private GameObject boundaryWarningPopup;
        [SerializeField] private float pushBackMagnitude = 1000f;
        private Animator _boundaryWarningAnimator;
        private bool _popupIsActive;
        // Caching property index for optimization
        private static readonly int Show = Animator.StringToHash("Show");
        private const float AnimationTime = 3f; // Adjust if animation is changed
        private Vector3 _boundaryMin, _boundaryMax;
        

        private void Awake()
        {
            // Setting and caching variables
            _popupIsActive = false;
            boundaryWarningPopup.SetActive(false);
            _boundaryWarningAnimator = boundaryWarningPopup.GetComponent<Animator>();
            _boundaryMin = gameObject.GetComponent<BoxCollider>().bounds.min;
            _boundaryMax = gameObject.GetComponent<BoxCollider>().bounds.max;
        }

        // Trigger Warning Popup once the player has left the boundaries.
        // Note: parameter argument should always be player, since we exclude all other layers!
        private void OnTriggerExit(Collider player)
        {
            PushBackPlayer(player);
            
            // Don't activate popup if it is still showing.
            if (_popupIsActive) return;
            StartCoroutine(ShowPopUp(AnimationTime));
        }

        private void OnTriggerEnter(Collider player)
        {
            // boundaryWarningPopup.SetActive(false);
            // Debug.Log("Player Entered Area!");
        }


        // Coroutine to show Warning popup animation and then disable it after it finishes.
        private IEnumerator ShowPopUp(float waitTime)
        {
            boundaryWarningPopup.SetActive(true);
            _popupIsActive = true;
            _boundaryWarningAnimator.SetTrigger(Show);
            yield return new WaitForSeconds(waitTime);
            boundaryWarningPopup.SetActive(false);
            _popupIsActive = false;
        }


        private Vector3 CalculatePushBackForce(Collider player)
        {
            var playerPosition = player.transform.position;
            float forceX = 0, forceZ = 0; // Currently, we don't care about the Y-axis.
            
            // Check if we are out of bounds on the X-Axis
            float temp = playerPosition.x-_boundaryMin.x;
            if (temp < 0)
            {
                forceX = temp;
            } else if (playerPosition.x-_boundaryMax.x > 0)
                forceX = playerPosition.x-_boundaryMax.x;
            
            // Check if we are out of bounds on the Z-Axis
            temp = playerPosition.z-_boundaryMin.z;
            if (temp < 0 )
            {
                forceZ = temp;
            } else if (playerPosition.z-_boundaryMax.z> 0)
                forceZ = playerPosition.z-_boundaryMax.z;

            return pushBackMagnitude*new Vector3(-forceX, 0f, -forceZ);
        }

        private void PushBackPlayer(Collider player)
        {
            var pushBackForce = CalculatePushBackForce(player);
            player.gameObject.GetComponent<Rigidbody>().AddForce(pushBackForce,ForceMode.Acceleration);
            // Debug.Log("Player: " + _gameObject.name + " Rigidbody: " + player.gameObject.GetComponent<Rigidbody>());
            // Debug.Log("Push Back Force: " + pushBackForce + " Applied");
            
        }
    }
}
