namespace WslToolbox.Gui.Collections
{
    public abstract class Collections
    {
        protected readonly object _source;

        protected Collections(object source)
        {
            _source = source;
        }
    }
}