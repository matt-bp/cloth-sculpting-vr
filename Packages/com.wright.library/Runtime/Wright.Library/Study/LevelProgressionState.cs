using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wright.Library.Study
{
    public class LevelProgressionState
    {
        private int _currentIndex;
        private InputMethods _currentInputMethod = InputMethods.KeyboardMouse;
        private readonly List<int> _tasks;
        private readonly Dictionary<int, CompleteState> _completeStates;
        
        public (string Name, int Task) LevelNameAndTask => GetCurrentLevelNameAndTask();

        /// <summary>
        /// Create progression state.
        /// </summary>
        /// <param name="tasks">Tasks to complete, already in order.</param>
        public LevelProgressionState(List<int> tasks)
        {
            Debug.Assert(tasks.Any());
            
            _completeStates = tasks
                .Select((_, i) => i)
                .ToDictionary(i => i, _ => new CompleteState());
            
            _tasks = tasks;
        }

        public bool AllLevelsComplete() => _completeStates.All(v => v.Value.AllDone);

        private class CompleteState
        {
            public bool KeyboardMouse { get; set; }
            public bool VR { get; set; }

            public bool AllDone => KeyboardMouse && VR;
        }

        public void Next()
        {
            UpdateFromCurrentInputMethod();

            _currentInputMethod = _currentInputMethod == InputMethods.KeyboardMouse
                ? InputMethods.VR
                : InputMethods.KeyboardMouse;

            if (_completeStates[_currentIndex].AllDone)
            {
                _currentIndex++;    
            }
        }

        private void UpdateFromCurrentInputMethod()
        {
            if (_currentInputMethod == InputMethods.KeyboardMouse)
            {
                _completeStates[_currentIndex].KeyboardMouse = true;
            }
            else
            {
                _completeStates[_currentIndex].VR = true;
            }
        }

        private (string, int) GetCurrentLevelNameAndTask() => ($"Level_{_currentInputMethod}", _tasks[_currentIndex]);
            
    }
}