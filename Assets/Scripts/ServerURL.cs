using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerURL : MonoBehaviour
{
    public void Text_Table()
    {
        Application.OpenURL("http://106.247.250.251:31866/read_texts");
    }
    public void Int_Table()
    {
        Application.OpenURL("http://106.247.250.251:31866/read_ints");
    }
    public void Float_Table()
    {
        Application.OpenURL("http://106.247.250.251:31866/read_floats");
    }
}
