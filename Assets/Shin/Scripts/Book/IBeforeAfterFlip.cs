
public interface IBeforeAfterFlip
{
    public void OnBeforeFlip(int currentStage, out int waitTime);
    public void OnAfterFlip(int currentStage);
}