using System;
using System.Collections.Generic;

namespace DeckOfCards
{
	class Program
	{
		const int SINGLE = 1;
		const int WAR = 4;
		static bool gameState = true;
		static Random random = new();

		static List<Card> UnshuffledDeck = new();
		static List<Card> ShuffledDeck = new();
		static List<Card> PlayerOne = new();
		static List<Card> PlayerTwo = new();
		static List<Card> Pot = new();

		static void Main(string[] args)
		{
			CreateUnshuffledDeck();
			ShuffleTheUnshuffledDeck();
			DealTheCardsToThePlayers();
			while (gameState)
			{
				// Breaks the game loop if one player doesn't have ANY cards.
				if(!ConfirmPlayersHaveEnoughCards(SINGLE))
				{
					gameState = false;
					DeclareWinner();
					break;
				}
				// Allowed to run if a player has at least ONE card in their pile.
				PlayHand();
			}
		}
		private static void DealTheCardsToThePlayers()
		{
			int deckSize = ShuffledDeck.Count;
			for (int i = 0; i < deckSize; i++)
			{
				if (i % 2 == 0)
				{
					PlayerOne.Add(ShuffledDeck[i]);
				}
				else
				{
					PlayerTwo.Add(ShuffledDeck[i]);
				}
			}
		}
		private static void ShuffleTheUnshuffledDeck()
		{
			int deckSize = UnshuffledDeck.Count;
			for (int i = 0; i < deckSize; i++)
			{
				int index = random.Next(UnshuffledDeck.Count);

				ShuffledDeck.Add(UnshuffledDeck[index]);
				UnshuffledDeck.RemoveAt(index);
			}
		}
		private static void CreateUnshuffledDeck()
		{
			for (int i = 0; i <= (int)Values.Ace; i++)
			{
				for (int j = 0; j <= (int)Suits.Clubs; j++)
				{
					Card card = new((Values)i, (Suits)j);
					UnshuffledDeck.Add(card);
				}
			}
		}
		static void PlayHand()
		{
			if (PlayerOne[0].Value > PlayerTwo[0].Value)
			{
				Console.WriteLine($"Player one wins: {PlayerOne[0].Name} beats {PlayerTwo[0].Name}");
				AddToPot(SINGLE);
				PlayerOne.AddRange(Pot);
				Pot.Clear();
				DisplayHandAndPotAmounts();
			}
			else if (PlayerOne[0].Value < PlayerTwo[0].Value)
			{
				Console.WriteLine($"Player two wins: {PlayerTwo[0].Name} beats {PlayerOne[0].Name}");
				AddToPot(SINGLE);
				PlayerTwo.AddRange(Pot);
				Pot.Clear();
				DisplayHandAndPotAmounts();
			}
			else
			{
				FightAWar();
			}
		}
		private static void FightAWar()
		{
			if (ConfirmPlayersHaveEnoughCards(WAR))
			{
				Console.WriteLine($"WAR - P1: {PlayerOne[0].Name} vs. P2: {PlayerTwo[0].Name}");
				Console.WriteLine($"WAR - First 3 cards from each player go into the Pot");
				AddToPot(WAR);
				DisplayHandAndPotAmounts();
			}
			else
			{
				EmptyPlayerHandsToEndGame();
			}
		}
		private static void EmptyPlayerHandsToEndGame()
		{
			if (PlayerOne.Count < 4 && PlayerTwo.Count >= 4)
			{
				Console.WriteLine($"Player One doesn't have enough cards to finish the war!");
				Pot.AddRange(PlayerOne);
				PlayerOne.Clear();
			}
			else if (PlayerTwo.Count < 4 && PlayerOne.Count >= 4)
			{
				Console.WriteLine($"Player Two doesn't have enough cards to finish the war!");
				Pot.AddRange(PlayerTwo);
				PlayerTwo.Clear();
			}
			else
			{
				Console.WriteLine($"Neither player has enough cards to finish the war!");
				Pot.AddRange(PlayerOne);
				Pot.AddRange(PlayerTwo);
				PlayerOne.Clear();
				PlayerTwo.Clear();
			}
		}
		private static void DisplayHandAndPotAmounts()
		{
			Console.WriteLine($"Card count - P1: {PlayerOne.Count} P2: {PlayerTwo.Count} Pot: {Pot.Count}");
		}
		private static void AddToPot(int numberOfCardsToAdd)
		{
			for (int i = 0; i < numberOfCardsToAdd; i++)
			{
				Pot.Add(PlayerOne[i]);
				Pot.Add(PlayerTwo[i]);
			}
			PlayerOne.RemoveRange(0, numberOfCardsToAdd);
			PlayerTwo.RemoveRange(0, numberOfCardsToAdd);
		}
		private static void DeclareWinner()
		{
			if (PlayerTwo.Count == 0 && PlayerOne.Count != 0)
			{
				Console.WriteLine($"Player one wins!");
			}
			else if (PlayerOne.Count == 0 && PlayerTwo.Count != 0)
			{
				Console.WriteLine($"Player two wins!");
			}
			else
			{
				Console.WriteLine($"Both players are out of cards! What are the odds? No really, maybe we can figure that out.");
			}
		}
		private static bool ConfirmPlayersHaveEnoughCards(int numberOfCardsRequired)
		{
			return !(PlayerOne.Count < numberOfCardsRequired || PlayerTwo.Count < numberOfCardsRequired);
		}
	}
}
