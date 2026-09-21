using UnityEngine;

namespace CodeBase.Infrastructure.Audio
{
    public interface IAudioService
    {
        void PlaySfx(AudioClip clip);

        void PlayMusic(AudioClip clip, bool loop = true);
        void StopMusic();

        void SetMasterVolume(float volume);
        void SetMusicVolume(float volume);
        void SetSfxVolume(float volume);
    }
}