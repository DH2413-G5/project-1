using UnityEngine;
using Oculus.Interaction.Input;

public class SwimmerAudioController : MonoBehaviour
{
    [Tooltip("Audio clip for the main swim stroke sound.")]
    [SerializeField] private AudioClip swimStroke;

    [Tooltip("Audio clip for the tail of the swim stroke sound.")]
    [SerializeField] private AudioClip swimStrokeTail;

    [Tooltip("Audio source for the left hand swim sounds.")]
    private AudioSource leftHandAudioSource;

    [Tooltip("Audio source for the right hand swim sounds.")]
    private AudioSource rightHandAudioSource;

    [Tooltip("Audio source for playing left hand swim stroke tail sounds.")]
    private AudioSource leftTailAudioSource;

    [Tooltip("Audio source for playing right hand swim stroke tail sounds.")]
    private AudioSource rightTailAudioSource;

    [Tooltip("Threshold value to determine if the hand is moving.")]
    [SerializeField] private float movementThreshold = 0.3f;

    [Tooltip("Speed at which audio volume decreases when hand stops moving.")]
    [SerializeField] private float audioFadeSpeed = 1.5f;
    
    [Tooltip("Coefficient to determine the relationship between controller speed and audio volume. A higher value means that the impact of speed changes on the volume will be smaller.")]
    [SerializeField] private float controllerVolumeCoefficient = 2f;
    
    [Tooltip("Coefficient to determine the relationship between hand tracking speed and audio volume. A higher value means that the impact of speed changes on the volume will be smaller.")]
    [SerializeField] private float handTrackingVolumeCoefficient = 10f;

    private Vector3 leftControllerVelocity;
    private Vector3 rightControllerVelocity;
    private Vector3 leftHandTrackingVelocity;
    private Vector3 rightHandTrackingVelocity;
    private float handTrackingVelocity;
    private float controllerVelocity;
    private SwimmerHandTracking _swimmerHandTracking;
    private (Vector3 leftVelocityVector, Vector3 rightVelocityVector) _velocities;

    private void Awake()
    {
        _swimmerHandTracking = GetComponent<SwimmerHandTracking>();
        leftHandAudioSource = gameObject.AddComponent<AudioSource>();
        rightHandAudioSource = gameObject.AddComponent<AudioSource>();
        leftTailAudioSource = gameObject.AddComponent<AudioSource>();
        rightTailAudioSource = gameObject.AddComponent<AudioSource>();
        leftTailAudioSource.loop = true;
        rightTailAudioSource.loop = true;
        leftTailAudioSource.clip = swimStrokeTail;
        rightTailAudioSource.clip = swimStrokeTail;
    }
    private void Update()
    {
        ProcessHandAudio(OVRInput.Controller.LTouch, leftHandAudioSource, leftTailAudioSource);
        ProcessHandAudio(OVRInput.Controller.RTouch, rightHandAudioSource, rightTailAudioSource);
    }

    private void ProcessHandAudio(OVRInput.Controller controller, AudioSource audioSource, AudioSource tailAudioSource)
    {
        
        if (controller == OVRInput.Controller.LTouch)
        {
            leftHandTrackingVelocity = _velocities.leftVelocityVector;
            leftControllerVelocity = OVRInput.GetLocalControllerVelocity(controller);
        }
        else if (controller == OVRInput.Controller.RTouch)
        {
            rightHandTrackingVelocity = _velocities.rightVelocityVector;
            rightControllerVelocity = OVRInput.GetLocalControllerVelocity(controller);
        }

        handTrackingVelocity=leftHandTrackingVelocity.magnitude+rightHandTrackingVelocity.magnitude;
        controllerVelocity = leftControllerVelocity.magnitude + rightControllerVelocity.magnitude;
        Debug.Log("handTrackingVelocity"+ handTrackingVelocity);
        if (!(handTrackingVelocity > movementThreshold || controllerVelocity > movementThreshold))
        {
            FadeOutAudio(audioSource);
            FadeOutAudio(tailAudioSource);
            return;
        }

        float speed;
        if(controllerVelocity > 0)
        {
            speed = controllerVelocity;
            audioSource.volume = (speed >= controllerVolumeCoefficient) ? 1f : speed / controllerVolumeCoefficient;  // 重置音量
            tailAudioSource.volume= audioSource.volume;
        }

        if (handTrackingVelocity>0)
        {
            speed = handTrackingVelocity;
            audioSource.volume = (speed >= handTrackingVolumeCoefficient) ? 1f : speed / handTrackingVolumeCoefficient;  // 重置音量
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
        if(controllerVelocity > 0)
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
        else
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
        Debug.Log("Received velocities: " + velocities);
        _velocities=velocities;
    }
}
