using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class AccountMaker_Popup : UI_Popup
{
    enum InputFields
    {
        Nickname_InputField
    }

    enum Lables
    {
        Placeholder,
        Nickname_Label,
        Ok_Label,
        Guid_Label_0,
        Guid_Label_1,
        Guid_Label_2
    }

    enum Buttons
    {
        Ok_Btn
    }

    enum Sprites
    {
        Block,
        Background,
    }

    public override void Init()
    {
        base.Init();

        Bind<UIInput>(typeof(InputFields));
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Lables));
        Bind<UISprite>(typeof(Sprites));

        //BindSprite<UIInput>(typeof(InputFields));
        //BindSprite<UIButton>(typeof(Buttons));
        //BindSprite<UISprite>(typeof(Sprites));

        //SetButtonSwap(typeof(Buttons));

        Get<UIInput>((int)InputFields.Nickname_InputField).onChange.Add(new EventDelegate(() =>
        {
            UILabel label = Get<UIInput>((int)InputFields.Nickname_InputField).label;
            UILabel placeholder = Get<UILabel>((int)Lables.Placeholder);
            if(label.text.Length > 0)
            {
                placeholder.gameObject.SetActive(false);
            }
            else
            {
                placeholder.gameObject.SetActive(true);
            }
        }));

        Get<UIInput>((int)InputFields.Nickname_InputField).onSubmit.Add(new EventDelegate(OnClickOkButton));
        Get<UIButton>((int)Buttons.Ok_Btn).onClick.Add(new EventDelegate(OnClickOkButton));
    }

    private void OnClickOkButton()
    {
        UIInput nicknameInput = Get<UIInput>((int)InputFields.Nickname_InputField);
        string nickname = nicknameInput.label.text;

        if (nickname.Length > 12 || nickname.Length < 2)
        {
            Managers.UI.ShowPopupUIAsync<NicknameLimitLengthExceeded_Popup>();
            return;
        }

        for (int i = 0; i < nickname.Length; ++i)
        {
            if ((int)nickname[i] >= 65 && (int)nickname[i] <= 90)
                continue;
            else if ((int)nickname[i] >= 95 && (int)nickname[i] <= 122)
                continue;
            else if ((int)nickname[i] >= 48 && (int)nickname[i] <= 57)
                continue;

            Managers.UI.ShowPopupUIAsync<InvalidNickname_Popup>();
            return;
        }

        PacketTransmission.SendSignUpPacket(Volt_PlayerData.instance.UserToken.Length, Volt_PlayerData.instance.UserToken, nickname.Length, nickname);
        ClosePopupUI();
    }
}
