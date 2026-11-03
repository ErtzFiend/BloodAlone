using System;
using System.IO;
using System.Reflection;
using System.Linq;

class Program {
    static void Main() {
        string rwDir = @"C:\Program Files (x86)\Steam\steamapps\common\RimWorld";
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
            string name = args.Name.Split(',')[0] + ".dll";
            string path = Path.Combine(rwDir, name);
            if (File.Exists(path)) return Assembly.LoadFrom(path);
            if (args.Name.StartsWith("UnityEngine")) {
                string stubs = Path.Combine(rwDir, "UnityStubs.dll");
                if (File.Exists(stubs)) return Assembly.LoadFrom(stubs);
                string compat = Path.Combine(rwDir, "Waylith.UnityCompat.dll");
                if (File.Exists(compat)) return Assembly.LoadFrom(compat);
            }
            return null;
        };

        Assembly rw = Assembly.LoadFrom(Path.Combine(rwDir, "Assembly-CSharp.dll"));
        Type pnt = rw.GetType("RimWorld.Pawn_NeedsTracker");
        Type needDefType = rw.GetType("RimWorld.NeedDef");
        Type pawnType = rw.GetType("Verse.Pawn");
        Type geneTrackerType = rw.GetType("RimWorld.Pawn_GeneTracker");

        Console.WriteLine("Pawn fields in Pawn_NeedsTracker:");
        foreach (var f in pnt.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            Console.WriteLine("  " + f.FieldType.Name + " " + f.Name);
        }

        Console.WriteLine("Pawn fields/properties related to genes:");
        foreach (var p in pawnType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            if (p.Name.Contains("Gene") || p.PropertyType == geneTrackerType) {
                Console.WriteLine("  Prop: " + p.PropertyType.Name + " " + p.Name);
            }
        }
        foreach (var f in pawnType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            if (f.Name.Contains("gene") || f.FieldType == geneTrackerType) {
                Console.WriteLine("  Field: " + f.FieldType.Name + " " + f.Name);
            }
        }

        Type needDefOf = rw.GetType("RimWorld.NeedDefOf");
        if (needDefOf != null) {
            Console.WriteLine("NeedDefOf fields:");
            foreach (var f in needDefOf.GetFields(BindingFlags.Public | BindingFlags.Static)) {
                if (f.Name.Contains("Food") || f.Name.Contains("Hemogen")) {
                    Console.WriteLine("  " + f.Name);
                }
            }
        }

        Type geneDefOf = rw.GetType("RimWorld.GeneDefOf");
        if (geneDefOf != null) {
            Console.WriteLine("GeneDefOf fields:");
            foreach (var f in geneDefOf.GetFields(BindingFlags.Public | BindingFlags.Static)) {
                if (f.Name.Contains("Hemogen")) {
                    Console.WriteLine("  " + f.Name);
                }
            }
        }
    }
}
