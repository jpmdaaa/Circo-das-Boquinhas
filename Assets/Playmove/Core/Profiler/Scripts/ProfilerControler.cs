using Tayx.Graphy;
using UnityEngine;

public class ProfilerControler : MonoBehaviour
{
    public bool enableOnStartup;
    public GameObject profilerButton;
    public GameObject modesButton;
    private GraphyManager graphy;
    private int mode = 3;
    private bool profilerActive;

    private void Start()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        //RemoveProfiler();
        GraphySetup();
#else
        RemoveProfiler();
#endif
    }

    public void ProfilerToggle()
    {
        graphy.ToggleActive();

        if (!profilerActive)
        {
            profilerActive = true;
            modesButton.gameObject.SetActive(true);
        }
        else if (profilerActive)
        {
            profilerActive = false;
            modesButton.gameObject.SetActive(false);
        }
    }

    public void ProfilerToggleModes()
    {
        //quero mimir, vai esse negocio feio mesmo
        if (mode == 3)
        {
            mode = 1;
            graphy.PresetChange(mode);
        }
        else if (mode == 1)
        {
            mode = 2;
            graphy.PresetChange(mode);
        }
        else if (mode == 2)
        {
            mode = 3;
            graphy.PresetChange(mode);
        }
    }

    private void GraphySetup()
    {
        graphy = GetComponentInChildren<GraphyManager>();

        if (enableOnStartup)
        {
            graphy.EnableOnStartup = true;
            graphy.ToggleActive();
            profilerActive = true;
        }

        if (profilerActive)
            modesButton.gameObject.SetActive(true);
        else
            modesButton.gameObject.SetActive(false);
    }

    private void RemoveProfiler()
    {
        Destroy(profilerButton);
        Destroy(this);
        Destroy(graphy.gameObject);
    }
}