using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Persistent (DontDestroyOnLoad) audio hub. Plays BGM per <see cref="BgmType"/> and
/// looks up SFX by name from a serialized list (cached into a dictionary at Awake).
/// Survives scene loads so menu/gameplay music transitions without re-instantiation.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip menuBGM;
    [SerializeField] private AudioClip gameplayBGM;

    [System.Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip clip;
    }

    [Header("SFX (looked up by name)")]
    [SerializeField] private SoundEffect[] sfxList;

    // Fast name -> clip lookup, built once.
    private readonly Dictionary<string, AudioClip> _sfxLookup = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Build SFX lookup once.
        _sfxLookup.Clear();
        if (sfxList != null)
        {
            foreach (var sfx in sfxList)
            {
                if (!string.IsNullOrEmpty(sfx.name) && sfx.clip != null && !_sfxLookup.ContainsKey(sfx.name))
                    _sfxLookup.Add(sfx.name, sfx.clip);
            }
        }
    }

    /// <summary>Switches BGM if a different track is requested. No-op if already playing it.</summary>
    public void PlayBGM(BgmType type)
    {
        if (bgmSource == null) return;

        AudioClip target = type == BgmType.Gameplay ? gameplayBGM : menuBGM;
        if (target == null || bgmSource.clip == target) return;

        bgmSource.clip = target;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    /// <summary>Plays a one-shot SFX by name. Silently ignores unknown/missing clips.</summary>
    public void PlaySFX(string sfxName)
    {
        if (sfxSource == null || string.IsNullOrEmpty(sfxName)) return;
        if (_sfxLookup.TryGetValue(sfxName, out var clip) && clip != null)
            sfxSource.PlayOneShot(clip);
    }
}
