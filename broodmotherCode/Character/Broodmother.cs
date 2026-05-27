using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using broodmother.broodmotherCode.Cards;
using broodmother.broodmotherCode.Cards.ShiftCards;
using broodmother.broodmotherCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace broodmother.broodmotherCode.Character;
public class Broodmother : PlaceholderCharacterModel
{
    public const string CharacterId = "Broodmother";

    public static readonly Color Color = new("DAA520");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 80;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        
        // ModelDb.Card<StrikeBroodmother>(),
        // ModelDb.Card<StrikeBroodmother>(),
        // ModelDb.Card<StrikeBroodmother>(),
        // ModelDb.Card<StrikeBroodmother>(),
        // ModelDb.Card<DefendBroodmother>(),
        // ModelDb.Card<DefendBroodmother>(),
        // ModelDb.Card<DefendBroodmother>(),
        // ModelDb.Card<DefendBroodmother>(),
        // ModelDb.Card<BugSwarm>(),
        // ModelDb.Card<HurlHive>()
        
        ModelDb.Card<DefendBroodmother>(),
        ModelDb.Card<DefendBroodmother>(),
        ModelDb.Card<Cocoon>(),
        ModelDb.Card<StrikeBroodmother>(),
        ModelDb.Card<StrikeBroodmother>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BurningBlood>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<broodmotherCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<broodmotherRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<broodmotherPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}