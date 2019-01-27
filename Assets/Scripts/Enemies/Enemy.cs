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
        return this.attacks[attackPattern[currentPatternIndex]];
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
        this.attacks = new Attack[] { new Attack(2000, 10, "Shutter Shaker"), new Attack(2400, 15, "Chim chimney") };
        this.attackPattern = new int[] { 0, 0, 1 };
    }
}

public class Factory : Enemy
{
    public Factory()
    {
        this.name = "Factory";
        this.attacks = new Attack[] { new Attack(1500, 7, "Sound the alarm"),
            new Attack(2400, 12, "Staff Evacuation"),
            new Attack(4000, 17, "Pollution Puke")};
        this.attackPattern = new int[] { 0, 1, 2 };
    }
}

public class Skyscraper : Enemy
{
    public Skyscraper()
    {
        this.name = "Skyscraper";
        this.attacks = new Attack[] { new Attack(2000, 2, "Spiky spiky"),
            new Attack(10000, 23, "Shuriken from the sky"),
            new Attack(40000, 47, "Snakescraper")};
        this.attackPattern = new int[] { 0, 0, 1, 0, 0, 2 };
    }
}

public class Cathedral : Enemy
{
    public Cathedral()
    {
        this.name = "Cathedral";
        this.attacks = new Attack[] { new Attack(2000, 2, "Belly Belly"),
            new Attack(10000, 27, "Sacrushed Chapel"),
            new Attack(40000, 47, "Growing Faith")};
        this.attackPattern = new int[] { 0, 1, 2, 1, 0 };
    }
}

public class Boss : Enemy
{
    public Boss()
    {
        this.name = "City Boss";
        this.attacks = new Attack[] { new Attack(2000, 7, "City Quake"),
            new Attack(8000, 27, "Maximal Pollution Crisis"),
            new Attack(35000, 47, "Tornado buildings")};
        this.attackPattern = new int[] { 0, 1, 2, 1, 0 };
    }
}