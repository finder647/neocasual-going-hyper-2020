using NeoCasual.GoingHyper.Inputs;
using NeoCasual.GoingHyper.MeshSlice;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private ShavingsView _shavings;
        [SerializeField] private GameObject _testResult;

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
            _input.Update ();

            if (Input.GetKeyDown (KeyCode.Space))
            {
                _slicer.SliceObject (_testResult, new float[] { 10f, 20f, 30f });
            }
        }

        private void CommunicateEvent ()
        {
            _input.OnStartSwiping += _shavings.OnStartSwiping;
            _input.OnSwiping += _shavings.OnSwiping;
        }
    }
}