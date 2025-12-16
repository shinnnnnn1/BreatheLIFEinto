using UnityEngine;

public interface IBookAfterFlip
{
    public void OnBeforeFlip(int currentPage);
    public void OnAfterFlip(int currentPage);
}
