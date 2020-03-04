using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using UnityEngine.XR.WSA;

public class SpeechHandler : MonoBehaviour
{
    public string HidePlaneCmd = "hide plane";
    public GameObject Plane;

    public string ShootCmd = "shoot";
    public CannonBehavior Cannon;

    public string ResetSceneCmd = "reset scene";

    public string ToggleMapCmd = "toggle spatial map";
    public SpatialMappingRenderer SpatialMap;

    private KeywordRecognizer _keywordRecognizer;

    void Start()
    {
        _keywordRecognizer = new KeywordRecognizer(new[] { HidePlaneCmd, ResetSceneCmd, ShootCmd, ToggleMapCmd });
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        var cmd = args.text;
        if (cmd == HidePlaneCmd)
        {
            Plane.SetActive(false);
        }
        else if (cmd == ShootCmd)
        {
            Cannon.Shoot();
        }
        else if (cmd == ResetSceneCmd)
        {
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
        else if (cmd == ToggleMapCmd)
        {
            SpatialMap.renderState = 
                SpatialMap.renderState == SpatialMappingRenderer.RenderState.Occlusion 
                ? SpatialMappingRenderer.RenderState.Visualization 
                : SpatialMappingRenderer.RenderState.Occlusion;
        }
    }

    private void OnDestroy()
    {
        if (_keywordRecognizer != null)
        {
            if (_keywordRecognizer.IsRunning)
            {
                _keywordRecognizer.Stop();
            }
            _keywordRecognizer.Dispose();
        }
    }
}
