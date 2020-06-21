using DG.Tweening;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class MoldView : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        public void CloseAnimation (System.Action onComplete)
        {
            _animator.SetTrigger("Open");
            DOTween.To(value => { }, 0, 1, 1).OnComplete(() => onComplete?.Invoke());
        }

        public void OpenAnimation ()
        {
            _animator.SetTrigger("Up");
        }

        public void PutAnimation (System.Action onComplete = null)
        {
            _animator.SetTrigger("Put");
            DOTween.To(value => { }, 0, 1, .5f).OnComplete(() => onComplete?.Invoke());
        }
    }
}