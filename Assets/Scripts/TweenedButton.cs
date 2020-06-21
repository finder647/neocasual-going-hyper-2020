using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NeoCasual.GoingHyper
{
    public class TweenedButton : Button
    {
        public override void OnPointerDown (PointerEventData eventData)
        {
            base.OnPointerDown (eventData);
            transform.DOScale (Vector3.one * 0.75f, 0.1f).SetEase (Ease.InOutFlash);
        }

        public override void OnPointerUp (PointerEventData eventData)
        {
            base.OnPointerUp (eventData);
            transform.DOScale (Vector3.one, 0.25f).SetEase (Ease.OutBounce);
        }
    }
}