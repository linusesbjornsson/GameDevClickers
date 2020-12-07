using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static Job;
using static Reward;

public class GameStudio : Entity, ILevelRewardProvider
{
    private bool _isActive = false;
    private List<GameStudioType> _availableTypes = new List<GameStudioType>();
    private Dictionary<GameStudioGenre.Genre, GameStudioGenre> _availableGenres = new Dictionary<GameStudioGenre.Genre, GameStudioGenre>();
    private List<GameStudioFeature> _availableFeatures = new List<GameStudioFeature>();
    private RewardManager _rewardManager;

    public static readonly int GAME_STUDIO_PRICE = 25000;

    protected override void OnAwake()
    {
        base.OnAwake();
        _rewardManager = new RewardManager(this);
        _availableTypes.Add(new GameStudioType(this, "Solo Project", 0.75f));
        _availableGenres.Add(GameStudioGenre.Genre.RPG, new GameStudioGenre(this, GameStudioGenre.Genre.RPG, new FocusArea[] {
            new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, 1f),
            new FocusArea(FocusArea.FocusPoint.DESIGN, .7f),
            new FocusArea(FocusArea.FocusPoint.AUDIO, .7f)
        }));
        _availableGenres.Add(GameStudioGenre.Genre.ACTION, new GameStudioGenre(this, GameStudioGenre.Genre.ACTION, new FocusArea[] {
            new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, .8f),
            new FocusArea(FocusArea.FocusPoint.DESIGN, 1f),
            new FocusArea(FocusArea.FocusPoint.AUDIO, .65f)
        }));
        _availableGenres.Add(GameStudioGenre.Genre.ADVENTURE, new GameStudioGenre(this, GameStudioGenre.Genre.ADVENTURE, new FocusArea[] {
            new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, .7f),
            new FocusArea(FocusArea.FocusPoint.DESIGN, 1f),
            new FocusArea(FocusArea.FocusPoint.AUDIO, .7f)
        }));
        _availableGenres.Add(GameStudioGenre.Genre.HORROR, new GameStudioGenre(this, GameStudioGenre.Genre.HORROR, new FocusArea[] {
            new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, .65f),
            new FocusArea(FocusArea.FocusPoint.DESIGN, .85f),
            new FocusArea(FocusArea.FocusPoint.AUDIO, 1f)
        }));
        _availableFeatures.Add(new GameStudioFeature(this, "Multiplayer", 1.2f, 5));
        GameStudioFeature onlineFeature = new GameStudioFeature(this, "Online", 1.3f, 5);
        _rewardManager.AddReward(new Reward(2, onlineFeature, "Feature: " + onlineFeature.GetName(), "Add online capabilities to your games", RewardType.FEATURE));
        GameStudioType smallGame = new GameStudioType(this, "Small Game", 0.85f);
        _rewardManager.AddReward(new Reward(3, smallGame, "Type: " + smallGame.GetName(), "Adds the capability to create small games", RewardType.TYPE));
        _rewardManager.AddReward(new Reward(5, null, "Lab", "Unlocks the lab", RewardType.INFO));
        _rewardManager.AddReward(new Reward(25, null, "Award winning games", "Completing projects will give you awards that can be spent on prestige upgrades", RewardType.INFO));
        _rewardManager.FilterRewards();
    }

    private void Awake()
    {
        OnAwake();
    }

    public GameStudioGenre GetGenre(GameStudioGenre.Genre genre)
    {
        return _availableGenres[genre];
    }

    public List<GameStudioGenre> GetAvailableGenres()
    {
        return _availableGenres.Values.ToList();
    }

    public List<GameStudioType> GetAvailableTypes()
    {
        return _availableTypes;
    }

    public List<GameStudioFeature> GetAvailableFeatures()
    {
        return _availableFeatures;
    }

    public void AddFeature(GameStudioFeature feature)
    {
        _availableFeatures.Add(feature);
        OnFeatureChanged?.Invoke(this, new FeatureArgs(feature));
    }

    public bool IsActivated()
    {
        return _isActive;
    }

    public void Activate()
    {
        _isActive = true;
        OnActivated?.Invoke(this, EventArgs.Empty);
    }

    public RewardManager GetManager()
    {
        return _rewardManager;
    }

    public void AddReward(Reward reward)
    {
        switch (reward.Type)
        {
            case RewardType.FEATURE:
                AddFeature((GameStudioFeature)reward.Payload);
                break;
        }
    }

    public void Initialize(GameStudioData data, Player player)
    {
        Initialize(data.EntityData, new Entity[] { player });
        _isActive = data.IsActivated;
        GameStudioGenre action = GetGenre(GameStudioGenre.Genre.ACTION);
        action.Initialize(data.ActionData);
        GameStudioGenre adventure = GetGenre(GameStudioGenre.Genre.ADVENTURE);
        adventure.Initialize(data.AdventureData);
        GameStudioGenre horror = GetGenre(GameStudioGenre.Genre.HORROR);
        horror.Initialize(data.HorrorData);
        GameStudioGenre rpg = GetGenre(GameStudioGenre.Genre.RPG);
        rpg.Initialize(data.RpgData);
        _rewardManager.FilterRewards();
    }

    public EventHandler<FeatureArgs> OnFeatureChanged;
    public EventHandler OnActivated;
}
