using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wright.Library.Study
{
    public class LevelProgressionState
    {
        public string CurrentLevelName => _levelNames[0];
        private readonly List<string> _levelNames;
        
        /// <summary>
        /// Create progression state.
        /// </summary>
        /// <param name="levelNames">Names of the scenes that we will be using.</param>
        /// <param name="startingIndex">Where to start in this list (used for Latin Squares).</param>
        public LevelProgressionState(List<string> levelNames, int startingIndex)
        {
            Debug.Assert(startingIndex >= 0 && startingIndex < levelNames.Count);
            Debug.Assert(levelNames.Any());

            _levelNames = levelNames;
        }
    }
}