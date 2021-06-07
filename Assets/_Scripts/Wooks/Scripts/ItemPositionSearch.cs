using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPositionSearch : MonoBehaviour
{
    public static ItemPositionSearch S;
    //맨처음 경기장 정보를 받았을때 타일타입별로 배열 저장.
    TileData[] tileDatas = new TileData[81];
    RobotData[] robotDatas = new RobotData[4];
    int[] vpTiles; //= { 2,4,5,7,9};
    int[] kitTiles;// = { 45,12,81};

    private void Awake()
    {
        S = this;   
    }

    void RenewTileInfo(TileData[] newTileDatas)
    {
        for (int i = 0; i < tileDatas.Length; i++)
        {
            for (int j = 0; j < newTileDatas.Length; j++)
            {
                if (tileDatas[i].tileIdx == newTileDatas[j].tileIdx)
                {
                    tileDatas[i] = newTileDatas[j];
                    break;
                }
            }
        }
    }
    void RenewRobotInfo(RobotData[] newRobotDatas)
    {
        for(int i = 0; i < robotDatas.Length; i++)
        {
            for (int j = 0; j < newRobotDatas.Length; j++)
            {
                if (robotDatas[i].tileIdx == newRobotDatas[j].tileIdx)
                {
                    robotDatas[i] = newRobotDatas[j];
                    break;
                }
            }
        }
    }
    public int SearchVPPlace()
    {
        int idx = Random.Range(0, vpTiles.Length);
        return vpTiles[idx];
    }
    public int SearchRepairKitPlace()
    {
        //int idx = Random.Range(0, kitTiles.Length);
        foreach (var item in tileDatas)
        {
            if (!item.isHaveRepairKit)
                return item.tileIdx;
        }
        return 0;
    }
    int[] GetRobotTiles()
    {
        int[] tmp = new int[robotDatas.Length];
        for (int i = 0; i < robotDatas.Length; i++)
        {
            tmp[i] = robotDatas[i].tileIdx;
        }
        return tmp;
    }
    public int SearchModulePlace()
    {
        int idx = 0;
        int[] robotTiles = GetRobotTiles();
        List<int> canPlaceModuleTiles = new List<int>();
        foreach (var item in tileDatas)
        {
            if (robotTiles[idx] == item.tileIdx)
            {
                idx++;
                continue;
            }
            if (item.tileType == Volt_Tile.TileType.pits ||
                item.tileType == Volt_Tile.TileType.startingSpace ||
                item.tileType == Volt_Tile.TileType.none)
                continue;
            canPlaceModuleTiles.Add(item.tileIdx);
        }
        return canPlaceModuleTiles[Random.Range(0, canPlaceModuleTiles.Count)];
    }
}
