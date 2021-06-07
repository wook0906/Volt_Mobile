public class InfoShop
{
    public int ID;
    public int priceAssetType; // 가격 지불 타입(골드 or 다이아몬드)
    public int price;
    public int count;   //상품 갯수
    
    public InfoShop(int ID, int assetType, int price, int count)
    {
        this.ID = ID;
        this.priceAssetType = assetType;
        this.price = price;
        this.count = count;
    }

    public override string ToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append("AssetType: " + this.priceAssetType);
        builder.Append(" /Price: " + this.price);
        builder.Append(" /Count: " + this.count);

        return builder.ToString();
    }
}

public class InfoACHCondition
{
    public int ID;
    public int conditionType;
    public int condition; // 조건 값
    public int rewardType;
    public int reward;  // 보상 갯수
    public bool isAccomplish; // 보상 수령여부

    public InfoACHCondition(int ID, int conditionType, int condition, int rewardType, int reward)
    {
        this.ID = ID;
        this.conditionType = conditionType;
        this.condition = condition;
        this.rewardType = rewardType;
        this.reward = reward;
    }

    public override string ToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append("ConditionType: " + this.conditionType);
        builder.Append(" /Condition: " + this.condition);
        builder.Append(" /RewardType: " + this.rewardType);
        builder.Append(" /Reward: " + this.reward);

        return builder.ToString();
    }
}

