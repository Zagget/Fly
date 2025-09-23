using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Scale objects", menuName = "Scriptable Objects/Powers/Scale Objects")]

public class ScaleObject : BaseTogglePower
{
    [SerializeField] float minScale;
    Dictionary<IGrabbable, Vector3> grabbables = new();
    
    float firstDistance;
    float difference;

    public override void Start()
    {
        Grabber.onGrab += AddGrabbable;
        Grabber.onRelease += RemoveGrabbable;

        GetCurrentGrabbable(playerController.leftGrabber);
        GetCurrentGrabbable(playerController.rightGrabber);
        GetCurrentGrabbable(playerController.desktopGrabber);
        firstDistance = (playerController.leftGrabber.transform.position - playerController.rightGrabber.transform.position).sqrMagnitude;
    }

    public override void Update()
    {
        HandleScaleAmount();

        foreach (var grabbable in grabbables)
        {
            grabbable.Key.rb.transform.localScale = grabbable.Value * difference;
        }
    }

    private void HandleScaleAmount()
    {
        if (rigManager.usingVr)
        {
            float distance = (playerController.leftGrabber.transform.position - playerController.rightGrabber.transform.position).sqrMagnitude;
            difference = distance / firstDistance;
        }
        else difference = Mathf.Sin(Time.time) * Time.deltaTime;
    }

    public override void End()
    {
        Grabber.onGrab -= AddGrabbable;
        Grabber.onRelease -= RemoveGrabbable;
        grabbables.Clear();
    }

    public void AddGrabbable(IGrabbable grabbable)
    {
        if (grabbable == null  || grabbables.ContainsKey(grabbable)) return;
        grabbables.Add(grabbable, grabbable.rb.transform.localScale);
    }

    public void RemoveGrabbable(IGrabbable grabbable)
    {
        if (grabbables.ContainsKey(grabbable)) grabbables.Remove(grabbable);
    }

    private void GetCurrentGrabbable(Grabber grabber)
    {
        if (grabber != null) AddGrabbable(grabber.currentGrabbed);
    }
}
