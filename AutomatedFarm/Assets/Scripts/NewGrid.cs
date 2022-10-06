using UnityEngine;

public class NewGrid : Singleton<NewGrid>
{
    [Range(1,100)]
    [SerializeField] private float size = 1;

    int xSize, ySize, zSize;
    Vector3 result;

    ///<summary>
    /// Return the normalized position of the object on the grid. EX: (0.9f, 1f, 1.35f) = (1, 1, 1.5f).
    ///</summary>
    public Vector3 GetGridPoint(Vector3 position)
    {
        position -= transform.position;

        xSize = Mathf.RoundToInt(position.x / size);
        ySize = Mathf.RoundToInt(position.y / size);
        zSize = Mathf.RoundToInt(position.z / size);

        result = new Vector3(
            xSize * size,
            ySize * size,
            zSize * size);

        result += transform.position;

        return result;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;

    //     for (int x = 0; x < 10; x += (int)size)
    //     {
    //         for (int z = 0; z < 10; z += (int)size)
    //         {
    //             var point = GetGridPoint(new Vector3(x, 0, z));
    //             Gizmos.DrawSphere(point, 0.1f);
    //         }
    //     }
    // }

}
