using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] int xSize;
    [SerializeField] int ySize;
    [SerializeField] int cellSize;

    Grid grid;

    private void Start()
    {
        grid = new Grid(xSize, ySize, cellSize);
    }

}

public class Grid
{
    int xSize, ySize;
    int cellSize;
    int[,] gridArray;

    public Grid(int xSize, int ySize, int cellSize)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        this.cellSize = cellSize;

        CreateGrid();

    }

    public void CreateGrid()
    {
        gridArray = new int[xSize, ySize];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                Debug.DrawLine(GetWorldPos(i, j), GetWorldPos(i, j + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPos(i, j), GetWorldPos(i + 1, j), Color.red, 100f);
            }
        }

        //Debug.DrawLine(GetWorldPos(0, ySize), GetWorldPos(xSize, ySize), Color.red, 100f);
        //Debug.DrawLine(GetWorldPos(xSize, 0), GetWorldPos(xSize, ySize), Color.red, 100f);

    }

    public Vector3 GetWorldPos(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize;
    }

}

