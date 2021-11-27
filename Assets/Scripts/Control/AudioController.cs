using System.Collections.Generic;
using Parts;
using UnityEngine;

namespace Control
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip shot;

        private Dictionary<string, AudioSource> _audioObjects;
        private void Start()
        {
            this.InstantiateAudioObjects();
            
            Weapon.ShotFiredEvent += (sender, args) => { this._audioObjects["ShotAudio"].Play(); };
        }

        private void InstantiateAudioObjects()
        {
            this._audioObjects = new Dictionary<string, AudioSource>();

            this.AddAudioObject("ShotAudio", this.shot, 0.2f);
        }

        private void AddAudioObject(string objectName, AudioClip clip, float volume)
        {
            var audioSourceGameObject = new GameObject
            {
                transform = { parent = this.gameObject.transform},
                name = objectName
            };
            var source = audioSourceGameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;

            this._audioObjects.Add(objectName, source);
        }
    }
}