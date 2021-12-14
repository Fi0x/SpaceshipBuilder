using System.Collections.Generic;
using BuildingScripts;
using FlightScripts;
using FlightScripts.Enemies;
using Parts;
using UnityEngine;

namespace Control
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip shot;
        [SerializeField] private AudioClip menuClick;
        [SerializeField] private AudioClip buildPartAdded;
        [SerializeField] private AudioClip buildPartRemoved;
        [SerializeField] private AudioClip partDestroyed;
        [SerializeField] private AudioClip asteroidDestroyed;
        [SerializeField] private AudioClip enemyDestroyed;
        [SerializeField] private AudioClip resourceCollected;
        [SerializeField] private AudioClip gameLost;
        [SerializeField] private AudioClip gameWon;
        [SerializeField, Range(0, 1)] private float defaultVolume = 0.4f;

        private Dictionary<SoundName, AudioSource> _audioSources;
        private void Start()
        {
            this.InstantiateAudioObjects();
            
            Weapon.ShotFiredEvent += (sender, args) => { this._audioSources[SoundName.Shot].Play(); };
            SceneChanger.InGameButtonClickedEvent += (sender, args) => { this._audioSources[SoundName.MenuClick].Play(); };
            StatMenu.StatMenuClosedEvent += (sender, args) => { this._audioSources[SoundName.MenuClick].Play(); };
            MainMenu.MenuButtonClickedEvent += (sender, args) => { this._audioSources[SoundName.MenuClick].Play(); };
            DragAndDrop.ShipPartAddedEvent += (sender, args) => { this._audioSources[SoundName.BuildPartAdded].Play(); };
            DragAndDrop.ShipPartRemovedEvent += (sender, args) => { this._audioSources[SoundName.BuildPartRemoved].Play(); };
            SpaceshipPart.ShipPartLostEvent += (sender, args) => { this._audioSources[SoundName.PartDestroyed].Play(); };
            AsteroidBehaviour.AsteroidDestroyedEvent += (sender, args) => { this._audioSources[SoundName.AsteroidDestroyed].Play(); };
            SpaceshipPart.ResourceCollectedEvent += (sender, args) => { this._audioSources[SoundName.ResourceCollected].Play(); };
            GameManager.LevelCompletedEvent += (sender, args) => { this._audioSources[args.Won ? SoundName.GameWon : SoundName.GameLost].Play(); };
            Enemy.EnemyDestroyedEvent += (sender, args) => { this._audioSources[SoundName.EnemyDestroyed].Play(); };

            MainMenu.VolumeChangedEvent += (sender, args) =>
            {
                if(!args.Effects)
                    return;

                foreach (var source in this._audioSources)
                    source.Value.volume = this.defaultVolume * args.NewVolume;
                
                this._audioSources[SoundName.Shot].volume = this.defaultVolume * 0.5f * args.NewVolume;
                this._audioSources[SoundName.BuildPartRemoved].volume = this.defaultVolume * 0.2f * args.NewVolume;
                this._audioSources[SoundName.PartDestroyed].volume = this.defaultVolume * 0.3f * args.NewVolume;
                this._audioSources[SoundName.GameWon].volume = this.defaultVolume * 0.5f * args.NewVolume;
            };
        }

        private void InstantiateAudioObjects()
        {
            this._audioSources = new Dictionary<SoundName, AudioSource>();

            this.AddAudioObject(SoundName.Shot, this.shot, this.defaultVolume * 0.5f);
            this.AddAudioObject(SoundName.MenuClick, this.menuClick, this.defaultVolume);
            this.AddAudioObject(SoundName.BuildPartAdded, this.buildPartAdded, this.defaultVolume);
            this.AddAudioObject(SoundName.BuildPartRemoved, this.buildPartRemoved, this.defaultVolume * 0.2f);
            this.AddAudioObject(SoundName.PartDestroyed, this.partDestroyed, this.defaultVolume * 0.3f);
            this.AddAudioObject(SoundName.AsteroidDestroyed, this.asteroidDestroyed, this.defaultVolume);
            this.AddAudioObject(SoundName.EnemyDestroyed, this.enemyDestroyed, this.defaultVolume);
            this.AddAudioObject(SoundName.ResourceCollected, this.resourceCollected, this.defaultVolume);
            this.AddAudioObject(SoundName.GameLost, this.gameLost, this.defaultVolume);
            this.AddAudioObject(SoundName.GameWon, this.gameWon, this.defaultVolume * 0.5f);
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
            BuildPartAdded,
            BuildPartRemoved,
            PartDestroyed,
            AsteroidDestroyed,
            EnemyDestroyed,
            ResourceCollected,
            GameLost,
            GameWon,
        }
    }
}