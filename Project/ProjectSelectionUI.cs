using System;
using UnityEngine;

public class ProjectSelectionUI : MonoBehaviour
{
    private ProjectElement selectedProjectType = null;
    protected RectTransform _rectTransform;
    public GameStudio studio;

    void Start()
    {
        Setup();
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public virtual void Setup()
    {
        if (name.Equals("Type Content"))
        {
            foreach (GameStudioType type in studio.GetAvailableTypes())
            {
                AddProjectSetupElement(type);
            }
        }
        else
        {
            foreach (GameStudioGenre genre in studio.GetAvailableGenres())
            {
                AddProjectSetupElement(genre);
            }
        }
        if (selectedProjectType != null)
        {
            selectedProjectType.SetSelected(false);
            selectedProjectType = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddProjectSetupElement(IGameStudioElement element)
    {
        ProjectElement projectType = ProjectElement.Create(transform, element);
        projectType.AddClickListener(() => ProjectTypeSelected(projectType));
    }

    public virtual void ProjectTypeSelected(ProjectElement type)
    {
        if (selectedProjectType == null)
        {
            selectedProjectType = type;
            type.SetSelected(true);
        }
        else
        {
            if (!type.GetName().Equals(selectedProjectType.GetName()))
            {
                selectedProjectType.SetSelected(false);
                type.SetSelected(true);
                selectedProjectType = type;
            }
            else if (!selectedProjectType.IsSelected())
            {
                selectedProjectType.SetSelected(true);
            }
        }
        OnProjectSetupElementSelected?.Invoke(this, new ProjectSelectionArgs(selectedProjectType));
    }

    public EventHandler<ProjectSelectionArgs> OnProjectSetupElementSelected;
}
