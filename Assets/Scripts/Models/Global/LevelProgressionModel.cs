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
        [SerializeField] private List<int> tasks;
        
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
            // Do latin square design here? And just pass the list to the progression state, it doesn't have to worry about that!
            _state = new LevelProgressionState(tasks);
        }

        private void OnSwitchedInput()
        {
            Debug.Log("Switched input");
            if (!_state.AllLevelsComplete())
            {
                Debug.Log("Go to next scene");

                var (levelName, task) = _state.LevelNameAndTask;
                StartCoroutine(StartLevelWithInputMethod(levelName, task));
            }
            else
            {
                Debug.Log("Show you're done screen.");
            }
        }

        private void OnStart()
        {
            StartCoroutine(StartLevelWithInputMethod(_state.CurrentSwitchScene, null));
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
                StartCoroutine(StartLevelWithInputMethod(_state.CurrentSwitchScene, null));
            }
        }

        private static IEnumerator StartLevelWithInputMethod(string sceneName, int? currentTask)
        {
            var load = SceneManager.LoadSceneAsync(sceneName);

            while (!load.isDone) yield return null;

            if (currentTask.HasValue)
            {
                Messenger<int>.Broadcast(ModelToPresenter.CURRENT_TASK, currentTask.Value, MessengerMode.DONT_REQUIRE_LISTENER);    
            }
        }

        private static void LoadEndScene()
        {
            SceneManager.LoadScene("End");
        }
    }
}