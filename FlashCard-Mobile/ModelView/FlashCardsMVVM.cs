using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashCard_Mobile.Model;
using FlashCard_Mobile.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCard_Mobile.ModelView
{
    public partial class FlashCardsMVVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Card> cards = new ObservableCollection<Card>();
        [ObservableProperty]
        private ObservableCollection<Card> unKnownCardList = new ObservableCollection<Card>();




        [ObservableProperty]
        private int currentCard = 1;

        [ObservableProperty]
        private int updatedCardId = 0;

        [ObservableProperty]
        private int totalCards = 0;


        [ObservableProperty]
        bool isVerso = false;

        [ObservableProperty]
        bool allCardKnow = false;


        [ObservableProperty]
        private string cardValue = "Chargement en cours...";

        [ObservableProperty]
        private string rectoInput = "";

        [ObservableProperty]
        private string versoInput = "";

        [ObservableProperty]
        private string oldRecto = "";

        [ObservableProperty]
        private string oldVerso = "";

        [ObservableProperty]
        DateTime startTime;



        public Action<int> TranslateCard {  get; set; }
        private int angle = 0;

        private bool firstTour = true;


        public FlashCardsMVVM()
        {
            RefreshCards();

            firstTour = true ;

            if (Cards.Count > 0)
            {
                cardValue = cards[0].Recto;
            }
            else
            {
                CardValue = "Vous n'avez pas encore enregistrer de carte";
            }
        }

        ////////////////////////////////////////////// CRUD //////////////////////////////////////////////



        [RelayCommand]
        public async void AddCard()
        {
            RectoVerso rv = new RectoVerso();
            rv.R = RectoInput;
            rv.V = VersoInput;

            if (rv.R == null || rv.V == null)
            {
                await Shell.Current.DisplayAlert("Erreur Lors de l'ajout", "Merci de rentrer des valeur valide !", "OK");
                return;
            }

            Card card = new Card()
            {
                Recto = rv.R,
                Verso = rv.V
            };

            using (var dbContext = new CardsContext())
            {
                dbContext.Add(card);
                await dbContext.SaveChangesAsync();
            }

            Cards.Add(card);

            await Shell.Current.DisplayAlert("Réussite", "La carte à bien été ajouté", "continuer");

            Debug.WriteLine(Cards.Count);
        }

        [RelayCommand]
        private async void DeleteCard(int cardId)
        {
            using (var dbContext = new CardsContext())
            {
                await dbContext.Cards
                    .Where(dbCard => dbCard.Id == cardId)
                    .ExecuteDeleteAsync();

                RefreshCards(dbContext);
            }
            await Shell.Current.DisplayAlert("Réussite", "Vous avez bien suprimmer la carte !!", "OK");
        }

        [RelayCommand]
        private async void UpdateCard()
        {


            if (RectoInput == "") { RectoInput = OldRecto; return; }
            if (VersoInput == "") { VersoInput = OldVerso; return; }
            
            using (var dbContext = new CardsContext())
            {
                await dbContext.Cards
                    .Where(dbCard => dbCard.Id == UpdatedCardId)
                    .ExecuteUpdateAsync(setter => setter.SetProperty(dbCard => dbCard.Recto, RectoInput).SetProperty(dbCard => dbCard.Verso, VersoInput));

                RefreshCards(dbContext);
            }

            await Shell.Current.DisplayAlert("Réussite", "Vous avez bien modifier la carte !!", "OK");
            await Shell.Current.Navigation.PopAsync();
            
        }



        ////////////////////////////////////////////// OTHER //////////////////////////////////////////////



        public void RefreshCards(CardsContext? context = null)
        {
            Cards.Clear();

            using (var dbContext = context ?? new CardsContext())
            {
                foreach (Card dbCard in dbContext.Cards)
                {
                    Cards.Add(dbCard);
                }
            }

            TotalCards = Cards.Count;

            //CardValue = Cards[0].Recto;

        }


        [RelayCommand]
        private void ChangeCard(bool isKnow)
        {

            if (!isKnow) { UnKnownCardList.Add(Cards[CurrentCard - 1]); }


            if (CurrentCard < TotalCards)
            {
                CurrentCard += 1;

                CardValue = Cards[CurrentCard - 1].Recto;
            }
            else
            {
                CurrentCard = 1;
                Cards.Clear();
                Cards = new ObservableCollection<Card>(UnKnownCardList);
                TotalCards = Cards.Count;

                if (TotalCards == 0)
                {
                    Debug.WriteLine(StartTime);
                    Debug.WriteLine(DateTime.Now);

                    TimeSpan timeDiff = DateTime.Now.Subtract(StartTime);

                    Shell.Current.DisplayAlert("BRAVO !!!", $"Vous avez terminer votre liste !! \nVous avez pris {Math.Round(timeDiff.TotalSeconds, 2)} Secondes !", "Merci");
                    Shell.Current.Navigation.PopAsync();
                }

                UnKnownCardList.Clear();
            }
        }

        public void Default_ShakeDetected(object? sender, EventArgs e)
        {
            Debug.WriteLine("Shaked");
            ChangeCard(false);
        }


        [RelayCommand]
        private void ChangeCardFace()
        {
            if (Cards.Count > 0)
            {
                if (IsVerso)
                {
                    IsVerso = false;
                    
                    if(firstTour) { CardValue = Cards[CurrentCard - 1].Recto; }
                    else { CardValue = UnKnownCardList[CurrentCard - 1].Recto; }
                }
                else
                {
                    IsVerso = true;
                    if (firstTour) { CardValue = Cards[CurrentCard - 1].Verso; }
                    else { CardValue = UnKnownCardList[CurrentCard - 1].Verso; }
                }
            }
        }

        [RelayCommand]
        private void RotateCard()
        {
            if (TranslateCard != null)
            {
                if (angle == 180)
                {
                    angle = 0;
                }
                else
                {
                    angle = 180;
                }
                
                TranslateCard.Invoke(angle);
            }
        }



        ////////////////////////////////////////////// NAVIGATION //////////////////////////////////////////////
        


        [RelayCommand]
        private void moveToCardPage()
        {
            Shell.Current.Navigation.PushAsync(new CardPage());
        }

        [RelayCommand]
        private async void GoAddCardPage()
        {
            await Shell.Current.Navigation.PushAsync(new AddCard());
        }

        [RelayCommand]
        private async void GoAllCardsPage()
        {
            await Shell.Current.Navigation.PushAsync(new AllCard());
        }

        [RelayCommand]
        private async void GoToLobby()
        {
            await Shell.Current.DisplayAlert("Vous avez terminé", "Bravo vous avez termnié votre liste !!", "Merci");
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand]
        private async void GoToUpdatePage(Card card)
        {
            await Shell.Current.Navigation.PushAsync(new UpdatePage(card.Id, card.Recto, card.Verso));
        }
    }
}
