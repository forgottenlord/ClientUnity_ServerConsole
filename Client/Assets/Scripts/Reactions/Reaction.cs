using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Reactions
{
    public abstract class Reaction
    {
        protected NetClient client;
        public abstract void Process(string[] message);
    }
}
