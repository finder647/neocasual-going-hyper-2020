using UnityEngine;

namespace NeoCasual.GoingHyper.Inputs
{
    public class InputManager
    {
        private InputConfig _config;

        private Vector2 _mouseOrigin;
        private bool _isSwiping;

        public InputManager ()
        {
            _config = ResourceManager.GetScriptable<InputConfig> ("Input");
        }

        public void CheckUpdate ()
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
            _isSwiping = true;
        }

        private void Swiping ()
        {

        }

        private void FinishSwipe ()
        {
            if (!_isSwiping)
            {
                return;
            }

            _isSwiping = false;
        }
    }
}