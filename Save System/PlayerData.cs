
 [System.Serializable]
public class PlayerData
{
    public EntityData EntityData;
    public string Money;
    public string Technology;
    public string Design;
    public string Audio;
    public string Awards;
    public string PendingAwards;


    public PlayerData(Player player)
    {
        EntityData = new EntityData(player);
        Money = player.Money.ToString();
        Technology = player.Technology.ToString();
        Design = player.Design.ToString();
        Audio = player.Audio.ToString();
        Awards = player.Awards.ToString();
        PendingAwards = player.PendingAwards.ToString();
    }
}
