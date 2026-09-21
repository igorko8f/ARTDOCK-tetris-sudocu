using CodeBase.Infrastructure.ResourcesProvider;
using UnityEngine;

namespace CodeBase.Infrastructure.Audio
{
    [CreateAssetMenu(fileName = "SoundsConfig", menuName = "Gameplay/Audio/SoundsConfig")]
    public class SoundsConfig : ScriptableObject, IResource
    {
        public AudioClip BackgroundClip;
        
        public AudioClip DestroySFX;
        public AudioClip PlaceSFX;
        public AudioClip RotateSFX;
        public AudioClip TakeSFX;
        
    }
}