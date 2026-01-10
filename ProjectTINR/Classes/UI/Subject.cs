using System;
using System.Collections.Generic;

namespace ProjectTINR.Classes.UI;

public interface Subject
{
    public void AddObserver(Observer observer);
    public void RemoveObserver(Observer observer);
    public List<Observer> Observers {get; set;}
    public void Notify() {
        foreach (var observer in Observers){
            observer.Notify();
        }
    }

}
