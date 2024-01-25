using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Wright.Library.Study
{
    public class LevelProgressionState
    {
        private int _currentIndex;
        private InputMethod _currentInputMethod;
        private readonly List<int> _tasks;
        private readonly Dictionary<int, CompleteState> _completeStates;

        public (string Name, int Task) LevelNameAndTask => GetCurrentLevelNameAndTask();
        public string CurrentSwitchScene => $"Switch_To_{_currentInputMethod}";

        /// <summary>
        /// Create progression state.
        /// </summary>
        /// <param name="tasks">Tasks to complete, already in order.</param>
        /// <param name="startingInputMethod">Which input method the user will start with.</param>
        public LevelProgressionState(List<int> tasks, InputMethod startingInputMethod)
        {
            Debug.Assert(tasks.Any());
            
            _completeStates = tasks
                .Select((_, i) => i)
                .ToDictionary(i => i, _ => new CompleteState());
            
            _tasks = tasks;

            _currentInputMethod = startingInputMethod;
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

            _currentInputMethod = _currentInputMethod == InputMethod.KeyboardMouse
                ? InputMethod.VR
                : InputMethod.KeyboardMouse;

            if (_completeStates[_currentIndex].AllDone)
            {
                _currentIndex++;    
            }
        }

        private void UpdateFromCurrentInputMethod()
        {
            if (_currentInputMethod == InputMethod.KeyboardMouse)
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