using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpartaRemixStudio2022
{

    public static class UniLoad
    {
        public static void LoadObject(this IComplexObject ico, Stream s)
        {
            int lenght = StreamHelper.LoadUnmanaged<int>(s);

            if (lenght == 0) ico = null;

            for (int i = 0; i < lenght; i++)
            {
                uint name = StreamHelper.LoadUnmanaged<uint>(s);
                int lenght2 = StreamHelper.LoadUnmanaged<int>(s);
                if (!ico.AcceptVariable(name, s, lenght2))
                {
                    int left = lenght2;

                    byte[] b = new byte[4096];

                    while (left > 0)
                    {
                        s.Read(b, 0, Math.Min(left, 4096));
                        left -= Math.Min(left, 4096);
                    }
                }
            }
        }
        public static T CreateObject<T>(Stream s) where T : IComplexObject, new()
        {
            T ico = new T();
            ico.SetDefaultState();
            ico.LoadObject(s);
            return ico;
        }

        public static void LoadObject(this IComplexObject ico, string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            UniLoad.LoadObject(ico, fs);
            fs.Close();
            fs.Dispose();
        }

        public static int GetLenght(this IComplexObject ico)
        {
            if (ico == null) return 0;

            List<uint> vars = ico.GetVarNamesToSave();
            int totalLenght = 0;

            foreach (uint u in vars)
            {
                totalLenght += ico.ReportLenghtOfVariable(u) + 4 + 4;
            }

            return totalLenght + 4;
        }
        public static void Save(Stream s, IComplexObject ico)
        {
            if (ico != null)
            {
                List<uint> vars = ico.GetVarNamesToSave();

                StreamHelper.SaveUnmanaged(s, vars.Count);

                foreach (uint u in vars)
                {
                    StreamHelper.SaveUnmanaged(s, u);
                    StreamHelper.SaveUnmanaged(s, ico.ReportLenghtOfVariable(u));
                    ico.SaveVariable(u, s);
                }
            }
        }
        public static void Save(string file, IComplexObject ico)
        {
            Stream s = new FileStream(file, FileMode.Create);

            List<uint> vars = ico.GetVarNamesToSave();

            StreamHelper.SaveUnmanaged(s, vars.Count);

            foreach (uint u in vars)
            {
                StreamHelper.SaveUnmanaged(s, u);
                StreamHelper.SaveUnmanaged(s, ico.ReportLenghtOfVariable(u));
                ico.SaveVariable(u, s);
            }

            s.Close();
            s.Dispose();
        }
    }
    public interface IComplexObject
    {
        bool AcceptVariable(uint id, Stream s, int lenght);
        void SetDefaultState();
        void SaveVariable(uint id, Stream s);

        int ReportLenghtOfVariable(uint id);
        List<uint> GetVarNamesToSave();
    }
    public static class StreamHelper
    {
        unsafe public static byte[] ToByteArray<T>(this T argument) where T : unmanaged
        {
            var size = sizeof(T);
            var result = new byte[size];
            byte* p = (byte*)&argument;
            for (var i = 0; i < size; i++)
                result[i] = *p++;
            return result;
        }

        public static void SaveUnmanaged<T>(Stream s, T element) where T : unmanaged
        {
            byte[] b = element.ToByteArray();
            s.Write(b, 0, b.Length);
        }
        public static void SaveString(Stream s, string input)
        {
            if (input == null)
            {
                s.WriteByte(0);
            }
            else
            {
                s.WriteByte(1);
                byte[] b = Encoding.UTF32.GetBytes(input);
                SaveUnmanaged(s, b.Length);
                s.Write(b, 0, b.Length);
            }
        }
        public static void SaveArray<T>(Stream s, T[] arr, Action<Stream, T> ElementSave)
        {
            if (arr == null)
            {
                s.WriteByte(0);
            }
            else
            {
                s.WriteByte(1);
                StreamHelper.SaveUnmanaged(s, arr.Length);

                for (int i = 0; i < arr.Length; i++)
                {
                    ElementSave(s, arr[i]);
                }
            }
        }
        public static void SaveArray2<T>(Stream s, T[,] arr, Action<Stream, T> ElementSave)
        {
            if (arr == null)
            {
                s.WriteByte(0);
            }
            else
            {
                s.WriteByte(1);
                StreamHelper.SaveUnmanaged(s, arr.GetLength(0));
                StreamHelper.SaveUnmanaged(s, arr.GetLength(1));

                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        ElementSave(s, arr[i, j]);
                    }
                }
            }
        }
        public static void SaveList<T>(Stream s, List<T> arr, Action<Stream, T> ElementSave)
        {
            if (arr == null)
            {
                s.WriteByte(0);
            }
            else
            {
                s.WriteByte(1);
                StreamHelper.SaveUnmanaged(s, arr.Count);

                for (int i = 0; i < arr.Count; i++)
                {
                    ElementSave(s, arr[i]);
                }
            }
        }
        public static void SaveDictionary<T, U>(Stream s, Dictionary<T, U> dict, Action<Stream, T> KeySave, Action<Stream, U> ValueSave)
        {
            if (dict == null)
            {
                s.WriteByte(0);
            }
            else
            {
                s.WriteByte(1);
                StreamHelper.SaveUnmanaged(s, dict.Count);

                foreach (KeyValuePair<T, U> kp in dict)
                {
                    KeySave(s, kp.Key);
                    ValueSave(s, kp.Value);
                }
            }
        }

        public unsafe static T LoadUnmanaged<T>(Stream s) where T : unmanaged
        {
            int size = sizeof(T);
            byte[] b = new byte[size];
            s.Read(b, 0, size);
            T t = default;
            byte* p = (byte*)&t;
            for (var i = 0; i < size; i++)
            {
                *p = b[i];
                p++;
            }
            return t;
        }
        public static string LoadString(Stream fs)
        {
            int x = fs.ReadByte();
            if (x <= 0) return null;

            int l = LoadUnmanaged<int>(fs);
            byte[] b = new byte[l];
            fs.Read(b, 0, l);

            return Encoding.UTF32.GetString(b);
        }
        public static T[] LoadArray<T>(Stream s, Func<Stream, T> ElementLoad)
        {
            int b = s.ReadByte();
            if (b <= 0) return null;

            int l = StreamHelper.LoadUnmanaged<int>(s);

            T[] arr = new T[l];

            for (int i = 0; i < l; i++)
            {
                arr[i] = ElementLoad(s);
            }

            return arr;
        }
        public static T[,] LoadArray2<T>(Stream s, Func<Stream, T> ElementLoad)
        {
            int b = s.ReadByte();
            if (b <= 0) return null;

            int l1 = StreamHelper.LoadUnmanaged<int>(s);
            int l2 = StreamHelper.LoadUnmanaged<int>(s);

            T[,] arr = new T[l1, l2];

            for (int i = 0; i < l1; i++)
            {
                for (int j = 0; j < l2; j++)
                {
                    arr[i, j] = ElementLoad(s);
                }
            }

            return arr;
        }
        public static List<T> LoadList<T>(Stream s, Func<Stream, T> ElementLoad)
        {
            int b = s.ReadByte();
            if (b <= 0) return null;

            int l = StreamHelper.LoadUnmanaged<int>(s);

            List<T> arr = new List<T>();

            for (int i = 0; i < l; i++)
            {
                arr.Add(ElementLoad(s));
            }

            return arr;
        }
        public static Dictionary<T, U> LoadDictionary<T, U>(Stream s, Func<Stream, T> KeyLoad, Func<Stream, U> ValueLoad)
        {
            int b = s.ReadByte();
            if (b <= 0) return null;

            int l = StreamHelper.LoadUnmanaged<int>(s);
            Dictionary<T, U> dict = new Dictionary<T, U>();

            for (int i = 0; i < l; i++)
            {
                dict.Add(KeyLoad(s), ValueLoad(s));
            }

            return dict;
        }

        public static Tuple<T1> LoadTuple<T1>(Stream s, Func<Stream, T1> ElementLoad1)
        {
            int x = s.ReadByte();
            if (x <= 0) return null;

            return new Tuple<T1>(ElementLoad1(s));
        }
        public static void SaveTuple<T1>(Stream s, Tuple<T1> t, Action<Stream, T1> ElementSave1)
        {
            if (t == null)
            {
                s.WriteByte(0);
            }
            else
            {
                s.WriteByte(1);
                ElementSave1(s, t.Item1);
            }
        }
        public static Tuple<T1, T2> LoadTuple<T1, T2>(Stream s, Func<Stream, T1> ElementLoad1, Func<Stream, T2> ElementLoad2)
        {
            int x = s.ReadByte();
            if (x <= 0) return null;

            return new Tuple<T1, T2>(ElementLoad1(s), ElementLoad2(s));
        }
        public static void SaveTuple<T1, T2>(Stream s, Tuple<T1, T2> t, Action<Stream, T1> ElementSave1, Action<Stream, T2> ElementSave2)
        {
            if (t == null)
            {
                s.WriteByte(0);
            }
            else
            {
                ElementSave1(s, t.Item1);
                ElementSave2(s, t.Item2);
            }
        }
        public static Tuple<T1, T2, T3> LoadTuple<T1, T2, T3>(Stream s, Func<Stream, T1> ElementLoad1, Func<Stream, T2> ElementLoad2, Func<Stream, T3> ElementLoad3)
        {
            int x = s.ReadByte();
            if (x <= 0) return null;

            return new Tuple<T1, T2, T3>(ElementLoad1(s), ElementLoad2(s), ElementLoad3(s));
        }
        public static void SaveTuple<T1, T2, T3>(Stream s, Tuple<T1, T2, T3> t, Action<Stream, T1> ElementSave1, Action<Stream, T2> ElementSave2, Action<Stream, T3> ElementSave3)
        {
            if (t == null)
            {
                s.WriteByte(0);
            }
            else
            {
                ElementSave1(s, t.Item1);
                ElementSave2(s, t.Item2);
                ElementSave3(s, t.Item3);
            }
        }
        public static Tuple<T1, T2, T3, T4> LoadTuple<T1, T2, T3, T4>(Stream s, Func<Stream, T1> ElementLoad1, Func<Stream, T2> ElementLoad2, Func<Stream, T3> ElementLoad3, Func<Stream, T4> ElementLoad4)
        {
            int x = s.ReadByte();
            if (x <= 0) return null;

            return new Tuple<T1, T2, T3, T4>(ElementLoad1(s), ElementLoad2(s), ElementLoad3(s), ElementLoad4(s));
        }
        public static void SaveTuple<T1, T2, T3, T4>(Stream s, Tuple<T1, T2, T3, T4> t, Action<Stream, T1> ElementSave1, Action<Stream, T2> ElementSave2, Action<Stream, T3> ElementSave3, Action<Stream, T4> ElementSave4)
        {
            if (t == null)
            {
                s.WriteByte(0);
            }
            else
            {
                ElementSave1(s, t.Item1);
                ElementSave2(s, t.Item2);
                ElementSave3(s, t.Item3);
                ElementSave4(s, t.Item4);
            }
        }
        public static Tuple<T1, T2, T3, T4, T5> LoadTuple<T1, T2, T3, T4, T5>(Stream s, Func<Stream, T1> ElementLoad1, Func<Stream, T2> ElementLoad2, Func<Stream, T3> ElementLoad3, Func<Stream, T4> ElementLoad4, Func<Stream, T5> ElementLoad5)
        {
            int x = s.ReadByte();
            if (x <= 0) return null;

            return new Tuple<T1, T2, T3, T4, T5>(ElementLoad1(s), ElementLoad2(s), ElementLoad3(s), ElementLoad4(s), ElementLoad5(s));
        }
        public static void SaveTuple<T1, T2, T3, T4, T5>(Stream s, Tuple<T1, T2, T3, T4, T5> t, Action<Stream, T1> ElementSave1, Action<Stream, T2> ElementSave2, Action<Stream, T3> ElementSave3, Action<Stream, T4> ElementSave4, Action<Stream, T5> ElementSave5)
        {
            if (t == null)
            {
                s.WriteByte(0);
            }
            else
            {
                ElementSave1(s, t.Item1);
                ElementSave2(s, t.Item2);
                ElementSave3(s, t.Item3);
                ElementSave4(s, t.Item4);
                ElementSave5(s, t.Item5);
            }
        }
        public static int GetLenght<T>(Tuple<T> a, Func<T, int> l)
        {
            if (a == null) return 1;
            else return 1 + l(a.Item1);
        }
        public static int GetLenght<T, T2>(Tuple<T, T2> a, Func<T, int> l, Func<T2, int> l2)
        {
            if (a == null) return 1;
            else return 1 + l(a.Item1) + l2(a.Item2);
        }
        public static int GetLenght<T, T2, T3>(Tuple<T, T2, T3> a, Func<T, int> l, Func<T2, int> l2, Func<T3, int> l3)
        {
            if (a == null) return 1;
            else return 1 + l(a.Item1) + l2(a.Item2) + l3(a.Item3);
        }
        public static int GetLenght<T, T2, T3, T4>(Tuple<T, T2, T3, T4> a, Func<T, int> l, Func<T2, int> l2, Func<T3, int> l3, Func<T4, int> l4)
        {
            if (a == null) return 1;
            else return 1 + l(a.Item1) + l2(a.Item2) + l3(a.Item3) + l4(a.Item4);
        }
        public static int GetLenght<T, T2, T3, T4, T5>(Tuple<T, T2, T3, T4, T5> a, Func<T, int> l, Func<T2, int> l2, Func<T3, int> l3, Func<T4, int> l4, Func<T5, int> l5)
        {
            if (a == null) return 1;
            else return 1 + l(a.Item1) + l2(a.Item2) + l3(a.Item3) + l4(a.Item4) + l5(a.Item5);
        }

        public unsafe static int GetUnmanagedLenght<T>(T o) where T : unmanaged
        {
            int size = sizeof(T);
            return size;
        }
        public static int GetLenght(string o) => o == null ? 1 : 1 + 4 + 4 * o.Length;
        public static int GetLenght<T>(T[] a, Func<T, int> l)
        {
            if (a == null) return 1;
            int c = 1 + 4;
            for (int i = 0; i < a.Length; i++)
            {
                c += l(a[i]);
            }
            return c;
        }
        public static int GetLenght<T>(T[,] a, Func<T, int> l)
        {
            if (a == null) return 1;
            int c = 1 + 8;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    c += l(a[i, j]);
                }
            }
            return c;
        }
        public static int GetLenght<T>(List<T> a, Func<T, int> l)
        {
            if (a == null) return 1;
            int c = 1 + 4;
            foreach (T b in a)
            {
                c += l(b);
            }
            return c;
        }
        public static int GetLenght<T, U>(Dictionary<T, U> a, Func<T, int> lt, Func<U, int> lu)
        {
            if (a == null) return 1;
            int c = 1 + 4;
            foreach (KeyValuePair<T, U> b in a)
            {
                c += lt(b.Key);
                c += lu(b.Value);
            }
            return c;
        }
    }

    public static class CodeGeneration
    {
        public static string[] unmanagedTypes = new string[]
        {
            "sbyte", "byte", "short", "ushort", "int", "uint", "long", "ulong",
            "char", "float", "double", "decimal", "bool", "string"
        };
        public static void FileToCode(string inputFile, string outputFile)
        {
            StreamReader sr = new StreamReader(inputFile);
            StreamWriter sw = new StreamWriter(outputFile);

            List<string> derives = new List<string>();
            string name = "";
            List<CG_VariableDescription> variables = new List<CG_VariableDescription>();

            while (true)
            {
                string line = sr.ReadLine();

                if (line == null) break;
                if (line.StartsWith("ICO"))
                {
                    if (name != "")
                    {
                        // DO IT
                        sw.Write(GenerateCode(name, derives, variables));

                        // CLEAR
                        derives.Clear();
                        variables.Clear();
                    }
                    name = line.Substring(4);
                }
                else if (line.StartsWith("UNM"))
                {
                    string[] parts = line.Split(new char[] { ' ' }, 6);
                    CG_VariableDescription vard = new CG_VariableDescription(parts[4], parts[1], parts[3], parts[5], parts[2], new CG_UnmanagedTypeOrString(parts[3]));
                    variables.Add(vard);
                }
                else if (line.StartsWith("DER"))
                {
                    derives.Add(line.Substring(4));
                }
                else if (line.StartsWith("VAR"))
                {
                    string[] parts = line.Split(new char[] { ' ' }, 6);
                    CG_VariableDescription vard = new CG_VariableDescription(parts[4], parts[1], parts[3], parts[5], parts[2]);
                    variables.Add(vard);
                }
                else if (line.StartsWith("PARV"))
                {
                    string[] parts = line.Split(new char[] { ' ' }, 6);
                    CG_VariableDescription vard = new CG_VariableDescription(parts[4], parts[1], parts[3], parts[5], parts[2]);
                    vard.IsParameter = true;
                    variables.Add(vard);
                }
                else if (line.StartsWith("PARM"))
                {
                    string[] parts = line.Split(new char[] { ' ' }, 4);

                    foreach (CG_VariableDescription v in variables)
                    {
                        if (v.Id == parts[1])
                        {
                            v.Par_DependsOn = parts[2];
                            v.Par_TypeAsign = parts[3];
                        }
                    }
                }
            }

            if (name != "")
            {
                sw.WriteLine(GenerateCode(name, derives, variables));
            }

            sw.Flush();
            sw.Close();
            sr.Close();
        }
        public static string GenerateCode(string className, List<string> derives, List<CG_VariableDescription> variables)
        {
            string code = "";
            code += $"partial class {className} : IComplexObject";
            foreach (string v in derives)
            {
                code += $", {v}";
            }
            code += "\n{\n";

            // VAR DEFS
            foreach (var v in variables)
            {
                if (v.Access == "private")
                {
                    code += $"    private {v.Type} {v.Name} {{ get; set; }}\n";
                }
                else if (v.Access == "publicget")
                {
                    code += $"    public {v.Type} {v.Name} {{ get; private set; }}\n";
                }
                else
                {
                    code += $"    public {v.Type} {v.Name} {{ get; set; }}\n";
                }
            }
            code += "    \n";

            // ACC
            code += "    public bool AcceptVariable(uint id, Stream s, int lenght)\n";
            code += "    {\n";
            code += "        switch (id)\n";
            code += "        {\n";
            foreach (var v in variables)
            {
                if (!v.IsParameter)
                {
                    code += $"            case {v.Id}: {v.Name} = {v.TypeTree.AcceptVariableClause("s")} return true;\n";
                }
                else
                {
                    string refName = "?";
                    foreach (var v2 in variables)
                    {
                        if (v2.Id == v.Par_DependsOn) refName = v2.Name;
                    }
                    code += $"            case {v.Id}: {v.Name} = {v.Par_TypeAsign.Replace("$", refName)}; {v.Name}.SetDefaultState(); {v.Name}.LoadObject(s); return true;\n";
                }
            }
            code += "            default: return false;\n";
            code += "        }\n";
            code += "    }\n";

            // DEFAULT
            code += "    public void SetDefaultState()\n";
            code += "    {\n";
            foreach (var v in variables)
            {
                code += $"        {v.Name} = {v.DefaultValue};\n";
            }
            code += "    }\n";

            // SAVE
            code += "    public void SaveVariable(uint id, Stream s)\n";
            code += "    {\n";
            code += "        switch (id)\n";
            code += "        {\n";
            foreach (var v in variables)
            {
                code += $"            case {v.Id}: {v.TypeTree.SaveVariableClause("s", v.Name)} break;\n";
            }
            code += "        }\n";
            code += "    }\n";

            // LENGHT
            code += "    public int ReportLenghtOfVariable(uint id)\n";
            code += "    {\n";
            code += "        switch (id)\n";
            code += "        {\n";
            foreach (var v in variables)
            {
                code += $"            case {v.Id}: {v.TypeTree.ReportLengthClause(v.Name)}\n";
            }
            code += "            default: return 0;\n";
            code += "        }\n";
            code += "    }\n";

            // VAR NAMES
            code += "    public List<uint> GetVarNamesToSave()\n";
            code += "    {\n";
            code += "        return new List<uint>() {";
            bool first = true;
            foreach (var v in variables)
            {
                if (!first) code += ", ";
                code += v.Id;
                first = false;
            }
            code += "};\n";
            code += "    }\n";

            // CONSTRUCTOR
            code += $"    public {className}()\n";
            code += "    {\n";
            code += "        SetDefaultState();\n";
            code += "    }\n";

            // END
            code += "}\n";
            code += "\n";

            return code;
        }
        public static Tuple<CG_TypeDescription, string> ParseType(string typeString)
        {
            string firstPart = "";
            bool started = false;
            CG_TypeDescription current = null;
            List<CG_TypeDescription> parameters = new List<CG_TypeDescription>();
            for (int i = 0; i < typeString.Length; i++)
            {
                if (typeString[i] == '<')
                {
                    started = true;
                    string rest = typeString.Substring(i + 1);
                    Tuple<CG_TypeDescription, string> t = ParseType(rest);
                    parameters.Add(t.Item1);
                    typeString = t.Item2;
                    i = -1;
                }
                else if (typeString[i] == ',')
                {
                    if (!started)
                    {
                        if (current == null) current = GetType(firstPart);
                        return new Tuple<CG_TypeDescription, string>(current, typeString.Substring(i));
                    }
                    else
                    {
                        string rest = typeString.Substring(i + 1);
                        Tuple<CG_TypeDescription, string> t = ParseType(rest);
                        parameters.Add(t.Item1);
                        typeString = t.Item2;
                        i = -1;
                    }
                }
                else if (typeString[i] == '>')
                {
                    if (!started)
                    {
                        if (current == null) current = GetType(firstPart);
                        return new Tuple<CG_TypeDescription, string>(current, typeString.Substring(i));
                    }
                    else
                    {
                        // BUILD type
                        if (firstPart == "List")
                        {
                            current = new CG_ListType(parameters[0]);
                        }
                        else if (firstPart == "Dictionary")
                        {
                            current = new CG_DictionaryType(parameters[0], parameters[1]);
                        }
                        else if (firstPart == "Tuple")
                        {
                            if (parameters.Count == 1)
                            {
                                current = new CG_Tuple1Type(parameters[0]);
                            }
                            else if (parameters.Count == 2)
                            {
                                current = new CG_Tuple2Type(parameters[0], parameters[1]);
                            }
                            else if (parameters.Count == 3)
                            {
                                current = new CG_Tuple3Type(parameters[0], parameters[1], parameters[2]);
                            }
                            else if (parameters.Count == 4)
                            {
                                current = new CG_Tuple4Type(parameters[0], parameters[1], parameters[2], parameters[3]);
                            }
                            else if (parameters.Count == 5)
                            {
                                current = new CG_Tuple5Type(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                            }
                        }
                        started = false;
                    }
                }
                else if (typeString[i] == '[' || typeString[i] == ']')
                {
                    if (current == null) current = GetType(firstPart);
                    if (typeString[i + 1] == ',')
                    {
                        current = new CG_Array2Type(current);
                        i += 2;
                    }
                    else
                    {
                        current = new CG_Array1Type(current);
                        i++;
                    }
                }
                else if (typeString[i] != ' ')
                {
                    firstPart += typeString[i];
                }
            }
            if (current == null) current = GetType(firstPart);
            return new Tuple<CG_TypeDescription, string>(current, "");
        }
        static CG_TypeDescription GetType(string type)
        {
            if (unmanagedTypes.Contains(type)) return new CG_UnmanagedTypeOrString(type);
            return new CG_ComplexBaseType(type);
        }
    }
    public static class CodeGenerationExample
    {
        public static string Example1()
        {
            return "ICO Level\r\nDER IDisposable\r\nVAR 0x100 private ushort[,] BlockStates new ushort[3,3]\r\nVAR 0x200 publicget Tuple<ushort[,],int> Test new Tuple<ushort[,],int>(new ushort[3,3],0)";
        }
    }

    public class CG_VariableDescription
    {
        public bool IsParameter { get; set; }
        public string Par_TypeAsign { get; set; }
        public string Par_DependsOn { get; set; }

        public string Name { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string DefaultValue { get; set; }
        public string Access { get; set; }

        public CG_VariableDescription(string name, string id, string type, string defaultValue, string access)
        {
            Name = name;
            Id = id;
            Type = type;
            DefaultValue = defaultValue;
            TypeTree = CodeGeneration.ParseType(type).Item1;
            Access = access;

            IsParameter = false;
            Par_TypeAsign = null;
            Par_DependsOn = null;
        }
        public CG_VariableDescription(string name, string id, string type, string defaultValue, string access, CG_TypeDescription typeTree)
        {
            Name = name;
            Id = id;
            Type = type;
            DefaultValue = defaultValue;
            TypeTree = TypeTree;
            Access = access;

            IsParameter = false;
            Par_TypeAsign = null;
            Par_DependsOn = null;
        }

        public CG_TypeDescription TypeTree { get; }
    }
    public abstract class CG_TypeDescription
    {
        public abstract string GetTypeName();

        public abstract string AcceptVariableClause(string streamName);
        public abstract string SaveVariableClause(string streamName, string variableName);
        public abstract string ReportLengthClause(string variableName);
    }
    public class CG_UnmanagedTypeOrString : CG_TypeDescription
    {
        string specificType;

        public CG_UnmanagedTypeOrString(string specificType)
        {
            this.specificType = specificType;
        }

        public override string GetTypeName()
        {
            return specificType;
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = "";
            if (specificType == "string") ret = $"StreamHelper.LoadString({streamName});";
            else ret = $"StreamHelper.LoadUnmanaged<{specificType}>({streamName});";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = "";
            if (specificType == "string") ret = $"StreamHelper.SaveString({streamName}, {variableName});";
            else ret = $"StreamHelper.SaveUnmanaged<{specificType}>({streamName}, {variableName});";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            if (specificType != "string") return $"return StreamHelper.GetUnmanagedLenght<{specificType}>({variableName});";
            else return $"return StreamHelper.GetLenght({variableName});";
        }
    }
    public class CG_ComplexBaseType : CG_TypeDescription
    {
        string name;

        public CG_ComplexBaseType(string name)
        {
            this.name = name;
        }

        public override string GetTypeName()
        {
            return name;
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"UniLoad.CreateObject<{name}>({streamName});";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"UniLoad.Save({streamName}, {variableName});";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            return $"return UniLoad.GetLenght({variableName});";
        }
    }
    public class CG_Array1Type : CG_TypeDescription
    {
        CG_TypeDescription elementType;

        public CG_Array1Type(CG_TypeDescription elementType)
        {
            this.elementType = elementType;
        }

        public override string GetTypeName()
        {
            return $"{elementType.GetTypeName()}[]";
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"StreamHelper.LoadArray<{elementType.GetTypeName()}>";
            ret += $"({streamName}";
            ret += $", ({streamName}s) => {{ return {elementType.AcceptVariableClause(streamName + "s")} }}";
            ret += ");";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"StreamHelper.SaveArray({streamName}, {variableName}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType.SaveVariableClause(streamName + "s", variableName + "0")} }});";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            return $"return StreamHelper.GetLenght<{elementType.GetTypeName()}>({variableName}, {variableName}0 => {{ {elementType.ReportLengthClause(variableName + "0")} }});";
        }
    }
    public class CG_Array2Type : CG_TypeDescription
    {
        CG_TypeDescription elementType;

        public CG_Array2Type(CG_TypeDescription elementType)
        {
            this.elementType = elementType;
        }

        public override string GetTypeName()
        {
            return $"{elementType.GetTypeName()}[,]";
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"StreamHelper.LoadArray2<{elementType.GetTypeName()}>";
            ret += $"({streamName}";
            ret += $", ({streamName}s) => {{ return {elementType.AcceptVariableClause(streamName + "s")} }}";
            ret += ");";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"StreamHelper.SaveArray2({streamName}, {variableName}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType.SaveVariableClause(streamName + "s", variableName + "0")} }});";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            return $"return StreamHelper.GetLenght<{elementType.GetTypeName()}>({variableName}, {variableName}0 => {{ {elementType.ReportLengthClause(variableName + "0")} }});";
        }
    }
    public class CG_ListType : CG_TypeDescription
    {
        CG_TypeDescription elementType;

        public CG_ListType(CG_TypeDescription elementType)
        {
            this.elementType = elementType;
        }

        public override string GetTypeName()
        {
            return $"List<{elementType.GetTypeName()}>";
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"StreamHelper.LoadList<{elementType.GetTypeName()}>";
            ret += $"({streamName}";
            ret += $", ({streamName}s) => {{ return {elementType.AcceptVariableClause(streamName + "s")} }}";
            ret += ");";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"StreamHelper.SaveList({streamName}, {variableName}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType.SaveVariableClause(streamName + "s", variableName + "0")} }});";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            return $"return StreamHelper.GetLenght<{elementType.GetTypeName()}>({variableName}, {variableName}0 => {{ {elementType.ReportLengthClause(variableName + "0")} }});";
        }
    }
    public class CG_DictionaryType : CG_TypeDescription
    {
        CG_TypeDescription keyType;
        CG_TypeDescription valueType;

        public CG_DictionaryType(CG_TypeDescription keyType, CG_TypeDescription valueType)
        {
            this.keyType = keyType;
            this.valueType = valueType;
        }

        public override string GetTypeName()
        {
            return $"Dictionary<{keyType.GetTypeName()}, {valueType.GetTypeName()}>";
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"StreamHelper.LoadDictionary<{keyType.GetTypeName()}, {valueType.GetTypeName()}>";
            ret += $"({streamName}";
            ret += $", ({streamName}s) => {{ return {keyType.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {valueType.AcceptVariableClause(streamName + "s")} }}";
            ret += ");";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"StreamHelper.SaveDictionary({streamName}, {variableName}";
            ret += $", ({streamName}s, {variableName}0) => {{ {keyType.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {valueType.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += ");";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            string ret = $"return StreamHelper.GetLenght<{keyType.GetTypeName()},{valueType.GetTypeName()}>({variableName}";
            ret += $", {variableName}0 => {{ {keyType.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {valueType.ReportLengthClause(variableName + "0")} }}";
            ret += ");";
            return ret;
        }
    }
    public class CG_Tuple1Type : CG_TypeDescription
    {
        CG_TypeDescription elementType1;

        public CG_Tuple1Type(CG_TypeDescription elementType1)
        {
            this.elementType1 = elementType1;
        }

        public override string GetTypeName()
        {
            return $"Tuple<{elementType1.GetTypeName()}>";
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"StreamHelper.LoadTuple<{elementType1.GetTypeName()}>";
            ret += $"({streamName}";
            ret += $", ({streamName}s) => {{ return {elementType1.AcceptVariableClause(streamName + "s")} }}";
            ret += ");";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"StreamHelper.SaveTuple<{elementType1.GetTypeName()}>({streamName}, {variableName}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType1.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += ");";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            string ret = $"return StreamHelper.GetLenght<{elementType1.GetTypeName()}>({variableName}";
            ret += $", {variableName}0 => {{ {elementType1.ReportLengthClause(variableName + "0")} }}";
            ret += ");";
            return ret;
        }
    }
    public class CG_Tuple2Type : CG_TypeDescription
    {
        CG_TypeDescription elementType1;
        CG_TypeDescription elementType2;

        public CG_Tuple2Type(CG_TypeDescription elementType1, CG_TypeDescription elementType2)
        {
            this.elementType1 = elementType1;
            this.elementType2 = elementType2;
        }

        public override string GetTypeName()
        {
            return $"Tuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}>";
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"StreamHelper.LoadTuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}>";
            ret += $"({streamName}";
            ret += $", ({streamName}s) => {{ return {elementType1.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType2.AcceptVariableClause(streamName + "s")} }}";
            ret += ");";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"StreamHelper.SaveTuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}>({streamName}, {variableName}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType1.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType2.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += ");";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            string ret = $"return StreamHelper.GetLenght<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}>({variableName}";
            ret += $", {variableName}0 => {{ {elementType1.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType2.ReportLengthClause(variableName + "0")} }}";
            ret += ");";
            return ret;
        }
    }
    public class CG_Tuple3Type : CG_TypeDescription
    {
        CG_TypeDescription elementType1;
        CG_TypeDescription elementType2;
        CG_TypeDescription elementType3;

        public CG_Tuple3Type(CG_TypeDescription elementType1, CG_TypeDescription elementType2, CG_TypeDescription elementType3)
        {
            this.elementType1 = elementType1;
            this.elementType2 = elementType2;
            this.elementType3 = elementType3;
        }

        public override string GetTypeName()
        {
            return $"Tuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}>";
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"StreamHelper.LoadTuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}>";
            ret += $"({streamName}";
            ret += $", ({streamName}s) => {{ return {elementType1.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType2.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType3.AcceptVariableClause(streamName + "s")} }}";
            ret += ");";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"StreamHelper.SaveTuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}>({streamName}, {variableName}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType1.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType2.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType3.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += ");";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            string ret = $"return StreamHelper.GetLenght<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}>({variableName}";
            ret += $", {variableName}0 => {{ {elementType1.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType2.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType3.ReportLengthClause(variableName + "0")} }}";
            ret += ");";
            return ret;
        }
    }
    public class CG_Tuple4Type : CG_TypeDescription
    {
        CG_TypeDescription elementType1;
        CG_TypeDescription elementType2;
        CG_TypeDescription elementType3;
        CG_TypeDescription elementType4;

        public CG_Tuple4Type(CG_TypeDescription elementType1, CG_TypeDescription elementType2, CG_TypeDescription elementType3, CG_TypeDescription elementType4)
        {
            this.elementType1 = elementType1;
            this.elementType2 = elementType2;
            this.elementType3 = elementType3;
            this.elementType4 = elementType4;
        }

        public override string GetTypeName()
        {
            return $"Tuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}, {elementType4.GetTypeName()}>";
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"StreamHelper.LoadTuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}, {elementType4.GetTypeName()}>";
            ret += $"({streamName}";
            ret += $", ({streamName}s) => {{ return {elementType1.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType2.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType3.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType4.AcceptVariableClause(streamName + "s")} }}";
            ret += ");";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"StreamHelper.SaveTuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}, {elementType4.GetTypeName()}>({streamName}, {variableName}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType1.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType2.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType3.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType4.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += ");";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            string ret = $"return StreamHelper.GetLenght<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}, {elementType4.GetTypeName()}>({variableName}";
            ret += $", {variableName}0 => {{ {elementType1.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType2.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType3.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType4.ReportLengthClause(variableName + "0")} }}";
            ret += ");";
            return ret;
        }
    }
    public class CG_Tuple5Type : CG_TypeDescription
    {
        CG_TypeDescription elementType1;
        CG_TypeDescription elementType2;
        CG_TypeDescription elementType3;
        CG_TypeDescription elementType4;
        CG_TypeDescription elementType5;

        public CG_Tuple5Type(CG_TypeDescription elementType1, CG_TypeDescription elementType2, CG_TypeDescription elementType3, CG_TypeDescription elementType4, CG_TypeDescription elementType5)
        {
            this.elementType1 = elementType1;
            this.elementType2 = elementType2;
            this.elementType3 = elementType3;
            this.elementType4 = elementType4;
            this.elementType5 = elementType5;
        }

        public override string GetTypeName()
        {
            return $"Tuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}, {elementType4.GetTypeName()}, {elementType5.GetTypeName()}>";
        }
        public override string AcceptVariableClause(string streamName)
        {
            string ret = $"StreamHelper.LoadTuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}, {elementType4.GetTypeName()}, {elementType5.GetTypeName()}>";
            ret += $"({streamName}";
            ret += $", ({streamName}s) => {{ return {elementType1.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType2.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType3.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType4.AcceptVariableClause(streamName + "s")} }}";
            ret += $", ({streamName}s) => {{ return {elementType5.AcceptVariableClause(streamName + "s")} }}";
            ret += ");";
            return ret;
        }
        public override string SaveVariableClause(string streamName, string variableName)
        {
            string ret = $"StreamHelper.SaveTuple<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}, {elementType4.GetTypeName()}, {elementType5.GetTypeName()}>({streamName}, {variableName}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType1.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType2.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType3.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType4.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += $", ({streamName}s, {variableName}0) => {{ {elementType5.SaveVariableClause(streamName + "s", variableName + "0")} }}";
            ret += ");";
            return ret;
        }
        public override string ReportLengthClause(string variableName)
        {
            string ret = $"return StreamHelper.GetLenght<{elementType1.GetTypeName()}, {elementType2.GetTypeName()}, {elementType3.GetTypeName()}, {elementType4.GetTypeName()}, {elementType5.GetTypeName()}>({variableName}";
            ret += $", {variableName}0 => {{ {elementType1.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType2.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType3.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType4.ReportLengthClause(variableName + "0")} }}";
            ret += $", {variableName}0 => {{ {elementType5.ReportLengthClause(variableName + "0")} }}";
            ret += ");";
            return ret;
        }
    }
}
