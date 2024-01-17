using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wright.Library.Study
{
    public class LevelProgressionState
    {
        private int _currentIndex;
        private InputMethods _currentInputMethod = InputMethods.KeyboardMouse;
        public (string Name, InputMethods input) LevelNameAndInput => (_levelNames[_currentIndex], _currentInputMethod);

        private readonly List<string> _levelNames;
        private readonly Dictionary<int, CompleteState> _completeStates;

        /// <summary>
        /// Create progression state.
        /// </summary>
        /// <param name="levelNames">Names of the scenes that we will be using.</param>
        public LevelProgressionState(List<string> levelNames)
        {
            Debug.Assert(levelNames.Any());
            
            _completeStates = levelNames
                .Select((_, i) => i)
                .ToDictionary(i => i, _ => new CompleteState());
            
            _levelNames = levelNames;
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
    }
}