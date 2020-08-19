using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class ButtonController : MonoBehaviour
{
    public abstract void OnMouseDown();
    public abstract void OnMouseUpAsButton();
}
