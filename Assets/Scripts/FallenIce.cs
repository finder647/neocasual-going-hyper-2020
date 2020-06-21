using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class FallenIce : MonoBehaviour
    {
        [SerializeField]
        private Transform _meshTransform;

        public void Drop(Vector3 position)
        {
            transform.position = position;
        }
    }
}
