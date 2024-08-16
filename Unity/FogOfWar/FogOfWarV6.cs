using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


class FogOfWarV6 : MonoBehaviour
{
    class Obstacle
    {
        Vector2 location;
        float size;
        float halfsize;

        public Obstacle(Vector2 location, float size)
        {
            this.location = location;
            this.size = size;
            halfsize = size / 2;
        }

        public bool IsBlocked(Vector2 lightSource, int lightRange, Vector2 target)
        {
            if (Vector2.Distance(location, lightSource) - 0.1f >= lightRange)
            {
                return false;
            }

            Vector2 tl = target - lightSource;
            Vector2 ol = location - lightSource;

            float tan = halfsize / ol.sqrMagnitude;
            float rate = Vector2.Dot(tl, ol) / ol.sqrMagnitude;
            if (rate < 1)
            {
                return false;
            }


            Vector2 proj = rate * ol;

            // float projected =  ol.magnitude * d;
            float d = (tl - proj).sqrMagnitude;
            float error = tan * proj.sqrMagnitude;

            // Debug.Log("halfsize: " + halfsize + "tan: " + tan + "d: " + d + " error: " + error);
            return d < error;
        }
    }



    public int baseGridSizeX = 10;
    public int baseGridSizeY = 10;
    public int resolution = 1;
    public GameObject player;
    public GameObject obstacleObject;
    public float obstacleSize = 3f;
    public int lightRange = 3;

    Obstacle obstacle;

    GameObject plane;
    Texture2D texture;
    Vector3 prev;

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

        texture = new Texture2D(gridSizeX, gridSizeY);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        Color[] colors = new Color[gridSizeX * gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                colors[x + y * gridSizeX] = Color.black;
            }
        }
        texture.SetPixels(0, 0, gridSizeX, gridSizeY, colors);
        texture.Apply();

        Vector2Int gr = ToGridCoordinate(obstacleObject.transform.localPosition * resolution);
        obstacle = new Obstacle(new Vector2(gr.x, gr.y), obstacleSize * resolution);

        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(baseGridSizeX / 10f, baseGridSizeY / 10f, baseGridSizeX / 10f);
        // Material material = new Material(Shader.Find("URP/Lit"));
        // material.mainTexture = texture;

        // plane.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
        plane.GetComponent<Renderer>().material.mainTexture = texture;
    }


    void OnDestroy()
    {
    }

    void Update()
    {
        // ResetFogOfWar(prev, _lightRange);
        // UpdateFogOfWar(player.transform.localPosition, _lightRange, Color.white);
        UpdateFog();
    }

    void UpdateFog()
    {
        FogRefreshRateTimer += Time.deltaTime;
        if (FogRefreshRateTimer < FogRefreshRate)
        {  
            // UpdateFogPlaneTextureBuffer();
            // texture.Apply();
            return;
        }
        else
        {
            FogRefreshRateTimer -= FogRefreshRate;
        }

        ResetFogOfWar(prev, _lightRange);
        UpdateFogOfWar(player.transform.localPosition, _lightRange, Color.white);
        prev = player.transform.localPosition;
    }

    void ResetFogOfWar(Vector3 lightPosition, int lightRange)
    {
        Vector2Int location = ToGridCoordinate(lightPosition * resolution);
        Color color = Color.black;
        // Vector2Int obstacleLocation = ToGridCoordinate(obstacle.transform.localPosition * resolution);

        int cx = location.x;
        int cy = location.y;

        int x = lightRange;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            for (int i = -y; i <= y; ++i)
            {
                int nx = cx + i;
                int ny1 = cy + x;
                int ny2 = cy - x;


                if (nx >= 0 && nx < gridSizeX && ny1 >= 0 && ny1 < gridSizeY)
                {
                    texture.SetPixel(nx, ny1, color);
                }

                if (nx >= 0 && nx < gridSizeX && ny2 >= 0 && ny2 < gridSizeY)
                {
                    texture.SetPixel(nx, ny2, color);
                }

            }

            for (int i = -x; i <= x; ++i)
            {
                int nx = cx + i;
                int ny1 = cy + y;
                int ny2 = cy - y;

                if (nx >= 0 && nx < gridSizeX && ny1 >= 0 && ny1 < gridSizeY)
                {
                    texture.SetPixel(nx, ny1, color);
                }

                if (nx >= 0 && nx < gridSizeX && ny2 >= 0 && ny2 < gridSizeY)
                {
                    texture.SetPixel(nx, ny2, color);
                }
            }

            y++;

            if (radiusError < 0)
            {
                radiusError += 2 * y + 1;
            }
            else
            {
                x--;
                radiusError += 2 * (y - x + 1);
            }
        }
        texture.Apply();
    }

    void UpdateFogOfWar(Vector3 lightPosition, int lightRange, Color color)
    {


        Vector2Int location = ToGridCoordinate(lightPosition * resolution);
        Vector2 lv = new Vector2(location.x, location.y);

        int cx = location.x;
        int cy = location.y;

        int x = lightRange;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            for (int i = -y; i <= y; ++i)
            {
                int nx = cx + i;
                int ny1 = cy + x;
                int ny2 = cy - x;

                bool block1 = obstacle.IsBlocked(lv, lightRange, new Vector2(nx, ny1));
                bool block2 = obstacle.IsBlocked(lv, lightRange, new Vector2(nx, ny2));

                if (!block1 && nx >= 0 && nx < gridSizeX && ny1 >= 0 && ny1 < gridSizeY)
                {
                    texture.SetPixel(nx, ny1, color);
                }

                if (!block2 && nx >= 0 && nx < gridSizeX && ny2 >= 0 && ny2 < gridSizeY)
                {
                    texture.SetPixel(nx, ny2, color);
                }

            }

            for (int i = -x; i <= x; ++i)
            {
                int nx = cx + i;
                int ny1 = cy + y;
                int ny2 = cy - y;

                bool block1 = obstacle.IsBlocked(lv, lightRange, new Vector2(nx, ny1));
                bool block2 = obstacle.IsBlocked(lv, lightRange, new Vector2(nx, ny2));

                if (!block1 && nx >= 0 && nx < gridSizeX && ny1 >= 0 && ny1 < gridSizeY)
                {
                    texture.SetPixel(nx, ny1, color);
                }

                if (!block2 && nx >= 0 && nx < gridSizeX && ny2 >= 0 && ny2 < gridSizeY)
                {
                    texture.SetPixel(nx, ny2, color);
                }
            }

            y++;

            if (radiusError < 0)
            {
                radiusError += 2 * y + 1;
            }
            else
            {
                x--;
                radiusError += 2 * (y - x + 1);
            }
        }
        texture.Apply();
    }

    private void UpdateFogPlaneTextureBuffer()
    {
        for (int xIterator = 0; xIterator < gridSizeX; xIterator++)
        {
            for (int yIterator = 0; yIterator < gridSizeY; yIterator++)
            {
                Color bufferPixel = texture.GetPixel(xIterator, yIterator);
                Color targetPixel = texture.GetPixel(xIterator, yIterator);

                texture.SetPixel(xIterator, yIterator, Color.Lerp(
                    bufferPixel,
                    targetPixel,
                    2.5f * Time.deltaTime));
            }
        }

        texture.Apply();
    }


    Vector2Int ToGridCoordinate(Vector3 location)
    {
        int x = Mathf.RoundToInt(Mathf.Abs(location.x - gridSizeX / 2));
        int y = Mathf.RoundToInt(Mathf.Abs(location.z - gridSizeY / 2));
        return new Vector2Int(x, y);
    }

}
