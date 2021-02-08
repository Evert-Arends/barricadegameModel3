using System.IO;
using System.Threading.Tasks;
using Barricade.Controllers;

namespace Barricade
{
    class Program
    {
        private static GameController _gameController;
        static async Task Main(string[] args)
        {
            // Load text file into memory
            var lines = await File.ReadAllLinesAsync("../../../Input/bla.txt");
            _gameController = new GameController(lines);

            _gameController.RunGame();
        }
    }
}   