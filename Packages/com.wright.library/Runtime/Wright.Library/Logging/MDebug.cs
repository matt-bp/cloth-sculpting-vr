using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Wright.Library.Logging
{
    public static class MDebug
    {
        public static void Log(string text,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            Debug.LogFormat("[ME] [{0:HH:mm:ss}] {1,30} {2,15} ({3,3}): {4}", DateTime.Now, Path.GetFileName(file), member, line, text);
        }
    }
}