using System;
using System.Collections.Generic;

namespace Barricade.Views
{
    public class GameView
    {
        public static void PrintCurrentGameState(IEnumerable<string> lines)
        {
            // Console.Clear();
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        // VIEW want bepaald hoe mn ding er uitziet
        // Zorgen dat de view totaal los te koppelen is, en te vervangen voor een andere view
        // Domein logica representatie teruggeven aan de view, en dat koppel je aan de view
        // Alle shit die je moet doen voor specifieke representatie in de view stompen.


    }
}