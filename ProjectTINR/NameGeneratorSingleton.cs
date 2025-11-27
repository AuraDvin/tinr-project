using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ProjectTINR;

public class NameGeneratorSingleton {
    static NameGeneratorSingleton _instance;
    static private Dictionary<string, HashSet<int>> _ids;

    private NameGeneratorSingleton() {
    }

    public static NameGeneratorSingleton Instance {
        get {
            if (_instance == null) {
                _instance = new();
                _ids = new();
            }
            return _instance;
        }
    }

    public string GetName(string prefix) {
        Random rnd = new();
        int val = (int)rnd.NextInt64();
        if (_ids.ContainsKey(prefix)) {
            while (_ids[prefix].Contains(val)) {
                val = (int)rnd.NextInt64();
            }
        }
        else {
            _ids.Add(prefix, new());
        }
        _ids[prefix].Add(val);
        return prefix + val.ToString();
    }

}
