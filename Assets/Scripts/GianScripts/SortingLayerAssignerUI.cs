using UnityEngine;

public class SortingLayerAssignerUI : MonoBehaviour
{
    private Canvas canvas;

    public void Awake()
    {
        canvas = this.gameObject.GetComponent<Canvas>();
        Invoke("SetSortingLayers", 0.01f);
    }
    public void SetSortingLayers()
    {
        canvas.sortingLayerName = Mathf.RoundToInt(this.gameObject.transform.position.y).ToString();
        canvas.sortingOrder = Mathf.RoundToInt(this.gameObject.transform.position.x) * 10;
    }
}
