using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class Shaker3D : MonoBehaviour
    {
        public delegate void Shaker3DEvent();

        [SerializeField]
        private Transform _shakeTarget;
        [SerializeField]
        private ShakeSetting3D _offset;
        [SerializeField]
        private ShakeSetting3D _setting;
        [SerializeField]
        private AnimationCurve _magnitudeDamp;

        [Header("Perlin Noise")]
        [SerializeField]
        private float _perlinFreq = 30;
        [Range(-1, 1)]
        [SerializeField]
        private float _perlinSeed = 0;
        [SerializeField]
        private bool _isRandomPerlinSeed = true;

        private float _elapsedTime;
        private Vector3 _currentDirTrauma = Vector3.one;
        private Vector3 _currentRotTrauma = Vector3.one;
        private Vector3 _originalPosition;
        private Vector3 _originalRotation;

        public Shaker3DEvent OnShakeStarted { get; set; }
        public Shaker3DEvent OnShakeFinished { get; set; }
        public Transform ShakeTarget => _shakeTarget;
        public bool IsShaking { get; private set; }

        public void StartShake(ShakeSetting3D setting)
        {
            _setting = setting;

            if (_isRandomPerlinSeed)
                _perlinSeed = Random.value * 2.0f - 1.0f;

            StartShake();
        }

        [ContextMenu("Shake")]
        public void StartShake()
        {
            if (!gameObject.activeInHierarchy || !ShakeTarget.gameObject.activeInHierarchy)
            {
                Debug.Log("Can't start shaking a nonactive transform...");
                return;
            }

            if (IsShaking) StopShake();

            _elapsedTime = 0;
            IsShaking = true;
            _currentDirTrauma = Vector3.Scale(_setting.PosTrauma, _setting.PosTrauma);
            _currentRotTrauma = _setting.RotTrauma;
            SetInitialTargetPosition();
            SetInitialTargetRotation();

            if (OnShakeStarted != null)
                OnShakeStarted.Invoke();
        }

        public void StopShake()
        {
            IsShaking = false;
            ResetTarget();
            OnShakeFinished?.Invoke();
        }

        /// <summary>
        /// Update Perlin Noise settings for <see cref="Shaker3D"/>.
        /// </summary>
        /// <param name="perlinFreq">30 is recommended</param>
        /// <param name="isRandomSeed">should randomize perlin seed each shake or not</param>
        /// <param name="perlinSeed">a value between -1 and 1</param>
        public void UpdatePerlinSettings(float perlinFreq, bool isRandomSeed, float perlinSeed = 0)
        {
            _perlinFreq = perlinFreq;
            _isRandomPerlinSeed = isRandomSeed;

            if (!_isRandomPerlinSeed)
                _perlinSeed = Mathf.Clamp(perlinSeed, -1, 1);
        }

        private void Awake()
        {
            if (_magnitudeDamp == null || _magnitudeDamp.length == 0)
            {
                _magnitudeDamp = new AnimationCurve(
                    new Keyframe[]{
                        new Keyframe(0, 0.85f, 0, 1.9f),
                        new Keyframe(0.125f, 1),
                        new Keyframe(0.5f, 1),
                        new Keyframe(1, 0)
                    });
            }
        }

        private void Start()
        {
            if (!_shakeTarget) _shakeTarget = transform;

            SetInitialTargetPosition();
            SetInitialTargetRotation();
        }

        private void Update()
        {
            if (IsShaking)
            {
                if (_elapsedTime < _setting.Duration)
                {
                    float percentComplete = _elapsedTime / _setting.Duration;
                    Vector3 damperedPosOffset = _offset.PosTrauma * _magnitudeDamp.Evaluate(percentComplete);
                    Vector3 damperedRotOffset = _offset.RotTrauma * _magnitudeDamp.Evaluate(percentComplete);

                    var posNoise = new Vector3(
                        keijiro.Perlin.Noise(new Vector2(_perlinSeed, _elapsedTime) * _perlinFreq),
                        keijiro.Perlin.Noise(new Vector2(_perlinSeed + 1, _elapsedTime) * _perlinFreq),
                        keijiro.Perlin.Noise(new Vector2(_perlinSeed + 2, _elapsedTime) * _perlinFreq));

                    var rotNoise = new Vector3(
                        keijiro.Perlin.Noise(new Vector2(_perlinSeed + 3, _elapsedTime) * _perlinFreq),
                        keijiro.Perlin.Noise(new Vector2(_perlinSeed + 4, _elapsedTime) * _perlinFreq),
                        keijiro.Perlin.Noise(new Vector2(_perlinSeed + 5, _elapsedTime) * _perlinFreq));

                    var posShakeVector = new Vector3(
                        posNoise.x * damperedPosOffset.x * _currentDirTrauma.x,
                        posNoise.y * damperedPosOffset.y * _currentDirTrauma.y,
                        posNoise.z * damperedPosOffset.z * _currentDirTrauma.z);

                    var rotShakeVector = new Vector3(
                        rotNoise.x * damperedRotOffset.x * _currentRotTrauma.x,
                        rotNoise.y * damperedRotOffset.y * _currentRotTrauma.y,
                        rotNoise.z * damperedRotOffset.z * _currentRotTrauma.z);

                    UpdateTargetPosition(_originalPosition + posShakeVector);
                    UpdateTargetRotation(_originalRotation + rotShakeVector);

                    _elapsedTime += Time.deltaTime;
                }
                else
                {
                    StopShake();
                }

            }
        }

        private void UpdateTargetPosition(Vector3 targetPosition)
        {
            if (ShakeTarget is RectTransform rect)
            {
                rect.anchoredPosition = targetPosition;
            }
            else
            {
                ShakeTarget.localPosition = targetPosition;
            }
        }

        private void UpdateTargetRotation(Vector3 targetRotation)
        {
            ShakeTarget.localEulerAngles = targetRotation;
        }

        private void ResetTarget()
        {
            UpdateTargetPosition(_originalPosition);
            UpdateTargetRotation(_originalRotation);
        }

        private void SetInitialTargetPosition()
        {
            if (ShakeTarget is RectTransform rect)
            {
                _originalPosition = rect.anchoredPosition;
            }
            else
            {
                _originalPosition = ShakeTarget.localPosition;
            }
        }

        private void SetInitialTargetRotation()
        {
            _originalRotation = ShakeTarget.localEulerAngles;
        }
    }
    
    [System.Serializable]
    public struct ShakeSetting3D
    {
        public Vector3 PosTrauma;
        public Vector3 RotTrauma;
        public float Duration;
    }
}