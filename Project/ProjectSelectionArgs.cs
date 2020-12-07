using System;

public class ProjectSelectionArgs : EventArgs
{
    public ProjectElement Element { get; private set; }
    public ProjectSelectionArgs(ProjectElement element)
    {
        Element = element;
    }
}
