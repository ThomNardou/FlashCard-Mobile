namespace FlashCard_Mobile;
using MauiIcons.Core;

public partial class AllCard : ContentPage
{
    public AllCard()
    {
        InitializeComponent();

        _ = new MauiIcon();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        vm.RefreshCards();
    }
}