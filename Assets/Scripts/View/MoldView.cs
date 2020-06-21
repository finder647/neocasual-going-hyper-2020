using DG.Tweening;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class MoldView : MonoBehaviour
    {
        public event StartOpenEvent OnStartOpeningMold;
        public delegate void StartOpenEvent ();

        public void CloseAnimation ()
        {
            transform.DOMoveY (2.25f, 0.5f);

            CoroutineHelper.WaitForSeconds (0.25f, () =>
            {
                transform.DORotate (Vector3.right * 180f, 0.5f).OnComplete (OpenAnimation);
            });
        }

        public void OpenAnimation ()
        {
            CoroutineHelper.WaitForSeconds (1f, () =>
            {
                OnStartOpeningMold?.Invoke ();
                transform.DOMoveY (10.25f, 1f);
            });
        }
    }
}