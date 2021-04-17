using System;

namespace Easy.Endpoints
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EndpointControllerAttribute : Attribute
    {
        public EndpointControllerAttribute(string name)
        {
            Name = name;
        }

        public string Name { get;  }
    }
}
