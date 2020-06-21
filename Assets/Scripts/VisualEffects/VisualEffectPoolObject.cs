using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper.VisualEffects
{
    public class VisualEffectPoolObject : MonoBehaviour
    {
        [SerializeField]
        private List<ParticleSystem> _particles;

        private float _lifeTime;
        private float _startPlayTime;

        public bool IsPlaying { get; protected set; }
        public System.Action<VisualEffectPoolObject> OnFinishedPlaying { get; set; }
        public System.Action OnPlay { get; set; }

        public void Play(Vector3 position)
        {
            transform.position = position;
            Play();
        }

        public void Play()
        {
            foreach (var particle in _particles)
            {
                particle.Play();
            }
            _startPlayTime = Time.time;
            IsPlaying = true;

            OnPlay?.Invoke();
        }

        public void Stop()
        {
            foreach (var particle in _particles)
            {
                particle.Stop();
            }
            IsPlaying = false;
        }

        public bool IsParticleAlive()
        {
            foreach (var particle in _particles)
            {
                if (particle.IsAlive()) return true;
            }

            return false;
        }

        private void LateUpdate()
        {
            if (!IsPlaying) return;

            if (Time.time - _startPlayTime > _lifeTime && !IsParticleAlive())
            {
                Stop();
            }
        }

        private void Awake()
        {
            if (_particles == null || _particles.Count == 0)
            {
                _particles = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
            }

            if (_particles.Count > 0 && _particles[0])
            {
                _lifeTime = _particles[0].main.duration;
            }
        }
    }
}
