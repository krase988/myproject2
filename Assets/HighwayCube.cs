using UnityEngine;

public class HighwayCube : RoadCube
{
    void Reset()
    {
        price = 500;
        placeholderScale = new Vector3(0.9f, 0.3f, 0.5f);
    }

    public override void OnPlaced(GridCube at)
    {
        base.OnPlaced(at);
        RoadManager.Instance.RegisterHighway(this, at);
    }

    public override void OnRemoved(GridCube from)
    {
        base.OnRemoved(from);
        RoadManager.Instance.UnregisterHighway(this, from);
    }
}