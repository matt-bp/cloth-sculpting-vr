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
        [SerializeField] private ParticipantModel participantModel;
        
        private void OnEnable()
        {
            Debug.Log("LPM Enable");
            Messenger.AddListener(PresenterToModel.SWITCHED_INPUT, OnSwitchedInput);
            Messenger.AddListener(PresenterToModel.TASK_COMPLETE, OnTaskComplete);
        }

        private void OnDisable()
        {
            Debug.Log("LPM Disable");
            Messenger.RemoveListener(PresenterToModel.SWITCHED_INPUT, OnSwitchedInput);
            Messenger.RemoveListener(PresenterToModel.TASK_COMPLETE, OnTaskComplete);
        }

        private void OnSwitchedInput()
        {
            Debug.Log("Switched input");
            if (!_state.AllLevelsComplete())
            {
                Debug.Log("Go to next scene");

                var (levelName, task) = _state.LevelNameAndTask;
                StartCoroutine(StartSceneWithInputMethod(levelName, task));
            }
            else
            {
                Debug.Log("Show you're done screen.");
            }
        }

        public void StartFirstSwitchScene()
        {
            var results = LatinSquare.GetTasksAndInputMethod(participantModel.ParticipantNumber);

            var tasksWithTutorial = results.Tasks.ToList();
            tasksWithTutorial.Insert(0, 0);
            
            _state = new LevelProgressionState(tasksWithTutorial, results.startingInputMethod);
            
            StartCoroutine(StartSceneWithInputMethod(_state.CurrentSwitchScene, null));
        }

        public (int Completed, int Total) GetProgression() => (_state.CountCompleted, _state.TotalCount);

        private void OnTaskComplete()
        {
            _state.Next();
            
            if (_state.AllLevelsComplete())
            {
                LoadEndScene();
            }
            else
            {
                StartCoroutine(StartSceneWithInputMethod(_state.CurrentSwitchScene, null));
            }
        }

        private static IEnumerator StartSceneWithInputMethod(string sceneName, int? currentTask)
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