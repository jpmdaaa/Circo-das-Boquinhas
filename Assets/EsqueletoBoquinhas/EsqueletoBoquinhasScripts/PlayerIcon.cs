using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    public TMP_Text text;
    public Image avatarImage;
    public GameObject avatar;
    public GameObject noAvatar;
    public Animator star1;
    public Animator star2;
    public Animator star3;
    public AudioClip starSound;

    public void SetupSlot(string name, string avatarPath)
    {
        if (name == null)
        {
//            print("null");
            avatar.SetActive(false);
            noAvatar.SetActive(true);
        }
        else
        {
//            print(name);
            name = name.Split(" ")[0];
            text.text = name;
            avatarImage.sprite = LoadSprite(avatarPath);
            avatar.SetActive(true);
            noAvatar.SetActive(false);
        }
    }

    private Sprite LoadSprite(string path)
    {
//        print(path);
        if (string.IsNullOrEmpty(path)) return null;
        if (File.Exists(path))
        {
            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }

        return null;
    }

    public void ShowStars(int number)
    {
        StartCoroutine(IShowStars(number));
    }

    private IEnumerator IShowStars(int number)
    {
        var delay = 0.5f;
        if (number == 0)
        {
            star1.SetTrigger("Show");
            PlayStarSound();
            yield return new WaitForSeconds(delay);
            star2.SetTrigger("Show");
            PlayStarSound();
            yield return new WaitForSeconds(delay);
            star3.SetTrigger("Show");
            PlayStarSound();
        }
        else if (number == 1)
        {
            star1.SetTrigger("Show");
            PlayStarSound();
            yield return new WaitForSeconds(delay);
            star2.SetTrigger("Show");
            PlayStarSound();
        }
        else if (number >= 2)
        {
            star1.SetTrigger("Show");
            PlayStarSound();
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
    }

    public void PlayStarSound()
    {
        AudioSystem.Instance.PlaySilenceableSfx(starSound);
    }
}