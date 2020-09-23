using Assets.Scripts.Creatures;

namespace Assets.Scripts.GameInterface.CreatureInfo
{
    public interface IKeyedValueDisplay
    {
        string Key { get; }

        float Value { set; }
    }
}