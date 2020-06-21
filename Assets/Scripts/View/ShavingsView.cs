﻿using DG.Tweening;
using UnityEngine;
using VFX = NeoCasual.GoingHyper.VisualEffects.VisualEffectProvider;

namespace NeoCasual.GoingHyper
{
    public class ShavingsView : MonoBehaviour
    {
        [Header ("Shavings Config")]
        [SerializeField]
        private Transform _shaveHandle;
        [SerializeField]
        private float _holdTimeToRotation = 150f;
        [SerializeField]
        private float _rotForceThreshold = 25;

        [Header ("Ice Config")]
        [SerializeField]
        private Transform _fallingIceRoot;
        [SerializeField]
        private FallingIce _fallingIcePrefab;

        private Vector3 _prevHoldPos;
        private float _rotPosPotential;

        public event ComeEvent OnComeToScreen;
        public delegate void ComeEvent ();

        public void OnHolding (float deltaHoldTime)
        {
            Vector3 rotationForce = new Vector3 (0f, deltaHoldTime * _holdTimeToRotation, 0f);
            _shaveHandle.transform.localEulerAngles += rotationForce;
            _rotPosPotential += rotationForce.y;

            if (_rotPosPotential > _rotForceThreshold)
            {
                float fallenIceCount = _rotPosPotential / _rotForceThreshold;
                for (int i = 0; i < fallenIceCount; i++)
                {
                    var fallingIce = Instantiate(_fallingIcePrefab, _fallingIceRoot);
                    fallingIce.Drop(_fallingIceRoot.position);
                }

                _rotPosPotential = 0;
                VFX.Instance.PlayVFXAt(Constant.VFX_ICE_SHRED_01, _fallingIceRoot.position);
            }
        }

        public void ComeAnimation ()
        {
            transform.DOMoveY (6f, 0.5f).OnComplete (() => OnComeToScreen?.Invoke ());
        }

        public void LeaveAnimation ()
        {
            transform.DOMoveY (8.5f, 0.5f);
        }
    }
}