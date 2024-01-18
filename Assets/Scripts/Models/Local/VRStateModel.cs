using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;
using Wright.Library.Logging;

namespace Models.Local
{
    public class VRStateModel : MonoBehaviour
    {
        public IEnumerator StartVR()
        {
            MDebug.Log("Initializing XR...");
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
            }
            else
            {
                MDebug.Log("Starting XR...");
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }

        public void StopVR()
        {
            MDebug.Log("Stopping XR...");

            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            MDebug.Log("XR stopped completely.");
        }
    }
}