using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ProjectTINR.Classes;

public class LevelSerializer {
    private Dictionary<LevelType, Scene> _levelScenes = [];
    static private readonly LevelSerializer _instance = new LevelSerializer();
    static public LevelSerializer Instance => _instance;
    private LevelSerializer() { }
    public void SerializeLevel(Level level) {
        _levelScenes[level.Type] = [];
        foreach (var obj in level.Scene) {
            _levelScenes[level.Type].Add(obj);
        }
    }

    // This assumes that the elements in level.Scene are removed as game Components 
    public void DeserializeLevel(Level level) {
        LevelType levelType = level.Type; 
        if (_levelScenes.ContainsKey(levelType)) {
            level.Scene = new Scene(); 
            level.Scene = _levelScenes[levelType];
        } else {
            throw new ArgumentException("No serialized data for the specified level type.");
        }
    }

}
