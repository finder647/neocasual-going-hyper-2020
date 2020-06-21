using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class MoldBase : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider _boxCollider;
        [SerializeField]
        private LayerMask _layerMask;
        [SerializeField]
        private FallenIce _fallenIcePrefab;
        [SerializeField]
        private int _fallenIceCount;

        private void OnCollisionEnter(Collision collision)
        {
            if ((1 << collision.gameObject.layer & _layerMask.value) != 0 || collision.contactCount > 0)
            {
                for (int i = 0; i < _fallenIceCount; i++)
                {
                    var position = collision.GetContact(0).point;
                    position.x = Random.Range(-_boxCollider.size.x, _boxCollider.size.x);
                    position.z = Random.Range(-_boxCollider.size.z, _boxCollider.size.z);

                    var fallenIce = Instantiate(_fallenIcePrefab, transform.parent);
                    fallenIce.transform.position = position;
                }
            }
        }
    }
}
