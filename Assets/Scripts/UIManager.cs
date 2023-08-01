using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI overview;
    [SerializeField] private Image image;


    public void SetValue(Texture2D texture = null, string title = null, string overview = null)
    {
        if(texture != null)
            ApplySprite(texture);

        if(title != null && overview != null)
            ApplyText(title, overview);
    }

    #region Apply

    private void ApplySprite(Texture2D texture2D = null)
    {
        Rect rec = new Rect(0, 0, texture2D.width, texture2D.height);
        Sprite currentSprite = Sprite.Create(texture2D, rec, new Vector2(0.5f,0.5f), 100);
        image.sprite = currentSprite;
    }

    private void ApplyText(string titleValue, string overviewValue)
    {
        title.text = titleValue;
        overview.text = overviewValue;
    }

    #endregion Apply
}
