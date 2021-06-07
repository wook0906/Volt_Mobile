using UnityEngine;
using UnityEditor;

public class SignUpFailNicknameSamePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Managers.UI.ShowPopupUIAsync<DuplicateNickname_Popup>();
        //Debug.Log("SignUpFailNicknameSamePacket Unpack");
        //Debug.Log("닉네임이 겹치는 처리를 해줘야 해!");

        //Debug.Log("해당 계정명은 사용할 수 없는 계정명 입니다.");
        //Volt_TitleSceneManager.S.accountCreateErrorPanel.SetActive(true);

        //UILabel errorMsg = Volt_TitleSceneManager.S.accountCreateErrorPanel.GetComponentInChildren<UILabel>();
        //switch (Application.systemLanguage)
        //{
        //    case SystemLanguage.French:
        //        errorMsg.text = "L'identifiant existe déjà";
        //        break;
        //    case SystemLanguage.German:
        //        errorMsg.text = "Diese nickname existiert bereits.";
        //        break;
        //    case SystemLanguage.Korean:
        //        errorMsg.text = "해당 닉네임은 이미 존재합니다.";
        //        break;
        //    default:
        //        errorMsg.text = "This nickname already exists.";
        //        break;
        //}
        return;
    }

}