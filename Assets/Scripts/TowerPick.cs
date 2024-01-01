using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPick : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Material glowMaterial;
    [SerializeField] private Material graywMaterial;

    [SerializeField] private Button towerPickerButton; // Add this line

    private Material defaultMaterial;

    public static TowerPick instance;
    public bool picked = false;

    void Awake()
    {
        instance = this;
        defaultMaterial = towerPickerButton.image.material;
        towerPickerButton.onClick.AddListener(onTowerButtonClicked);
    }

    public void onTowerButtonClicked()
    {
        //todo check if pickable
        Debug.Log("Picked");      
        picked = !picked;
        if (picked)
        {
            towerPickerButton.image.material = glowMaterial;
        }
        else
        {
            towerPickerButton.image.material = defaultMaterial;
        }
    }

    public void UnPick()
    {
        picked = false;
        towerPickerButton.image.material = defaultMaterial;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Level.instance.Coins < 10)
        {
            towerPickerButton.image.material = graywMaterial;
            towerPickerButton.interactable = false;
        }
        else
        {
            towerPickerButton.image.material = defaultMaterial;
            towerPickerButton.interactable = true;    
        }
    }
}
