using System.Collections;
using UnityEngine;

// This class manages a ViveController. Both for the local player (connected to an actal Vive) and the proxy players.
// The headset is also considered a controller.
public class ViveController : MonoBehaviour {

    public new Transform transform; // Use this as the parent for anything you want to attach to the controller.
    public ViveAvatar viveAvatar; // Link to the ViveAvatar object containing this controller
    public int index; // index of controller in viveavatar controller array. Can be compared to HEAD, LEFT, RIGHT constants

    // --- only for localvive ---

    public GameObject localGameObject; // The local SteamVR GameObject of the controller

    // This helper function can be used to access the SteamVR controller functions directly.
    // A few helper function have been defined below, but for the entire functionality direct calls should be used.
    public SteamVR_Controller.Device localDevice
    {
        get
        {
            if (!gotDevice)
            {
                SteamVR_TrackedObject.EIndex deviceIndex = localGameObject.GetComponent<SteamVR_TrackedObject>().index;
                if (deviceIndex != SteamVR_TrackedObject.EIndex.None)
                {
                    gotDevice = true;
                    _localDevice = SteamVR_Controller.Input((int)deviceIndex);
                }
            }

            return _localDevice;
        }
    }


    // Helper function for real controller
    // This can potentially be changed to return correct responsed for remote controllers also by syncing.
    public bool isTrigger { get { return localDevice.GetPress(SteamVR_Controller.ButtonMask.Trigger); } }
    public bool isGrip { get { return localDevice.GetPress(SteamVR_Controller.ButtonMask.Grip); } }
    public bool isTouchPad {get { return localDevice.GetPress(SteamVR_Controller.ButtonMask.Touchpad); } }
    public bool isTriggerClick { get { return localDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger); } }
    public bool isGripClick { get { return localDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip); } }
    public bool isTouchPadClick { get { return localDevice.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad); } }


    // helper function to get the model of the controller
    public GameObject model
    {
        get { return localGameObject.transform.Find("Model").gameObject; }
    }

    // Create a long vibration in the hand controller
    public void LongHaptic(int iters = 30, ushort length = 2000, float delay = 0.01f)
    {
        StartCoroutine(DoLongHaptic(iters, length, delay));
    }

    private bool gotDevice = false;
    private SteamVR_Controller.Device _localDevice = new SteamVR_Controller.Device(999); // return a dummy 

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3},{4},{5},{6}", transform.position.x, transform.position.y, transform.position.z, transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    public void fromString(string str)
    {
        string[] numstr = str.Split(',');
        float[] nums = new float[numstr.Length];
        for (int i = 0; i < numstr.Length; i++) nums[i] = float.Parse(numstr[i]);

        transform.position = new Vector3(nums[0], nums[1], nums[2]);
        transform.rotation = new Quaternion(nums[3], nums[4], nums[5], nums[6]);
    }


    IEnumerator DoLongHaptic(int iters, ushort length, float delay) { 
    {
            for (int i = 0; i < iters; i++)
            {
                localDevice.TriggerHapticPulse(length);
                yield return new WaitForSeconds(delay);
            }
        }

    }


}

