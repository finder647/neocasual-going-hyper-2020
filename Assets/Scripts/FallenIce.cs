using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class FallenIce : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rb;
        [SerializeField]
        private Transform _meshTransform;

        public void Drop(Vector3 position, int spawnIndex)
        {
            if (spawnIndex > 0)
                position.x += _meshTransform.localScale.x * (spawnIndex % 2 != 0 ? 1 : -1);

            _rb.position = position;
        }
    }
}
