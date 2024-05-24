using FlashCard_Mobile.ModelView;

namespace FlashCard_Mobile;

public partial class UpdatePage : ContentPage
{
	public UpdatePage(int id, string oldRecto, string oldVerso)
	{
		InitializeComponent();
		var vm = this.BindingContext as FlashCardsMVVM;
		vm.UpdatedCardId = id;
		vm.OldRecto = oldRecto;
		vm.OldVerso = oldVerso;
	}
}