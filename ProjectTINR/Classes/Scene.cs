using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectTINR.Classes;

public class Scene : ArrayList {
    private readonly Dictionary<Type, object> _typeCache = new();
    private readonly List<object> _pendingAdds = new();
    private readonly List<object> _pendingRemoves = new();
    private int _iterationCount = 0;

    public void RemoveByType<T>() {
        T item = default;
        bool found = false;
        foreach (var component in this) {
            if (component is T thing) {
                item = thing;
                found = true;
                break;
            }
        }
        if (found) {
            Remove(item);
        }
    }

    public T FindByType<T>() {
        var t = typeof(T);
        if (_typeCache.TryGetValue(t, out var cached)) {
            return (T)cached;
        }
        foreach (var component in this) {
            if (component is T thing) {
                _typeCache[t] = thing;
                return thing;
            }
        }
        return default;
    }

    public new IEnumerator GetEnumerator() {
        _iterationCount++;
        var baseEnum = base.GetEnumerator();
        try {
            while (baseEnum.MoveNext()) {
                yield return baseEnum.Current;
            }
        }
        finally {
            _iterationCount--;
            if (_iterationCount == 0) ProcessPending();
        }
    }

    private void ProcessPending() {
        if (_pendingRemoves.Count > 0) {
            foreach (var r in _pendingRemoves) {
                base.Remove(r);
                _typeCache.Clear();
                Console.WriteLine("Cleared cache");
                // var keysToRemove = new List<Type>();
                // foreach (var kv in _typeCache) {
                //     if (ReferenceEquals(kv.Value, r)) {
                //         Console.WriteLine("Removing key from cache");
                //         keysToRemove.Add(kv.Key);
                //     }
                // }
                // foreach (var k in keysToRemove) _typeCache.Remove(k);
            }
            _pendingRemoves.Clear();
            Console.WriteLine("Finished removing from scene :)");
        }

        if (_pendingAdds.Count > 0) {
            foreach (var a in _pendingAdds) base.Add(a);
            _pendingAdds.Clear();
            Console.WriteLine("Finished adding to scene :)");
        }

    }

    public new int Add(object value) {
        if (_iterationCount > 0) {
            _pendingAdds.Add(value);
            return -1;
        }
        return base.Add(value);
    }

    public new void Remove(object value) {
        if (_iterationCount > 0) {
            Console.WriteLine($"Added {value} to pending removes in Scene :)");
            _pendingRemoves.Add(value);
            return;
        }
        base.Remove(value);
        var keysToRemove = new List<Type>();
        foreach (var kv in _typeCache) {
            if (ReferenceEquals(kv.Value, value)) keysToRemove.Add(kv.Key);
        }
        foreach (var k in keysToRemove) _typeCache.Remove(k);
    }
}
