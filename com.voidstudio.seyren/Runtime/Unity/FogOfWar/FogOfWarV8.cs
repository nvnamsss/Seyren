using System;
using System.Collections.Generic;
using Seyren.Universe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/*
In this version, vision sharing is applied
*/
class FogOfWarV8 : MonoBehaviour
{
    public int baseGridSizeX = 10;
    public int baseGridSizeY = 10;
    public int resolution = 1;
    public GameObject player;
    public GameObject obstacleObject;
    public Material material;
    public int obstacleSize = 1;
    public int lightRange = 3;
    [SerializeField]
    [Range(1, 5)]
    private float fogLerpSpeed = 2.5f;
    [SerializeField]
    private Color fogColor = new Color32(5, 15, 25, 255);
    [SerializeField]
    [Range(0, 1)]
    private float fogPlaneAlpha = 1;
    GameObject plane;
    Texture2D textureBuffer;
    Texture2D textureTarget;
    Vector3 prev;
    Map map;
    List<LightSource> lightSources;
    Shadowcaster shadowcaster;

    int gridSizeX;
    int gridSizeY;
    int _lightRange = 3;

    float FogRefreshRateTimer = 0;
    public float FogRefreshRate = 1f / 60f; // 1 / fps

    void Start()
    {
        prev = player.transform.position;
        gridSizeX = baseGridSizeX * resolution;
        gridSizeY = baseGridSizeY * resolution;
        _lightRange = lightRange * resolution;


        CreateShadowCaster();
        CreatePlane();
    }

    void CreatePlane()
    {
        textureBuffer = new Texture2D(gridSizeX, gridSizeY);
        textureTarget = new Texture2D(gridSizeX, gridSizeY);

        textureBuffer.wrapMode = TextureWrapMode.Clamp;
        textureBuffer.filterMode = FilterMode.Bilinear;
        Color[] colors = new Color[gridSizeX * gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                textureBuffer.SetPixel(x, y, fogColor);
            }
        }
        textureBuffer.Apply();

        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(baseGridSizeX / 10f, baseGridSizeY / 10f, baseGridSizeX / 10f);

        // plane.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
        plane.GetComponent<MeshRenderer>().material = new Material(material);
        plane.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", textureBuffer);

        Graphics.CopyTexture(textureTarget, textureBuffer);
    }

    void CreateShadowCaster()
    {
        map = new Map(gridSizeX, gridSizeY);
        shadowcaster = new Shadowcaster(gridSizeX, gridSizeY);
        lightSources = new List<LightSource>();

        Vector3Int lightLocation = ToGridCoordinate(player.transform.position * resolution);
        lightSources.Add(new LightSource("player", (Vector2Int)lightLocation, _lightRange, 0));

        foreach (LightSource lightSource in lightSources)
        {
            shadowcaster.RegisterLightSource(lightSource);
        }

        Vector3Int obstacleCoords = ToGridCoordinate(obstacleObject.transform.position * resolution);
        shadowcaster.AddObstacle(obstacleCoords.x, obstacleCoords.y, obstacleSize * resolution);
    }

    void OnDestroy()
    {
    }

    void Update()
    {
        UpdateFog();
    }

    void UpdateFog()
    {
        FogRefreshRateTimer += Time.deltaTime;
        if (FogRefreshRateTimer < FogRefreshRate)
        {
            UpdateFogOfWarBuffer();
            return;
        }
        else
        {
            FogRefreshRateTimer -= FogRefreshRate;
        }

        UpdatePlayer();
        ResetFogOfWar();
        shadowcaster.Compute();

        UpdateFogOfWarTarget();
        UpdateFogOfWarBuffer();
    }

    void ResetFogOfWar()
    {
        shadowcaster.Reset();
    }

    void UpdateFogOfWarTarget()
    {
        plane.GetComponent<MeshRenderer>().material.SetColor("_Color", fogColor);

        textureTarget.SetPixels(shadowcaster.GetColors(fogPlaneAlpha));

        textureTarget.Apply();
    }

    void UpdateFogOfWarBuffer()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Color bufferPixel = textureBuffer.GetPixel(x, y);
                Color targetPixel = textureTarget.GetPixel(x, y);

                textureBuffer.SetPixel(x, y, Color.Lerp(
                    bufferPixel,
                    targetPixel,
                    fogLerpSpeed * Time.deltaTime));

            }
        }

        textureBuffer.Apply();
    }

    void UpdatePlayer()
    {
        // Debug.Log(lightSources.Count);
        Vector3Int lightLocation = ToGridCoordinate(player.transform.position * resolution);
        lightSources[0].Location = (Vector2Int)lightLocation;
        lightSources[0].Height = lightLocation.z;
        Debug.Log(lightLocation);
    }


    Color GetColor(TileType v)
    {
        switch (v)
        {
            case TileType.Revealed:
                return Color.white;
            case TileType.Obstacle:
                return Color.red;
            case TileType.Hidden:
                return Color.black;
            default:
                return Color.red;
        }
    }

    Vector3Int ToGridCoordinate(Vector3 location)
    {
        int x = Mathf.RoundToInt(Mathf.Abs(location.x - gridSizeX / 2));
        int y = Mathf.RoundToInt(Mathf.Abs(location.z - gridSizeY / 2));
        int z = Mathf.RoundToInt(Mathf.Abs(location.y - gridSizeY / 2));

        return new Vector3Int(x, y, z);
    }

}
