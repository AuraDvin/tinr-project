using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

using ProjectTINR.Classes;

namespace ProjectTINR.Classes.UI;

public interface ComplexUIElement : SimpleUIElement {
    public Scene Children { get; set; }

}
