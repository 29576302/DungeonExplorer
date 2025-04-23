using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    /// <summary>
    /// This interface is used to define the CreateDescription method for all items, as all items must have a description.
    /// </summary>
    interface IDescribable
    {
        void CreateDescription();
    }
}
