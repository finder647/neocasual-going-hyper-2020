using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class ShavingsView : MonoBehaviour
    {
        public float swipeDistanceToRotation = 10f;

        private Vector3 _prevHoldPosition;

        public void OnStartSwiping (Vector2 position)
        {
            _prevHoldPosition = position;
        }

        public void OnSwiping (Vector2 position, Vector2 distance)
        {
            Vector3 rotationForce = Vector3.zero;
            if (_prevHoldPosition.x != position.x)
            {
                rotationForce = new Vector3 (
                    0f, Mathf.Abs (distance.x) * swipeDistanceToRotation, 0f
                );

                _prevHoldPosition = position;
            }

            transform.eulerAngles += rotationForce;
        }
    }
}