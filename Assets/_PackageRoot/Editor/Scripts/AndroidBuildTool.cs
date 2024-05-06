#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Viter.BuildTools
{
    public class AndroidBuildTool
    {
        private const string CHEATS = "CHEATS";
        private const string PATH_TO_BUILD_FOLDER = "../Build";
        private static string pathToBuidVersion;

        [MenuItem("Build/Build Android")]
        public static void BuildAll()
        {
            pathToBuidVersion = Path.Combine(Application.dataPath, PATH_TO_BUILD_FOLDER, Application.version);

            EnableCheats(true);
            string fileformat = "apk";
            string debug = "cheats";
            EditorUserBuildSettings.buildAppBundle = false;
            Build(fileformat, debug);

            EnableCheats(false);
            debug = "release";
            Build(fileformat, debug);

            fileformat = "aab";
            EditorUserBuildSettings.buildAppBundle = true;
            Build(fileformat, debug);

            CreateZipFile();
        }

        [MenuItem("Build/Build Cheats APK")]
        public static void BuildCheeatsApk()
        {
            pathToBuidVersion = Path.Combine(Application.dataPath, PATH_TO_BUILD_FOLDER, Application.version);

            EnableCheats(true);
            string fileformat = "apk";
            string debug = "cheats";
            EditorUserBuildSettings.buildAppBundle = false;
            Build(fileformat, debug);
        }

        private static void CreateZipFile()
        {
            string zipPath = pathToBuidVersion + ".zip";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            try
            {
                ZipFile.CreateFromDirectory(pathToBuidVersion, zipPath);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }


        [MenuItem("Build/EnableCheats")]
        public static void EnableCheats()
        {
            EnableCheats(true);
        }

        [MenuItem("Build/DisableCheats")]
        public static void DisableCheats()
        {
            EnableCheats(false);
        }

        private static void Build(string fileformat, string debug)
        {
            BuildPlayerOptions buildPlayerOptions = GetBuildOptions(fileformat, debug);


            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"{debug} {fileformat}  succeeded: " + summary.totalSize + " bytes");
                System.Diagnostics.Process.Start("explorer.exe", Path.Combine(pathToBuidVersion, $"{fileformat}/{debug}/"));
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log($"{debug} {fileformat} failed");
            }
        }


        private static BuildPlayerOptions GetBuildOptions(string format, string debug)
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            string[] scenes = EditorBuildSettings.scenes.Where(x => x.enabled).Select(x => x.path).ToArray();
            buildPlayerOptions.scenes = scenes;
            string clearAppName = RemoveSpecialCharacters(Application.productName);
            buildPlayerOptions.locationPathName = Path.Combine(pathToBuidVersion, $"{format}/{debug}/{clearAppName}{Application.version}_{debug}.{format}");
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;
            return buildPlayerOptions;
        }

        public static string RemoveSpecialCharacters(string input)
        {
            Regex r = new Regex("(?:[^a-z0-9]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }

        // [MenuItem("Build/TestEnableCheats")]
        // public static void TestEnableCheats()
        // {
        //     EnableCheats(true);
        // }

        // [MenuItem("Build/TestDisableCheats")]
        // public static void TestDisableCheats()
        // {
        //     EnableCheats(false);
        // }


        private static void EnableCheats(bool enable)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android);
            Debug.Log(defines);
            string[] defs = defines.Split(';');
            List<string> newDefs = defs.ToList();
            if (enable)
            {
                if (!newDefs.Contains(CHEATS))
                {
                    newDefs.Add(CHEATS);
                }
            }
            else
            {
                newDefs.Remove(CHEATS);
            }
            string newDefines = string.Join(';', newDefs);
            Debug.Log(newDefines);
            PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android, newDefines);
            Debug.Log(PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android));
            AssetDatabase.Refresh();
            return;
        }
    }
}
#endif
