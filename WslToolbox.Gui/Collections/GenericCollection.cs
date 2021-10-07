namespace WslToolbox.Gui.Collections
{
    public abstract class GenericCollection
    {
        protected readonly object Source;

        protected GenericCollection(object source)
        {
            Source = source;
        }
    }
}