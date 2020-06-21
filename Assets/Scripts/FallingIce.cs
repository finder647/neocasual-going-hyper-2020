using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class FallingIce : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _rotationClamp;
        [SerializeField]
        private Rigidbody _rb;
        [SerializeField]
        private LayerMask _layers;
        [SerializeField]
        private FallenIce _fallenIcePrefab;

        private int _fallenIceCount;
        private bool _hasFallen;

        public void Drop(Vector3 position, int fallenIceCount)
        {
            position.x += Random.Range(-.15f, .15f);

            _rb.position = position;
            _rb.rotation = Quaternion.Euler(0, 0, Random.Range(_rotationClamp.x, _rotationClamp.y));
            _rb.isKinematic = false;

            _fallenIceCount = fallenIceCount;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_hasFallen) return;

            if ((1 << collision.gameObject.layer & _layers.value) != 0 && collision.contactCount > 0)
            {
                _hasFallen = true;

                //for (int i = 0; i < _fallenIceCount; i++)
                //{
                //    var fallenIce = Instantiate(_fallenIcePrefab, collision.transform.parent);
                //    fallenIce.Drop(collision.GetContact(0).point, i);
                //}                
            }
        }

        private void Update()
        {
            if (_hasFallen) Destroy(gameObject);
        }
    }
}
