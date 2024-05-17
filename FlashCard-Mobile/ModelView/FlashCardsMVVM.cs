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
        private string cardValue = "Chargement en cours...";

        [ObservableProperty]
        private int currentCard = 1;

        [ObservableProperty]
        bool isVerso = false;

        [ObservableProperty]
        private string rectoInput = "";

        [ObservableProperty]
        private string versoInput = "";

        [ObservableProperty]
        private int totalCards = 0;

        public Action<int> TranslateCard {  get; set; }
        private int angle = 0;






        public FlashCardsMVVM()
        {
            RefreshCards();
            cardValue = cards[0].Recto;
        }

        [RelayCommand]
        private async void AddCard()
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

        }

        [RelayCommand]
        private void Test()
        {
            Debug.WriteLine(RectoInput, VersoInput);
        }

        [RelayCommand]
        private void moveToCardPage()
        {
            Shell.Current.Navigation.PushAsync(new CardPage());
        }

        [RelayCommand]
        private void ChangeCard()
        {
            if (CurrentCard < Cards.Count)
            {
                CurrentCard += 1;
                CardValue = Cards[CurrentCard - 1].Recto;
            }
            else
            {
                GoToLobby();
            }
        }

        private async void GoToLobby()
        {
            await Shell.Current.DisplayAlert("Vous avez terminé", "Bravo vous avez termnié votre liste !!", "Merci");
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand]
        private void ChangeCardFace()
        {
            if (IsVerso)
            {
                IsVerso = false;
                CardValue = Cards[CurrentCard - 1].Recto;
            }
            else
            {
                IsVerso = true;
                CardValue = Cards[CurrentCard - 1].Verso;
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

        [RelayCommand]
        private async void GoAddCardPage()
        {
            await Shell.Current.Navigation.PushAsync(new AddCard());
        }
    }
}
