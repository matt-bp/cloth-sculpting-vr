using System.Collections.Generic;

namespace Wright.Library.Study
{
    public static class LatinSquare
    {
        /// <summary>
        /// I'm sure there is a more algorithmic way to accomplish this, but there are only 6 possibilities,
        /// and I wanted to make sure I got it right!
        /// </summary>
        private static readonly Dictionary<int, (int[] Tasks, InputMethod inputMethod)> Data =
            new()
            {
                { 0, (new[] { 1, 2, 3 }, InputMethod.KeyboardMouse) },
                { 1, (new[] { 1, 2, 3 }, InputMethod.VR) },
                { 2, (new[] { 2, 3, 1 }, InputMethod.KeyboardMouse) },
                { 3, (new[] { 2, 3, 1 }, InputMethod.VR) },
                { 4, (new[] { 3, 1, 2 }, InputMethod.KeyboardMouse) },
                { 5, (new[] { 3, 1, 2 }, InputMethod.VR) },
            };

        public static (int[] Tasks, InputMethod startingInputMethod) GetTasksAndInputMethod(int p) => Data[p % Data.Count];
    }
}