using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace DungeonExplorer
{
    /// <summary>
    /// Used to encapsulate methods for saving/loading the game.
    /// </summary>
    class Save
    {
        /// <summary>
        /// Method used to save the game to a .dat file.
        /// BinaryFormatter is used to serialise the game state into a storeable format.
        /// </summary>
        /// <param name="gameState">Game state to be saved.</param>
        /// <param name="path">Where the game will be saved.</param>
        public static void SaveGame(Game gameState, string path)
        {
            // FileStream is used to create a file at the specified path.
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, gameState);
            }
        }
        /// <summary>
        /// Method used to load/deserialise the game from a .dat file.
        /// </summary>
        /// <param name="path">Path to the .dat file.</param>
        public static Game LoadGame(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Game)formatter.Deserialize(fileStream);
            }
        }
    }
}
