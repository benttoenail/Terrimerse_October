using UnityEngine;
using System.Collections;

public abstract class ControllerFunctionality : MonoBehaviour {

    public bool isPerformingAction = false;

    public abstract void HandleInput();
}
