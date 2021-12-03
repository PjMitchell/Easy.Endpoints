using System;
using System.Reflection;

namespace Easy.Endpoints
{
    internal static class GenericTypeHelper
    {
        public static bool MatchExpectedGeneric(Type target, Type expectedGenericType)
        {
            try
            {
                return target.GenericTypeArguments.Length == expectedGenericType.GetTypeInfo().GenericTypeParameters.Length
                    && target == expectedGenericType.MakeGenericType(target.GenericTypeArguments);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
