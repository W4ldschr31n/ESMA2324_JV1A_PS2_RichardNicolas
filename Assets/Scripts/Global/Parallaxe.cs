using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallaxe : MonoBehaviour
{
    // Parameters
    [SerializeField]
    private float parallaxeFactorX;


    // External components
    private Camera mainCamera;

    // Data
    private const int PPU = 128; // Pixels per unit

    private void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(mainCamera.transform.position.x * (1 - parallaxeFactorX), transform.position.y, transform.position.z);
        transform.position = PixelPerfectClamp(newPosition);

    }


    private Vector3 PixelPerfectClamp(Vector3 vectorToClamp)
    {
        // Prevent jittering by clamping the new position relatively to the PPU
        Vector3 vectorInPixels = new Vector3(
            Mathf.CeilToInt(vectorToClamp.x * PPU),
            Mathf.CeilToInt(vectorToClamp.y * PPU),
            Mathf.CeilToInt(vectorToClamp.z * PPU)
        );
        return vectorInPixels / PPU;
    }
}