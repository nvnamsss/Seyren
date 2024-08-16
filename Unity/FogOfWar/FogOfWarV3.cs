using UnityEngine;


class FogOfWarV3 : MonoBehaviour
{
    struct GridCell {
        public int x;
        public int y;
        public GridCell(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public GameObject player;
    GameObject plane;
    Texture2D texture;
    Vector3 prev;

    int textureWidth;
    int textureHeight;

    void Start()
    {
        prev = player.transform.position;
        textureWidth = gridSizeX * 2;
        textureHeight = gridSizeY * 2;
        texture = new Texture2D(textureWidth, textureHeight);
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                texture.SetPixel(x, y, Color.black);
            }
        }

        texture.SetPixel(5, 5, Color.white);
        texture.Apply();

        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(gridSizeX / 10f, gridSizeY / 10f, gridSizeX / 10f);
        Material material = new Material(Shader.Find("Diffuse"));
        material.mainTexture = texture;

        plane.GetComponent<Renderer>().material = material;
    }


    void OnDestroy()
    {
    }

    void Update()
    {
        UpdateFogOfWar(prev, 30, false);
        UpdateFogOfWar(player.transform.localPosition, 30, true);

        prev = player.transform.localPosition;
    }

    void UpdateFogOfWar(Vector3 lightPosition, float lightRange, bool visible)
    {
        GridCell location = ToGridCoordinate(lightPosition);
        texture.SetPixel(color: Color.white, x: location.x, y: location.y);
        int cx = Mathf.RoundToInt(location.x);
        int cy = Mathf.RoundToInt(location.y);

        int radius = LightRange2Radius(lightRange);
        int x = radius;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            for (int i = -x; i < x; ++i)
            {
                int nx = cx + i;
                int ny1 = cy + y;
                int ny2 = cy - y;

                if (nx >= 0 && nx < textureWidth && ny1 >= 0 && ny1 < textureHeight)
                {
                    Debug.Log("nx: " + nx + " ny1: " + ny1);
                    if (visible) {
                        texture.SetPixel(nx, ny1, Color.white);
                    } else {
                        texture.SetPixel(nx, ny1, Color.black);
                    }
                }

                if (nx >= 0 && nx < textureWidth && ny2 >= 0 && ny2 < textureHeight)
                {
                    if (visible) {
                        texture.SetPixel(nx, ny2, Color.white);
                    } else {
                        texture.SetPixel(nx, ny2, Color.black);
                    }
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

    GridCell ToGridCoordinateV2(Vector3 location)
    {
        int x = Mathf.RoundToInt(Mathf.Abs(location.x - textureWidth / 2));
        int y = Mathf.RoundToInt(Mathf.Abs(location.z - textureHeight / 2));

        return new GridCell(x, y);
    }
}
