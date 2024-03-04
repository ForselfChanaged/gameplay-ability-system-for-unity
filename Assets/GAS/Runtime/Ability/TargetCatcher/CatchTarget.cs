using System.Collections.Generic;
using GAS.Runtime.Component;

namespace GAS.Runtime.Ability.TargetCatcher
{
    public class CatchTarget : TargetCatcherBase
    {
        public override List<AbilitySystemComponent> CatchTargets(AbilitySystemComponent target)
        {
            return new List<AbilitySystemComponent>() { target };
        }
    }
}