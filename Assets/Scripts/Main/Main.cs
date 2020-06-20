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
            
        }
    }
}