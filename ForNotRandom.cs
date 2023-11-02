using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForNotRandom : MonoBehaviour
{
    private void Awake()
    {
        Container.isRandom = false;
    }
}
