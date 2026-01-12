using System;

namespace ProjectTINR.Classes.UI;

public interface Observer
{
    public void Notify();
    public void AddToSubject(Subject subject) {
        subject.AddObserver(this);
    }
    public void Notify(string message, object? args);
}
