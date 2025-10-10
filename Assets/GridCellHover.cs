using UnityEngine;

public class GridCellHover : MonoBehaviour
{
    private Renderer rend;

    void Awake()  // 혹은 Start()
    {
        rend = GetComponent<Renderer>();
        SetWhite(); // 초기값
    }

    void OnMouseEnter()
    {
        SetGreen();
    }

    void OnMouseExit()
    {
        SetWhite();
    }

    void SetGreen()
    {
        if (rend != null)
            rend.material.color = Color.green;
    }

    void SetWhite()
    {
        if (rend != null)
            rend.material.color = Color.white;
    }
}
