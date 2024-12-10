using ToolBox.Serialization;
using UnityEditor.Overlays;
using UnityEngine;
public class SoundManager : Singleton<SoundManager>
{
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource; // Nhạc nền
    [SerializeField] private AudioSource soundEffectSource; // Hiệu ứng âm thanh
    public AudioClip defaultBackgroundMusic;

    [Header("Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.75f;              // Âm lượng nhạc nền
    [Range(0f, 1f)] public float sfxVolume = 0.75f;                // Âm lượng hiệu ứng âm thanh

    private void Start()
    {
        // Thiết lập âm lượng ban đầu
        InitVolume();
    }

    /// <summary>
    /// Cập nhật âm lượng
    /// </summary>
    private void InitVolume()
    {
        if (backgroundMusicSource)
        {
            if (DataSerializer.HasKey(SaveKey.MusicVolume))
            {
                musicVolume = DataSerializer.Load<float>(SaveKey.MusicVolume);
            }
            backgroundMusicSource.volume = musicVolume;
        }
        if (soundEffectSource)
        {
            if (DataSerializer.HasKey(SaveKey.SFXVolume))
            {
                sfxVolume = DataSerializer.Load<float>(SaveKey.SFXVolume);
            }
            soundEffectSource.volume = sfxVolume;
        }
    }

    public void UpdateMusicVolume(float value)
    {
        musicVolume = value;
        backgroundMusicSource.volume = musicVolume;
    }

    public void UpdateSfxVolume(float value)
    {
        sfxVolume = value;
        soundEffectSource.volume = sfxVolume;
    }

    /// <summary>
    /// Phát nhạc nền
    /// </summary>
    public void PlayBackgroundMusic(AudioClip musicClip, bool loop = true)
    {
        if (backgroundMusicSource.clip == musicClip) return; // Nếu nhạc đang phát thì bỏ qua

        backgroundMusicSource.clip = musicClip;
        backgroundMusicSource.loop = loop;
        backgroundMusicSource.Play();
    }

    /// <summary>
    /// Phát hiệu ứng âm thanh
    /// </summary>
    public void PlaySoundEffect(AudioClip sfxClip)
    {
        if (soundEffectSource)
        {
            soundEffectSource.PlayOneShot(sfxClip, sfxVolume); // Phát hiệu ứng âm thanh
        }
    }

    /// <summary>
    /// Ngừng phát nhạc nền
    /// </summary>
    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }

    /// <summary>
    /// Fade out nhạc nền
    /// </summary>
    public void FadeOutMusic(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = backgroundMusicSource.volume;

        while (backgroundMusicSource.volume > 0)
        {
            backgroundMusicSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        backgroundMusicSource.Stop();
        backgroundMusicSource.volume = startVolume;
    }
}
