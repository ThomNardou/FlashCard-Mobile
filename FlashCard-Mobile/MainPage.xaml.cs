using FlashCard_Mobile.ModelView;

namespace FlashCard_Mobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            //var vme = BindingContext as FlashCardsMVVM;

            //this.vm.Cards = vme.Cards;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.RefreshCards();
        }
    }

}
