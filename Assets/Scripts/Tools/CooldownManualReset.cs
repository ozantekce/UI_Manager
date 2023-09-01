using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
/// CooldownManualReset is look like Cooldown but it cannot reset timer automatic you must reset with Reset() method.
///</summary>
public class CooldownManualReset
{
    private float cooldown;
    private float lastTime;

    private bool first = true;

    public CooldownManualReset(float cooldown)
    {
        this.cooldown = cooldown;
        lastTime = Time.realtimeSinceStartup;
    }

    public void SetCooldown(float cooldown)
    {
        this.cooldown = cooldown;
    }

    public bool TimeOver()
    {

        if (ElapsedTime() >= cooldown)
        {
            first = false;
            return true;
        }
        else
            return false||first;

    }

    public void ResetTimer()
    {
        first = false;
        lastTime = Time.realtimeSinceStartup;
    }

    public float ElapsedTime()
    {

        return (Time.realtimeSinceStartup - lastTime);
    }
}
