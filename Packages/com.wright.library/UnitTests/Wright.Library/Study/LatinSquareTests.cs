using NUnit.Framework;
using Wright.Library.Study;

namespace UnitTests.Wright.Library.Study
{
    public class LatinSquareTests
    {
        [Test]
        public void GetTasksAndInputMethod_With0_ReturnsFirst()
        {
            const int p = 0;

            var result = LatinSquare.GetTasksAndInputMethod(p);
            
            Assert.That(result.Tasks, Is.EquivalentTo(new[] {1, 2, 3}));
            Assert.That(result.startingInputMethod, Is.EqualTo(InputMethod.KeyboardMouse));
        }
        
        [Test]
        public void GetTasksAndInputMethod_With11_WrapsAroundToLastOne()
        {
            const int p = 11;

            var result = LatinSquare.GetTasksAndInputMethod(p);
            
            Assert.That(result.Tasks, Is.EquivalentTo(new[] {3, 1, 2}));
            Assert.That(result.startingInputMethod, Is.EqualTo(InputMethod.VR));
        }
    }
}