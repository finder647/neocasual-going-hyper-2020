using NeoCasual.GoingHyper.Inputs;
using NeoCasual.GoingHyper.MeshSlice;
using NeoCasual.GoingHyper.UIs;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public enum GameState
    {
        Init,
        Gameplay,
        PostGameplay
    }

    public class Main : MonoBehaviour
    {
        public delegate void GameEvent(GameState currentState);

        [SerializeField] private ShavingsView _shavings;
        [SerializeField] private GameObject _testResult;
        [SerializeField] private MainUI _mainUI;

        private InputManager _input;
        private MeshSlicer _slicer;
        private GameState _state;

        public GameState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnGameStateChanged?.Invoke(_state);
                }
            }
        }
        public event GameEvent OnGameStateChanged;

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
            _input.OnStartSwiping += _mainUI.OnStartSwiping;
        }
    }
}