using System;

public class ToolArgs : EventArgs
{
    public ConcreteTool Tool { get; private set; }
    public ToolUpgrade Upgrade { get; private set; }
    public ToolArgs(ConcreteTool tool)
    {
        Tool = tool;
    }

    public ToolArgs(ConcreteTool tool, ToolUpgrade upgrade) : this(tool)
    {
        Upgrade = upgrade;
    }
}
