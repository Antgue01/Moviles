using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTransformer 
{
    
    
    /// <summary>
    /// Transforms the input from screen coordinates to an object local coordinates
    /// </summary>
    /// <param name="originalInputPos">The raw input as it's detected by Unity</param>
    /// <param name="relativeObject">the object from which we want to obtain the relative coordinates</param>
    /// <returns>the coordinates in the object local space. The (0,0) is top left and it increases from left to right and from top to bot</returns>
    public Vector2 getInputPos(Vector3 originalInputPos,Transform relativeObject)
    {
        Vector2 transformed = relativeObject.worldToLocalMatrix.MultiplyPoint3x4(Camera.main.ScreenToWorldPoint(originalInputPos));
        transformed.y = -transformed.y;
       return transformed;
    }

    /// <summary>
    /// Transforms the input from screen coordinates to an object local coordinates as a grid with Rows and Cols
    /// </summary>
    /// <param name="relativeObject"></param>
    /// <param name="maxRows"></param>
    /// <param name="maxCols"></param>
    /// <returns></returns>
	public Vector2Int getTilePos(Vector3 originalInputPos, Transform relativeObject, int maxRows, int maxCols)
	{
        Vector2 gridPosition = getInputPos(originalInputPos, relativeObject);

        //Check if not valid
        if (gridPosition.x >= maxCols || gridPosition.x < 0 || gridPosition.y >= maxRows || gridPosition.y < 0)
            return Vector2Int.one * -1;

        Vector2Int tileRowCol = new Vector2Int((int)gridPosition.y, (int)gridPosition.x);
		return tileRowCol;
	}
}