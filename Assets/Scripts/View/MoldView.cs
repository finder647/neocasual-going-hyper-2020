using DG.Tweening;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class MoldView : MonoBehaviour
    {
        public void CloseAnimation (System.Action onComplete)
        {
            transform.DOMoveY (2.25f, 0.5f);

            CoroutineHelper.WaitForSeconds (0.25f, () =>
            {
                transform.DORotate (Vector3.right * 180f, 0.5f).OnComplete (() => onComplete?.Invoke ());
            });
        }

        public void OpenAnimation ()
        {
            transform.DOMoveY (10.25f, 1f);
        }

        public void PutAnimation (System.Action onComplete = null)
        {
            transform.rotation = Quaternion.identity;
            transform.DOMoveY (0f, 0.5f).OnComplete (() => onComplete?.Invoke ());
        }
    }
}