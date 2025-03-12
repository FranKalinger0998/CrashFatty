using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public enum PlayerColor { Red, Green, Blue, Yellow }
    public PlayerColor selectedColor;
    

    private void Start()
    {
        ApplyColor(selectedColor);
    }

    private void ApplyColor(PlayerColor color)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            switch (color)
            {
                case PlayerColor.Red:
                    renderer.material.color = Color.red;
                    break;
                case PlayerColor.Green:
                    renderer.material.color = Color.green;
                    break;
                case PlayerColor.Blue:
                    renderer.material.color = Color.blue;
                    break;
                case PlayerColor.Yellow:
                    renderer.material.color = Color.yellow;
                    break;
            }
        }
    }
}