using System;

public class FeatureArgs : EventArgs
{
    public GameStudioFeature Feature { get; private set; }
    public FeatureArgs(GameStudioFeature feature)
    {
        Feature = feature;
    }
}