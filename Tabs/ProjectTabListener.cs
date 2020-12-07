using System;
using UnityEngine;
using UnityEngine.UI;

public class ProjectTabListener : MonoBehaviour
{
    public GameStudio studio;
    private Button tab;

    private void Awake()
    {
        tab = GetComponent<Button>();
    }

    private void Start()
    {
        if (studio.IsActivated())
        {
            tab.interactable = true;
        }
        studio.OnActivated += OnActivated;
    }

    private void OnActivated(object sender, EventArgs args)
    {
        tab.interactable = true;
    }
}
