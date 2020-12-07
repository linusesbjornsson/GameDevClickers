using System;
using System.Collections.Generic;
using System.Numerics;

public class SoundStudio : ConcreteTool
{
    public SoundStudio(int level, Entity player) : base("Sound Studio", level, player)
    {
    }

    public SoundStudio(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Sound Studio", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(100);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 1.25)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increases effectiveness of Audio by <TOOL_VALUE>%");
    }
    public override string ToolTipModifier()
    {
        return BigRational.Multiply(GetModifier(), new BigRational(100)).Format();
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(0.02f) * new BigRational(Level);
        }
        return _modifier;
    }

    public override object Clone()
    {
        SoundStudio tool = new SoundStudio(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.SOUND_STUDIO;
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new Producer(1, _entity, this));
        SetupUpgrade(new Producer(2, _entity, this));
        SetupUpgrade(new Producer(3, _entity, this));
        SetupUpgrade(new Producer(4, _entity, this));
        SetupUpgrade(new Producer(5, _entity, this));
        SetupUpgrade(new Producer(6, _entity, this));
        SetupUpgrade(new Producer(7, _entity, this));
        SetupUpgrade(new Producer(8, _entity, this));
        SetupUpgrade(new Producer(9, _entity, this));
        SetupUpgrade(new Producer(10, _entity, this));
    }
}
