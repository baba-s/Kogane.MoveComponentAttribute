using System;

namespace Kogane
{
    [AttributeUsage( AttributeTargets.Class )]
    public sealed class MoveComponentUpAttribute : Attribute
    {
        public Type Type { get; }

        public MoveComponentUpAttribute( Type type )
        {
            Type = type;
        }
    }
}