using NeoCasual.GoingHyper.Inputs;
using NeoCasual.GoingHyper.MeshSlice;
using NeoCasual.GoingHyper.UIs;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public enum GameState
    {
        WaitingInput,
        Result,
        WaitForOpenResult,
        AfterResult,
        Spawning
    }

    public class Main : MonoBehaviour
    {
        [SerializeField] private MainUI _mainUI;
        [SerializeField] private ShavingsView _shavings;
        [SerializeField] private MoldView _mold;
        [SerializeField] private FillResultView _fillResult;
        [SerializeField] private MoldFilter _moldFilter;
        [SerializeField] private MoldBase _moldBase;

        private InputManager _input;
        private MeshSlicer _slicer;

        private int _levelIndex;
        private GameState _currentState;

        private int _maxLevel => _fillResult.ResultObjectCount;

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
            _input.OnStartHolding += OnStartHolding;
            _input.OnHolding += OnHolding;
            _input.OnTapped += OnTapped;

            _moldFilter.OnFallenIceCountChanged += OnFallenIceCountChanged;
            _moldFilter.OnFallenIceCountChanged += _mainUI.OnIceStackChanged;

            _fillResult.OnShowcaseStarted += () =>
            {
                _currentState = GameState.AfterResult;
            };

            _shavings.OnComeToScreen += () =>
            {
                _currentState = GameState.WaitingInput;
            };
        }

        private void NextStage ()
        {
            if (++_levelIndex < _maxLevel)
            {
                _currentState = GameState.Spawning;

                _fillResult.HideResult ();
                _moldFilter.ChangeActiveMold (_levelIndex);
                _moldBase.ChangeActivedMold (_levelIndex);
                _mold.PutAnimation (_shavings.ComeAnimation);
                _mainUI.ShowProgressBarAnimation ();
            }
            else
            {
                // All Level Complete
            }
        }

        #region Input Listener
        private void OnStartHolding ()
        {
            if (_currentState == GameState.WaitingInput)
            {
                _mainUI.InputCheck ();
            }
        }

        private void OnHolding (float deltaHoldTime)
        {
            if (_currentState == GameState.WaitingInput)
            {
                _shavings.OnHolding (deltaHoldTime);
            }
        }

        private void OnTapped ()
        {
            if (_currentState == GameState.WaitForOpenResult)
            {
                _mainUI.HideProgressBarAnimation ();
                _mold.OpenAnimation ();
                _fillResult.ShowResult (_levelIndex);
            }

            else if (_currentState == GameState.AfterResult)
            {
                NextStage ();
            }
        }
        #endregion

        private void FillToResultTransition ()
        {
            _shavings.LeaveAnimation ();
            CoroutineHelper.WaitForSeconds (2f, () =>
            {
                _mold.CloseAnimation (() =>
                {
                    _currentState = GameState.WaitForOpenResult;
                    _moldBase.ClearFallenIces ();
                });
            });
        }

        private void OnFallenIceCountChanged (float fillPercentage)
        {
            if (_currentState == GameState.Result)
            {
                return;
            }

            if (fillPercentage >= 1f)
            {
                _currentState = GameState.Result;
                FillToResultTransition ();
            }
        }
    }
}