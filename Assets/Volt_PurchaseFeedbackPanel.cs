using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_PurchaseFeedbackPanel : MonoBehaviour
{
    public UILabel feedbackMsg;

    public void Feedback(EShopPurchase assetsType, bool isSuccess)
    {
        feedbackMsg.text = Volt_Utils.GetItemNameByLanguage(assetsType);   
        feedbackMsg.text += " ";

        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                if (isSuccess)
                    feedbackMsg.text += "Réussite des achats";
                else
                    feedbackMsg.text += "L'achat a échoué";
                break;
            case SystemLanguage.German:
                if (isSuccess)
                    feedbackMsg.text += "Kauf erfolgreich";
                else
                    feedbackMsg.text += "Kauf fehlgeschlagen";
                break;
            case SystemLanguage.Korean:
                if (isSuccess)
                    feedbackMsg.text += "구매 성공";
                else
                    feedbackMsg.text += "구매 실패";
                break;
            default:
                if (isSuccess)
                    feedbackMsg.text += "Purchase Success";
                else
                    feedbackMsg.text += "Purchase Failed";
                break;
        }
    }
}
