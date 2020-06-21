using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NeoCasual.GoingHyper.UIs
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _handUI;
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private float _sliderFillDuration = 0.05f;

        private bool _isFirstInputDone;
        private Tweener _sliderTweener;

        public void Initialize()
        {
            _slider.value = 0;
        }

        public void InputCheck()
        {
            if (_isFirstInputDone) return;

            _handUI.gameObject.SetActive(false);
            _isFirstInputDone = true;
        }

        public void OnIceStackChanged(float fillPercentage)
        {
            _sliderTweener?.Kill();

            var lastSliderValue = _slider.value;
            _sliderTweener = DOTween.To(value =>
            {
                _slider.value = Mathf.Lerp(lastSliderValue, fillPercentage, value);
            }, 0, 1, _sliderFillDuration);
        }
    }
}
