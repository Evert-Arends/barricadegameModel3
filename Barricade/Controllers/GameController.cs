using System;
using System.Collections.Generic;
using System.Linq;
using Barricade.Models;
using Barricade.Views;

namespace Barricade.Controllers
{
    public class GameController
    {
        private readonly GameView _gameView;
        private readonly GameModel _gameModel;

        public GameController(string[] lines)
        {
            _gameModel = new GameModel();
            _gameModel.GenerateBoardField(lines);
            _gameModel.MoveBarriers();
            _gameView = new GameView();
        }


        public void RunGame()
        {
            GameView.PrintCurrentGameState(ViewData());
            PieceMovement();
        }


        private string[] ViewData()
        {
            var list = new List<string>();
            if (_gameModel.BarricadePieces.Count > 0)
            {
                list.Add(new string($"You have {_gameModel.BarricadePieces.Count + 1} barricades to move"));
            }
            else
            {
                list.Add(new string($"You have {_gameModel.Die.ThrowAmount.ToString()} steps left"));

            }

            foreach (var field in _gameModel.Fields)
            {
                // First we check horizontal to see if there are any connections to the right
                var strings = string.Join("", field.Select(ToSquareConnectionString));
                list.Add(strings);

                // Then we do a vertical check if we need to add | or stuff to the array as a new row
                // (To render in the view).
                var enumerable = string.Join("", field.Select(ToVerticalConnectionString));
                list.Add(enumerable);
            }

            return list.ToArray();
        }

        private static string ToVerticalConnectionString(Field field)
        {
            return field?.DownConnectedField != null ? "| " : "  ";
        }

        private static string ToSquareConnectionString(Field field)
        {
            var fieldCharacter = field?.ToString() ?? " ";
            var connectionCharacter = field?.RightConnectedField != null ? "-" : " ";
            return fieldCharacter + connectionCharacter;
        }

        public void PieceMovement()
        {
            var oldModel = _gameModel;
            var run = true;
            while (run)
            {
                GameView.PrintCurrentGameState(ViewData());
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    //TODO move to view
                    case ConsoleKey.DownArrow:
                        Console.WriteLine("Down arrow pressed");
                        Console.WriteLine(_gameModel.MovePieceDown().ToString());
                        break;
                    case ConsoleKey.UpArrow:
                        Console.WriteLine("Up arrow pressed");
                        Console.WriteLine(_gameModel.MovePieceUp().ToString());
                        break;
                    case ConsoleKey.LeftArrow:
                        Console.WriteLine("Left arrow pressed");
                        Console.WriteLine(_gameModel.MovePieceLeft().ToString());
                        break;
                    case ConsoleKey.RightArrow:
                        Console.WriteLine("Right arrow pressed");
                        Console.WriteLine(_gameModel.MovePieceRight().ToString());
                        break;
                    case ConsoleKey.N:
                        Console.WriteLine("Next Piece requested");
                        _gameModel.NextPiece();
                        break;
                    case ConsoleKey.Enter:
                        _gameModel.MoveDone();
                        break;
                    case ConsoleKey.S:
                        Console.WriteLine("Stopping");
                        run = false;
                        break;
                }
            }
        }
    }
}