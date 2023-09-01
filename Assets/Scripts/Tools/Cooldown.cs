using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Cooldown is a simple timer, when we look at a timer we know if the target time has passed and we reset it. Looking at the timer can be considered Ready()
///</summary>
public class Cooldown
{
    
    
    private float cooldown;

    private float lastTime;


    public Cooldown(float cooldown)
    {
        this.cooldown = cooldown;
        lastTime = Time.time;
    }

    public void SetCooldown(float cooldown)
    {
        this.cooldown = cooldown;
    }

    public bool Ready()
    {

        if (ElapsedTime() >= cooldown)
        {
            Reset();
            return true;
        }
        else
            return false;

    }

    public bool NotReady()
    {

        return !Ready();

    }

    public bool Peek()
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
