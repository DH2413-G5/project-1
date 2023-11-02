using UnityEngine;
using Oculus.Interaction.Input;
using UnityEngine.Serialization;

public class SwimmerAudioController : MonoBehaviour
{
    [Header("Audio")]
    
    [Tooltip("Audio clip for the main swim stroke sound.")]
    [SerializeField] private AudioClip swimStroke;

    [Tooltip("Audio clip for the tail of the swim stroke sound.")]
    [SerializeField] private AudioClip swimStrokeTail;
    
    [Tooltip("Speed at which audio volume decreases when hand stops moving.")]
    [SerializeField] private float audioFadeSpeed = 1.5f;
    
    [Header("Hand Tracking")]
    
    [Tooltip("Threshold value to determine if the hand is moving.")]
    [SerializeField] private float handTrackingMovementThreshold = 0.3f;
    
    [Tooltip("Coefficient to determine the relationship between hand tracking speed and audio volume. A higher value means that the impact of speed changes on the volume will be smaller.")]
    [SerializeField] private float handTrackingVolumeCoefficient = 10f;
    
    [Tooltip("Window size for the moving average filter.")]
    [SerializeField] private int windowSize = 5;
    
    [Header("Controller")]
    [Tooltip("Threshold value to determine if the hand is moving.")]
    [SerializeField] private float controllerMovementThreshold = 0.4f;
    
    [Tooltip("Coefficient to determine the relationship between controller speed and audio volume. A higher value means that the impact of speed changes on the volume will be smaller.")]
    [SerializeField] private float controllerVolumeCoefficient = 2f;
    
    //Audio source for the left/right hand swim stroke sounds and swim stroke tail sounds.
    private AudioSource leftHandAudioSource;
    private AudioSource rightHandAudioSource;
    private AudioSource leftTailAudioSource;
    private AudioSource rightTailAudioSource;
    
    private MovingAverageFilter leftHandVelocityFilter;
    private MovingAverageFilter rightHandVelocityFilter;

    private Vector3 leftControllerVelocity;
    private Vector3 rightControllerVelocity;
    private Vector3 leftHandTrackingVelocity;
    private Vector3 rightHandTrackingVelocity;
    private float handTrackingVelocity;
    private float controllerVelocity;
    private (Vector3 leftVelocityVector, Vector3 rightVelocityVector) _velocities;
    
    private Vector3 previousLeftHandLocalPosition;
    private float previousLeftHandTime;
    private Vector3 previousRightHandLocalPosition;
    private float previousRightHandTime;

    private void Awake()
    {
        leftHandAudioSource = gameObject.AddComponent<AudioSource>();
        rightHandAudioSource = gameObject.AddComponent<AudioSource>();
        leftTailAudioSource = gameObject.AddComponent<AudioSource>();
        rightTailAudioSource = gameObject.AddComponent<AudioSource>();
        leftTailAudioSource.loop = true;
        rightTailAudioSource.loop = true;
        leftTailAudioSource.clip = swimStrokeTail;
        rightTailAudioSource.clip = swimStrokeTail;
        
        // 初始化滑动平均滤波器
        leftHandVelocityFilter = new MovingAverageFilter(windowSize);
        rightHandVelocityFilter = new MovingAverageFilter(windowSize);
    }
    private void FixedUpdate()
    {
        ProcessHandAudio(OVRInput.Controller.LTouch, leftHandAudioSource, leftTailAudioSource);
        ProcessHandAudio(OVRInput.Controller.RTouch, rightHandAudioSource, rightTailAudioSource);
    }

    private void ProcessHandAudio(OVRInput.Controller controller, AudioSource audioSource, AudioSource tailAudioSource)
    {
        
        if (controller == OVRInput.Controller.LTouch)
        {
            leftControllerVelocity = OVRInput.GetLocalControllerVelocity(controller);
            handTrackingVelocity= leftHandTrackingVelocity.magnitude;  
            /*Debug.Log("handTrackingVelocity:"+handTrackingVelocity);
            DebugUIManager.Instance.UpdateDebugText("handTrackingVelocity:" + handTrackingVelocity);*/
        }
        else if (controller == OVRInput.Controller.RTouch)
        {
            rightControllerVelocity= OVRInput.GetLocalControllerVelocity(controller);
            handTrackingVelocity= rightHandTrackingVelocity.magnitude;
        }

        controllerVelocity = leftControllerVelocity.magnitude + rightControllerVelocity.magnitude;
        if (!(handTrackingVelocity > (handTrackingMovementThreshold) || controllerVelocity > controllerMovementThreshold))
        {
            FadeOutAudio(audioSource);
            FadeOutAudio(tailAudioSource);
            return;
        }

        float speed;
        if(controllerVelocity > 0)
        {
            //Reset the volume
            speed = controllerVelocity;
            audioSource.volume = (speed >= controllerVolumeCoefficient) ? 1f : speed / controllerVolumeCoefficient;
            tailAudioSource.volume= audioSource.volume;
        }
        else if (handTrackingVelocity>0)
        {
            //Reset the volume
            speed = handTrackingVelocity;
            audioSource.volume = (speed >= handTrackingVolumeCoefficient) ? 1f : speed / handTrackingVolumeCoefficient;
            tailAudioSource.volume= audioSource.volume;
        }
        
        if (!audioSource.isPlaying && !tailAudioSource.isPlaying)
        {
            audioSource.clip = swimStroke;
            audioSource.Play();
            Invoke(nameof(PlaySwimStrokeTail), 0.2f);
        }
    }

    private void PlaySwimStrokeTail()
    {
        if (!leftTailAudioSource.isPlaying)
        { 
            leftTailAudioSource.Play();
        }
        if (!rightTailAudioSource.isPlaying)
        {
            rightTailAudioSource.Play();
        }
        
    }

    private void FadeOutAudio(AudioSource audioSource)
    {
        if (audioSource.isPlaying)
        {
            audioSource.volume -= audioFadeSpeed * Time.deltaTime;
            if (audioSource.volume <= 0)
            {
                audioSource.Stop();
            }
        }
    }
    
    public void ReceiveVelocities((Vector3 leftVelocityVector, Vector3 rightVelocityVector) velocities)
    {
        _velocities=velocities;
        // Debug.Log("ReceiveVelocities"+_velocities.leftVelocityVector.magnitude);
        // Use a moving average filter for processing speed
        leftHandTrackingVelocity = leftHandVelocityFilter.Process(_velocities.leftVelocityVector);
        rightHandTrackingVelocity = rightHandVelocityFilter.Process(_velocities.rightVelocityVector);
    }
}
