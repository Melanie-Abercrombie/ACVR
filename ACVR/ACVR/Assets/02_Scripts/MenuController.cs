using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    // consider implementing wrap position instead of clamp position

    List<GameObject> items;
    Vector2Int currentPosition;
    int maxRows;            // 0 indicates no max
    int maxColumns;         // 0 indicates no max

    public void AdvanceUp()
    {
        currentPosition.y -= 1;
        ClampYPosition();
    }

    public void AdvanceDown()
    {
        currentPosition.y += 1;
        ClampYPosition();
    }

    public void AdvanceLeft()
    {
        currentPosition.x -= 1;
        ClampXPosition();
    }

    public void AdvanceRight()
    {
        currentPosition.x += 1;
        ClampXPosition();
    }

    public bool IsEmpty()
    {
        return (GetCurrentElementCount() == 0);
    }

    public int GetCurrentRowNumber()
    {
        return currentPosition.y;
    }

    public int GetCurrentColNumber()
    {
        return currentPosition.x;
    }

    public int GetCurrentElementCount()
    {
        return items.Count;
    }

    public int GetMaxRowCount()
    {
        // 0 indicates no max
        return maxRows;
    }

    public int GetMaxColCount()
    {
        // 0 indicates no max
        return maxColumns;
    }

    public bool HasNoRowLimit()
    {
        return maxRows == 0;
    }

    public bool HasNoColLimit()
    {
        return maxColumns == 0;
    }

    protected int SetMaxRows(int desired)
    {
        if (desired < 1) 
            Debug.LogWarning("attempted to set max rows to an invalid value");
        else
            maxRows = desired;

        return GetMaxRowCount();
    }

    protected int SetMaxCols(int desired)
    {
        if (desired < 0)
            Debug.LogWarning("attempted to set max columns to an invalid value");
        else
            maxColumns = desired;

        return GetMaxColCount();
    }

    protected void DelimitRows()
    {
        maxRows = -1;
    }

    protected void DelimitColumns()
    {
        maxColumns = -1;
    }

    private void ClampPosition()
    {
        ClampXPosition();
        ClampYPosition();
    }

    private void ClampXPosition()
    {
        if (!HasNoColLimit())
        {
            int rowCount = Mathf.CeilToInt(GetCurrentElementCount() / GetMaxColCount());
            int spillOver = (GetCurrentElementCount() % GetMaxColCount());

            if (currentPosition.y == rowCount)  //on partially filled row
            {
                currentPosition.x = Mathf.Clamp(currentPosition.x, 0, spillOver);
            }
            else
            {
                currentPosition.x = Mathf.Clamp(currentPosition.x, 0, GetMaxColCount());
            }
        }
    }

    private void ClampYPosition()
    {
        if (!HasNoRowLimit())
        {
            int colCount = Mathf.CeilToInt(GetCurrentElementCount() / GetMaxRowCount());
            int spillOver = (GetCurrentElementCount() % GetMaxRowCount());

            if (currentPosition.y == colCount)  //on partially filled row
            {
                currentPosition.x = Mathf.Clamp(currentPosition.x, 0, spillOver);
            }
            else
            {
                currentPosition.x = Mathf.Clamp(currentPosition.x, 0, GetMaxRowCount());
            }
        }
    }

    private void DeleteCurrent()
    {
        if (IsEmpty())
        {
            Debug.LogWarning("Tried to delete from an empty menu");
        }

        // TODO: actually delete (think about desired functionality)
    }

    // delete current
    // insert (needs to check less than max rows * columns)
}
