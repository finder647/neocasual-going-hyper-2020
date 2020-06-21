﻿using NeoCasual.GoingHyper.Inputs;
using NeoCasual.GoingHyper.MeshSlice;
using NeoCasual.GoingHyper.UIs;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public enum GameState
    {
        WaitingInput,
        Result
    }

    public class Main : MonoBehaviour
    {
        [SerializeField] private MainUI _mainUI;
        [SerializeField] private ShavingsView _shavings;
        [SerializeField] private MoldView _mold;
        [SerializeField] private FillResultView _fillResult;
        [SerializeField] private MoldFilter _moldFilter;

        private InputManager _input;
        private MeshSlicer _slicer;

        private GameState _currentState;

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

            if (_currentState == GameState.WaitingInput)
            {
                _input.Update (deltaTime);
            }
        }

        private void CommunicateEvent ()
        {
            _input.OnHolding += _shavings.OnHolding;
            _input.OnStartHolding += _mainUI.InputCheck;

            _mold.OnStartOpeningMold += () => _fillResult.ShowResult (0);

            _moldFilter.OnFallenIceCountChanged += OnFallenIceCountChanged;
            _moldFilter.OnFallenIceCountChanged += _mainUI.OnIceStackChanged;
        }

        private void FillToResultTransition ()
        {
            _shavings.TakeAnimation ();
            CoroutineHelper.WaitForSeconds (2f, _mold.CloseAnimation);
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