using UnityEngine;
using UnityEditor;

public class SignInFailPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //DB에 존재하지 않는 토큰을 전달하면 회신됌.(신규 유저로 판단됌)
        //Debug.Log("SignInFailPacket Unpack");
        //Debug.Log("너임마 없는 아이디야!");
        LoginScene_UI loginScene_UI = GameObject.FindObjectOfType<LoginScene_UI>();
        if(loginScene_UI == null)
        {
            Debug.LogError("SignInFail 패킷 처리 안됨 [LoginScene_UI 없음!!]");
            return;
        }
        loginScene_UI.OnFailSignIn();
    }

}