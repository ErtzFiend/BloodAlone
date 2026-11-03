using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BloodAlone
{
    [StaticConstructorOnStartup]
    public static class Bootstrap
    {
        static Bootstrap()
        {
            new Harmony("bloodalone.food-removal").PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    // ShouldHaveNeed is consulted when NeedsTracker constructs its needs. The
    // tracker is not reconstructed when genes change, so this hook refreshes its
    // food slot after every supported gene change and after game-load exposure.
    internal static class HemogenicFoodNeedRefresh
    {
        internal static void Refresh(Pawn pawn)
        {
            if (pawn?.needs == null) return;
            pawn.needs.AddOrRemoveNeedsAsAppropriate();

            // AddOrRemoveNeedsAsAppropriate consults ShouldHaveNeed. It is
            // intentionally the sole mutation entry point; do not manually edit
            // the tracker's private food field.
        }
    }

    [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
    internal static class HemogenicFoodNeedPatch
    {
        private static void Postfix(Pawn_NeedsTracker __instance, NeedDef nd, ref bool __result)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
            if (nd != NeedDefOf.Food || pawn == null) return;
            if (pawn.genes?.HasActiveGene(GeneDefOf.Hemogenic) == true)
                __result = false;
        }
    }

    [HarmonyPatch(typeof(Pawn_GeneTracker), "ExposeData")]
    internal static class HemogenicGeneLoadPatch
    {
        private static void Postfix(Pawn_GeneTracker __instance)
        {
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
                HemogenicFoodNeedRefresh.Refresh(__instance?.pawn);
        }
    }
}
