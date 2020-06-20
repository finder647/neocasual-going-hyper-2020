using UnityEngine;

namespace NeoCasual.GoingHyper.Inputs
{
    public class InputManager
    {
        private InputConfig _config;

        private Vector2 _mouseOrigin;
        private Vector2 _mousePos;
        private Vector2 _swipeDistance;
        private bool _isSwiping;

        public event StartSwipeEvent OnStartSwiping;
        public event SwipingEvent OnSwiping;
        public event FinishSwipeEvent OnFinishSwiping;

        public delegate void StartSwipeEvent (Vector2 position);
        public delegate void SwipingEvent (Vector2 position, Vector2 distance);
        public delegate void FinishSwipeEvent (Vector2 position);

        public InputManager ()
        {
            _config = ResourceManager.GetScriptable<InputConfig> ("Input");

            Input.multiTouchEnabled = false;
        }

        public void Update ()
        {
            if (Input.GetMouseButtonDown (0))
            {
                StartSwipe ();
            }

            if (Input.GetMouseButton (0))
            {
                Swiping ();
            }

            if (Input.GetMouseButtonUp (0))
            {
                FinishSwipe ();
            }
        }

        private void StartSwipe ()
        {
            _isSwiping = false;
            _mouseOrigin = Input.mousePosition;
        }

        private void Swiping ()
        {
            _mousePos = Input.mousePosition;

            _swipeDistance.x = (_mousePos.x - _mouseOrigin.x) / Screen.width;
            _swipeDistance.y = (_mousePos.y - _mouseOrigin.y) / Screen.height;

            bool isValidSwipe = Mathf.Abs (_swipeDistance.x) > _config.SwipeMinDetectDistance ||
                Mathf.Abs (_swipeDistance.y) > _config.SwipeMinDetectDistance;

            if (!_isSwiping && isValidSwipe)
            {
                _isSwiping = true;
                OnStartSwiping?.Invoke (_mousePos);
            }

            if (_isSwiping)
            {
                OnSwiping?.Invoke (_mousePos, _swipeDistance);
            }
        }

        private void FinishSwipe ()
        {
            if (!_isSwiping)
            {
                return;
            }

            _isSwiping = false;
            OnFinishSwiping?.Invoke (_mousePos);
        }
    }
}