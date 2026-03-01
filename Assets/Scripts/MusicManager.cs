using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Source")]
    public AudioSource musicSource;

    [Header("Clips by Scene")]
    public AudioClip draftMusic;   
    public AudioClip battleMusic; 

    [Header("Volumes")]
    [Range(0f, 1f)] public float musicVolume = 0.25f;
    public float fadeDuration = 0.6f;

    Coroutine fadeRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f;
        musicSource.volume = musicVolume;

        SceneManager.sceneLoaded += OnSceneLoaded;
      
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        PlayForScene(SceneManager.GetActiveScene().name, true);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForScene(scene.name, false);
    }

    void PlayForScene(string sceneName, bool instant)
    {
        AudioClip target = null;

        if (sceneName == "CardGame")
            target = draftMusic;
        else if (sceneName == "Tower")
            target = battleMusic;

        if (target == null) return;
        if (musicSource.clip == target && musicSource.isPlaying) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        if (instant)
        {
            musicSource.clip = target;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        else
        {
            fadeRoutine = StartCoroutine(FadeSwitch(target));
        }
    }

    IEnumerator FadeSwitch(AudioClip newClip)
    {
        float startVol = musicSource.volume;

        // Fade Out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVol, 0f, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = 0f;

        // Switch Clip
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade In
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = musicVolume;
    }
}