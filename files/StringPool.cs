using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class StringPool
{
    private static Dictionary<uint, string> toString;
    private static Dictionary<string, uint> toNumber;
    private static bool initialized = false;
    private static uint closest;

    private static void Init()
    {
        if (initialized) 
            return;

        // we just initialize toNumber for this demonstration

        toNumber = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);

        // here the game would initialize the two dictionaries, toNumber and toString
        // by reading the game's manifest using the filesystem.
        // we just initialize a few values to keep it simple

        // the bone ID-s are not accurate
        toNumber.Add("head", 1);
        toNumber.Add("spine2", 2);
        toNumber.Add("spine3", 3);
        toNumber.Add("spine4", 4);

        initialized = true;
    }

    public static uint Get(string key)
    {
        // replicate System_String::IsNullOrEmpty
        if (key.Length == 0)
            return 0;

        StringPool.Init();

        uint result = 0;
        if (toNumber.TryGetValue(key, out result))
            return result;

        Debug.LogWarning($"StringPool.GetNumber - no number for string '{key}'");

        return 0; 
    }

    // new functions required for anti cheat checks
    public static void CopytoNumber(out Dictionary<string, uint> toNumberOut)
    {
        StringPool.Init();

        toNumberOut = toNumber;
    }
    
    public static void ForceInit()
    {
        // set this to false so we can initialize it to original values
        initialized = false;

        StringPool.Init();
    }

    // we need this to simulate what cheats would do to test our detection
    public static void SimulateCheat()
    {
        StringPool.Init();

        // cheats are able to write to these variables.
        // we mimic what they most commonly do, which is setting the transform bone ID-s to
        // a value that represents a different transform bone

        toNumber["spine3"] = toNumber["head"]; 
    }

}
