using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using UnityEngine;
using TMPro;
public static class AssignmentBuild
{
    static object Field(object target, string name) => target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(target);
    static void Call(object target, string method) => target.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(target, null);
    static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
    public static void Web()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        var player = UnityEngine.Object.FindFirstObjectByType<Player>();
        var display = UnityEngine.Object.FindFirstObjectByType<TimerText>();
        var timer = (Timer)Field(display, "_gameTimer");
        timer.BeginTimer(2f);
        Check(timer.GetTimeLeft() == 2f && !timer.IsFinished(), "Timer start failed");
        Call(timer, "Update");
        Check(timer.GetTimeLeft() <= 2f && timer.GetTimeLeft() >= 0f, "Timer tick failed");
        foreach (var name in new[] { "_leftKick", "_rightKick" })
        {
            var kick = (KickWeapon)Field(player, name);
            kick.ActivateKick();
            var life = (Timer)Field(kick, "_lifeTimer");
            Check(kick.gameObject.activeSelf && life.GetTimeLeft() == 0.2f, "Kick activation failed");
            life.BeginTimer(0f);
            Call(kick, "Update");
            Check(!kick.gameObject.activeSelf, "Kick did not deactivate");
        }
        player.IncreasePoints();
        player.IncreasePoints();
        Check(((TMP_Text)Field(player, "_pointsText")).text == "Points: 2", "Scoring failed");
        int total = 2;
        var foods = UnityEngine.Object.FindObjectsByType<Kickable>(FindObjectsSortMode.None);
        Check(foods.Length == 8, "Expected all eight food items");
        var rightKick = (KickWeapon)Field(player, "_rightKick");
        var trigger = rightKick.GetComponent<Collider2D>();
        foreach (var food in foods)
        {
            int expected = food.name.StartsWith("steak") ? 3 :
                (food.name == "sushi" || food.name == "Chicken") ? 2 : 1;
            Check((int)Field(food, "_pointValue") == expected, "Wrong food value: " + food.name);
            Check(ReferenceEquals(Field(food, "_player"), player), "Wrong player: " + food.name);
            Check(ReferenceEquals(Field(food, "_rigidbody"), food.GetComponent<Rigidbody2D>()), "Wrong body: " + food.name);
            food.GetType().GetMethod("OnTriggerEnter2D", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(food, new object[] { trigger });
            total += expected;
            Check(((TMP_Text)Field(player, "_pointsText")).text == "Points: " + total, "Wrong score after kicking " + food.name);
            Debug.Log("FOOD CHECK PASSED: " + food.name + " adds " + expected);
        }
        Call(display, "Start");
        Call(display, "Update");
        Check(((TMP_Text)Field(display, "_timerDisplay")).text == "30.0", "Timer display failed");
        timer.BeginTimer(0f);
        Call(display, "Update");
        Check(!player.enabled && ((GameObject)Field(display, "_gameOverText")).activeSelf, "Game over failed");
        Debug.Log("MG3 CHECKS PASSED: timer, both kick lifetimes, scoring, display, game over.");
        var testObject = new GameObject("Default value check");
        var defaults = testObject.AddComponent<Timer>();
        var serialized = new SerializedObject(defaults);
        Debug.Log("MG3 SERIALIZED DEFAULTS: float=" + serialized.FindProperty("defaultFloat").floatValue + ", int=" + serialized.FindProperty("defaultInt").intValue + ", bool=" + serialized.FindProperty("defaultBool").boolValue + ", string='" + serialized.FindProperty("defaultString").stringValue + "'");
        UnityEngine.Object.DestroyImmediate(testObject);
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = new[] { "Assets/Scenes/Main.unity" },
            locationPathName = "../../MG3-Web",
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        });
        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Web build failed: " + report.summary.result);
    }
}
