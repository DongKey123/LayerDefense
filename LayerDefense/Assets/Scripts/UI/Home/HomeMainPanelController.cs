
public class HomeMainPanelController : UIBaseController
{
    private HomeMainPanel homeMainPanelView = null;
    private UIManager uiManager;

    private const string cardSettingPanelName = "Home/CardSettingPanel";
    private const string stagePanelName = "Home/StagePanel";

    protected override void Awake()
    {
        base.Awake();

        //Cashing
        uiManager = UIManager.getInstance;


        //Button 설정
        homeMainPanelView = this.GetComponent<HomeMainPanel>();
        homeMainPanelView.SetStartButton(OpenStagePanel);
        homeMainPanelView.SetCardSettingButton(OpenSettingCardPanel);
    }


    private void OpenStagePanel()
    {
        uiManager.Show<StagePanelController>(stagePanelName);
    }

    private void OpenSettingCardPanel()
    {
        uiManager.Show<CardSettingPanelController>(cardSettingPanelName);
    }
}
