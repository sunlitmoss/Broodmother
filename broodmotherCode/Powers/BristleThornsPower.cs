using broodmother.broodmotherCode.Cards;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Powers;

public class BristleThornsPower : TemporaryThornsPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Bristle>();
}