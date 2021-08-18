using System;
using System.Collections.Generic;

namespace DeckOfCards
{
	class Program
	{

		static List<Card> Pot = new();
		static bool gameState = true;

		static Random random = new();
		static List<Card> UnshuffledDeck = new();
		static List<Card> ShuffledDeck = new();
		static List<Card> PlayerOne = new();
		static List<Card> PlayerTwo = new();

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
				if(!ConfirmCardCount(PlayerOne, PlayerTwo, 1))
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
			if (PlayerOne[0].Value > PlayerTwo[0].Value)
			{
				Console.WriteLine($"Player one wins: {PlayerOne[0].Name} beats {PlayerTwo[0].Name}");

				PlayerOne.Add(PlayerOne[0]);
				PlayerOne.RemoveAt(0);

				PlayerOne.Add(PlayerTwo[0]);
				PlayerTwo.RemoveAt(0);
				PlayerOne.AddRange(Pot);
				Pot.Clear();
				Console.WriteLine($"Card count - P1: {PlayerOne.Count} P2: {PlayerTwo.Count} Pot: {Pot.Count}");
			}
			else if (PlayerOne[0].Value < PlayerTwo[0].Value)
			{
				Console.WriteLine($"Player two wins: {PlayerTwo[0].Name} beats {PlayerOne[0].Name}");

				PlayerTwo.Add(PlayerTwo[0]);
				PlayerTwo.RemoveAt(0);

				PlayerTwo.Add(PlayerOne[0]);
				PlayerOne.RemoveAt(0);
				PlayerTwo.AddRange(Pot);
				Pot.Clear();
				Console.WriteLine($"Card count - P1: {PlayerOne.Count} P2: {PlayerTwo.Count} Pot: {Pot.Count}");
			}
			else
			{
				if(ConfirmCardCount(PlayerOne, PlayerTwo, 4))
				{

					Console.WriteLine(PlayerOne.Count);
					Console.WriteLine(PlayerTwo.Count);
					Console.WriteLine($"WAR - First 3 cards from each player go into the Pot");
					Console.WriteLine($"Player one puts in {PlayerOne[1].Name}, {PlayerOne[2].Name}, {PlayerOne[3].Name}");
					Console.WriteLine($"Player two puts in {PlayerTwo[1].Name}, {PlayerTwo[2].Name}, {PlayerTwo[3].Name}");
					Console.WriteLine($"Plus their initial cards of: {PlayerOne[0].Name} and {PlayerTwo[0].Name}");

					Pot.Add(PlayerOne[0]);
					Pot.Add(PlayerOne[1]);
					Pot.Add(PlayerOne[2]);
					Pot.Add(PlayerOne[3]);

					PlayerOne.RemoveRange(0, 4);

					Pot.Add(PlayerTwo[0]);
					Pot.Add(PlayerTwo[1]);
					Pot.Add(PlayerTwo[2]);
					Pot.Add(PlayerTwo[3]);

					PlayerTwo.RemoveRange(0, 4);

					Console.WriteLine($"Card count - P1: {PlayerOne.Count} P2: {PlayerTwo.Count} Pot: {Pot.Count}");
				}
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
