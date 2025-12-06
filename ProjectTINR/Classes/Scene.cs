using System.Collections;

namespace ProjectTINR.Classes; 
public class Scene : ArrayList {
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
        foreach (var component in this) {
            if (component is T thing) {
                return thing;
            }
        }
        return default;
    }
}