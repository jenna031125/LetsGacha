using UnityEngine;

public interface IGrabbable
{
    public void GrabStart(VRControllerGrab controller);
    public void GrabEnd();
}
