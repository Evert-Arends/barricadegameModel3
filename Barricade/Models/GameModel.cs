using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Barricade.Models
{
    public class GameModel
    {
        private const int AmountOfPlayers = 4;
        private const int FieldRowsInVillage = 5;
        private const int BottomRowIndex = 8;
        public readonly List<BarricadePiece> BarricadePieces = new List<BarricadePiece>(6);
        private readonly List<Player> _players = new List<Player>(AmountOfPlayers);
        public Field[][] Fields { get; private set; }

        // Not just an active player because barriers don't have those.
        private Piece _activePiece;
        private Player _activePlayer;
        public Player WinningPlayer;

        private Field GameWoodField { get; set; }

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

            var barrier = BarricadePieces.FirstOrDefault();
            MoveBarrier(barrier);
            BarricadePieces.Remove(barrier);
        }

        private void MoveBarrier(BarricadePiece barricadePiece)
        {
            SetPieceActive(barricadePiece);
        }

        public string GetActivePlayerName()
        {
            return _activePlayer?.GetName();
        }

        private bool MoveAllowed(Field fieldTo)
        {
            return _activePiece.MoveAllowed(fieldTo, Die);
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
            var legalMove = MoveAllowed(connectedField);
            if (!legalMove) return false;

            MovePieceToNewField(_activePiece, connectedField);
            _activePiece.VisitedFields ??= new List<Field>();
            _activePiece.VisitedFields.Add(connectedField);
            Die.RemoveOneMove();

            return true;
        }

        private void SetPieceActive(Piece piece)
        {
            _activePiece = piece;
            // For reset purposes.
            _activePiece.StartOutField = _activePiece.PieceField;
        }

        private void ResetPiece(Piece piece)
        {
            piece.VisitedFields = new List<Field>();
            // Remove reference
            piece.PieceField.Pieces.Remove(piece);
            // New reference
            piece.PieceField = piece.StartOutField;
            if (piece.StartOutField.Pieces != null) piece.StartOutField.Pieces.Add(piece);

            if (piece is PlayerPiece)
            {
                Die.RevertMoveAmount();
            }
        }

        public void MoveDone()
        {
            var currentField = _activePiece.PieceField;

            if (currentField is FinishField)
            {
                WinningPlayer = _activePlayer;
                return;
            }

            // This does not matter for barricades, because the die is 0 then anyway.
            if (Die.MovesRemaining != 0)
            {
                // If player can't move (stuck) skip this player.
                if (Die.MovesRemaining.Equals(Die.ArchivedMovesRemaining))
                {
                    NextPlayer();
                    return;
                }

                // Pressing enter when you're not done resets the piece to start position.
                ResetPiece(_activePiece);
                return;
            }

            if (!currentField.IsMoveAllowed())
            {
                ResetPiece(_activePiece);
                return;
            }

            if (currentField.Crowded())
            {
                // Check if it's a barricade and revert all, or slay a piece.
                if (currentField.Pieces.FirstOrDefault() is BarricadePiece barricadePiece)
                {
                    MoveBarrier(barricadePiece);
                }
                else if (currentField.Pieces.FirstOrDefault() is PlayerPiece firstFieldPiece)
                {
                    // Check if the piece on the field is from the player or not.
                    if (_activePlayer.OwnsPiece(firstFieldPiece) ||
                        currentField is RestField)
                    {
                        // Reset because you can't have two of the same pieces one one field except woods.
                        // Can't slay on a rest piece.
                        ResetPiece(_activePiece);
                        return;
                    }

                    // Move the other piece back to home.
                    firstFieldPiece.StartOutField = null;
                    if (currentField.IsVillage)
                    {
                        MovePieceToNewField(firstFieldPiece, GameWoodField);
                    }
                    else
                    {
                        MovePieceToNewField(firstFieldPiece, firstFieldPiece.StartField);
                    }

                    NextPlayer();
                }
            }
            else
            {
                NextPlayer();
            }
        }

        private static void MovePieceToNewField(Piece piece, Field fieldTo)
        {
            piece.PieceField.Pieces.Remove(piece);
            piece.PieceField = fieldTo;
            piece.PieceField.Pieces.Add(piece);
        }


        private void NextPlayer()
        {
            if (BarricadePieces.Count > 0)
            {
                MoveBarriers();
            }
            else
            {
                _activePiece.VisitedFields = new List<Field>();
                SetPieceActive(_players[_playerTurnNumber].ActivePlayerPiece =
                    _players[_playerTurnNumber].PlayerPieces[0]);
                _activePlayer = _players[_playerTurnNumber];
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
            // Not allowed to walk with different pieces when you already walked with one.
            if (Die.MovesRemaining.Equals(Die.ArchivedMovesRemaining))
            {
                SetPieceActive(_activePiece.NextPiece(Die));
            }
        }

        private void AssignVillage()
        {
            for (int i = 1; i < FieldRowsInVillage; i++)
            {
                foreach (Field field in Fields[i])
                {
                    if (field != null) field.IsVillage = true;
                }
            }
        }

        private void AssignBottomRow()
        {
            foreach (Field field in Fields[BottomRowIndex])
            {
                field.IsBottomRow = true;
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

            SeparateStartFields();
            AssignVillage();
            AssignBottomRow();
        }

        private void SeparateStartFields()
        {
            foreach (var player in _players)
            {
                Field normalField = null;
                foreach (var playerStartField in player.StartFields)
                {
                    if (playerStartField.TopConnectedField is NormalField normal)
                    {
                        normalField = normal;
                        break;
                    }
                }

                foreach (var playerStartField in player.StartFields)
                {
                    playerStartField.TopConnectedField = normalField;
                    playerStartField.LeftConnectedField = null;
                    playerStartField.RightConnectedField = null;
                    playerStartField.DownConnectedField = null;
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
                piece.PieceOwner = _players[toInt - 1];
            }

            return field;
        }

        private Field AddBarricadeToField()
        {
            BarricadePiece piece = new BarricadePiece();
            NormalField field = new NormalField();
            field.Pieces.Add(piece);
            piece.PieceField = field;
            // If null create it
            piece.VisitedFields ??= new List<Field>();
            piece.VisitedFields.Add(field);
            BarricadePieces.Add(piece);
            return field;
        }

        private WoodField GetWoodField()
        {
            WoodField woodField = new WoodField();
            GameWoodField = woodField;
            return woodField;
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
                case 'w': return GetWoodField();
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