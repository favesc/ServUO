// Recover v1.2 (mod of "Raisor's Heal v1.2")
// By Nerun
// Recover Hits, Stamina, Mana, Thirst and Hunger
// Ressurrect if dead
//
using Server.Targeting;
using Server.Commands.Generic;
using Server.Mobiles;

namespace Server.Commands
{
    public class Recover
    {
        public static void Initialize()
        {
            Register();
        }

        public static void Register()
        {
            CommandSystem.Register("Recover", AccessLevel.GameMaster, new CommandEventHandler(Recover_OnCommand));
            CommandSystem.Register("Rec", AccessLevel.GameMaster, new CommandEventHandler(Recover_OnCommand));
        }

        private class RecoverTarget : Target
        {
            public RecoverTarget(Mobile m) : base(-1, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object o)
            {
                Mobile m;
                BaseCreature mx;
                if (!BaseCommand.IsAccessible(from, o))
                    from.SendMessage("That is not accessible.");
                else if (o is BaseCreature) { mx = (BaseCreature)o; mx.PlaySound(0x214); mx.FixedEffect(0x376A, 10, 16); mx.ResurrectPet(); mx.Hits = mx.HitsMax; mx.Stam = mx.StamMax; }
                else if (o is Mobile)
                {
                    m = (Mobile)o;
                    if (!m.Alive)
                    {
                        m.PlaySound(0x214);
                        m.FixedEffect(0x376A, 10, 16);
                        m.Resurrect();
                        from.SendMessage("Resurrect it.");
                    }
                    m.Hits = m.HitsMax;
                    m.Stam = m.StamMax;
                    m.Mana = m.ManaMax;
                    m.Thirst = 20;
                    m.Hunger = 20;
                    from.SendMessage("Recover it.");
                }
                else
                    from.SendMessage("That is not a mobile.");
            }
        }

        [Usage("Recover")]
        [Aliases("Rec")]
        [Description("Ressurrects and recovers Thirst, Hunger, Hits, Stam and Mana of the targeted at the maximum level.")]
        private static void Recover_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new RecoverTarget(e.Mobile);
        }
    }
}
