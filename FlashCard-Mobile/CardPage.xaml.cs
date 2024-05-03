using MauiIcons.Core;

namespace FlashCard_Mobile;

public partial class CardPage : ContentPage
{
	public CardPage()
	{
		InitializeComponent();
        // Temporary Workaround for url styled namespace in xaml
        _ = new MauiIcon();
    }
}