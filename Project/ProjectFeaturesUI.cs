using System;
using TMPro;

public class ProjectFeaturesUI : ProjectSelectionUI
{
    public override void Setup()
    {
        studio.OnFeatureChanged += OnFeatureChanged;
        foreach (GameStudioFeature feature in studio.GetAvailableFeatures())
        {
            AddProjectSetupElement(feature);
        }
    }

    private void OnFeatureChanged(object sender, FeatureArgs args)
    {
        AddProjectSetupElement(args.Feature);
    }

    public override void ProjectTypeSelected(ProjectElement type)
    {
        type.SetSelected(!type.IsSelected());
        if (type.IsSelected())
        {
            OnProjectSetupElementSelected?.Invoke(this, new ProjectSelectionArgs(type));
        }
        else
        {
            OnProjectFeatureRemoved?.Invoke(this, new ProjectSelectionArgs(type));
        }
    }

    public EventHandler<ProjectSelectionArgs> OnProjectFeatureRemoved;
}
