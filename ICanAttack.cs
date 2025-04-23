using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    /// <summary>
    /// This interface is used to define the AttackTarget method for all creatures.
    /// </summary>
    interface ICanAttack
    {
        void AttackTarget(Creature target);
    }
}
