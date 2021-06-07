using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ArenaSection
{
    None,
    Center,
    OneQudrant,
    TwoQudrant,
    ThreeQudrant,
    FourQudrant,
    CenterTop,
    CenterBack,
    CenterLeft,
    CenterRight
}
public struct RobotPlaceData
{
    public int playerNumber;
    public int x;
    public int y;
}

public class Volt_ArenaSetter : MonoBehaviour
{
    ////for Non-Networking
    //public int[] normalTileIdxs;
    //public int[] vpTileIdxs;
    //public int[] kitTileIdxs;
    //public int[] pitTileIdxs;
    //public int[] voltageTileIdxs;
    ////for Non-Networking
    [Header("TestSetting")]
    public bool isTest;

    [Header("Set in Inspector")]
    public List<Volt_Tile> tileObjList;
    //public GameObject vpTokenPrefab;
    public Vector3 tileScale;
    public GameObject mapFrame;
    public List<Transform> playerTransforms;

    [Header("Set in Scripts")]
    public int numOfVPSetupTile;
    public int numOfModule;
    public int numOfRepairKit;
    public int numOfSetOnVoltageSpace;
    public static Volt_ArenaSetter S;
    Volt_Tile[,] tileArray;

    public List<Volt_Tile> needSyncTiles;

    float tileSpacing;
    float spacingTileToTile;
    public float DistanceBetweenTiles { get; set; }
    public List<Volt_Robot> robotsInArena;



    private int[] fallTileIdx1 = { 3, 4, 5,
                                    27, 35, 36,
                                    44, 45, 53,
                                    75, 76, 77 };
    private int[] fallTileIdx2 = { 10, 11, 12, 13, 14,
                                    15, 16, 19, 25, 28,
                                    34, 37, 43, 46, 52,
                                    55, 61, 64, 65, 66,
                                    67, 68, 69, 70 };
    [HideInInspector]
    public List<Volt_Tile> fallTiles1;
    [HideInInspector]
    public List<Volt_Tile> fallTiles2;

    public GameObject dustParticle;

    int row;
    int colum;

