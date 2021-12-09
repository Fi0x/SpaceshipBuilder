using UnityEngine;

namespace Control
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private AudioClip freeFlightMusic;
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip buildMusic;
        
        private AudioSource _freeFlightSource;
        private AudioSource _menuSource;
        private AudioSource _buildSource;
        
        private void Start()
        {
            this.InstantiateAudioObjects();
            this._menuSource.Play();
            
            SceneChanger.SceneChangedEvent += this.HandleSceneChange;

            //TODO: Listen for Volume change and act on it
        }

        private void InstantiateAudioObjects()
        {
            this._freeFlightSource = this.GetAudioSourceObject(this.freeFlightMusic, 0.1f);
            this._menuSource = this.GetAudioSourceObject(this.menuMusic, 0.1f);
            this._buildSource = this.GetAudioSourceObject(this.buildMusic, 0.1f);
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
                    this._freeFlightSource.Stop();
                    this._buildSource.Stop();
                    this._menuSource.Play();
                    break;
                case SceneChanger.Scene.Build:
                    this._menuSource.Stop();
                    this._freeFlightSource.Stop();
                    this._buildSource.Play();
                    break;
                case SceneChanger.Scene.Flight:
                    this._menuSource.Stop();
                    this._buildSource.Stop();
                    this._freeFlightSource.Play();
                    break;
            }
        }
    }
}