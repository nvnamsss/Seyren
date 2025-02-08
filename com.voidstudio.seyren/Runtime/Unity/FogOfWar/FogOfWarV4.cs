using UnityEngine;


class FogOfWarV4 : MonoBehaviour
{
    struct GridCell {
        public int x;
        public int y;
        public GridCell(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
    public int baseGridSizeX = 10;
    public int baseGridSizeY = 10;
    public int resolution = 1;
    public GameObject player;

    GameObject plane;
    Texture2D texture;
    Vector3 prev;

    int gridSizeX;
    int gridSizeY;
    int lightRange = 30;

    void Start()
    {
        prev = player.transform.position;
        gridSizeX = baseGridSizeX * resolution;
        gridSizeY = baseGridSizeY * resolution;
        lightRange = lightRange * resolution;

        texture = new Texture2D(gridSizeX, gridSizeY);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                texture.SetPixel(x, y, Color.black);
            }
        }

        texture.SetPixel(5, 5, Color.white);
        texture.Apply();

        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(baseGridSizeX / 10f, baseGridSizeY / 10f, baseGridSizeX / 10f);
        Material material = new Material(Shader.Find("Diffuse"));
        material.mainTexture = texture;

        plane.GetComponent<Renderer>().material = material;
    }


    void OnDestroy()
    {
    }

    void Update()
    {
        UpdateFogOfWar(prev, lightRange, Color.black);
        UpdateFogOfWar(player.transform.localPosition, lightRange, Color.white);

        prev = player.transform.localPosition;
    }

    void UpdateFogOfWar(Vector3 lightPosition, float lightRange, Color color)
    {
        GridCell location = ToGridCoordinate(lightPosition * resolution);
        texture.SetPixel(color: Color.white, x: location.x, y: location.y);
        int cx = Mathf.RoundToInt(location.x);
        int cy = Mathf.RoundToInt(location.y);

        int radius = LightRange2Radius(lightRange);
        int x = radius;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            // texture.SetPixel(cx + y, cy + x, Color.white);
            // texture.SetPixel(cx - y, cy + x, Color.white);  
            for (int i = -y; i <= y; ++i)
            {
                if (cx + i >= 0 && cx + i < gridSizeX && cy + x >= 0 && cy + x < gridSizeY)
                {
                    texture.SetPixel(cx + i, cy + x, color);
                }

                if (cx + i >= 0 && cx + i < gridSizeX && cy - x >= 0 && cy - x < gridSizeY)
                {
                    texture.SetPixel(cx + i, cy - x, color);
                }
                // texture.SetPixel(cx + i, cy + x, color);
                // texture.SetPixel(cx + i, cy - x, color);
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

    int LightRange2Radius(float lightRange)
    {
        return Mathf.RoundToInt(lightRange / 10f);
    }

    GridCell ToGridCoordinate(Vector3 location)
    {
        int x = Mathf.RoundToInt(Mathf.Abs(location.x - gridSizeX / 2));
        int y = Mathf.RoundToInt(Mathf.Abs(location.z - gridSizeY / 2));
        return new GridCell(x, y);
    }

}
