using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace broodmother.broodmotherCode.Utils;

public class BroodmotherInsectSlots
{
    public static readonly Vector2[] InsectSlots = new Vector2[5]
    {
        new Vector2(-265, -270),
        new Vector2(-335, -100),
        new Vector2(-175, -140),
        new Vector2(-325, 30),
        new Vector2(-180, 40)
    };

    private static readonly Creature?[] _occupants = new Creature?[5];

    public static int GetNextSlot()
    {
        for (int i = 0; i < _occupants.Length; i++)
        {
            if (_occupants[i] == null)
                return i;
        }
        return -1;
    }

    public static void OccupySlot(int slot, Creature creature)
    {
        _occupants[slot] = creature;
    }

    public static void EmptySlot(int slot)
    {
        _occupants[slot] = null;
    }

    public static void Reset()
    {
        for (int i = 0; i < _occupants.Length; i++)
        {
            _occupants[i] = null;
        }
    }
}