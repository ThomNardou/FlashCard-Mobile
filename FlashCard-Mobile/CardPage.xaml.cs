using FlashCard_Mobile.ModelView;
using MauiIcons.Core;

namespace FlashCard_Mobile;

public partial class CardPage : ContentPage
{
	public CardPage()
	{
		InitializeComponent();
        // Temporary Workaround for url styled namespace in xaml
        _ = new MauiIcon();

        var vm = BindingContext as FlashCardsMVVM;
        vm.TranslateCard = MoveCardAnimation;
        vm.UnKnownCardList.Clear();

        vm.StartTime = DateTime.Now;

        Accelerometer.Default.ShakeDetected += vm.Default_ShakeDetected;
        Accelerometer.Default.Start(SensorSpeed.Default);

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Accelerometer.Default.Stop();
    }



    private void MoveCardAnimation(int angle)
    {
        this.cardBorder.RotateYTo(angle);
        this.CardContent.RotateYTo(angle);
    }
}