    private void Awake()
    {
        S = this;
        tileArray = new Volt_Tile[9, 9];
        needSyncTiles = new List<Volt_Tile>();
        spacingTileToTile = tileScale.x * 0.1f;
        int listIdx = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                tileArray[i, j] = tileObjList[listIdx];
                if (isTest)
                    tileArray[i, j].gameObject.AddComponent<BoxCollider>();
                listIdx++;
            }
        }
        row = tileArray.GetLength(0);
        colum = tileArray.GetLength(1);
        DistanceBetweenTiles = ((tileScale.x + tileScale.z) / 2) + spacingTileToTile;
        //Debug.Log($"#################################DistanceBetweenTiles: {DistanceBetweenTiles}");
        tileSpacing = ((tileScale.x + tileScale.z) / 2) + spacingTileToTile;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public IEnumerator SetupField()
    {
        yield return StartCoroutine(SortTileObject());
        Volt_GameManager.S.PlayerSetupStart();
        switch (Volt_GameManager.S.mapType)
        {
            case MapType.TwinCity:
                break;
            case MapType.Rome:
                Volt_GameManager.S.ballistaLaunchPoints = GameObject.FindGameObjectsWithTag("Ballista").OrderBy(x => int.Parse(x.name)).ToArray();

                foreach (var item in Volt_GameManager.S.ballistaLaunchPoints)
                {
                    //Debug.Log("ballista : " + item.name);
                }
                break;
            case MapType.Ruhrgebiet:
                foreach (var item in fallTileIdx1)
                {
                    fallTiles1.Add(GetTileByIdx(item));
                }
                foreach (var item in fallTileIdx2)
                {
                    fallTiles2.Add(GetTileByIdx(item));
                }
                break;
            case MapType.Tokyo:
                break;
            default:
                break;
        }
    }

    public Volt_Tile[,] GetTileArray()
    {
        return tileArray;
    }

    IEnumerator SortTileObject()
    {
        //Vector3 tilePosition = Vector3.zero;
        //Vector3 mapFramePosition = Vector3.zero;
        int idx = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                tileArray[i, j].tileIndex = idx;
                idx++;
                Vector3 scale = tileArray[i, j].transform.localScale;
                scale.x *= tileScale.x;
                scale.y *= tileScale.y;
                scale.z *= tileScale.z;
                //tileArray[i, j].transform.localScale = scale;
                //tileArray[i, j].transform.localPosition = tilePosition;
                tileArray[i, j].tilePosInArray.x = i;
                tileArray[i, j].tilePosInArray.y = j;
                //tilePosition.x += tileSpacing;
                if (tileArray[i, j].transform.Find("ResponseParticle"))
                {
                    tileArray[i, j].responseParticle = tileArray[i, j].transform.Find("ResponseParticle").GetComponent<ParticleSystem>();
                }
                if (tileArray[i, j].pTileType == Volt_Tile.TileType.vpSpace || tileArray[i, j].pTileType == Volt_Tile.TileType.workShop)
                {
                    switch (Volt_PlayerManager.S.I.playerNumber)
                    {
                        case 1:
                            tileArray[i, j].transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            break;
                        case 2:
                            tileArray[i, j].transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                            break;
                        case 3:
                            tileArray[i, j].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                            break;
                        case 4:
                            tileArray[i, j].transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            break;
                        default:

                            break;
                    }
                }
                if (tileArray[i, j].pTileType == Volt_Tile.TileType.voltageSpace)
                {
                    switch (Volt_PlayerManager.S.I.playerNumber)
                    {
                        case 1:
                            tileArray[i, j].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                            break;
                        case 2:
                            tileArray[i, j].transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            break;
                        case 3:
                            tileArray[i, j].transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            break;
                        case 4:
                            tileArray[i, j].transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                            break;
                        default:

                            break;
                    }
                }
            }
            //tilePosition.x = 0f;
            //tilePosition.z += tileSpacing;
        }
        //mapFrame.transform.localPosition = new Vector3(4.4f * tileScale.x, 0, 4.4f * tileScale.z);
        //mapFrame.transform.localScale = tileScale;
        foreach (var item in Volt_PlayerManager.S.GetPlayers())
        {
            item.transform.SetParent(mapFrame.transform);
            item.transform.localPosition = Vector3.zero;
        }
        yield break;
    }
    private bool IsPhillar(int x, int y)
    {
        if (x == 8 && (y == 0 || y == 8))
            return true;
        if (x == 0 && (y == 0 || y == 8))
            return true;
        return false;
    }

    private bool IsPhillar(Volt_Tile.TilePosInArena tilePos)
    {
        return IsPhillar(tilePos.x, tilePos.y);
    }

    /// <summary>
    /// 두 타일 사이의 거리를 반환한다.
    /// </summary>
    /// <param name="tile1"></param>
    /// <param name="tile2"></param>
    /// <returns></returns>
    public int GetDistanceBetweenTiles(Volt_Tile tile1, Volt_Tile tile2)
    {
        Vector3 dir = (tile2.transform.position - tile1.transform.position).normalized;

        int distance = 0;
        distance = Mathf.Abs(tile2.tilePosInArray.x - tile1.tilePosInArray.x) +
                Mathf.Abs(tile2.tilePosInArray.y - tile1.tilePosInArray.y);

        if (dir == Vector3.right || dir == Vector3.left ||
            dir == Vector3.forward || dir == Vector3.back)
        {
            return distance;
        }
        else
        {
            return distance / 2;
        }
    }

    public Volt_Tile GetTile(int x, int y)
    {
        if (!(x >= 0 && x < 9 && y >= 0 && y < 9))
        {
            //print("Get Tile err");
            return null;
        }
        return tileArray[x, y];
    }

    /// <summary>
    /// 월드 좌표를 매개변수로 받아 해당 좌표의 타일을 리턴한다.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Volt_Tile GetTile(Vector3 pos)
    {
        Volt_Tile.TilePosInArena tilePos = GetTilePos(pos);
        return GetTile(tilePos.x, tilePos.y);
    }

    /// <summary>
    /// 월드좌표를 기준으로 direction쪽으로 distance만큼 떨어진 곳에 있는 타일을 리턴한다.
    /// 단, 맵 밖을 벗어날 경우 제일 끝 타일을 리턴한다.
    /// (0,0), (8,8), (8,0), (0,8) 이 4개의 타일은 접근불가 타일이므로 direction을 기준으로 한 칸 전의 타일을 리턴한다.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public Volt_Tile GetTile(Vector3 pos, Vector3 direction, int distance = 1)
    {
        Volt_Tile startTile = GetTile(pos);
        Volt_Tile targetTile = distance == 0 ? startTile : GetTile(startTile, direction, distance);

        return targetTile;
    }
    /// <summary>
    /// startTile을 기준으로 direction 방향으로 distance만큼 떨어진 곳에 있는 타일을 리턴한다.
    /// 단, 맵 밖을 벗어날 경우 제일 끝 타일을 리턴한다.
    /// (0,0), (8,8), (8,0), (0,8) 이 4개의 타일은 접근불가 타일이므로 direction을 기준으로 한 칸 전의 타일을 리턴한다.
    /// </summary>
    /// <param name="startTile"></param>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    private Volt_Tile GetTile(Volt_Tile startTile, Vector3 direction, int distance = 1)
    {
        Vector2Int dir2D = Util.ConvertWorldDirToTileDir(direction);
        Vector2Int startTilePos = new Vector2Int(startTile.tilePosInArray.x, startTile.tilePosInArray.y);
        Vector2Int destTilePos = startTilePos + dir2D * distance;

        while (destTilePos.x > 8 || destTilePos.x < 0 ||
            destTilePos.y > 8 || destTilePos.y < 0 ||
            this.tileArray[destTilePos.x, destTilePos.y].pTileType == Volt_Tile.TileType.none)
        {
            destTilePos -= dir2D;
        }
        //destTilePos.x = Mathf.Min(8, destTilePos.x);
        //destTilePos.x = Mathf.Max(0, destTilePos.x);
        //destTilePos.y = Mathf.Min(8, destTilePos.y);
        //destTilePos.y = Mathf.Max(0, destTilePos.y);

        //while (this.tileArray[destTilePos.x, destTilePos.y].pTileType == Volt_Tile.TileType.none)
        //{
        //    destTilePos -= dir2D;
        //}

        return this.tileArray[destTilePos.x, destTilePos.y];
    }


    /// <summary>
    /// 월드좌표와 방향 그리고 거리를 입력받아 월드좌표에 있는 타일을 시작으로 direction 방향으로 distance만큼 떨어진 타일까지
    /// 포함하여 distance 거리 안에 있는 모든 타일들을 반환한다.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>

    public Volt_Tile[] GetTiles(int startTileIdx, int endTileIdx)
    {
        Volt_Tile startTile = GetTileByIdx(startTileIdx);
        Volt_Tile endTile = GetTileByIdx(endTileIdx);
        Vector3 direction = (endTile.transform.position - startTile.transform.position);
        int distance = GetDistanceBetweenTiles(startTile, endTile);

        return GetTiles(startTile, direction, distance);
    }

    /// <summary>
    /// 출발 타일과 도착 타일까지의 타일들을 반환한다.(출발 타일은 제외되고, 도착 타일은 포함된다.)
    /// </summary>
    /// <param name="startTile"></param>
    /// <param name="endTile"></param>
    /// <returns></returns>
    public Volt_Tile[] GetTiles(Volt_Tile startTile, Volt_Tile endTile)
    {
        Vector3 direction = (endTile.transform.position - startTile.transform.position);
        int distance = GetDistanceBetweenTiles(startTile, endTile);

        return GetTiles(startTile, direction, distance);
    }

    public Volt_Tile[] GetTiles(Vector3 pos, Vector3 direction, int distance)
    {
        Volt_Tile tile = GetTile(pos);
        return GetTiles(tile, direction, distance);
    }

    /// <summary>
    /// 타일과 방향 그리고 거리를 입력받아 월드좌표에 있는 타일을 시작으로 direction 방향으로 distance만큼 떨어진 타일까지
    /// 포함하여 distance 거리 안에 있는 모든 타일들을 반환한다.
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public Volt_Tile[] GetTiles(Volt_Tile tile, Vector3 direction, int distance)
    {
        //Volt_Tile.TilePosInArena tilePos = tile.tilePosInArray;
        List<Volt_Tile> tiles = new List<Volt_Tile>();

        if (distance == 0)
        {
            tiles.Add(tile);
            return tiles.ToArray();
        }

        for (int dist = 1; dist <= distance; ++dist)
        {
            Volt_Tile item = GetTile(tile, direction, dist);
            if (tiles.Contains(item))
                continue;

            tiles.Add(item);
        }

        if (tiles.Count == 0)
            return new Volt_Tile[1] { tile };

        return tiles.ToArray();
    }

    public Volt_Tile GetTileByIdx(int idx)
    {
        return tileObjList[idx];
    }

    public Volt_Tile[] GetTilesUsingDirection(Vector3 pos, Vector3 dir, int numOfTiles)
    {
        Volt_Tile.TilePosInArena tilePos = GetTilePos(pos);

        List<Volt_Tile> tiles = new List<Volt_Tile>();

        for (int i = 0; i < numOfTiles; i++)
        {
            if (Volt_Utils.IsForward(dir))
            {
                if (tilePos.x >= 8)
                    continue;
                tilePos.x++;
            }
            else if (Volt_Utils.IsForwardRight(dir))
            {
                if (tilePos.x >= 8 || tilePos.y >= 8)
                    continue;
                tilePos.x++;
                tilePos.y++;
            }
            else if (Volt_Utils.IsRight(dir))
            {
                if (tilePos.y >= 8)
                    continue;
                tilePos.y++;
            }
            else if (Volt_Utils.IsBackRight(dir))
            {
                if (tilePos.y >= 8 || tilePos.x <= 0)
                    continue;
                tilePos.x--;
                tilePos.y++;
            }
            else if (Volt_Utils.IsBackward(dir))
            {
                if (tilePos.x <= 0)
                    continue;
                tilePos.x--;
            }
            else if (Volt_Utils.IsBackLeft(dir))
            {
                if (tilePos.x <= 0 || tilePos.y <= 0)
                    continue;
                tilePos.x--;
                tilePos.y--;
            }
            else if (Volt_Utils.IsLeft(dir))
            {
                if (tilePos.y <= 0)
                    continue;
                tilePos.y--;
            }
            else if (Volt_Utils.IsForwardLeft(dir))
            {
                if (tilePos.x >= 8 || tilePos.y <= 0)
                    continue;
                tilePos.x++;
                tilePos.y--;
            }
            if (IsPhillar(tilePos))
                continue;
            tiles.Add(this.tileArray[tilePos.x, tilePos.y]);
        }
        return tiles.ToArray();
    }
    public Volt_Tile[] GetDiagonalTiles(Vector3 pos, int range)
    {
        List<Volt_Tile> diagonalTiles = new List<Volt_Tile>();
        foreach (var item in Volt_ArenaSetter.S.GetTilesUsingDirection(pos, (Vector3.forward + Vector3.right).normalized, range))
        {
            diagonalTiles.Add(item);
        }
        foreach (var item in Volt_ArenaSetter.S.GetTilesUsingDirection(pos, (Vector3.forward + Vector3.left).normalized, range))
        {
            diagonalTiles.Add(item);
        }
        foreach (var item in Volt_ArenaSetter.S.GetTilesUsingDirection(pos, (Vector3.back + Vector3.left).normalized, range))
        {
            diagonalTiles.Add(item);
        }
        foreach (var item in Volt_ArenaSetter.S.GetTilesUsingDirection(pos, (Vector3.back + Vector3.right).normalized, range))
        {
            diagonalTiles.Add(item);
        }
        return diagonalTiles.ToArray();
    }
    public Volt_Tile[] GetCrossTiles(Vector3 pos, int range)
    {
        List<Volt_Tile> crossTiles = new List<Volt_Tile>();
        foreach (var item in Volt_ArenaSetter.S.GetTilesUsingDirection(pos, Vector3.forward, range))
        {
            crossTiles.Add(item);
        }
        foreach (var item in Volt_ArenaSetter.S.GetTilesUsingDirection(pos, Vector3.left, range))
        {
            crossTiles.Add(item);
        }
        foreach (var item in Volt_ArenaSetter.S.GetTilesUsingDirection(pos, Vector3.back, range))
        {
            crossTiles.Add(item);
        }
        foreach (var item in Volt_ArenaSetter.S.GetTilesUsingDirection(pos, Vector3.right, range))
        {
            crossTiles.Add(item);
        }
        return crossTiles.ToArray();
    }

    public Volt_Tile GetForwardTile(Transform transform)
    {
        return GetForwardTile(transform.position, transform.forward);
    }

    public Volt_Tile GetForwardTile(Vector3 pos, Vector3 forward)
    {
        Volt_Tile.TilePosInArena tilePos = GetTilePos(pos);
        int x = tilePos.x;
        int y = tilePos.y;

        if (Volt_Utils.IsForward(forward))
        {
            if (tilePos.x >= 8)
                return null;
            tilePos.x++;
        }
        else if (Volt_Utils.IsForwardRight(forward))
        {
            if (tilePos.x >= 8 || tilePos.y >= 8)
                return null;
            tilePos.x++;
            tilePos.y++;
        }
        else if (Volt_Utils.IsRight(forward))
        {
            if (tilePos.y >= 8)
                return null;
            tilePos.y++;
        }
        else if (Volt_Utils.IsBackRight(forward))
        {
            if (tilePos.y >= 8 || tilePos.x <= 0)
                return null;
            tilePos.x--;
            tilePos.y++;
        }
        else if (Volt_Utils.IsBackward(forward))
        {
            if (tilePos.x <= 0)
                return null;
            tilePos.x--;
        }
        else if (Volt_Utils.IsBackLeft(forward))
        {
            if (tilePos.x <= 0 || tilePos.y <= 0)
                return null;
            tilePos.x--;
            tilePos.y--;
        }
        else if (Volt_Utils.IsLeft(forward))
        {
            if (tilePos.y <= 0)
                return null;
            tilePos.y--;
        }
        else if (Volt_Utils.IsForwardLeft(forward))
        {
            if (tilePos.x >= 8 || tilePos.y <= 0)
                return null;
            tilePos.x++;
            tilePos.y--;
        }

        if (IsPhillar(tilePos))
            return null;

        return tileArray[tilePos.x, tilePos.y];
    }
    public Volt_Tile[] GetForwardAllTile(Vector3 pos, Vector3 forward)
    {
        Volt_Tile.TilePosInArena boardPos = GetTilePos(pos);
        int x = (int)boardPos.x;
        int y = (int)boardPos.y;
        List<Volt_Tile> tileList = new List<Volt_Tile>();

        if (Volt_Utils.IsForward(forward))
        {
            for (int i = tileArray.GetLength(0) - 1; i > x; --i)
            {
                if (IsPhillar(i, y))
                    break;
                tileList.Add(tileArray[i, y]);
            }
        }
        else if (Volt_Utils.IsForwardRight(forward))
        {
            int i = x + 1;
            int j = y + 1;

            while (i < row && j < colum)
            {
                if (IsPhillar(i, j))
                    break;
                tileList.Add(tileArray[i, j]);
                i++;
                j++;
            }
        }

        else if (Volt_Utils.IsRight(forward))
        {
            for (int i = tileArray.GetLength(1) - 1; i > y; i--)
            {
                if (IsPhillar(x, i))
                    break;
                tileList.Add(tileArray[x, i]);
            }
        }

        else if (Volt_Utils.IsBackRight(forward))
        {
            int i = x - 1;
            int j = y + 1;
            while (i >= 0 && j < colum)
            {
                if (IsPhillar(i, j))
                    break;
                tileList.Add(tileArray[i, j]);
                i--;
                j++;
            }
        }

        else if (Volt_Utils.IsBackward(forward))
        {
            for (int i = 0; i < x; ++i)
            {
                if (IsPhillar(i, y))
                    break;
                tileList.Add(tileArray[i, y]);
            }
        }

        else if (Volt_Utils.IsBackLeft(forward))
        {
            int i = x - 1;
            int j = y - 1;
            while (i >= 0 && j >= 0)
            {
                if (IsPhillar(i, j))
                    break;
                tileList.Add(tileArray[i, j]);
                i--;
                j--;
            }
        }

        else if (Volt_Utils.IsLeft(forward))
        {
            for (int i = 0; i < y; i++)
            {
                if (IsPhillar(x, i))
                    break;
                tileList.Add(tileArray[x, i]);
            }
        }

        else if (Volt_Utils.IsForwardLeft(forward))
        {
            int i = x + 1;
            int j = y - 1;
            while (i < row && j >= 0)
            {
                if (IsPhillar(i, j))
                    break;
                tileList.Add(tileArray[i, j]);
                i++;
                j--;
            }
        }
        return tileList.ToArray();
    }

    /// <summary>
    /// 월드 좌표와 방향을 인자값으로 받아와, 월드 좌표에있는 타일을 기준으로 direction쪽으로 존재하는 모든 타일을 받아온다.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Volt_Tile[] GetAllTiles(Vector3 pos, Vector3 direction)
    {
        Volt_Tile tile = GetTile(pos);
        return GetAllTiles(tile, direction);
    }

    /// <summary>
    /// 타일과 방향을 인자값으로 받아와, 타일을 기준으로 direction쪽으로 존재하는 모든 타일을 받아온다.
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Volt_Tile[] GetAllTiles(Volt_Tile tile, Vector3 direction)
    {
        Volt_Tile.TilePosInArena boardPos = tile.tilePosInArray;
        int x = (int)boardPos.x;
        int y = (int)boardPos.y;
        List<Volt_Tile> tileList = new List<Volt_Tile>();

        if (Volt_Utils.IsForward(direction))
        {
            for (int i = tileArray.GetLength(0) - 1; i > x; --i)
            {
                if (IsPhillar(i, y))
                    break;
                tileList.Add(tileArray[i, y]);
            }
        }
        else if (Volt_Utils.IsForwardRight(direction))
        {
            int i = x + 1;
            int j = y + 1;

            while (i < row && j < colum)
            {
                if (IsPhillar(i, j))
                    break;
                tileList.Add(tileArray[i, j]);
                i++;
                j++;
            }
        }

        else if (Volt_Utils.IsRight(direction))
        {
            for (int i = tileArray.GetLength(1) - 1; i > y; i--)
            {
                if (IsPhillar(x, i))
                    break;
                tileList.Add(tileArray[x, i]);
            }
        }

        else if (Volt_Utils.IsBackRight(direction))
        {
            int i = x - 1;
            int j = y + 1;
            while (i >= 0 && j < colum)
            {
                if (IsPhillar(i, j))
                    break;
                tileList.Add(tileArray[i, j]);
                i--;
                j++;
            }
        }

        else if (Volt_Utils.IsBackward(direction))
        {
            for (int i = 0; i < x; ++i)
            {
                if (IsPhillar(i, y))
                    break;
                tileList.Add(tileArray[i, y]);
            }
        }

        else if (Volt_Utils.IsBackLeft(direction))
        {
            int i = x - 1;
            int j = y - 1;
            while (i >= 0 && j >= 0)
            {
                if (IsPhillar(i, j))
                    break;
                tileList.Add(tileArray[i, j]);
                i--;
                j--;
            }
        }

        else if (Volt_Utils.IsLeft(direction))
        {
            for (int i = 0; i < y; i++)
            {
                if (IsPhillar(x, i))
                    break;
                tileList.Add(tileArray[x, i]);
            }
        }

        else if (Volt_Utils.IsForwardLeft(direction))
        {
            int i = x + 1;
            int j = y - 1;
            while (i < row && j >= 0)
            {
                if (IsPhillar(i, j))
                    break;
                tileList.Add(tileArray[i, j]);
                i++;
                j--;
            }
        }
        return tileList.ToArray();
    }

    public Volt_Tile.TilePosInArena GetTilePos(Volt_Tile tile)
    {
        Volt_Tile.TilePosInArena pos;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (tileArray[i, j] == tile)
                {
                    pos.x = i;
                    pos.y = j;
                    return pos;
                }
            }
        }
        pos.x = 99;
        pos.y = 99;
        return pos;
    }

    // 추가된 함수
    // GetTilePos 함수의 bot의 worldPosition을 매개변수로 받는 버전
    public Volt_Tile.TilePosInArena GetTilePos(Vector3 pos)
    {
        Volt_Tile.TilePosInArena tilePos = new Volt_Tile.TilePosInArena();
        tilePos.x = 0; tilePos.y = 0;

        float halfWidth = DistanceBetweenTiles / 2;
        bool flag = false;
        for (int i = 0; i < tileArray.GetLength(0); i++)
        {
            for (int j = 0; j < tileArray.GetLength(1); j++)
            {
                Vector3 tileMin = tileArray[i, j].transform.position +
                    (Vector3.left + Vector3.back) * halfWidth;
                Vector3 tileMax = tileArray[i, j].transform.position +
                    (Vector3.right + Vector3.forward) * halfWidth;

                if ((pos.x >= tileMin.x && pos.x <= tileMax.x) &&
                    (pos.z >= tileMin.z && pos.z <= tileMax.z))
                {
                    tilePos = tileArray[i, j].tilePosInArray;
                    flag = true;
                    break;
                }
            }
            if (flag)
                break;
        }
        return tilePos;
    }
    // 추가된 함수
    // 한 칸 앞으로 이동이 가능한지 판별 후 참, 거짓을 반환한다.
    public bool IsCanMoveToNextTile(Transform transform)
    {
        return IsCanMoveToNextTile(transform.position, transform.forward);
    }

    public bool IsCanMoveToNextTile(Vector3 pos, Vector3 forward)
    {
        Volt_Tile nextTile = GetForwardTile(pos, forward);

        if (nextTile != null && nextTile.pTileType != Volt_Tile.TileType.pits &&
            nextTile.pTileType != Volt_Tile.TileType.none)
            return true;
        return false;
    }
    // 추가된 함수
    // 조건 1.반드시 맵은 홀수x홀수 일 것! 2.가운데 타일들에는 구덩이가 없을 것!
    // 현재 보드게임의 맵들은 모두 위의 조건을 충족하고 있음
    // 함수 호출 조건 : 킬봇이 현재 서있는 타일에서 한 칸 더 앞으로 갈 수 없는 경우
    // ex. 다음 타일이 없을 때, 앞에 구덩이가 있을 때
    // 타일에 표시된 방향으로 회전은 다음을 기준으로 방향이 정해진다.
    // 맵을 5 구역으로 나누어 각 구역에서 봇의 진행 방향에 따라 봇이 회전하는 방향이 정해진다.
    // 5 구역은 중앙, 1사분면, 2사분면, 3사분면, 4사분면으로 구성된다.
    /*  (|=타일)
     |       |  
     | 2     | 1사분면 구역
     |       |
     |       |
     | | | | | | | | |  -> 각 구역을 나눠주는 축이 되어주는 타일들(중앙 구역)
     |       |
     | 3     | 4
     |       |
     |       |
     */
    /// <summary>
    /// 맵을 5 구역(중앙, 1사분면, 2사분면, 3사분면, 4사분면)으로 나눈 후
    /// 봇이 현재 서 있는 타일이 어느 구역인지를 반환한다.
    /// </summary>
    /// <param name="tile"></param>
    /// <returns>맵의 구역</returns>

    public ArenaSection GetSection(Volt_Tile tile)
    {
        Volt_Tile.TilePosInArena tilePos = GetTilePos(tile);

        int firstIdx = 0;
        int midIdx = tileArray.GetLength(0) / 2;
        int lastIdx = tileArray.GetLength(0) - 1;

        // if current tile is 3 qudrant.
        if (tilePos.x >= firstIdx && tilePos.x <= midIdx - 1 &&
            tilePos.y >= firstIdx && tilePos.y <= midIdx - 1)
        {
            return ArenaSection.ThreeQudrant;
        }
        // if current tile is 4 qudrant.
        else if (tilePos.x >= firstIdx && tilePos.x <= midIdx - 1 &&
          tilePos.y >= midIdx + 1 && tilePos.y <= lastIdx)
        {
            return ArenaSection.FourQudrant;
        }
        // if current tile is 2 qudrant.
        else if (tilePos.x >= midIdx + 1 && tilePos.x <= lastIdx &&
          tilePos.y >= firstIdx && tilePos.y <= midIdx - 1)
        {
            return ArenaSection.TwoQudrant;
        }
        // if current tile is 1 qudrant.
        else if (tilePos.x >= midIdx + 1 && tilePos.x <= lastIdx &&
          tilePos.y >= midIdx + 1 && tilePos.y <= lastIdx)
        {
            return ArenaSection.OneQudrant;
        }
        // if current tile is mid.
        else if (tilePos.x == midIdx && tilePos.y == midIdx) // &&로 바꾸고...?
        {
            return ArenaSection.Center;
        }
        else if (tilePos.x == midIdx && tilePos.y < midIdx)
        {
            return ArenaSection.CenterLeft;
        }
        else if (tilePos.x == midIdx && tilePos.y > midIdx)
        {
            return ArenaSection.CenterRight;
        }
        else if (tilePos.x < midIdx && tilePos.y == midIdx)
        {
            return ArenaSection.CenterBack;
        }
        else if (tilePos.x > midIdx && tilePos.y == midIdx)
        {
            return ArenaSection.CenterTop;
        }
        else
        {
            //Debug.Log("Error you stand wrong tile ");
            //Debug.Log("Tile[" + tilePos.x + "][" + tilePos.y + "]");
            return ArenaSection.None;
        }
    }

    /// <summary>
    /// 맵을 5 구역(중앙, 1사분면, 2사분면, 3사분면, 4사분면)으로 나눈 후
    /// 봇이 현재 서 있는 타일이 어느 구역인지를 반환한다.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>맵의 구역</returns>
    public ArenaSection GetSection(Vector3 pos)
    {
        Volt_Tile tile = GetTile(pos);
        return GetSection(tile);
    }

    // 추가된 함수
    // 로봇이 현재 서있는 타일이 가장자리에 있는 타일인지 확인한다.
    public bool IsOnEdgeTile(Vector3 pos)
    {
        Volt_Tile.TilePosInArena tilePos = GetTilePos(pos);
        int x = tilePos.x;
        int y = tilePos.y;

        if (x == 0 || x == tileArray.GetLength(0) - 1 ||
            y == 0 || y == tileArray.GetLength(1) - 1)
            return true;
        return false;
    }

    public bool IsEdgeTile(Volt_Tile tile)
    {
        Volt_Tile.TilePosInArena tilePos = tile.tilePosInArray;

        if (tilePos.x == 0 || tilePos.x == tileArray.GetLength(0) - 1 ||
            tilePos.y == 0 || tilePos.y == tileArray.GetLength(1) - 1)
            return true;
        return false;
    }
    public Transform GetCenterTransform()
    {
        return GetTile(4, 4).transform;
    }

    public bool IsCanMoveNextTile(Vector3 pos, Vector3 dir)
    {
        Volt_Tile curTile = GetTile(pos);
        Volt_Tile nextTile = GetTile(pos, dir);


        // 가장자리 타일
        if (curTile == nextTile)
        {
            //Debug.Log($"curTile:{curTile.name}, nextTile:{nextTile.name}, dir:{dir}, ###################가장자리");
            return false;
        }
        RayController rayController = new RayController(pos, dir, 1.41f, LayerMask.GetMask("Wall"), "Wall");
        if (rayController.Raycast())
        {
            //Debug.Log("#######################벽");
            return false;
        }

        if (curTile.pTileType == Volt_Tile.TileType.none ||
            nextTile.pTileType == Volt_Tile.TileType.none)
        {
            //Debug.Log("##################타일 타입이 None");
            return false;
        }

        return true;
    }

    private void SetBombCandidateTiles(ref List<Volt_Tile> candidateTiles)
    {
        //Debug.Log("SetBombCandidateTiles");
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (tileArray[i, j].pTileType == Volt_Tile.TileType.none)
                    continue;
                candidateTiles.Add(tileArray[i, j]);
            }
        }

        foreach (var robot in robotsInArena)
        {
            Volt_Tile robotStandTile = GetTile(robot.transform.position);
            for (int i = 0; i < 5; i++)
            {
                candidateTiles.Add(robotStandTile);
            }

            for (int i = 0; i < 5; i++)
            {
                List<Volt_Tile> robotAdjTiles = new List<Volt_Tile>(robotStandTile.GetAdjecentTiles());

                for (int j = 0; j < robotAdjTiles.Count; j++)
                {
                    if (robotAdjTiles[j] == null) continue;
                    if (robotAdjTiles[j].pTileType == Volt_Tile.TileType.none)
                        continue;

                    candidateTiles.AddRange(robotAdjTiles);
                }
            }
        }

        int shuffleCount = candidateTiles.Count * 2;
        for (int i = 0; i < shuffleCount; i++)
        {
            int idx1 = Random.Range(0, candidateTiles.Count);
            int idx2 = Random.Range(0, candidateTiles.Count);

            Volt_Tile tmpTile = candidateTiles[idx1];
            candidateTiles[idx1] = candidateTiles[idx2];
            candidateTiles[idx2] = tmpTile;
        }
    }

    public List<Volt_Tile> candidateTiles = new List<Volt_Tile>();
    public Volt_Tile SearchBombSite()
    {
        //Debug.Log("SearchBombSite");
        this.candidateTiles = new List<Volt_Tile>();
        SetBombCandidateTiles(ref this.candidateTiles);
        try
        {
            //Debug.Log(candidateTiles.Count);
            Volt_Tile bombSite = this.candidateTiles[0];
            if (bombSite == null)
            {
                //Debug.Log("bombSite null: " +  bombSite.gameObject.name);
            }
            //Debug.Log(string.Format("BombSite tileIndex:{0}, x:{1}, y:{2}",
            //    bombSite.name, bombSite.tilePosInArray.x, bombSite.tilePosInArray.y));

            return bombSite;
        }
        catch (System.Exception ex)
        {
            //Debug.LogError("SearchBombSite Error!! [" + ex.Message + "]");
            return null;
        }
    }
    public bool IsAllTileAnimationDone(Volt_Tile[] animationTiles)
    {
        foreach (var item in animationTiles)
        {
            if (item.isAnimationPlay)
                return false;
        }
        return true;
    }
    public List<Volt_Tile> GetSuddenDeathTile(int option)
    {
        //option = option % 12;
        option = option % 6;
        List<int> selectedNumbers = new List<int>();
       
        
        //14,20,34,46,60,66
        switch (option)
        {
            case 0:
                selectedNumbers = AddValueAndReturnList(14);
                break;
            case 1:
                selectedNumbers = AddValueAndReturnList(20);
                break;
            case 2:
                selectedNumbers = AddValueAndReturnList(34);
                break;
            case 3:
                selectedNumbers = AddValueAndReturnList(46);
                break;
            case 4:
                selectedNumbers = AddValueAndReturnList(60);
                break;
            case 5:
                selectedNumbers = AddValueAndReturnList(66);
                break;
            default:
                //Debug.Log("GetSuddenDeathTile Error");
                break;
        }
        List<Volt_Tile> targetTiles = new List<Volt_Tile>();
        foreach (var item in selectedNumbers)
        {
            targetTiles.Add(GetTileByIdx(item));
        }
        return targetTiles;
    }

    List<int> AddValueAndReturnList(int idx1)//,int idx2,int idx3)
    {
        List<int> selectedNumbers = new List<int>();
        selectedNumbers.Add(idx1);
        //selectedNumbers.Add(idx2);
        //selectedNumbers.Add(idx3);
        return selectedNumbers;
    }
    List<Volt_Tile> GetTilesRobotStanding()
    {
        List<Volt_Tile> tiles = new List<Volt_Tile>();
        foreach (var item in robotsInArena)
        {
            tiles.Add(GetTile(item.transform.position));
        }
        return tiles;
    }
    public Volt_Tile GetRandomTileToPlace(List<Volt_Tile> candidateTiles)
    {
        List<Volt_Tile> tmpTiles = new List<Volt_Tile>(candidateTiles);
        Volt_Tile candidateTile;
        int tmpTileCount = tmpTiles.Count;
        for (int i = 0; i < tmpTileCount; i++)
        {
            candidateTile = tmpTiles[Random.Range(0, tmpTiles.Count)];
            if (candidateTile.GetRobotInTile() == null)
            {
                //Debug.Log($"return {candidateTile.tileIndex}");
                return candidateTile;
            }
            else
                tmpTiles.Remove(candidateTile);
        }

        //Debug.LogError(candidateTiles.Count +" 씹벌");
        Vector3 direction;
        switch (candidateTiles[(int)(candidateTiles.Count / 2)].arenaSection)
        {
            case ArenaSection.CenterTop:
                //Debug.Log("Center Top!!");
                direction = Vector3.back;
                break;
            case ArenaSection.CenterBack:
                //Debug.Log("Center Back!!");
                direction = Vector3.forward;
                break;
            case ArenaSection.CenterLeft:
                //Debug.Log("Center Left!!");
                direction = Vector3.right;
                break;
            case ArenaSection.CenterRight:
                //Debug.Log("Center Right!!");
                direction = Vector3.left;
                break;
            default:
                direction = Vector3.zero;
                //Debug.Log("AutoRobotSetup Error!!");
                break;
        }
        Volt_Tile exceptTile = GetTile(candidateTiles[candidateTiles.Count / 2].transform.position, direction, 1);
        //Debug.Log($"return {exceptTile.tileIndex}");
        return exceptTile;

    }
    public bool IsAllTileBlinkOff()
    {
        foreach (var item in tileArray)
        {
            if (item.BlinkOn)
                return false;
        }
        return true;
    }
}