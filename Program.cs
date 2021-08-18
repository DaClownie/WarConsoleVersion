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
			// Create the deck of cards into List UnshuffledDeck
			for (int i = 0; i <= (int)Values.Ace; i++)
			{
				for (int j = 0; j <= (int)Suits.Clubs; j++)
				{
					Card card = new((Values)i, (Suits)j);
					UnshuffledDeck.Add(card);
				}
			}
			// Shuffles deck, destroying the Unshuffled Deck in the process
			int deckSize = UnshuffledDeck.Count;
			for (int i = 0; i < deckSize; i++)
			{
				int index = random.Next(UnshuffledDeck.Count);

				ShuffledDeck.Add(UnshuffledDeck[index]);
				UnshuffledDeck.RemoveAt(index);
			}
			// Divides the cards evenly between each player, alternating back and forth like a real life deal of cards
			deckSize = ShuffledDeck.Count;
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
			// Game loop
			while (gameState)
			{
				// Breaks the game loop if one player doesn't have ANY cards.
				if(!ConfirmCardCount(PlayerOne, PlayerTwo, SINGLE))
				{
					gameState = false;
					DeclareWinner(PlayerOne, PlayerTwo);
					break;
				}
				// Allowed to run if a player has at least ONE card in their pile.
				War();
			}
		}

		static void War()
		{
			// Player One wins the hand
			if (PlayerOne[0].Value > PlayerTwo[0].Value)
			{
				Console.WriteLine($"Player one wins: {PlayerOne[0].Name} beats {PlayerTwo[0].Name}");
				AddToPot(SINGLE);
				PlayerOne.AddRange(Pot);
				Pot.Clear();
				Console.WriteLine($"Card count - P1: {PlayerOne.Count} P2: {PlayerTwo.Count} Pot: {Pot.Count}");
			}
			// Player Two wins the hand
			else if (PlayerOne[0].Value < PlayerTwo[0].Value)
			{
				Console.WriteLine($"Player two wins: {PlayerTwo[0].Name} beats {PlayerOne[0].Name}");
				AddToPot(SINGLE);
				PlayerTwo.AddRange(Pot);
				Pot.Clear();
				Console.WriteLine($"Card count - P1: {PlayerOne.Count} P2: {PlayerTwo.Count} Pot: {Pot.Count}");
			}
			else
			{
				// Players have enough cards for the Pot to fight a war
				if(ConfirmCardCount(PlayerOne, PlayerTwo, WAR))
				{
					Console.WriteLine($"WAR - P1: {PlayerOne[0].Name} vs. P2: {PlayerTwo[0].Name}");
					Console.WriteLine($"WAR - First 3 cards from each player go into the Pot");
					AddToPot(WAR);
					Console.WriteLine($"Card count - P1: {PlayerOne.Count} P2: {PlayerTwo.Count} Pot: {Pot.Count}");
				}
				// One or Both players do not have enough cards. Forces win/tie condition.
				else
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

			}
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
		private static void DeclareWinner(List<Card> playerOne, List<Card> playerTwo)
		{
			if (playerTwo.Count == 0 && playerOne.Count != 0)
			{
				Console.WriteLine($"Player one wins!");
			}
			else if (playerOne.Count == 0 && playerTwo.Count != 0)
			{
				Console.WriteLine($"Player two wins!");
			}
			else
			{
				Console.WriteLine($"Both players are out of cards! What are the odds? No really, maybe we can figure that out.");
			}
		}
		private static bool ConfirmCardCount(List<Card> playerOne, List<Card> playerTwo, int numberOfCardsRequired)
		{
			if (playerOne.Count < numberOfCardsRequired || playerTwo.Count < numberOfCardsRequired)
			{
				return false;
			}
			return true;
		}

	}
}
