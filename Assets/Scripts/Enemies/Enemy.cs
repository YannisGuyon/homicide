using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    public int[] attackPattern;
    public int currentPatternIndex = 0;
    public string name;

    public Attack GeCurrentAttack()
    {
        Attack result = this.attacks[attackPattern[currentPatternIndex]];
        this.updatePattern();
        return result;
    }

    public void updatePattern()
    {
        currentPatternIndex = (++currentPatternIndex % attackPattern.Length);
    }
}

public class Cottage : Enemy {
    public Cottage()
    {
        this.name = "Little Cottage";
        this.attacks = new Attack[] { new Attack(1000, 10, "Shutter Shaker"), new Attack(1200, 15, "Chim chimney") };
        this.attackPattern = new int[] { 0, 0, 1 };
    }
}