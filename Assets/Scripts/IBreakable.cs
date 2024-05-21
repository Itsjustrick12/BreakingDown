using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to break objects apart and create particle effects

public interface IBreakable
{
    public void HitObj(float damage);
    public void HitObj(float damage, Vector2 hitLocation, float knockForce, bool hitByPlayer);
    public void BreakObj();
}
