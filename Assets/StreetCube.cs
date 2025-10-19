using UnityEngine;

public class StreetCube : RoadCube
{
    public float growthFactorPerStreet = 1.03228f; // 지시한 값
    public int maxStackPerBuilding = 6;

    void Reset()
    {
        price = 100;
        placeholderScale = new Vector3(0.3f, 0.9f, 0.1f);
    }

    public override void OnPlaced(GridCube at)
    {
        base.OnPlaced(at);
        RoadManager.Instance.RegisterStreet(this, at);
    }

    public override void OnRemoved(GridCube from)
    {
        base.OnRemoved(from);
        RoadManager.Instance.UnregisterStreet(this, from);
    }
}