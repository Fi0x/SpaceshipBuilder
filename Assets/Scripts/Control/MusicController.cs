using System.Collections;
using UnityEngine;

namespace Control
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private AudioClip freeFlightMusic;
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip buildMusic;
        [SerializeField, Range(0, 1)] private float defaultVolume = 0.2f;
        [SerializeField] private float fadeTime = 0.5f;
        
        private AudioSource _freeFlightSource;
        private AudioSource _menuSource;
        private AudioSource _buildSource;
        
        private void Start()
        {
            this.InstantiateAudioObjects();
            this._menuSource.Play();
            
            SceneChanger.SceneChangedEvent += this.HandleSceneChange;

            MainMenu.VolumeChangedEvent += (sender, args) =>
            {
                if(args.Effects)
                    return;
                
                this._freeFlightSource.volume = this.defaultVolume * 0.7f * args.NewVolume;
                this._menuSource.volume = this.defaultVolume * args.NewVolume;
                this._buildSource.volume = this.defaultVolume * args.NewVolume;
            };
        }

        private void InstantiateAudioObjects()
        {
            this._freeFlightSource = this.GetAudioSourceObject(this.freeFlightMusic, this.defaultVolume * 0.7f);
            this._menuSource = this.GetAudioSourceObject(this.menuMusic, this.defaultVolume);
            this._buildSource = this.GetAudioSourceObject(this.buildMusic, this.defaultVolume);
        }

        private AudioSource GetAudioSourceObject(AudioClip clip, float volume)
        {
            var audioSourceGameObject = new GameObject
            {
                transform = { parent = this.gameObject.transform},
                name = clip.name
            };
            var source = audioSourceGameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;

            return source;
        }

        private void HandleSceneChange(object sender, SceneChanger.SceneChangedEventArgs args)
        {
            switch (args.NewScene)
            {
                case SceneChanger.Scene.Menu:
                    this.StartCoroutine(FadeOut(this._freeFlightSource, this.fadeTime));
                    this.StartCoroutine(FadeOut(this._buildSource, this.fadeTime));
                    this._menuSource.Play();
                    break;
                case SceneChanger.Scene.Build:
                    this.StartCoroutine(FadeOut(this._menuSource, this.fadeTime));
                    this.StartCoroutine(FadeOut(this._freeFlightSource, this.fadeTime));
                    this._buildSource.Play();
                    break;
                case SceneChanger.Scene.Flight:
                    this.StartCoroutine(FadeOut(this._menuSource, this.fadeTime));
                    this.StartCoroutine(FadeOut(this._buildSource, this.fadeTime));
                    this._freeFlightSource.Play();
                    break;
            }
        }
 
        private static IEnumerator FadeOut (AudioSource audioSource, float fadeTime)
        {
            var startVolume = audioSource.volume;
 
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
 
            audioSource.Stop ();
            audioSource.volume = startVolume;
        }
    }
}