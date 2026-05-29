using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace broodmother.broodmotherCode.Utils;

public class BroodmotherInsectSlots
{
    public static void SetSlots()
    {
        InsectSlots.Concat(ExtraSlots);
    }

    public static readonly Vector2[] InsectSlots = new Vector2[5]
    {
        new(-265, -270),
        new(-335, -100),
        new(-175, -140),
        new(-325, 30),
        new(-180, 40)
    };

    public static readonly Vector2[] ExtraSlots = new Vector2[3]
    {
        new(-80, -270),
        new(-90, -100),
        new(-70, -140)
    };

    private static readonly Creature?[] _occupants = new Creature?[5];

    public static int GetNextSlot()
    {
        for (var i = 0; i < _occupants.Length; i++)
            if (_occupants[i] == null)
                return i;

        return -1;
    }

    public static int CountOccupiedSlots()
    {
        var sum = 0;
        for (var i = 0; i < _occupants.Length; i++)
            if (_occupants[i] != null)
                sum++;

        return sum;
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
        for (var i = 0; i < _occupants.Length; i++) _occupants[i] = null;
    }
}