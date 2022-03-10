using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITracking : MonoBehaviour
{
    public TextMeshProUGUI moneyLabel;
    public TextMeshProUGUI fighterLabel;
    public TextMeshProUGUI missileLabel;
    public TextMeshProUGUI cruiserLabel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int tempNumFighters = 0;
        int tempNumMFighters = 0;
        int tempNumCruisers = 0;

        for(int i = 0; i < gameObject.transform.childCount; i++){
            GameObject currentChild = gameObject.transform.GetChild(i).gameObject;
            switch (currentChild.GetComponent<Ship>().whoAmI)
            {
                case Ship.ShipType.Fighter:
                    tempNumFighters++;
                    break;
                case Ship.ShipType.MFighter:
                    tempNumMFighters++;
                    break;
                case Ship.ShipType.Cruiser:
                    tempNumCruisers++;
                    break;
                //default:
            }
        }

        moneyLabel.text = GlobalVars.money.ToString();
        fighterLabel.text = tempNumFighters.ToString();
        missileLabel.text = tempNumMFighters.ToString();
        cruiserLabel.text = tempNumCruisers.ToString();
    }
}
