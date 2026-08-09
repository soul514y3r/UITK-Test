using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuSwitchScript : MonoBehaviour
{
    PanelRenderer panelRenderer;
    VisualElement Root;
    VisualElement main;
    Button Settings;
    TabView tabView;

    Button butSFX;
    Button butGRA;

    AudioSource audioSource;


    void Start()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        
        // Register the callback to get the root element
        panelRenderer.RegisterUIReloadCallback(OnUIReload);

        audioSource = gameObject.GetComponent<AudioSource>();


        
    }


    void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        // Store the reference to the root element
        Root = root;
        main = Root.Q<VisualElement>("MainScreenRoot");


    Settings = Root.Q<Button>("Settings_MainMenu");
    tabView = Root.Q<TabView>("SettingsMenu");
    butSFX = tabView.Q<Button>("ReturnButtonSFX");
    butGRA = tabView.Q<Button>("ReturnButtonGRA");

        
    Settings.clicked += SettingSwitch;
    butGRA.clicked += SettingSwitch;
    butSFX.clicked += SettingSwitch;
    tabView.activeTabChanged += OnActiveTabChanged;

    Debug.Log(Settings != null ? "Button found" : "Button NULL");
    Debug.Log(tabView != null ? "TabView found" : "TabView NULL");
    }

    void SettingSwitch()
    {
        if (tabView.resolvedStyle.display == DisplayStyle.None)
        {
            IevanPolkka();
            tabView.style.display = DisplayStyle.Flex;
            main.style.display = DisplayStyle.None;
        }
        else
        {
            sadness();
            tabView.style.display = DisplayStyle.None;
            main.style.display = DisplayStyle.Flex;
            
        }
    }
    void OnActiveTabChanged(Tab previousTab, Tab newTab)
    {
    // Do something while a specific tab is open
    if (newTab.name == "AudioTab")
    {
        IevanPolkka();
    }
    else
    sadness();

        
    }

    public void IevanPolkka()
    {
        audioSource.Play();
    }

    public void sadness()
    {
        audioSource.Stop();
    }
}
