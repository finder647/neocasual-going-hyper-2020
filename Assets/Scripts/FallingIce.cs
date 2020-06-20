using UnityEngine;

namespace IceShave
{
    public class FallingIce : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _rotationClamp;
        [SerializeField]
        private Rigidbody _rb;
        [SerializeField]
        private LayerMask _layers;

        public void Drop(Vector3 position)
        {
            position.x += Random.Range(-.15f, .15f);

            _rb.position = position;
            _rb.rotation = Quaternion.Euler(0, 0, Random.Range(_rotationClamp.x, _rotationClamp.y));
            _rb.isKinematic = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((1 << collision.gameObject.layer & _layers.value) != 0 && collision.contactCount > 0)
            {
                var newInstance = Instantiate(collision.gameObject, transform.parent);
                newInstance.transform.position = collision.GetContact(0).point;

                Destroy(gameObject);
            }
        }
    }
}
