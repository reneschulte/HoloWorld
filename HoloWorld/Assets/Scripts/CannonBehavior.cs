using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.VR.WSA.Input;

public class CannonBehavior : MonoBehaviour
{
    private GestureRecognizer _gestureRecognizer;

    public float ForceMagnitude = 300f;
    public GameObject GazeCursor;
    public Material CannonMaterial;
    public AudioSource ShootSound;
    public AudioClip CollisionClip;

    void Start()
    {
        _gestureRecognizer = new GestureRecognizer();
        _gestureRecognizer.TappedEvent += GestureRecognizerOnTappedEvent;
        _gestureRecognizer.NavigationUpdatedEvent += GestureRecognizerOnNavigationUpdatedEvent;
        _gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.NavigationX | GestureSettings.NavigationY | GestureSettings.NavigationZ);
        _gestureRecognizer.StartCapturingGestures();
    }

    private void GestureRecognizerOnNavigationUpdatedEvent(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
    {
        Debug.LogFormat("Nav Upd: {0} Offset: {1}", Enum.GetName(typeof(InteractionSourceKind), source), normalizedOffset);
    }

    private void GestureRecognizerOnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Shoot();
    }

    public void Shoot()
    {
        //    ShootSound.Play();

        var eyeball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        eyeball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        eyeball.GetComponent<Renderer>().material = CannonMaterial;

        var rigidBody = eyeball.AddComponent<Rigidbody>();
        rigidBody.mass = 0.5f;
        rigidBody.position = transform.position;
        var forward = transform.forward;
        forward = Quaternion.AngleAxis(-10, transform.right) * forward;
        rigidBody.AddForce(forward * ForceMagnitude);

        eyeball.AddComponent<AudioCollisionBehaviour>().SoundSoftCrash = CollisionClip;
    }

    void Update()
    {
        if (GazeCursor == null) return;

        var raycastHits = Physics.RaycastAll(transform.position, transform.forward);
        var firstHit = raycastHits.OrderBy(r => r.distance).FirstOrDefault();

        GazeCursor.transform.position = firstHit.point;
        GazeCursor.transform.forward = firstHit.normal;
    }

    private void OnDestroy()
    {
        if (_gestureRecognizer != null)
        {
            if (_gestureRecognizer.IsCapturingGestures())
            {
                _gestureRecognizer.StopCapturingGestures();
            }
            _gestureRecognizer.Dispose();
        }
    }

}
