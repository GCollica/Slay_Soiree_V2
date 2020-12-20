using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerAssigner : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        if (this.gameObject.CompareTag("Props"))
        {
            Invoke("SetSortingLayers", 0.01f);
            return;
        }
        else
        {
            InvokeRepeating("SetSortingLayers", 0.01f, 0.5f);
        }
    }
    public void SetSortingLayers()
    {
        spriteRenderer.sortingLayerName = (Mathf.RoundToInt(this.gameObject.transform.position.y * 2f) /2f).ToString();
        spriteRenderer.sortingOrder = Mathf.RoundToInt(this.gameObject.transform.position.x) * 10;
    }
}
