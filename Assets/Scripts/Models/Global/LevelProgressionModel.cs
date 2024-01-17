using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wright.Library.Messages;
using Wright.Library.Study;

namespace Models.Global
{
    public class LevelProgressionModel : MonoBehaviour
    {
        private LevelProgressionState _state;
        [SerializeField] private List<string> sceneNames;
        
        private void OnEnable()
        {
            Debug.Log("LPM Enable");
            Messenger.AddListener(PresenterToModel.SWITCHED_INPUT, OnSwitchedInput);
            Messenger.AddListener(GameEvents.START, OnStart);
            Messenger.AddListener(PresenterToModel.TASK_COMPLETE, OnTaskComplete);
        }

        private void OnDisable()
        {
            Debug.Log("LPM Disable");
            Messenger.RemoveListener(PresenterToModel.SWITCHED_INPUT, OnSwitchedInput);
            Messenger.RemoveListener(GameEvents.START, OnStart);
            Messenger.RemoveListener(PresenterToModel.TASK_COMPLETE, OnTaskComplete);
        }

        private void Start()
        {
            _state = new LevelProgressionState(sceneNames);
        }

        private void OnSwitchedInput()
        {
            Debug.Log("Switched input");
            if (!_state.AllLevelsComplete())
            {
                Debug.Log("Go to next scene");

                var (levelName, input) = _state.LevelNameAndInput;
                StartCoroutine(StartLevelWithInputMethod(levelName, input));
            }
            else
            {
                Debug.Log("Show you're done screen.");
            }
        }

        private void OnStart()
        {
            StartCoroutine(StartLevelWithInputMethod("Input_Switch", _state.LevelNameAndInput.input));
        }

        private void OnTaskComplete()
        {
            _state.Next();
            
            if (_state.AllLevelsComplete())
            {
                LoadEndScene();
            }
            else
            {
                StartCoroutine(StartLevelWithInputMethod("Input_Switch", _state.LevelNameAndInput.input));
            }
        }

        private static IEnumerator StartLevelWithInputMethod(string sceneName, InputMethods inputMethod)
        {
            var load = SceneManager.LoadSceneAsync(sceneName);

            while (!load.isDone) yield return null;
            
            Messenger<InputMethods>.Broadcast(ModelToPresenter.CURRENT_INPUT, inputMethod);
        }

        private static void LoadEndScene()
        {
            SceneManager.LoadScene("End");
        }
    }
}