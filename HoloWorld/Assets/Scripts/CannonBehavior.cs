using System;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.Input;

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
        _gestureRecognizer.Tapped += GestureRecognizerOnTappedEvent;
        _gestureRecognizer.NavigationUpdated += GestureRecognizerOnNavigationUpdatedEvent;
        _gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.NavigationX | GestureSettings.NavigationY | GestureSettings.NavigationZ);
        _gestureRecognizer.StartCapturingGestures();
    }

    private void GestureRecognizerOnNavigationUpdatedEvent(NavigationUpdatedEventArgs args)
    {
        Debug.LogFormat("Nav Upd: {0} Offset: {1}", Enum.GetName(typeof(InteractionSourceKind), args.source.kind), args.normalizedOffset);
    }

    private void GestureRecognizerOnTappedEvent(TappedEventArgs args)
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