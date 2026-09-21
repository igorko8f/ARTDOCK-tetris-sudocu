using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Infrastructure.Audio
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioMixer _audioMixer;

        private const string MasterVolume = "MasterVolume";
        private const string MusicVolume = "MusicVolume";
        private const string SfxVolume = "SfxVolume";

        public void PlaySfx(AudioClip clip)
        {
            if (clip == null)
                return;

            _sfxSource.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null)
                return;

            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }

        public void StopMusic() => 
            _musicSource.Stop();

        public void SetMasterVolume(float volume) => 
            SetMixerVolume(MasterVolume, volume);

        public void SetMusicVolume(float volume) => 
            SetMixerVolume(MusicVolume, volume);

        public void SetSfxVolume(float volume) => 
            SetMixerVolume(SfxVolume, volume);

        private void SetMixerVolume(string parameter, float volume)
        {
            volume = Mathf.Clamp01(volume);
            _audioMixer.SetFloat(parameter, Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f);
        }
    }
}