using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    /// <summary>
    /// Used to store explored rooms and their contents.
    /// </summary>
    [Serializable]
    class Map
    {
        private readonly List<Room> rooms;
        public int RoomCount => rooms.Count;
        public Map()
        {
            rooms = new List<Room>();
        }

        public void AddRoom(Room room)
        {
            rooms.Add(room);
        }
        /// <summary>
        /// Returns the room before the one taken as an input. Raises an error if the room is not in the list.
        /// </summary>
        /// <param name="room">The room taken as an input.</param>
        public Room LastRoom(Room room)
        {
            //If the room is not in the list, throw an error.
            if (!rooms.Contains(room))
            {
                throw new ArgumentException("Room not found in the map.");
            }
            //If the room is the first one, return null.
            if (room == rooms[0])
            {
                return null;
            }
            else
            {
                return rooms[rooms.IndexOf(room) - 1];
            }
        }
        /// <summary>
        /// Returns the room after the one taken as an input. Raises an error if the room is not in the list.
        /// </summary>
        /// <param name="room">The room taken as an input.</param>
        public Room NextRoom(Room room)
        {
            if (!rooms.Contains(room))
            {
                throw new ArgumentException("Room not found in the map.");
            }
            if (room == rooms[rooms.Count - 1])
            {
                return null;
            }
            else
            {
                return rooms[rooms.IndexOf(room) + 1];
            }
        }
        /// <summary>
        /// Returns the newest room in the list of rooms.
        /// </summary>
        public Room NewestRoom()
        {
            if (rooms.Count > 0)
            {
                return rooms[rooms.Count - 1];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Creates a string representation of the map.
        /// The room currently occupied is represented using [|], other rooms are represented using [].
        /// <param name="currentRoom">The room currently occupied.</param>"
        public string GetMap(Room currentRoom)
        {
            if (!rooms.Contains(currentRoom))
            {
                throw new ArgumentException("Room not found in the map.");
            }
            string map = "";
            foreach (Room room in rooms)
            {
                if (room == currentRoom)
                {
                    map += "[|]";
                }
                else
                {
                    map += "[]";
                }
            }
            return map;
        }
    }
}
