using ActionPlatformerKit;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class UIManager : BaseManager<UIManager>
{
    public GamePanel GamePanel;
    public MenuPanel MenuPanel;
    public SettingPanel SettingPanel;
    public WinPanel WinPanel;
    public LosePanel LoosePanel;
    public PausePanel PausePanel;



    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        MenuPanel.gameObject.SetActive(true);
        WinPanel.gameObject.SetActive(false);
        SettingPanel.gameObject.SetActive(false);
        GamePanel.gameObject.SetActive(false);
        PausePanel.gameObject.SetActive(false);
    }
}