using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class MoldBase : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _layerMask;
        [SerializeField]
        private FallenIce _fallenIcePrefab;
        [SerializeField]
        private int _fallenIceCount;

        [SerializeField]
        private BoxCollider[] _baseColliders;

        private BoxCollider _currentBoxCollider;
        private List<GameObject> _fallenIces = new List<GameObject> ();

        private void Awake ()
        {
            _currentBoxCollider = _baseColliders[0];
        }

        public void ChangeActivedMold (int index)
        {
            _currentBoxCollider.transform.parent.gameObject.SetActive (false);

            _currentBoxCollider = _baseColliders[index];
            _currentBoxCollider.transform.parent.gameObject.SetActive (true);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((1 << collision.gameObject.layer & _layerMask.value) != 0 || collision.contactCount > 0)
            {
                for (int i = 0; i < _fallenIceCount; i++)
                {
                    var position = collision.GetContact(0).point;
                    position.x = Random.Range(-_currentBoxCollider.size.x, _currentBoxCollider.size.x);
                    position.z = Random.Range(-_currentBoxCollider.size.z, _currentBoxCollider.size.z);

                    var fallenIce = Instantiate(_fallenIcePrefab, transform.parent);
                    fallenIce.transform.position = position;

                    _fallenIces.Add (fallenIce.gameObject);
                }
            }
        }

        public void ClearFallenIces ()
        {
            foreach (GameObject obj in _fallenIces)
            {
                Destroy (obj);
            }

            _fallenIces.Clear ();
        }
    }
}
