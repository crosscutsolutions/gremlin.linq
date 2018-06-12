using System;
using System.Collections.Generic;
using System.Text;

namespace Gremlin.Linq.Linq.Commands
{
    public class DropCommand : Command
    {
        public DropCommand(IGraphClient client) : base(client)
        {
            
        }

        public Selector ParentSelector { get; set; }

        public override string BuildGremlinQuery()
        {
            return ParentSelector.BuildGremlinQuery() + ".drop()";
        }
    }
}
