using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NeoCasual.GoingHyper.UIs
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _title;
        [SerializeField]
        private GameObject _handUI;

        [Header("Mold Fill UI")]
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private RectTransform _progressBarRect;
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

            _title.gameObject.SetActive (false);
            _handUI.gameObject.SetActive(false);
            _isFirstInputDone = true;
        }

        public void SetActiveHandUI (bool isActive)
        {
            _handUI.gameObject.SetActive (isActive);
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
            else if (fillPercentage <= 0f)
            {
                Initialize ();
            }
        }

        public void ShowProgressBarAnimation ()
        {
            _progressBarRect.DOAnchorPosX (0f, 0.25f);
        }

        public void HideProgressBarAnimation ()
        {
            _progressBarRect.DOAnchorPosX (-200f, 0.25f);
        }

        public void ShowButton (Button button)
        {
            button.gameObject.SetActive (true);

            DOTween.Kill (button);
            button.transform.localScale = Vector3.zero;
            button.transform.DOScale (1f, 0.5f).SetEase (Ease.OutBack, 2f);
        }
    }
}
