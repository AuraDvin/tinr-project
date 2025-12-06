using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ProjectTINR;

public class NameGeneratorSingleton {
    static private NameGeneratorSingleton s_instance;
    static private Dictionary<string, HashSet<int>> s_ids;

    private NameGeneratorSingleton() {
    }

    public static NameGeneratorSingleton Instance {
        get {
            if (s_instance == null) {
                s_instance = new();
                s_ids = [];
            }
            return s_instance;
        }
    }

    public string GetName(string prefix) {
        Random rnd = new();
        int val = (int)rnd.NextInt64();
        if (s_ids.ContainsKey(prefix)) {
            while (s_ids[prefix].Contains(val)) {
                val = (int)rnd.NextInt64();
            }
        }
        else {
            s_ids.Add(prefix, new());
        }
        s_ids[prefix].Add(val);
        return prefix + val.ToString();
    }

}
