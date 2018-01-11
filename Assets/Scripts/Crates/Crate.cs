using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public enum CType { Standard, Bounce, Arrow, MetalArrow , Life };

    [SerializeField]
    private CType type;
    public CType Type
    {
        get { return type; }
    }

    private CrateType crate;

    private void Start()
    {
        switch (type)
        {
            case CType.Standard:
                crate = new StandardCrate();
                break;

            case CType.Bounce:
                crate = new BounceCrate();
                break;

            case CType.Arrow:
                crate = new ArrowCrate();
                break;

            case CType.MetalArrow:
                crate = new MetalArrowCrate();
                break;

            case CType.Life:
                crate = new LifeCrate();
                break;
        }
    }

    public void OnBounce(out int Force, out bool Wumpa, out bool Life)
    {
        if(crate.OnBounce(out Force, out Wumpa, out Life))
        {
            Destroy(gameObject);
        }
    }
}



