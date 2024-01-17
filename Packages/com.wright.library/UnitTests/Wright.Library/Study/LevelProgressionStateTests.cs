using System.Collections.Generic;
using NUnit.Framework;
using Wright.Library.Study;

namespace UnitTests.Wright.Library.Study
{
    public class LevelProgressionStateTests
    {
        [Test]
        public void CurrentLevelName_AfterInitialized_ReturnsFirstName()
        {
            var list = CreateLevelNameList();
            var state = new LevelProgressionState(list, 0);

            var result = state.CurrentLevelName;

            Assert.That(result, Is.EqualTo("Tutorial"));
        }
        
        #region Helpers

        private static List<string> CreateLevelNameList()
        {
            return new List<string>()
            {
                "Tutorial",
                "One",
                "Two"
            };
        }

        #endregion
    }
}