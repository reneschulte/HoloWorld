using System;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.Input;
using System.Collections.Generic;

public class CannonBehavior : MonoBehaviour
{
    private GestureRecognizer _gestureRecognizer;
    private List<GameObject> _balls;

    public float ForceMagnitude = 300f;
    public int MaxBallsCount = 30;
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

        _balls = new List<GameObject>();
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

        var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ball.GetComponent<Renderer>().material = CannonMaterial;

        var rigidBody = ball.AddComponent<Rigidbody>();
        rigidBody.mass = 0.5f;
        rigidBody.position = transform.position;
        var forward = transform.forward;
        forward = Quaternion.AngleAxis(-10, transform.right) * forward;
        rigidBody.AddForce(forward * ForceMagnitude);

        ball.AddComponent<AudioCollisionBehaviour>().SoundSoftCrash = CollisionClip;
        
        // Keep track and destroy balls to keep the app responsible
        _balls.Add(ball);
        var toRemove = _balls.Count - MaxBallsCount;
        if (toRemove > 0)
        {
            for (var i = 0; i < toRemove; i++)
            {
                Destroy(_balls[0]);
                _balls.RemoveAt(0);
            }
        }
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