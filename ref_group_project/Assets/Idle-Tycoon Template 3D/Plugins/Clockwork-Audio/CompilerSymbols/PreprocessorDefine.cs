/*
 * Credits to Mirror for this code! <3
 * The #1 free open source game networking library for Unity!
 * https://github.com/MirrorNetworking/Mirror
*/

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;

namespace Quartzified.Audio
{
    static class PreprocessorDefine
    {
        /// <summary>
        /// Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        [InitializeOnLoadMethod]
        public static void AddDefineSymbols()
        {
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            HashSet<string> defines = new HashSet<string>(currentDefines.Split(';'))
            {
                "CLOCKWORK_AUDIO"
            };

            // Only touch PlayerSettings if we actually modified it,
            // Otherwise it shows up as changed in git each time.
            string newDefines = string.Join(";", defines);
            if (newDefines != currentDefines)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
            }
        }
    }
}

#endif