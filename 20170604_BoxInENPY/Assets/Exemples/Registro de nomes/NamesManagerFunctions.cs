using UnityEngine;
using System.Collections;

public class NamesManagerFunctions : MonoBehaviour
{
    [SerializeField]
    private Playmove.NamesRegisterButton[] _registerButtons;

    public void ClearAll()
    {
        Playmove.PYNamesManager.DeleteAll();
        ReLoadNames();
    }

    public void ReLoadNames()
    {
        Playmove.PYNamesManager.LoadNames();
    }

    public void RegisterNames()
    {
        foreach (Playmove.NamesRegisterButton register in _registerButtons)
        {
            foreach (Playmove.NamesManagerPopup.PlayerInfo info in register.Players)
            {
                if(!string.IsNullOrEmpty(info.Name))
                    Playmove.PYNamesManager.SaveName(info.Name, info.ClassId);
            }
        }

        ReLoadNames();
    }

}
