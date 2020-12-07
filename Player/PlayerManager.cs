
public class PlayerManager
{
    private Player _player;
    public PlayerManager(Player player)
    {
        _player = player;
    }

    public void Update(float time)
    {
        AddToolModifiers(time);
    }

    public void AddToolModifiers(float time)
    {
        Tool miner = _player.Tools.GetTool(Tool.ToolID.MINER);
        if (miner != null)
        {
            _player.Money += BigRational.Multiply(miner.GetModifier(), new BigRational(time));
        }
    }
}
