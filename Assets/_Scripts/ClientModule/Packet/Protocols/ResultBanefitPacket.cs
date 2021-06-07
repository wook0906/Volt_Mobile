using UnityEngine;


class ResultBenefitPacket : Packet
{
    
    public override void UnPack(byte[] buffer)
    { 
        Debug.Log("ResultBenefitPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        EBenefitType id;
        int result;

        id = (EBenefitType)ByteConverter.ToInt(buffer, ref startIndex);
        result = ByteConverter.ToInt(buffer, ref startIndex);//0:success 1:fail
        Debug.Log(result);

        Volt_PlayerData.instance.OnGetBenefit(8000001);
        Managers.Data.SetBenefitInfo(id, result);
        Managers.UI.ShowPopupUIAsync<GetBenefit_Popup>();

    }

}