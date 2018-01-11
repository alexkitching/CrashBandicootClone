using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CrateType
{
    protected int wumpaCount;
    protected int bounceForce;

    public abstract bool OnBounce(out int Force, out bool Wumpa, out bool Life);
    public abstract bool OnHit();
}

public class StandardCrate : CrateType
{
    public StandardCrate()
    {
        wumpaCount = 1;
        bounceForce = 10;
    }

    public override bool OnBounce(out int Force, out bool Wumpa, out bool Life)
    {
        Force = bounceForce;
        Wumpa = true;
        Life = false;
        --wumpaCount;
        if(wumpaCount <= 0)
        {
            return true;
        }
        return false;
    }

    public override bool OnHit()
    {
        return true;
    }
}

public class BounceCrate : CrateType
{
    public BounceCrate()
    {
        wumpaCount = UnityEngine.Random.Range(8, 10);
        bounceForce = 10;
    }

    public override bool OnBounce(out int Force, out bool Wumpa, out bool Life)
    {
        Force = bounceForce;
        Wumpa = true;
        Life = false;
        --wumpaCount;
        if (wumpaCount <= 0)
        {
            return true;
        }
        return false;
    }

    public override bool OnHit()
    {
        return true;
    }
}

public class ArrowCrate : CrateType
{
    public ArrowCrate()
    {
        wumpaCount = 1;
        bounceForce = 10;
    }

    public override bool OnBounce(out int Force, out bool Wumpa, out bool Life)
    {
        Force = bounceForce;
        Wumpa = false;
        Life = false;
        return false;
    }

    public override bool OnHit()
    {
        return true;
    }
}

public class MetalArrowCrate : CrateType
{
    public MetalArrowCrate()
    {
        wumpaCount = 0;
        bounceForce = 10;
    }

    public override bool OnBounce(out int Force, out bool Wumpa, out bool Life)
    {
        Force = bounceForce;
        Wumpa = false;
        Life = false;
        return false;
    }

    public override bool OnHit()
    {
        return false;
    }
}

public class LifeCrate : CrateType
{
    public LifeCrate()
    {
        wumpaCount = 0;
        bounceForce = 10;
    }

    public override bool OnBounce(out int Force, out bool Wumpa, out bool Life)
    {
        Force = bounceForce;
        Wumpa = false;
        Life = true;
        return true;
    }

    public override bool OnHit()
    {
        return true;
    }
}


