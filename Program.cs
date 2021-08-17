using System;
using System.Collections.Generic;

namespace DeckOfCards
{
	class Program
	{

		static List<Card> Pot = new();

		static void Main(string[] args)
		{
			Random random =  new();
			List<Card> UnshuffledDeck = new();
			List<Card> ShuffledDeck = new();
			List<Card> PlayerOne = new();
			List<Card> PlayerTwo = new();

			for (int i = 0; i <= (int)Values.Ace; i++)
			{
				for (int j = 0; j <= (int)Suits.Clubs; j++)
				{
					Card card = new((Values)i, (Suits)j);
					UnshuffledDeck.Add(card);
				}
			}

			int deckSize = UnshuffledDeck.Count;
			for (int i = 0; i < deckSize; i++)
			{
				ShuffledDeck.Add(UnshuffledDeck[random.Next(UnshuffledDeck.Count)]);
			}

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

			while (ConfirmCardCount(PlayerOne, PlayerTwo, 1))
			{
				War(PlayerOne, PlayerTwo);
			}
		}

		static bool ConfirmCardCount(List<Card> playerOne, List<Card> playerTwo, int numberOfCardsRequired)
		{
			if (playerOne.Count < numberOfCardsRequired && playerTwo.Count < numberOfCardsRequired)
			{
				Console.WriteLine($"Both players lose! Better luck next time!");
				return false;
			}
			else if (playerTwo.Count < numberOfCardsRequired)
			{
				Console.WriteLine($"Player one wins!");
				return false;
			}
			else if (playerOne.Count < numberOfCardsRequired)
			{
				Console.WriteLine($"Player two wins!");
				return false;
			}
			else
			{
				return true;
			}
		}

		static void War(List<Card> playerOne, List<Card> playerTwo)
		{
			ConfirmCardCount(playerOne, playerTwo, 1);
			if (playerOne[0].Value > playerTwo[0].Value)
			{
				Console.WriteLine($"Player one wins: {playerOne[0].Name} beats {playerTwo[0].Name}");

				playerOne.Add(playerOne[0]);
				playerOne.RemoveAt(0);

				playerOne.Add(playerTwo[0]);
				playerTwo.RemoveAt(0);
				playerOne.AddRange(Pot);
				Pot.Clear();
				Console.WriteLine($"Card count - P1: {playerOne.Count} P2: {playerTwo.Count} Pot: {Pot.Count}");
			}
			else if (playerOne[0].Value < playerTwo[0].Value)
			{
				Console.WriteLine($"Player two wins: {playerTwo[0].Name} beats {playerOne[0].Name}");

				playerTwo.Add(playerTwo[0]);
				playerTwo.RemoveAt(0);

				playerTwo.Add(playerOne[0]);
				playerOne.RemoveAt(0);
				playerTwo.AddRange(Pot);
				Pot.Clear();
				Console.WriteLine($"Card count - P1: {playerOne.Count} P2: {playerTwo.Count} Pot: {Pot.Count}");
			}
			else
			{
				ConfirmCardCount(playerOne, playerTwo, 4);

				Console.WriteLine($"WAR - First 3 cards from each player go into the Pot");
				Console.WriteLine($"Player one puts in {playerOne[1].Name}, {playerOne[2].Name}, {playerOne[3].Name}");
				Console.WriteLine($"Player two puts in {playerTwo[1].Name}, {playerTwo[2].Name}, {playerTwo[3].Name}");
				Console.WriteLine($"Plus their initial cards of: {playerOne[0].Name} and {playerTwo[0].Name}");

				Pot.Add(playerOne[0]);
				Pot.Add(playerOne[1]);
				Pot.Add(playerOne[2]);
				Pot.Add(playerOne[3]);

				playerOne.RemoveRange(0, 4);

				Pot.Add(playerTwo[0]);
				Pot.Add(playerTwo[1]);
				Pot.Add(playerTwo[2]);
				Pot.Add(playerTwo[3]);

				playerTwo.RemoveRange(0, 4);

				Console.WriteLine($"Card count - P1: {playerOne.Count} P2: {playerTwo.Count} Pot: {Pot.Count}");

				War(playerOne, playerTwo);
			}
		}
	}
}
