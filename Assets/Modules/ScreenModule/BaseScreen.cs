using UnityEngine;
using UnityEngine.Events;

public class BaseScreen : MonoBehaviour
{ 
    public ScreenType type;
    public Animator anim;

    public void onEnable()
    {
        if (anim != null)
        {
            anim.SetTrigger("Show");
        }
    }
    
    public void onDisable()
    {
        if (anim != null)
        {
            anim.SetTrigger("Hide");
        }
    }

}
