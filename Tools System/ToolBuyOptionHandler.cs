using System;

public class ToolBuyOptionHandler
{
    protected int _option = 1;
    private static ToolBuyOptionHandler _instance = null;

    protected ToolBuyOptionHandler()
    {

    }

    public void SetOption(int option)
    {
        _option = option;
        OnOptionChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetOption()
    {
        return _option;
    }

    public static ToolBuyOptionHandler Instance()
    {
        if (_instance == null)
        {
            _instance = new ToolBuyOptionHandler();
        }
        return _instance;
    }

    public EventHandler OnOptionChanged;
}