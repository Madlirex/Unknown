using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public MainMenuActions InputControls;

    public GameObject startMenu;
    public GameObject mainMenu;
    public GameObject activeMenu;

    public void Awake()
    {
        InputControls = new MainMenuActions();

        InputControls.Menu.Start.performed += _ => ExitStartMenu();
    }

    public void OnEnable()
    {
        InputControls.Enable();
    }

    public void OnDisable()
    {
        InputControls.Disable();
    }
    
    public void SetMenu(GameObject menu)
    {
        activeMenu.SetActive(false);
        activeMenu = menu;
        activeMenu.SetActive(true);
    }

    private void ExitStartMenu()
    {
        startMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
