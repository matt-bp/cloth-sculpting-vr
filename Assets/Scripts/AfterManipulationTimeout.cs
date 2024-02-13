using System;
using System.Collections;
using System.Linq;
using GrabTool.Mesh;
using UnityEngine;

public class AfterManipulationTimeout : MonoBehaviour
{
    [SerializeField] private MouseMeshDragger mouseMeshDragger;
    [SerializeField] private MeshRenderer clothRenderer;
    [SerializeField] private Material disabledMaterial;
    
    public void HandleDragComplete()
    {
        StartCoroutine(WaitAndEnableAgain());
    }

    private IEnumerator WaitAndEnableAgain()
    {
        mouseMeshDragger.SetDisabled(true);
        var previous = clothRenderer.material;
        clothRenderer.material = disabledMaterial;
        
        Debug.Log("Doing work...");
        yield return new WaitForSeconds(2);
        
        mouseMeshDragger.SetDisabled(false);
        clothRenderer.material = previous;
    }
}