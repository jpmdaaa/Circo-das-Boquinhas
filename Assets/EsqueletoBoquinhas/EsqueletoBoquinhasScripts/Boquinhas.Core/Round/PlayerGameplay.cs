using System.Collections;
using System.IO;
using Playmove.Avatars.API;
using Playmove.Avatars.API.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameplay : MonoBehaviour
{
    public int slotNumber;
    public TMP_Text text;
    public Image avatarImage;
    public AudioClip playerTurnSound;

    private Vector3 inPos;
    private Sprite m_AvatarImage;
    private Player m_Player;
    private RectTransform m_RectTransform;
    private Slot m_Slot;
    private Vector3 outPos;

    private Vector3 startingPos;

    private void Awake()
    {
        try
        {
            var deparaslotgambiarra = slotNumber;
            if (slotNumber == 2)
                deparaslotgambiarra = 3;
            else if (slotNumber == 3) deparaslotgambiarra = 2;
            m_Slot = AvatarAPI.CurrentSlots[deparaslotgambiarra];
            m_Player = m_Slot.Players[0];
        }
        catch
        {
            gameObject.SetActive(false);
            return;
        }

        m_RectTransform = GetComponent<RectTransform>();
        SetupSlot(m_Player.Name, m_Player.ThumbnailPath);

        startingPos = transform.position;
        switch (slotNumber)
        {
            case 0:
                inPos = new Vector3(-700f, -300f, 0f);
                outPos = new Vector3(-700f, -600f, 0f);
                break;
            case 1:
                inPos = new Vector3(700f, -300f, 0f);
                outPos = new Vector3(700f, -600f, 0f);
                break;
            case 3:
                inPos = new Vector3(-700f, 300f, 0f);
                outPos = new Vector3(-700f, 600f, 0f);
                break;
            case 2:
                inPos = new Vector3(700f, 300f, 0f);
                outPos = new Vector3(700f, 600f, 0f);
                break;
        }
    }

    public void SetupSlot(string name, string avatarPath)
    {
        if (name == null)
        {
            gameObject.SetActive(false);
            return;
        }

        text.text = name;
        avatarImage.sprite = LoadSprite(avatarPath);
    }

    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (File.Exists(path))
        {
            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            m_AvatarImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            return m_AvatarImage;
        }

        return null;
    }

    public Sprite GetAvatarSprite()
    {
        return m_AvatarImage;
    }

    public string GetAvatarName()
    {
        return m_Player.Name;
    }

    public void EnterTurn() // Animação de entrada no turno
    {
        StartCoroutine(EnterTurnCoroutine());
    }

    private IEnumerator EnterTurnCoroutine()
    {
        AudioSystem.Instance.PlaySilenceableSfx(playerTurnSound);
        var t = 0f;
        //Vector3 endScale = new Vector3(1.25f, 1.25f, 1f);

        while (t < 1f)
        {
            t += Time.deltaTime * 5;
            transform.position = Vector3.Lerp(startingPos, new Vector3(960, 540, 0), t);
            //transform.localScale = Vector3.Lerp(Vector3.one, endScale, t);
            yield return null;
        }

        yield return null;
    }

    public void EnterGuess() // Animação de movimento para o painel de resposta
    {
        StartCoroutine(EnterGuessCoroutine());
    }

    private IEnumerator EnterGuessCoroutine()
    {
        var t = 0f;
        //Vector3 endScale = transform.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime * 5;
            transform.localPosition = Vector3.Lerp(Vector3.zero, inPos, t);
            //transform.localScale = Vector3.Lerp(endScale, Vector3.one, t);
            yield return null;
        }

        yield return null;
    }

    public void EnterGameplay(bool enter)
    {
        if (enter) StartCoroutine(EnterGameplayCoroutine());
        else StartCoroutine(LeaveGameplayCoroutine());
    }

    private IEnumerator EnterGameplayCoroutine()
    {
        var t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 5;
            m_RectTransform.anchoredPosition = Vector3.Lerp(outPos, inPos, t);
            yield return null;
        }

        yield return null;
    }

    private IEnumerator LeaveGameplayCoroutine()
    {
        var t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 5;
            m_RectTransform.anchoredPosition = Vector3.Lerp(inPos, outPos, t);
            yield return null;
        }

        yield return null;
    }
}