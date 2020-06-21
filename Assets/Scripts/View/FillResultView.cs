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

        private bool _isRotating;

        public event StartShowcaseEvent OnShowcaseStarted;
        public delegate void StartShowcaseEvent ();

        public int ResultObjectCount => _resultObjects.Length;

        private void Update ()
        {
            if (_isRotating)
            {
                transform.Rotate (0f, _rotateSpeed * Time.deltaTime, 0f);
            }
        }

        public void ShowResult (int index)
        {
            _resultObjects[index].SetActive (true);

            CoroutineHelper.WaitForSeconds (1f, ShowcaseAnimation);
        }

        public void ShowcaseAnimation ()
        {
            transform.DOMoveY (3f, 0.5f).OnComplete (() =>
            {
                _isRotating = true;
                OnShowcaseStarted?.Invoke ();
            });
        }
    }
}