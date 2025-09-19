using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// it is favorable that we make this class a singleton
// but this will do just fine for the purpose of this proof of concept
public class AntiCheat : MonoBehaviour
{
    // return true if we found something
    private static bool CheckStringPool()
    {
        uint headbone   = StringPool.Get("head");
        uint spine3bone = StringPool.Get("spine3");

        Debug.Log($"AntiCheat.CheckStringPool - headbone: {headbone}, spine3bone {spine3bone}");

        // simulate a cheat overwriting spine3's value to represent the head's value

        StringPool.SimulateCheat();

        headbone    = StringPool.Get("head");
        spine3bone  = StringPool.Get("spine3");

        // now we can see how inside HitTest::BuildAttackMessage if the client has hit
        // spine3 or any transform whats value has been overwritten to represent head
        // will now build an attack message that says the client has hit the head hitbone
        // instead of the correct hitbone.

        Debug.Log($"AntiCheat.CheckStringPool - headbone: {headbone}, spine3bone {spine3bone}");

        // to check if it has been modified by a cheat, we first save the values stored inside toNumber,
        // then we re-initialize it, so we can compare it to the values that are meant to be in there.

        Dictionary<string, uint> toNumberBefore;
        StringPool.CopytoNumber(out toNumberBefore);

        StringPool.ForceInit();

        Dictionary<string, uint> toNumberAfter;
        StringPool.CopytoNumber(out toNumberAfter);

        // check this just in case

        if (toNumberBefore.Count != toNumberAfter.Count)
            return true;

        bool cheatfound = false;

        foreach (var curvalue in toNumberBefore)
        {
            uint expected = 0;
            if (!toNumberAfter.TryGetValue(curvalue.Key, out expected))
            {
                Debug.LogWarning($"AntiCheat.CheckStringPool - key not found for '{curvalue.Key}'");
                cheatfound = true;
                continue;
            }

            if (curvalue.Value != expected)
            {
                Debug.LogWarning($"AntiCheat.CheckStringPool - value for '{curvalue.Key}' has been modified! from {expected} to {curvalue.Value}");
                cheatfound = true;
                continue;
            }
        }

        return cheatfound;
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            if (CheckStringPool())
            {
                Debug.LogError("Cheat Detected: force hitbox!");

                // we can send an RPC to the server here after detecting cheats
            }

            yield return new WaitForSeconds(60f);
        }
    }

    public void Start()
    {
        StartCoroutine(Loop());
    }    

}
