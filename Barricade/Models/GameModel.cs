using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Barricade.Models
{
    public class GameModel
    {
        private const int AmountOfPlayers = 4;
        public readonly List<BarricadePiece> BarricadePieces = new List<BarricadePiece>(6);
        private readonly List<Player> _players = new List<Player>(AmountOfPlayers);
        public Field[][] Fields { get; private set; }
        private Piece _activePiece;

        public readonly Die Die = new Die();

        private int _playerTurnNumber;

        public GameModel()
        {
            _playerTurnNumber = 0;
            GeneratePlayers();
        }


        public void MoveBarriers()
        {
            if (BarricadePieces.Count <= 0) return;

            MoveBarrier(BarricadePieces[0]);
            BarricadePieces.Remove(BarricadePieces[0]);
        }

        private void MoveBarrier(BarricadePiece barricadePiece)
        {
            SetPieceActive(barricadePiece);
        }

        private bool MoveAllowed(Field fieldFrom, Field fieldTo)
        {
            if (fieldFrom == null || fieldTo == null)
            {
                return false;
            }

            if (_activePiece is PlayerPiece && Die.ThrowAmount == 0)
            {
                return false;
            }

            switch (fieldTo)
            {
                // no amount check needed for woods.
                case WoodField _:
                    return true;
                // Barriers not on rest fields
                case RestField _:
                    return !(_activePiece is BarricadePiece);
            }

            if (fieldTo.Pieces == null)
            {
                return true;
            }

            if (_activePiece is BarricadePiece)
            {
                if (fieldTo.Pieces.Count > 1)
                {
                    return false;
                }
            }


            if (fieldTo.Pieces.Count == 0)
            {
                return true;
            }

            if (fieldTo.Pieces.Count > 0)
            {
                return fieldTo.Pieces[0] switch
                {
                    // you can slay them, but you can't skip over them.
                    BarricadePiece _ when Die.ThrowAmount == 1 => true,
                    BarricadePiece _ => false,
                    PlayerPiece _ => true,
                    _ => true
                };
            }

            return false;
        }

        public bool MovePieceUp()
        {
            return _activePiece.PieceField.TopConnectedField != null &&
                   MovePiece(_activePiece.PieceField.TopConnectedField);
        }

        public bool MovePieceDown()
        {
            return _activePiece.PieceField.DownConnectedField != null &&
                   MovePiece(_activePiece.PieceField.DownConnectedField);
        }

        public bool MovePieceLeft()
        {
            return _activePiece.PieceField.LeftConnectedField != null &&
                   MovePiece(_activePiece.PieceField.LeftConnectedField);
        }

        public bool MovePieceRight()
        {
            return _activePiece.PieceField.RightConnectedField != null &&
                   MovePiece(_activePiece.PieceField.RightConnectedField);
        }

        private bool MovePiece(Field connectedField)
        {
            var legalMove = MoveAllowed(_activePiece.PieceField, connectedField);
            if (!legalMove) return false;
            // need to implement Count and null check everywhere
            _activePiece.PieceField.Pieces.Remove(_activePiece);
            // Stack with the walked fields
            _activePiece.FieldsWalked.Push(_activePiece.PieceField);

            _activePiece.PieceField = connectedField;
            _activePiece.PieceField.Pieces.Add(_activePiece);
            Die.RemoveOneEye();

            return true;
        }

        private void SetPieceActive(Piece piece)
        {
            _activePiece = piece;
        }

        private void ResetPiece(Piece piece)
        {
            // Remove reference
            piece.PieceField.Pieces.Remove(piece);
            // New reference
            piece.PieceField = piece.StartOutField;
            piece.StartOutField.Pieces.Add(piece);

            if (piece is PlayerPiece)
            {
                Die.RevertThrownAmount();
            }
        }

        public void MoveDone()
        {
            // This does not matter for barricades, because die should be 0 anyway
            if (Die.ThrowAmount != 0)
            {
                // Pressing enter when you're not done resets the piece to start position.
                ResetPiece(_activePiece);
                return;
            }

            if (_activePiece.PieceField.Pieces.Count > 1)
            {
                // Check if it's a barricade and revert all, or slay a piece.
                if (_activePiece.PieceField.Pieces[0] is BarricadePiece)
                {
                    MoveBarrier(_activePiece.PieceField.Pieces[0] as BarricadePiece);
                }
                else
                {
                    // Slay player and move it home
                    NextPlayer();
                }
            }
            else
            {
                NextPlayer();
            }
        }

        private void NextPlayer()
        {
            if (BarricadePieces.Count > 0)
            {
                MoveBarriers();
            }
            else
            {
                SetPieceActive(_players[_playerTurnNumber].ActivePlayerPiece =
                    _players[_playerTurnNumber].PlayerPieces[0]);

                _playerTurnNumber++;

                if (_playerTurnNumber == _players.Count)
                {
                    _playerTurnNumber = 0;
                }
                
                // Set the backup location
                _activePiece.StartOutField = _activePiece.PieceField;
                
                Die.ThrowDie();
            }
        }

        public void NextPiece()
        {
            if (_activePiece is PlayerPiece activePiece)
            {
                var index = _players[_playerTurnNumber].PlayerPieces.FindIndex(a => a == activePiece);
                if (index == _players[_playerTurnNumber].PlayerPieces.Count)
                {
                    index = 0;
                }

                if (index >= 0)
                {
                    _activePiece = _players[_playerTurnNumber].PlayerPieces[index + 1];
                }
            }
        }

        public void GenerateBoardField(string[] lines)
        {
            // Linked list thingy (Basically)
            var field = lines.Select(line => line.ToCharArray()).ToArray();
            var height = field.Length;
            var width = field[0].Length;

            Fields = new Field[height / 2][];

            for (var i = 0; i < height; i += 2)
            {
                Fields[i / 2] = new Field[width / 2];

                for (var j = 0; j < width; j += 2)
                {
                    var fieldX = i / 2;
                    var fieldY = j / 2;
                    var currentField = Fields[fieldX][fieldY] = CreateField(field[i][j]);

                    // CheckTypeAndAssign(currentField);

                    if (j != 0 && currentField != null)
                    {
                        var previousField = Fields[fieldX][fieldY - 1];
                        if (previousField != null)
                        {
                            previousField.RightConnectedField = currentField;
                            currentField.LeftConnectedField = previousField;
                        }
                    }

                    if (i != 0 && currentField != null)
                    {
                        var linePlace = field[i - 1][j];
                        if (linePlace == '|')
                        {
                            var upField = Fields[fieldX - 1][fieldY];
                            currentField.TopConnectedField = upField;
                            upField.DownConnectedField = currentField;
                        }
                    }
                }
            }
        }


        private Field SetupPlayerField(char item)
        {
            var piece = new PlayerPiece(item.ToString());
            var field = new StartField();

            piece.StartField = field;

            piece.PieceField = field;
            field.Pieces.Add(piece);

            if (int.TryParse(item.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var toInt))
            {
                // subtract one because the config file starts from 1 and not 0
                _players[toInt - 1].PlayerPieces.Add(piece);
                _players[toInt - 1].StartFields.Add(field);
            }

            return field;
        }

        private Field AddBarricadeToField()
        {
            BarricadePiece piece = new BarricadePiece();
            NormalField field = new NormalField();
            field.Pieces.Add(piece);
            piece.PieceField = field;

            BarricadePieces.Add(piece);
            return field;
        }

        private Field CreateField(char toString)
        {
            var field = new NormalField();
            var items = "1234".ToArray();

            switch (char.ToLower(toString))
            {
                case 'r': return new RestField();
                case 'n': return new NormalField();
                case 'f': return new FinishField();
                case 'w': return new WoodField();
                case 'x': return AddBarricadeToField();
                case var e when items.Contains(e): return SetupPlayerField(e);
            }

            if (toString == '.')
            {
                return null;
            }

            return field;
        }


        private void GeneratePlayers()
        {
            for (var i = 0; i < AmountOfPlayers; i++)
            {
                var p = new Player();
                _players.Add(p);
            }
        }
    }
}