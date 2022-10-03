using System;

public interface IGrowBuild
{
    public void StartGrow();
    public event Action OnGrowEnd;
}
