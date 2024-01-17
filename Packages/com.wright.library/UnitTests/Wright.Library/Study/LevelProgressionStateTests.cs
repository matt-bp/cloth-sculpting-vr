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
            var state = CreateState();

            var result = state.LevelNameAndInput;

            Assert.That(result, Is.EqualTo(("Tutorial", InputMethods.KeyboardMouse)));
        }
        
        [Test]
        public void Next_WhenCalledAtStart_ReturnsNextInputMethod()
        {
            var state = CreateState();

            state.Next();
            var result = state.LevelNameAndInput;

            Assert.That(result, Is.EqualTo(("Tutorial", InputMethods.VR)));
        }
        
        [Test]
        public void Next_WhenCalledTwice_MovesToNextLevel()
        {
            var state = CreateState();

            state.Next();
            state.Next();
            var result = state.LevelNameAndInput;

            Assert.That(result, Is.EqualTo(("One", InputMethods.KeyboardMouse)));
        }
        
        #region Helpers

        private static LevelProgressionState CreateState() => new(CreateLevelNameList());
        
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