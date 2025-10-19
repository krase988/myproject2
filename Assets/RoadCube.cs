using UnityEngine;

public abstract class RoadCube : MonoBehaviour
{
    public int price = 0;
    public Vector3 placeholderScale = Vector3.one;

    // Grid에 배치될 때 호출 (GridCube 참조 전달)
    public virtual void OnPlaced(GridCube at)
    {
        // 기본은 아무것도 안함
    }

    public virtual void OnRemoved(GridCube from)
    {
        // 기본은 아무것도 안함
    }
}