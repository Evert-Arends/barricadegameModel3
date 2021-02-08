using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
// using Barricade.Enums;

namespace Barricade.Models
{
/*
    public class GameModelOld
    {
        public Square[][] Squares { get; set; }
        private Dictionary<int, List<Square>> _playerStartPositions = new Dictionary<int, List<Square>>();
        public List<Square> barricadeSquares = new List<Square>();
        private List<Square> _woodSquares = new List<Square>();
        private List<Square> _restSquares = new List<Square>();
        private List<Square> _finishSquares = new List<Square>();

        
        
        public int Dice { get; set; }

        private NormalSquare CreateSquare(char toString)
        {
            var squareImpl = new NormalSquare(toString.ToString());

            if (int.TryParse(toString.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var toInt))
            {
                var possibleSquare = _playerStartPositions.GetValueOrDefault(toInt, new List<Square>());
                possibleSquare?.Add(squareImpl);
                _playerStartPositions[toInt] = possibleSquare;
            }

            if (toString == '.')
            {
                return null;
            }

            return squareImpl;
        }

        private void CheckTypeAndAssign(Square currentSquare)
        {
            if (currentSquare == null) return;

            switch (currentSquare.ToString().ToLower())
            {
                case "x":
                    currentSquare.SquareType = SquareType.Barricade;
                    _barricadeSquares.Add(currentSquare);
                    break;
                case "w":
                    currentSquare.SquareType = SquareType.Woods;
                    _woodSquares.Add(currentSquare);
                    break;
                case "r":
                    currentSquare.SquareType = SquareType.Rest;
                    _restSquares.Add(currentSquare);
                    break;
                case "f":
                    currentSquare.SquareType = SquareType.FinishLine;
                    _finishSquares.Add(currentSquare);
                    break;
                case "n":
                    currentSquare.SquareType = SquareType.Normal;
                    _finishSquares.Add(currentSquare);
                    break;
                default:
                    currentSquare.SquareType = SquareType.Player;
                    break;
            }
        }

        private void RollDice()
        {
            var r = new Random();
            Dice = r.Next(1, 6);
        }

        public void GenerateBoardField(string[] lines)
        {
            // Linked list thingy (Basically)
            var field = lines.Select(line => line.ToCharArray()).ToArray();
            var height = field.Length;
            var width = field[0].Length;

            Squares = new Square[height / 2][];
            for (var i = 0; i < height; i += 2)
            {
                Squares[i / 2] = new Square[width / 2];

                for (var j = 0; j < width; j += 2)
                {
                    var squareX = i / 2;
                    var squareY = j / 2;
                    var currentSquare = Squares[squareX][squareY] = CreateSquare(field[i][j]);

                    CheckTypeAndAssign(currentSquare);

                    if (j != 0 && currentSquare != null)
                    {
                        var previousSquare = Squares[squareX][squareY - 1];
                        if (previousSquare != null)
                        {
                            previousSquare.RightConnectedSquare = currentSquare;
                            currentSquare.LeftConnectedSquare = previousSquare;
                        }
                    }

                    if (i != 0 && currentSquare != null)
                    {
                        var linePlace = field[i - 1][j];
                        if (linePlace == '|')
                        {
                            var upSquare = Squares[squareX - 1][squareY];
                            currentSquare.TopConnectedSquare = upSquare;
                            upSquare.DownConnectedSquare = currentSquare;
                        }
                    }
                }
            }
        }


        private void SelectBarricade()
        {
            
        }

        private void GetAvailableMoves()
        {
            
        }
        
        private List<MoveType> GetAvailableMoveTypesFromSquare(Square square)
        {
            List<MoveType> types = new List<MoveType>();

            if (square.DownConnectedSquare.ToString() != null)
            {
                types.Add(MoveType.Down);
            }

            if (square.TopConnectedSquare.ToString() != null)
            {
                types.Add(MoveType.Up);
            }

            if (square.RightConnectedSquare.ToString() != null)
            {
                types.Add(MoveType.Right);
            }

            if (square.LeftConnectedSquare.ToString() != null)
            {
                types.Add(MoveType.Left);
            }
            
            return types;
        }
    }
*/
}