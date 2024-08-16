using Seyren.Universe;
using UnityEngine;


class FogOfWarV2 : MonoBehaviour
{
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public Material material;
    public Mesh mesh;

    GraphicsBuffer commandBuf;
    GraphicsBuffer.IndirectDrawIndexedArgs[] commandData;
    const int commandCount = 2;

    Matrix4x4[] instData;

    void Start()
    {
        instData = new Matrix4x4[gridSizeX * gridSizeY];
        material.enableInstancing = true;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                instData[x * gridSizeY + y] = Matrix4x4.Translate(new Vector3(x, y, 0));
            }
        }

        commandBuf = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, commandCount, GraphicsBuffer.IndirectDrawIndexedArgs.size);
        commandData = new GraphicsBuffer.IndirectDrawIndexedArgs[commandCount];
        GenerateGridMap();
    }


    void OnDestroy()
    {
        commandBuf?.Release();
        commandBuf = null;
    }

    void GenerateGridMap()
    {
        RenderParams rp = new RenderParams(material);
        // rp.worldBounds = new Bounds(Vector3.zero, new Vector3(gridSizeX, gridSizeY, 1));
        Graphics.RenderMeshInstanced(rp, mesh, 0, instData);
    }

    void Update()
    {
        GenerateGridMap();
    }
}
