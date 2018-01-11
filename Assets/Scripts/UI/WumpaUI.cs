using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WumpaUI : MonoBehaviour
{
    [SerializeField]
    private int RotationRate;
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up, RotationRate);
	}
}
