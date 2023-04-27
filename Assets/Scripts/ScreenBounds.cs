using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class ScreenBounds : MonoBehaviour
{
    Camera _mainCam;
    BoxCollider2D _boxCollider;

    void Awake()
    {
        _mainCam = Camera.main;
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
    }

    private void Start()
    {
        transform.position = Vector3.zero;
        SetBoundsSize();
    }

    private void SetBoundsSize()
    {
        float ySize = _mainCam.orthographicSize * 2f;
        Vector2 boxColliderSize = new Vector2(ySize * _mainCam.aspect, ySize);
        _boxCollider.size = boxColliderSize;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SwitchToOtherSide(collision);
    }

    private void SwitchToOtherSide(Collider2D collision)
    {
        Vector3 colPos = collision.transform.position;
        if (Mathf.Abs(colPos.x) > _boxCollider.size.x / 2f)
        {
            colPos = new Vector3(-colPos.x, colPos.y, 0);
        }
        if (Mathf.Abs(colPos.y) > _boxCollider.size.y / 2f)
        {
            colPos = new Vector3(colPos.x, -colPos.y, 0);
        }
        collision.transform.position = colPos;
    }
}
