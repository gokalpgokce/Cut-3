using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public void BlueButton()
    {
        Game.Instance.ColorPicker(ItemType.Blue);
        Debug.Log("blue button basildi");
    }
    public void DarkBlueButton()
    {
        Game.Instance.ColorPicker(ItemType.Darkblue);
    }
    public void GreenButton()
    {
        Game.Instance.ColorPicker(ItemType.Green);
    }
    public void OrangeButton()
    {
        Game.Instance.ColorPicker(ItemType.Orange);
    }
    public void PurpleButton()
    {
        Game.Instance.ColorPicker(ItemType.Purple);
    }
    public void RedButton()
    {
        Game.Instance.ColorPicker(ItemType.Red);
    }
    public void YellowButton()
    {
        Game.Instance.ColorPicker(ItemType.Yellow);
    }
}
