﻿using UnityEngine;
using VFX = NeoCasual.GoingHyper.VisualEffects.VisualEffectProvider;

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

        public void Drop(Vector3 position)
        {
            position.x += Random.Range(-.15f, .15f);

            _rb.position = position;
            _rb.rotation = Quaternion.Euler(0, 0, Random.Range(_rotationClamp.x, _rotationClamp.y));
            _rb.isKinematic = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((1 << collision.gameObject.layer & _layers.value) != 0)
            {
                if (collision.contactCount > 0)
                    VFX.Instance.PlayVFXAt(Constant.VFX_ICE_BUMP_01, collision.GetContact(0).point);
                Destroy(gameObject);              
            }
        }
    }
}
