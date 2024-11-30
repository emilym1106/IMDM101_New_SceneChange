using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HighlightObject : MonoBehaviour
{
    private Color originalColor;
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    void OnMouseOver()
    {
        objectRenderer.material.color = Color.yellow;  // Highlight color
    }

    void OnMouseExit()
    {
        objectRenderer.material.color = originalColor;  // Revert to original
    }
}