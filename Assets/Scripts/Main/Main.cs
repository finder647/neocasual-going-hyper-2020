using NeoCasual.GoingHyper.Inputs;
using NeoCasual.GoingHyper.MeshSlice;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private ShavingsView _shavings;

        private InputManager _input;
        private MeshSlicer _slicer;

        private void Awake ()
        {
            _input = new InputManager ();
            _slicer = new MeshSlicer ();
        }

        private void Start ()
        {
            CommunicateEvent ();
        }

        private void Update ()
        {
            float deltaTime = Time.deltaTime;

            _input.Update (deltaTime);
        }

        private void CommunicateEvent ()
        {
            _input.OnHolding += _shavings.OnHolding;
        }
    }
}