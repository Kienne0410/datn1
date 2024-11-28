using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxOffset = -0.15f;

    private Camera cam;
    private Vector2 startPos;
    private Vector2 travel => (Vector2)cam.transform.position - startPos;
    private float origin;
    private void Awake() {
        origin = transform.position.x;
        cam = Camera.main;
    }

    private void Start() {
        startPos = transform.position;
    }

    private void FixedUpdate() {
    
        transform.position = startPos + travel * parallaxOffset;
        transform.position = new Vector2(origin,transform.position.y);
    }
}
