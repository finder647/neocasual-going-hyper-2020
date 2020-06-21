using UnityEngine;

namespace NeoCasual.GoingHyper.Inputs
{
    public class InputManager
    {
        private InputConfig _config;

        private bool _isHolding;
        private float _holdingPreTime;

        public event HoldEvent OnStartHolding;
        public event HoldingEvent OnHolding;
        public event HoldEvent OnFinishHolding;

        public delegate void HoldEvent ();
        public delegate void HoldingEvent (float deltaTime);

        public InputManager ()
        {
            _config = ResourceManager.GetScriptable<InputConfig> ("Input");

            Input.multiTouchEnabled = false;
        }

        public void Update (float deltaTime)
        {
            if (Input.GetMouseButtonDown (0))
            {
                StartHold ();
            }

            if (Input.GetMouseButton (0))
            {
                Holding (deltaTime);
            }

            if (Input.GetMouseButtonUp (0))
            {
                FinishHold ();
            }
        }

        private void StartHold ()
        {
            _isHolding = false;
            _holdingPreTime = 0f;
        }

        private void Holding (float deltaTime)
        {
            _holdingPreTime += Time.deltaTime;

            bool isValidHold = _holdingPreTime > _config.HoldMinDetectTime;
            if (!_isHolding && isValidHold)
            {
                _isHolding = true;
                OnStartHolding?.Invoke ();
            }

            if (_isHolding)
            {
                OnHolding?.Invoke (deltaTime);
            }
        }

        private void FinishHold ()
        {
            if (!_isHolding)
            {
                return;
            }

            _isHolding = false;
            OnFinishHolding?.Invoke ();
        }
    }
}