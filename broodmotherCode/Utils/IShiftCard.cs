using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Utils;

public interface IShiftCard
{
    public Type AlternateCardType { get; }
    public CardModel GetAlternateCard();
}