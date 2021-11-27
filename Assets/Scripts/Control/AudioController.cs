using System.Collections.Generic;
using FlightScripts;
using Parts;
using UnityEngine;

namespace Control
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip shot;
        [SerializeField] private AudioClip menuClick;
        [SerializeField] private AudioClip gameLost;
        [SerializeField] private AudioClip gameWon;
        [SerializeField] private AudioClip partDestroyed;
        [SerializeField] private AudioClip asteroidDestroyed;
        [SerializeField] private AudioClip enemyDestroyed;

        private Dictionary<SoundName, AudioSource> _audioSources;
        private void Start()
        {
            this.InstantiateAudioObjects();
            
            Weapon.ShotFiredEvent += (sender, args) => { this._audioSources[SoundName.Shot].Play(); };
            SceneChanger.InGameButtonClickedEvent += (sender, args) => { this._audioSources[SoundName.MenuClick].Play(); };
            StatMenu.StatMenuClosedEvent += (sender, args) => { this._audioSources[SoundName.MenuClick].Play(); };
            MainMenu.MenuButtonClickedEvent += (sender, args) => { this._audioSources[SoundName.MenuClick].Play(); };
            GameManager.LevelCompletedEvent += (sender, args) => { this._audioSources[args.Won ? SoundName.GameWon : SoundName.GameLost].Play(); };
            SpaceshipPart.ShipPartLostEvent += (sender, args) => { this._audioSources[SoundName.PartDestroyed].Play(); };
            AsteroidBehaviour.AsteroidDestroyedEvent += (sender, args) => { this._audioSources[SoundName.AsteroidDestroyed].Play(); };
        }

        private void InstantiateAudioObjects()
        {
            this._audioSources = new Dictionary<SoundName, AudioSource>();

            this.AddAudioObject(SoundName.Shot, this.shot, 0.2f);
            this.AddAudioObject(SoundName.MenuClick, this.menuClick, 0.2f);
            this.AddAudioObject(SoundName.GameLost, this.gameLost, 0.2f);
            this.AddAudioObject(SoundName.GameWon, this.gameWon, 0.2f);
            this.AddAudioObject(SoundName.PartDestroyed, this.partDestroyed, 0.2f);
            this.AddAudioObject(SoundName.AsteroidDestroyed, this.asteroidDestroyed, 0.2f);
            this.AddAudioObject(SoundName.EnemyDestroyed, this.enemyDestroyed, 0.2f);
        }

        private void AddAudioObject(SoundName objectName, AudioClip clip, float volume)
        {
            var audioSourceGameObject = new GameObject
            {
                transform = { parent = this.gameObject.transform},
                name = objectName.ToString()
            };
            var source = audioSourceGameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;

            this._audioSources.Add(objectName, source);
        }
        
        private enum SoundName
        {
            Shot,
            MenuClick,
            GameLost,
            GameWon,
            PartDestroyed,
            AsteroidDestroyed,
            EnemyDestroyed
        }
    }
}