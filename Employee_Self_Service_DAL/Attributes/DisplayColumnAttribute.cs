using System;

namespace Employee_Self_Service.DAL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DisplayColumnAttribute : Attribute
    {
        public string Name { get; }
        public bool IsVisible { get; set; } = true;

        public DisplayColumnAttribute(string name)
        {
            Name = name;
            
        }
    }
}