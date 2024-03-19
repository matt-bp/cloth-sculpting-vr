using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Wright.Library.Logging;

namespace Presenters.KM
{
    public class BrushSettingsHotkeys : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private Toggle toggle;
        
        public void Update()
        {
            var previous = dropdown.value;
            
            if (Keyboard.current.tabKey.wasPressedThisFrame && !Keyboard.current.shiftKey.isPressed)
            {
                dropdown.value = (dropdown.value + 1) % dropdown.options.Count;
            }
            
            if (Keyboard.current.shiftKey.isPressed && Keyboard.current.tabKey.wasPressedThisFrame ||
                     Keyboard.current.shiftKey.wasPressedThisFrame && Keyboard.current.tabKey.isPressed)
            {
                dropdown.value = (dropdown.value - 1 + dropdown.options.Count) % dropdown.options.Count;
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                dropdown.value = (int)GrabTool.Mesh.MouseMeshDragger.OnClickDrag.PlaneScreen;
            }
            
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                dropdown.value = (int)GrabTool.Mesh.MouseMeshDragger.OnClickDrag.PlaneXY;
            }
            
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                dropdown.value = (int)GrabTool.Mesh.MouseMeshDragger.OnClickDrag.PlaneYZ;;
            }
            
            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                dropdown.value = (int)GrabTool.Mesh.MouseMeshDragger.OnClickDrag.PlaneXZ;;
            }
            
            if (Keyboard.current.digit5Key.wasPressedThisFrame)
            {
                dropdown.value = (int)GrabTool.Mesh.MouseMeshDragger.OnClickDrag.PlaneSurface;;
            }
            
            if (Keyboard.current.digit6Key.wasPressedThisFrame)
            {
                dropdown.value = (int)GrabTool.Mesh.MouseMeshDragger.OnClickDrag.VectorSurface;;
            }
            
            if (Keyboard.current.digit7Key.wasPressedThisFrame)
            {
                dropdown.value = (int)GrabTool.Mesh.MouseMeshDragger.OnClickDrag.VectorCamera;;
            }

            if (dropdown.value != previous)
            {
                MDebug.Log($"Changed on dragging value (brush movement type) to {dropdown.value}");
            }

            if (Keyboard.current.xKey.wasPressedThisFrame)
            {
                toggle.isOn = !toggle.isOn;
                MDebug.Log($"Toggled brush orientation setting to {toggle.isOn}");
            }
        }
    }
}