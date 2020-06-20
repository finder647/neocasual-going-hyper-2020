using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class ShavingsView : MonoBehaviour
    {
        public float swipeDistanceToRotation = 20f;
        [Range (0f, 1f)] public float minDeltaSwipePosX = 0.05f;

        private Vector3 _prevHoldPos;

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

            transform.eulerAngles += rotationForce;
        }
    }
}