using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public enum ShopType
        {
            BatteryShop,
            DiamondShop,
            FrameDecorationShop,
            RobotSkinShop
        }

        public enum PriceType
        {
            Gold,
            Diamond,
            Cash,
            End
        }

        public enum ObjectType
        {
            Package,
            Battery,
            Diamond,
            Gold,
            edge,
            Skin,
            Emoticon,
            End
        }

        public enum ShopRobotSelectType
        {
            Mercury,
            Volt,
            Hound,
            Reaper,
            SkinType_End
        }
        


        public class ShopDataManager : MonoBehaviour
        {
            
            public static ShopDataManager instance;

            public PackageShopTable packageShop;
            public BatteryShopTable batteryShop;
            public DiamondShopTable diamondShop;
            public GoldShopTable goldShop;
            public FrameDecorationShopTable frameDecorationShop;
            public RobotSkinShopTable robotSkinShop;
            public EmoticonShopTable emoticonShop;

            private Dictionary<int, PackageShopModel> packageShopItemTable = new Dictionary<int, PackageShopModel>();
            private Dictionary<int, BatteryShopModel> batteryShopItemTable = new Dictionary<int, BatteryShopModel>();
            private Dictionary<int, DiamondShopModel> diamondShopItemTable = new Dictionary<int, DiamondShopModel>();
            private Dictionary<int, GoldShopModel> goldShopItemTable = new Dictionary<int, GoldShopModel>();
            private Dictionary<int, FrameDecorationShopModel> frameDecorationShopItemTable = new Dictionary<int, FrameDecorationShopModel>();
            private Dictionary<ShopRobotSelectType, RobotSkinShopModel[]> robotSkinShopItemTable = new Dictionary<ShopRobotSelectType, RobotSkinShopModel[]>();
            private Dictionary<ShopRobotSelectType, EmoticonShopModel[]> emoticonShopItemTable = new Dictionary<ShopRobotSelectType, EmoticonShopModel[]>();

            private bool isChargedAd = false;
            public bool IsChargedAd
            {
                get { return isChargedAd; }
                set { isChargedAd = value; }
            }

            private void Awake()
            {
                if (instance == null)
                    instance = this;
                else
                    Destroy(this.gameObject);
            }

            public void Init()
            {
                PackageShopTable_Decrypt packageDecrypt = new PackageShopTable_Decrypt();
                this.packageShop = packageDecrypt.obj.Clone() as PackageShopTable;
                packageDecrypt = null;

                BatteryShopTable_Decrypt batteryDecrypt = new BatteryShopTable_Decrypt();
                this.batteryShop = batteryDecrypt.obj.Clone() as BatteryShopTable;
                batteryDecrypt = null;

                DiamondShopTable_Decrypt diamondDecrypt = new DiamondShopTable_Decrypt();
                this.diamondShop = diamondDecrypt.obj.Clone() as DiamondShopTable;
                diamondDecrypt = null;

                GoldShopTable_Decrypt goldDecrypt = new GoldShopTable_Decrypt();
                this.goldShop = goldDecrypt.obj.Clone() as GoldShopTable;
                goldDecrypt = null;

                FrameDecorationShopTable_Decrypt frameDecoDecrypt = new FrameDecorationShopTable_Decrypt();
                this.frameDecorationShop = frameDecoDecrypt.obj.Clone() as FrameDecorationShopTable;
                frameDecoDecrypt = null;

                RobotSkinShopTable_Decrypt robotSkinDecrypt = new RobotSkinShopTable_Decrypt();
                this.robotSkinShop = robotSkinDecrypt.obj.Clone() as RobotSkinShopTable;
                robotSkinDecrypt = null;

                EmoticonShopTable_Decrypt emoticonDecrypt = new EmoticonShopTable_Decrypt();
                this.emoticonShop = emoticonDecrypt.obj.Clone() as EmoticonShopTable;
                emoticonDecrypt = null;

                Dictionary<int, InfoShop> shopInfos = SystemInfoManager.instance.shopInfos;


                foreach (PackageShopTable.Sheet sheet in packageShop.sheets)
                {
                    
                    foreach (PackageShopTable.Param param in sheet.list)
                    {
                        InfoShop tShopInfo;
                        if (shopInfos.TryGetValue(param.ID, out tShopInfo))
                        {
                            packageShopItemTable.Add(param.ID, new PackageShopModel(param.ID,
                                param.objectName, tShopInfo.priceAssetType, tShopInfo.price, "Package",
                                tShopInfo.count, param.objectICON, param.priceICON, param.iconAtlas, param.getBuyButtonNormalSprite,
                                param.getBuyButtonPushedSprite, param.atlas, param.font, param.priceFontSize,
                                param.objectFontSize));
                            //Debug.Log(packageShopItemTable[param.ID].ToString());
                        }
                        else
                        {
                            Debug.Log($"Warning package shop info is null @ {param.ID}");
                        }
                    }
                }

                foreach (BatteryShopTable.Sheet sheet in this.batteryShop.sheets)
                {
                    foreach (BatteryShopTable.Param param in sheet.list)
                    {
                        InfoShop tShopInfo;
                        if (shopInfos.TryGetValue(param.ID, out tShopInfo))
                        {
                            batteryShopItemTable.Add(param.ID, new BatteryShopModel(param.ID,
                                param.objectName, tShopInfo.priceAssetType, tShopInfo.price, "Battery",
                                tShopInfo.count, param.objectICON, param.priceICON, param.iconAtlas, param.getBuyButtonNormalSprite,
                                param.getBuyButtonPushedSprite, param.atlas, param.font, param.priceFontSize,
                                param.objectFontSize));
                            //Debug.Log(batteryShopItemTable[param.ID].ToString());
                        }
                        else
                        {
                            Debug.Log($"Warning battery shop info is null @ {param.ID}");
                        }
                    }
                }

                foreach (DiamondShopTable.Sheet sheet in this.diamondShop.sheets)
                {
                    foreach (DiamondShopTable.Param param in sheet.list)
                    {
                        InfoShop tShopInfo;
                        if (shopInfos.TryGetValue(param.ID, out tShopInfo))
                        {
                            diamondShopItemTable.Add(param.ID, new DiamondShopModel(param.ID, param.objectName,
                                tShopInfo.priceAssetType, tShopInfo.price, "Diamond", tShopInfo.count,
                                param.objectICON, param.priceICON, param.iconAtlas, param.getBuyButtonNormalSprite,
                                param.getBuyButtonPushedSprite, param.atlas, param.font, param.priceFontSize,
                                param.objectFontSize));
                            //Debug.Log(diamondShopItemTable[param.ID].ToString());
                        }
                        else
                        {
                            //Debug.Log($"Warning diamond shop info is null @ {param.ID}");
                        }
                    }
                }

                foreach (GoldShopTable.Sheet sheet in this.goldShop.sheets)
                {
                    foreach (GoldShopTable.Param param in sheet.list)
                    {
                        InfoShop tShopInfo;
                        if (shopInfos.TryGetValue(param.ID, out tShopInfo))
                        {
                            goldShopItemTable.Add(param.ID, new GoldShopModel(param.ID,
                                param.objectName, tShopInfo.priceAssetType, tShopInfo.price, "Gold",
                                tShopInfo.count, param.objectICON, param.priceICON, param.iconAtlas, param.getBuyButtonNormalSprite,
                                param.getBuyButtonPushedSprite, param.atlas, param.font, param.priceFontSize,
                                param.objectFontSize));
                            //Debug.Log(goldShopItemTable[param.ID].ToString());
                        }
                        else
                        {
                            Debug.Log($"Warning Gold shop info is null @ {param.ID}");
                        }
                    }
                }

                //foreach (FrameDecorationShopTable.Sheet sheet in this.frameDecorationShop.sheets)
                //{
                //    foreach (FrameDecorationShopTable.Param param in sheet.list)
                //    {
                //        InfoShop tShopInfo;
                //        if (shopInfos.TryGetValue(param.ID, out tShopInfo))
                //        {
                //            frameDecorationShopItemTable.Add(param.ID, new FrameDecorationShopModel(param.ID, param.edgeName, tShopInfo.priceAssetType, tShopInfo.price, "edge", tShopInfo.count, param.priceICON, param.iconAtlas, param.edgeSprite, param.getBuyButtonNormalSprite, param.getBuyButtonPushedSprite, param.atlas, param.font, param.priceFontSize));
                //            //Debug.Log(frameDecorationShopItemTable[param.ID].ToString());
                //        }
                //        else
                //        {
                //            Debug.Log($"Warning frame deco shop info is null @ {param.ID}");
                //        }
                //    }
                //}

                for (int i = 0; i < (int)ShopRobotSelectType.SkinType_End; ++i)
                {
                    robotSkinShopItemTable.Add((ShopRobotSelectType)i, new RobotSkinShopModel[0]);
                }

                foreach (RobotSkinShopTable.Sheet sheet in this.robotSkinShop.sheets)
                {
                    foreach (RobotSkinShopTable.Param param in sheet.list)
                    {
                        InfoShop tShopInfo;
                        if (shopInfos.TryGetValue(param.ID, out tShopInfo))
                        {
                            RobotSkinShopModel[] robotSkinShopModels;
                            ShopRobotSelectType skinType = (ShopRobotSelectType)Enum.Parse(typeof(ShopRobotSelectType), param.skinType);
                            if (robotSkinShopItemTable.TryGetValue(skinType, out robotSkinShopModels))
                            {
                                int index = robotSkinShopModels.Length;
                                Array.Resize<RobotSkinShopModel>(ref robotSkinShopModels, index + 1);
                                robotSkinShopModels[index] = new RobotSkinShopModel(param.ID, param.skinName_KOR, param.skinName_EN, param.skinName_GER,
                                    param.skinName_Fren, tShopInfo.priceAssetType,
                                tShopInfo.price, param.skinType, "Skin",
                                param.skinModel, tShopInfo.count, param.priceICON, param.iconAtlas, param.skinSprite, param.skinAtlas,
                                param.BuyButtonName_KR, param.BuyButtonName_EN, param.BuyButtonName_GER, param.BuyButtonName_Fren, param.getBuyButtonNormalSprite,
                                param.getBuyButtonPushedSprite, param.atlas, param.font, param.priceFontSize, param.skinNameFontSize);
                                robotSkinShopItemTable[skinType] = robotSkinShopModels;
                            }
                        }
                        else
                        {
                            Debug.Log($"Warning skin shop info is null @ {param.ID}");
                        }
                    }
                }

                for (int i = 0; i < (int)ShopRobotSelectType.SkinType_End; ++i)
                {
                    emoticonShopItemTable.Add((ShopRobotSelectType)i, new EmoticonShopModel[0]);
                }
                foreach (EmoticonShopTable.Sheet sheet in this.emoticonShop.sheets)
                {
                    foreach (EmoticonShopTable.Param param in sheet.list)
                    {
                        InfoShop tShopInfo;
                        if (shopInfos.TryGetValue(param.ID, out tShopInfo))
                        {
                            EmoticonShopModel[] emoticonShopModels;
                            ShopRobotSelectType robotType = (ShopRobotSelectType)Enum.Parse(typeof(ShopRobotSelectType), param.robotType);
                            if (emoticonShopItemTable.TryGetValue(robotType, out emoticonShopModels))
                            {
                                int index = emoticonShopModels.Length;
                                Array.Resize<EmoticonShopModel>(ref emoticonShopModels, index + 1);
                                emoticonShopModels[index] = new EmoticonShopModel(param.ID, param.emoticonName_KOR, param.emoticonName_EN, param.emoticonName_GER,
                                    param.emoticonName_Fren, tShopInfo.priceAssetType,
                                tShopInfo.price, param.robotType, "Emoticon",
                                param.emoticonModel, tShopInfo.count, param.priceICON, param.iconAtlas, param.emoticonSprite, param.emoticonAtlas,
                                param.BuyButtonName_KR, param.BuyButtonName_EN, param.BuyButtonName_GER, param.BuyButtonName_Fren, param.getBuyButtonNormalSprite,
                                param.getBuyButtonPushedSprite, param.atlas, param.font, param.priceFontSize, param.emoticonNameFontSize);
                                emoticonShopItemTable[robotType] = emoticonShopModels;
                            }

                        }
                        else
                        {
                            Debug.Log($"Warning emoticon shop info is null @ {param.ID}");
                        }
                    }
                }

                //                Volt_ShopUIManager.S.ShopDataLoadDoneCallback();
            }

            public Dictionary<int, PackageShopModel> GetPackageShopItemTable()
            {
                return packageShopItemTable;
            }

            public Dictionary<int, BatteryShopModel> GetBatteryShopItemTable()
            {
                return batteryShopItemTable;
            }

            public Dictionary<int, DiamondShopModel> GetDiamondShopItemTable()
            {
                return diamondShopItemTable;
            }

            public Dictionary<int, GoldShopModel> GetGoldShopItemTable()
            {
                return goldShopItemTable;
            }

            public Dictionary<int, FrameDecorationShopModel> GetFrameDecorationShopItemTable()
            {
                return frameDecorationShopItemTable;
            }

            public Dictionary<ShopRobotSelectType, RobotSkinShopModel[]> GetRobotSkinShopItemTable()
            {
                return robotSkinShopItemTable;
            }

            public RobotSkinShopModel SearchRobotSkinShopModel(int id)
            {
                foreach (var skinShopModels in robotSkinShopItemTable.Values)
                {
                    foreach (var skinShopModel in skinShopModels)
                    {
                        if (skinShopModel.ID == id)
                            return skinShopModel;
                    }
                }
                return null;
            }
            public Dictionary<ShopRobotSelectType, EmoticonShopModel[]> GetEmoticonShopItemTable()
            {
                return emoticonShopItemTable;
            }
        }
    }
}