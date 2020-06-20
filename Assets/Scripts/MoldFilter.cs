using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class MoldFilter : MonoBehaviour
    {
        public delegate void MoldEvent(float fillPercentage);

        public event MoldEvent OnFallenIceCountChanged;

        [SerializeField]
        private BoxCollider _moldBase;
        [SerializeField]
        private LayerMask _triggerMask;
        [SerializeField]
        private int _fallenIceTarget = 100;

        private List<GameObject> _fallenIces = new List<GameObject>();
        
        public int FallenIceCount => _fallenIces.Count;

        private void OnTriggerEnter(Collider other)
        {
            if ((1 << other.gameObject.layer & _triggerMask.value) != 0 && !_fallenIces.Contains(other.gameObject))
            {
                _fallenIces.Add(other.gameObject);

                float fillPercentage = (float)FallenIceCount / _fallenIceTarget;
                OnFallenIceCountChanged?.Invoke(fillPercentage);
                UpdateMoldBaseSize(fillPercentage);
            }
        }

        private void UpdateMoldBaseSize(float fillPercentage)
        {
            var moldBaseSize = _moldBase.size;
            var moldBaseCenter = _moldBase.center;
            moldBaseSize.y = Mathf.Lerp(0.1f, 1, fillPercentage);
            moldBaseCenter.y = moldBaseSize.y * .5f;
            
            _moldBase.size = moldBaseSize;
            _moldBase.center = moldBaseCenter;
        }
    }
}
