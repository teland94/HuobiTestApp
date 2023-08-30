using HuobiTestApp.Base;

namespace HuobiTestApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    void RefreshView_Refreshing(object sender, EventArgs e)
    {
        if (RefreshablePageBase.Current?.NavigationManager != null)
        {
            var navigationManager = RefreshablePageBase.Current.NavigationManager;
            navigationManager.NavigateTo(navigationManager.Uri, true, true);
            RefreshView.IsRefreshing = false;
        }
    }
}
