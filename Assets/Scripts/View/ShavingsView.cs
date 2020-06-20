using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class ShavingsView : MonoBehaviour
    {
        private Vector3 _originPos;

        public void OnStartSwiping (Vector2 position)
        {
            _originPos = transform.position;
        }

        public void OnSwiping (Vector2 position, Vector2 distance)
        {
            Vector3 targetPos = new Vector3 (
                _originPos.x + distance.x * 10f, transform.position.y, transform.position.z
            );

            transform.position = targetPos;
        }
    }
}