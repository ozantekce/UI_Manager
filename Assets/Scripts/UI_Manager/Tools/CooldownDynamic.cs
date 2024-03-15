using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownDynamic
{

    private float lastTime;
    public CooldownDynamic()
    {
        lastTime = Time.time;
    }

    public bool Ready(float cooldown)
    {

        if (ElapsedTime() >= cooldown)
        {
            Reset();
            return true;
        }
        else
            return false;

    }

    public bool NotReady(float cooldown)
    {
        return !Ready(cooldown);
    }

    public bool Peek(float cooldown)
    {

        if (ElapsedTime() >= cooldown)
        {
            return true;
        }
        else
            return false;

    }

    public void Reset()
    {
        lastTime = Time.time;
    }

    public float ElapsedTime()
    {
        return (Time.time - lastTime);
    }

}
