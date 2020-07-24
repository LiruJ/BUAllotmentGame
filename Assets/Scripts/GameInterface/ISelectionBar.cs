namespace Assets.Scripts.GameInterface
{
    /// <summary> Represents an item selection bar that can have items selected from it. </summary>
    /// <typeparam name="T"> The type of button that this bar supports. </typeparam>
    public interface ISelectionBar<T>
    {
        void OnButtonSelected(T button);
    }
}