using NeoCasual.GoingHyper.Inputs;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class Main : MonoBehaviour
    {
        private InputManager _input;

        private void Awake ()
        {
            _input = new InputManager ();
        }

        private void Start ()
        {
            CommunicateEvent ();
        }

        private void Update ()
        {
            _input.Update ();
        }

        private void CommunicateEvent ()
        {
            _input.OnStartSwiping += OnStartSwiping;
            _input.OnSwiping += OnSwiping;
            _input.OnFinishSwiping += OnFinishSwiping;
        }

        private void OnStartSwiping (Vector2 position)
        {
            Debug.Log ("Start Swipe: " + position);
        }

        private void OnSwiping (Vector2 position, Vector2 distance)
        {
            Debug.Log ("Swiping: " + position + ", " + distance);
        }

        private void OnFinishSwiping (Vector2 position)
        {
            Debug.Log ("Finish Swiping: " + position);
        }
    }
}