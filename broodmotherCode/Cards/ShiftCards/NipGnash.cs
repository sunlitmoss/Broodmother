using BaseLib.Utils;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards.ShiftCards;


public class Nip() : ShiftCard<Gnash>(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4m, ValueProp.Move),
        new DynamicVar("GnashAmount", 5m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Gnash.IncreaseAmount += DynamicVars["GnashAmount"].IntValue;
        if (ShiftRegistries.CombatPairs.TryGetValue(this, out var gnash))
            gnash.DynamicVars.Damage.BaseValue += DynamicVars["GnashAmount"].IntValue;
        
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["GnashAmount"].UpgradeValueBy(2m);
    }
}

[Pool(typeof(TokenCardPool))]
public class Gnash() : ShiftCard<Nip>(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TokenCardPool>();
    
    public static int IncreaseAmount = 0;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5m + IncreaseAmount, ValueProp.Move)
    ];
    
    protected override IEnumerable<CardKeyword> AdditionalKeywords =>
        new List<CardKeyword> { CardKeyword.Exhaust };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}