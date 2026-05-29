using broodmother.broodmotherCode.Cards.ShiftCards;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Utils;

public class ShiftRegistries
{
    public static readonly Dictionary<Type, Type> ShiftPairs = new()
    {
        { typeof(MetamorphicStrike), typeof(MetamorphicDefend) },
        { typeof(MetamorphicDefend), typeof(MetamorphicStrike) }
    };

    public static Dictionary<int, (Type, bool)> CombatPairs = new();

    public static void RegisterCombatPairs(CardModel card1, CardModel card2)
    {
        CombatPairs.Add(card1.GetHashCode(), (card2.GetType(), card2.IsUpgraded));
        CombatPairs.Add(card2.GetHashCode(), (card1.GetType(), card1.IsUpgraded));
    }
}