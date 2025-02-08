using Seyren.Universe;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public int gridSizeX = 25;
    public int gridSizeY = 25;
    private GameObject[,] gridCells;

    private Map map;

    Vector3 prevLightPosition;

    void Start()
    {
        IMapCreationStrategy<Map> mapCreationStrategy = new MapCreation(gridSizeX, gridSizeY, false);
        map = Map.Create(mapCreationStrategy);

        map.SetCellProperties(6, 6, false, false);
        CreateGrid();
        // CreateGridLines();
    }

    void CreateGridLines()
    {
        // Create vertical grid lines
        for (int x = 0; x <= gridSizeX; x++)
        {
            Vector3 startPos = new Vector3(x, 0, 0);
            Vector3 endPos = new Vector3(x, gridSizeY, 0);
            CreateLine(startPos, endPos);
        }

        // Create horizontal grid lines
        for (int y = 0; y <= gridSizeY; y++)
        {
            Vector3 startPos = new Vector3(0, y, 0);
            Vector3 endPos = new Vector3(gridSizeX, y, 0);
            CreateLine(startPos, endPos);
        }
    }

    void CreateLine(Vector3 startPos, Vector3 endPos)
    {
        GameObject line = new GameObject("GridLine");
        line.transform.SetParent(transform);

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    void CreateGrid()
    {
        gridCells = new GameObject[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                GameObject gridCell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gridCell.transform.position = new Vector3(x, y, 0);
                gridCell.transform.SetParent(transform);
                gridCells[x, y] = gridCell;

                if (map.GetCell(x, y).IsTransparent)
                {
                    ChangeColor(gridCell, Color.white);
                }
                else
                {
                    ChangeColor(gridCell, Color.black);
                }
            }
        }
    }

    void ChangeColor(GameObject cube, Color color)
    {
        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    void Update()
    {
        int x = Random.Range(0, gridSizeX);
        int y = Random.Range(0, gridSizeY);
        Vector3 lightPosition = new Vector3(x, y, 0);
        // UpdateFogOfWar(prevLightPosition, 3, false);
        // UpdateFogOfWar(lightPosition, 3, true);

        prevLightPosition = lightPosition;
        // Debug.Log("hi mom");
    }

    void UpdateFogOfWar(Vector3 lightPosition, float lightRange, bool visible)
    {
        int radius = Mathf.RoundToInt(lightRange);
        int x = radius;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            for (int i = -x; i < x; ++i)
            {
                int gridX = Mathf.RoundToInt(lightPosition.x + i);
                int gridY1 = Mathf.RoundToInt(lightPosition.y + y);
                int gridY2 = Mathf.RoundToInt(lightPosition.y - y);

                if (gridX >= 0 && gridX < gridSizeX && gridY1 >= 0 && gridY1 < gridSizeY)
                {
                    GameObject gridCell = gridCells[gridX, gridY1];

                    map.SetCellProperties(gridX, gridY1, visible, false);
                    if (visible) {
                        ChangeColor(gridCell, Color.white);
                    } else {
                        ChangeColor(gridCell, Color.black);
                    }
                }

                if (gridX >= 0 && gridX < gridSizeX && gridY2 >= 0 && gridY2 < gridSizeY)
                {
                    GameObject gridCell = gridCells[gridX, gridY2];

                    map.SetCellProperties(gridX, gridY2, visible, false);
                    if (visible) {
                        ChangeColor(gridCell, Color.white);
                    } else {
                        ChangeColor(gridCell, Color.black);
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
    }

}
