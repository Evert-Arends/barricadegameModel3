using System;
using System.Collections.Generic;
using System.Drawing;

namespace Barricade.Views
{
    public class GameView
    {
        public void PrintCurrentGameState(IEnumerable<string> lines)
        {
            Console.Clear();
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        public static Color GetPlayerColor(int number)
        {
            switch (number)
            {
                case 1:
                    return Color.Red;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Blue;
                case 4:
                    return Color.Fuchsia;
                default:
                    return Color.White;
            }
        }
        public static Color GetPieceColor(string piece)
        {
            switch (piece)
            {
                case "X":
                    return Color.Tomato;
                case "R":
                    return Color.Yellow;
                case "F":
                    return Color.CornflowerBlue;
                default:
                    return Color.White;
            }
        }
        
        public void PrintWinner(string winner)
        {
            Console.Clear();
            Console.WriteLine($"Congratulations player {winner}! You won the game.");
        }

        public Key ListenForKeys()
        {
            var key = Console.ReadKey().Key;
            Dictionary<ConsoleKey, Key> berm = new Dictionary<ConsoleKey, Key>()
            {
                {ConsoleKey.DownArrow, Key.Down},
                {ConsoleKey.UpArrow, Key.Up},
                {ConsoleKey.LeftArrow, Key.Left},
                {ConsoleKey.RightArrow, Key.Right},
                {ConsoleKey.Enter, Key.Enter},
                {ConsoleKey.S, Key.Stop},
                {ConsoleKey.N, Key.Next},
            };
            return berm[key];
        }


        // VIEW want bepaald hoe mn ding er uitziet
        // Zorgen dat de view totaal los te koppelen is, en te vervangen voor een andere view
        // Domein logica representatie teruggeven aan de view, en dat koppel je aan de view
        // Alle shit die je moet doen voor specifieke representatie in de view stompen.
    }
}