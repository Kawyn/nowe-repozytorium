using UnityEngine;

public class RemotePlug : MonoBehaviour
{
    public Plug plug;


    public void Power(Arguments args)
    {
        if (args.off)
        {
            plug.on = false;
            plug.Emit(false);
        }
        else
        {
            if(!plug.on)
            plug.skipFirst = true;
            plug.on = true;
        }
    }
}
