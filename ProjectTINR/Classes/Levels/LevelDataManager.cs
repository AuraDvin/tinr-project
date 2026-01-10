using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Levels;

public class LevelDataManager {

    private static Scene _sceneData = null;

    public static void SaveData(Level level) {
        GameObject[] arr = new GameObject[level.Scene.Count];
        level.Scene.CopyTo(arr);
        _sceneData = [.. arr];
    }

    public static bool LoadData(Level level) {
        if (_sceneData != null) {
            GameObject[] arr = new GameObject[_sceneData.Count];
            _sceneData.CopyTo(arr);
            level.Scene = [.. arr]; 
            return true;
        }
        return false;
    }

    public static void RemoveData() {
        _sceneData = null;
    }
}
