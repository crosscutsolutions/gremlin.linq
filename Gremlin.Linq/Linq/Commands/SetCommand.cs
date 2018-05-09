using System;
using System.Collections.Generic;
using System.Text;

namespace Gremlin.Linq.Linq.Commands
{
    using System.Reflection;

    public class SetCommand : Command {
        private readonly string _property;
        private readonly object _value;

        public IGremlinQueryable ParentSelector { get; set; }

        internal SetCommand(IGraphClient client, string property,object value) : base(client)
        {
            _property = property;
            _value = value;
        }

        public override string BuildGremlinQuery()
        {
            return ParentSelector.BuildGremlinQuery() + _property.BuildGremlinQueryForValue(_value);
        }
    }
    public class SetCommand<T> : SetCommand
    {

        public SetCommand(IGraphClient client, PropertyInfo property, object value) : base(client, property.Name,value)
        {
        }
        
    }
}
