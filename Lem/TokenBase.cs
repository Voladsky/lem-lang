using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    internal class TokenBase
    {
        public object value;
        public Position pos;
        public TokenBase(object value, Position pos)
        {
            this.value = value;
            this.pos = pos;
        }
    }
}
