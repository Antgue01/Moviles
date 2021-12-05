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

	public Vector2 getTilePos(Transform relativeObject, int maxRows, int maxCols)
	{
        Vector2 gridPosition = getInputPos(Input.mousePosition, relativeObject);

        if (gridPosition.x >= maxCols || gridPosition.x < 0 || gridPosition.y >= maxRows || gridPosition.y < 0)
            return Vector2.one * -1;

        Vector2 tileRowCol = new Vector2((int)gridPosition.y, (int)gridPosition.x);
		return tileRowCol;
	}
}
