using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Barricade.Models;
using Barricade.Views;
using Pastel;

//set Abstract data structure 
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
            _gameView.PrintCurrentGameState(ViewData());
            PieceMovement();
        }

        private string[] ViewData()
        {
            if (_gameModel.WinningPlayer != null)
            {
                _gameView.PrintWinner(_gameModel.WinningPlayer.ToString());
            }

            var list = new List<string>();
            if (_gameModel.BarricadePieces.Count > 0)
            {
                list.Add(new string($"You have {_gameModel.BarricadePieces.Count + 1} barricades to move"));
            }
            else
            {
                list.Add(new string($"You have {_gameModel.Die.MovesRemaining.ToString()} steps left"));
                list.Add(new string($"Current active player: {_gameModel.GetActivePlayerName()}"));
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

            if(int.TryParse(fieldCharacter, out var parsedField))
            {
                fieldCharacter = fieldCharacter.Pastel(GameView.GetPlayerColor(parsedField));
            }

            fieldCharacter = fieldCharacter.Pastel(GameView.GetPieceColor(fieldCharacter));


            return fieldCharacter + connectionCharacter;
        }

        private void PieceMovement()
        {
            var run = true;

            while (run)
            {
                Key pressedKey = _gameView.ListenForKeys();
                switch (pressedKey)
                {
                    case Key.Down:
                        _gameModel.MovePieceDown();
                        break;
                    case Key.Left:
                        _gameModel.MovePieceLeft();
                        break;
                    case Key.Right:
                        _gameModel.MovePieceRight();
                        break;
                    case Key.Up:
                        _gameModel.MovePieceUp();
                        break;
                    case Key.Stop:
                        run = false;
                        break;
                    case Key.Next:
                        _gameModel.NextPiece();
                        break;
                    case Key.Enter:
                        _gameModel.MoveDone();
                        break;
                }

                _gameView.PrintCurrentGameState(ViewData());
            }
        }
    }
}