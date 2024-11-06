using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderByY : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Đặt Sorting Order ngược lại với vị trí Y (càng cao thì Sorting Order càng thấp)
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }
}