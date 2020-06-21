using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class MoldFilter : MonoBehaviour
    {
        private readonly List<GameObject> _fallenIces = new List<GameObject>();

        [SerializeField]
        private LayerMask _triggerMask;
        [SerializeField]
        private int _fallenIceTarget = 100;

        [SerializeField]
        private BoxCollider[] _moldBases;

        private BoxCollider _currentMoldBase;

        public int FallenIceCount => _fallenIces.Count;

        public delegate void MoldEvent (float fillPercentage);
        public event MoldEvent OnFallenIceCountChanged;

        private void Awake ()
        {
            _currentMoldBase = _moldBases[0];
        }

        public void Initialize()
        {
            foreach(var fallenIce in _fallenIces)
            {
                Destroy(fallenIce?.gameObject);
            }

            _fallenIces.Clear();
        }

        public void ChangeActiveMold (int index)
        {
            Initialize ();
            _currentMoldBase = _moldBases[index];
        }

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
            var moldBaseSize = _currentMoldBase.size;
            var moldBaseCenter = _currentMoldBase.center;
            moldBaseSize.y = Mathf.Lerp(0.1f, 1, fillPercentage);
            moldBaseCenter.y = moldBaseSize.y * .5f;
            
            _currentMoldBase.size = moldBaseSize;
            _currentMoldBase.center = moldBaseCenter;
        }
    }
}
