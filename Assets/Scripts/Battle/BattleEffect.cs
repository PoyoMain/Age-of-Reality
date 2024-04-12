using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffect : MonoBehaviour
{
    void EffectOver()
    {
        SendMessageUpwards("EffectDone");
    }
}
