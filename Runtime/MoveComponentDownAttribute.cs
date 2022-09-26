using System;

namespace Kogane
{
    [AttributeUsage( AttributeTargets.Class )]
    public sealed class MoveComponentDownAttribute : Attribute
    {
        public Type Type { get; }

        public MoveComponentDownAttribute( Type type )
        {
            Type = type;
        }
    }
}