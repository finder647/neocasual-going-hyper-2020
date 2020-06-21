using DG.Tweening;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class FillResultView : MonoBehaviour
    {
        [SerializeField]
        private float _rotateSpeed = 25f;
        [SerializeField]
        private GameObject[] _resultObjects;
        [SerializeField]
        private float[] _resultShowcaseHeights;

        private bool _isRotating;
        private int _currentResultIndex;

        public int ResultObjectCount => _resultObjects.Length;

        public event StartShowcaseEvent OnShowcaseStarted;
        public delegate void StartShowcaseEvent ();

        private void Update ()
        {
            if (_isRotating)
            {
                transform.Rotate (0f, _rotateSpeed * Time.deltaTime, 0f);
            }
        }

        public void ShowResult (int index)
        {
            _currentResultIndex = index;
            _resultObjects[index].SetActive (true);

            CoroutineHelper.WaitForSeconds (1f, ShowcaseAnimation);
        }

        public void ShowcaseAnimation ()
        {
            transform.DOMoveY (_resultShowcaseHeights[_currentResultIndex], 0.5f).OnComplete (() =>
            {
                _isRotating = true;
                OnShowcaseStarted?.Invoke ();
            });
        }

        public void HideResult ()
        {
            _resultObjects[_currentResultIndex].SetActive (false);

            _isRotating = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}