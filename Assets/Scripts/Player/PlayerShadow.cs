using System;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;

    private void Update()
    {
        _shadowSpriteRenderer.sortingOrder = PlayerController.Instance.GetSpriteRenderer().sortingOrder - 1;
    }
}
