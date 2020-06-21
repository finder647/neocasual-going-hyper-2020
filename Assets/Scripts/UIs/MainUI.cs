using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NeoCasual.GoingHyper.UIs
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _handUI;

        [Header("Mold Fill UI")]
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private Image _starImage;
        [SerializeField]
        private Sprite _defaultStarImage;
        [SerializeField]
        private Sprite _fullStarImage;
        [SerializeField]
        private float _sliderFillDuration = 0.05f;

        private bool _isFirstInputDone;
        private Tweener _sliderTweener;

        public void Initialize()
        {
            _slider.value = 0;
            _starImage.sprite = _defaultStarImage;
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

            if (fillPercentage >= .95f)
            {
                _starImage.sprite = _fullStarImage;
            }
        }
    }
}
