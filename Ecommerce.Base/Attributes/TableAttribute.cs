namespace Ecommerce.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}
