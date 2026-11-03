using System;
using System.Reflection;
using HarmonyLib;
class Harness {
 static int Main() { try {
 System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += (ctx, name) => { var p = System.IO.Path.Combine(@"C:\Program Files (x86)\Steam\steamapps\common\RimWorld",name.Name+".dll"); return System.IO.File.Exists(p) ? ctx.LoadFromAssemblyPath(p) : null; };
 var a=Assembly.LoadFrom(@"C:\temp\rw_inspect\BloodAlone.dll");
 var h=new Harmony("bloodalone.test"); h.PatchAll(a);
 foreach(var m in h.GetPatchedMethods()) Console.WriteLine("PATCHED "+m.DeclaringType.FullName+"."+m.Name);
 Console.WriteLine("PATCHALL_SUCCESS"); return 0;
 } catch(Exception e) {Console.WriteLine(e);return 1;} }
}
