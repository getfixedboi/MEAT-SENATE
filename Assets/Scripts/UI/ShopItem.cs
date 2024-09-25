using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ItemBehaviour ItemRef;
    public UnityEngine.UI.Text ItemDescriptionText;
    private int _itemCost;
    public UnityEngine.UI.Text MeatCounterText;
    public void Start()
    {
        GetComponentInChildren<UnityEngine.UI.Image>().sprite = ItemRef.GetSprite();
        _itemCost = UnityEngine.Random.Range(10, 15);
        GetComponentInChildren<UnityEngine.UI.Text>().text = _itemCost.ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && PlayerSkills.MeatPieceCount >= _itemCost)
        {
            PlayerSkills.MeatPieceCount -= _itemCost;
            MeatCounterText.text = PlayerSkills.MeatPieceCount.ToString();
            GameObject obj = GameObject.Instantiate(ItemRef.gameObject, GameObject.FindWithTag("Player").transform.forward, new Quaternion());
            obj.GetComponent<ItemBehaviour>().isShop = true;
            obj.GetComponent<ItemBehaviour>().OnGet(true);
            ItemDescriptionText.text = "";
            Destroy(this.gameObject);
        }
    }
    public void OnPointerEnter(PointerEventData eventData) { ItemDescriptionText.text = ItemRef.GetDesc(); }
    public void OnPointerExit(PointerEventData eventData) { ItemDescriptionText.text = ""; }
}
