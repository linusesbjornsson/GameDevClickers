using System;
using UnityEngine;

public class ProjectUIManager : MonoBehaviour
{
    public ProjectSetupUI projectSetupUi;
    public ActiveProjectUI activeProjectUi;
    void Start()
    {
        projectSetupUi.OnProjectStarted += OnProjectStarted;
        activeProjectUi.OnProjectFinished += OnProjectFinished;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnProjectStarted(object sender, ProjectArgs args)
    {
        projectSetupUi.gameObject.SetActive(false);
        activeProjectUi.gameObject.SetActive(true);
        activeProjectUi.Setup(args.Project);
    }
    public void OnProjectFinished(object sender, ProjectArgs args)
    {
        projectSetupUi.gameObject.SetActive(true);
        activeProjectUi.gameObject.SetActive(false);
        projectSetupUi.Setup();
    }
}
