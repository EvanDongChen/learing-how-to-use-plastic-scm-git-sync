using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Card Pool")]
    public List<ShopCard> allCards;

    [Header("UI References")]
    public GameObject cardPrefab;
    public Transform shopPanel;
    [SerializeField] private GameObject shopCanvas;

    private int selectedCount = 0;
    private const int maxSelections = 3;

    private List<ShopCard> selectedCards = new();

    private Dictionary<Rarity, float> rarityChances = new()
    {
        { Rarity.Common, 60f },
        { Rarity.Rare, 25f },
        { Rarity.Epic, 10f },
        { Rarity.Legendary, 5f }
    };

    private void Awake()
    {
        GameManager.OnStateChange += GameManagerOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState state)
    {
        if (state == GameState.RoundEnd)
        {
            Debug.Log("GameManager state changed to RoundEnd. Activating shop.");
            shopCanvas.SetActive(true);
            PresentShop();
        }
        else
        {
            Debug.Log("GameManager state is not RoundEnd. Deactivating shop.");
            shopCanvas.SetActive(false);
        }
    }

    void Start()
    {
        shopCanvas.SetActive(false);
    }

    public void PresentShop()
    {
        ClearShop();
        selectedCount = 0;
        selectedCards.Clear();

        List<ShopCard> cardsToShow = GetRandomCards(5);
        foreach (var card in cardsToShow)
        {
            GameObject cardGO = Instantiate(cardPrefab, shopPanel);
            ShopCardUI cardUI = cardGO.GetComponent<ShopCardUI>();
            cardUI.Setup(card);

            Button button = cardGO.GetComponent<Button>();
            button.onClick.AddListener(() => OnCardSelected(cardUI, button));
        }
    }

    void OnCardSelected(ShopCardUI cardUI, Button button)
    {
        if (selectedCount >= maxSelections) return;
        ShopCard cardData = cardUI.GetCardData();
        selectedCards.Add(cardUI.GetCardData());
        selectedCount++;

        button.interactable = false; // Disable after picking

        if (cardData.note != null)
        {
            InventoryManager.Instance.AddNote(cardData.note);
        }

        Debug.Log($"Picked: {cardUI.GetCardData().cardName}");

        if (selectedCount == maxSelections)
        {
            Debug.Log("All 3 picks made.");
            shopCanvas.SetActive(false);
            // You can now proceed to apply effects, give cards to player, etc.
        }
    }

    List<ShopCard> GetRandomCards(int count)
    {
        List<ShopCard> results = new();

        for (int i = 0; i < count; i++)
        {
            Rarity chosenRarity = RollRarity();
            List<ShopCard> filtered = allCards.FindAll(c => c.rarity == chosenRarity);

            if (filtered.Count > 0)
                results.Add(filtered[Random.Range(0, filtered.Count)]);
        }

        return results;
    }

    Rarity RollRarity()
    {
        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        foreach (var pair in rarityChances)
        {
            cumulative += pair.Value;
            if (roll <= cumulative)
                return pair.Key;
        }

        return Rarity.Common; // Fallback
    }

    void ClearShop()
    {
        foreach (Transform child in shopPanel)
            Destroy(child.gameObject);
    }
}
