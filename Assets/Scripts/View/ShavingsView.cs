using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class ShavingsView : MonoBehaviour
    {
        public float swipeDistanceToRotation = 20f;
        [Range (0f, 1f)]
        public float minDeltaSwipePosX = 0.025f;

        [SerializeField]
        private Transform _shaveHandle;
        [SerializeField]
        private FallingIce _fallingIcePrefab;
        [SerializeField]
        public float _rotForceThreshold = 25;
        [SerializeField]
        public int _perFallingIceCount = 5;
        [SerializeField]
        private Transform _fallingIceRoot;

        private Vector3 _prevHoldPos;
        private float _rotPosPotential;

        public void OnStartSwiping (Vector2 position)
        {
            _prevHoldPos = position;
        }

        public void OnSwiping (Vector2 position, Vector2 distance)
        {
            float deltaPosX = Mathf.Abs (_prevHoldPos.x - position.x) / Screen.width;
            Vector3 rotationForce = Vector3.zero;

            if (deltaPosX > minDeltaSwipePosX)
            {
                rotationForce = new Vector3 (0f, deltaPosX * swipeDistanceToRotation, 0f);
                _prevHoldPos = position;
            }

            _shaveHandle.transform.eulerAngles += rotationForce;

            _rotPosPotential += rotationForce.y;

            if (_rotPosPotential > _rotForceThreshold)
            {
                float fallenIceCount = _rotPosPotential / _rotForceThreshold;
                for (int i = 0; i < fallenIceCount; i++)
                {
                    var fallingIce = Instantiate(_fallingIcePrefab, _fallingIceRoot);
                    fallingIce.Drop(_fallingIceRoot.position, _perFallingIceCount);
                }

                _rotPosPotential = 0;
            }
        }
    }
}