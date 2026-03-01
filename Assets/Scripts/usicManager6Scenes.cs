using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager6Scenes : MonoBehaviour
{
    public static MusicManager6Scenes Instance;

    [Header("Audio Source (drag your AudioSource here)")]
    public AudioSource musicSource;

    [Header("Scene Names (exact names in Build Settings)")]
    public string scene1Name = "CardGame";
    public string scene2Name = "Tower";
    public string scene3Name = "Draft2";
    public string scene4Name = "Tower2";
    public string scene5Name = "WinScene";
    public string scene6Name = "LostScreen";

    [Header("Music Clips (one per scene)")]
    public AudioClip scene1Music;
    public AudioClip scene2Music;
    public AudioClip scene3Music;
    public AudioClip scene4Music;
    public AudioClip scene5Music;
    public AudioClip scene6Music;

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 0.25f;
    public float fadeDuration = 0.6f;

    private string currentScene = "";
    private AudioClip currentClip;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        // Ensure AudioSource
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f; // 2D
        musicSource.volume = volume;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Start immediately (no "late start")
        PlayForScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForScene(scene.name);
    }

    private void PlayForScene(string sceneName)
    {
        if (sceneName == currentScene) return;
        currentScene = sceneName;

        AudioClip next = GetClipBySceneName(sceneName);
        if (next == null) return;

        // Already playing this clip
        if (currentClip == next && musicSource.isPlaying) return;

        // Fade switch
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(next));
    }

    private AudioClip GetClipBySceneName(string sceneName)
    {
        if (sceneName == scene1Name) return scene1Music;
        if (sceneName == scene2Name) return scene2Music;
        if (sceneName == scene3Name) return scene3Music;
        if (sceneName == scene4Name) return scene4Music;
        if (sceneName == scene5Name) return scene5Music;
        if (sceneName == scene6Name) return scene6Music;

        // Bu sahne için müzik yoksa: çalma
        return null;
    }

    private IEnumerator FadeTo(AudioClip nextClip)
    {
        // Fade out
        float startVol = musicSource.volume;
        if (musicSource.isPlaying && fadeDuration > 0f)
        {
            for (float t = 0f; t < fadeDuration; t += Time.unscaledDeltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVol, 0f, t / fadeDuration);
                yield return null;
            }
        }
        musicSource.volume = 0f;

        // Switch clip
        currentClip = nextClip;
        musicSource.Stop();
        musicSource.clip = currentClip;
        musicSource.Play();

        // Fade in
        if (fadeDuration > 0f)
        {
            for (float t = 0f; t < fadeDuration; t += Time.unscaledDeltaTime)
            {
                musicSource.volume = Mathf.Lerp(0f, volume, t / fadeDuration);
                yield return null;
            }
        }
        musicSource.volume = volume;
    }
}