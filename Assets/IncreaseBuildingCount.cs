using UnityEngine;

public class IncreaseBuildingCount : MonoBehaviour
{
    [Header("참조")]
    public CountGrowingBuilding countGrowingBuilding; // 건설 제한 관리
    public CountMoney countMoney; // 재화 관리

    [Header("설정")]
    public int maxUseCount = 5; // 최대 사용 횟수 (나중에 변경 가능)
    public int baseCost = 100;  // 첫 사용시 재화 소모량
    public int costIncrement = 200; // 두번째부터 증가분

    private int useCount = 0; // 현재까지 사용 횟수

    /// <summary>
    /// 버튼 클릭 시 호출
    /// </summary>
    public void OnButtonClick()
    {
        if (useCount >= maxUseCount)
        {
            //Debug.Log("더 이상 사용할 수 없습니다.");
            MessageManager.Instance.ShowMessage("더 이상 사용할 수 없습니다.");
            return;
        }

        int cost = GetCurrentCost();
        if (countMoney.currentMoney < cost)
        {
            //Debug.Log("재화가 부족합니다.");
            MessageManager.Instance.ShowMessage("재화가 부족합니다.");
            return;
        }

        // 재화 차감
        countMoney.AddMoney(-cost);

        // 건설 제한 증가
        countGrowingBuilding.maxCount += 1;
        countGrowingBuilding.AddCurrentCount(1); // 현재 카운트도 1 증가

        useCount++;
        //Debug.Log($"건설 제한이 {countGrowingBuilding.maxCount}로 증가! (남은 사용 가능 횟수: {maxUseCount - useCount})");
        MessageManager.Instance.ShowMessage($"건설 제한이 {countGrowingBuilding.maxCount}로 증가! (남은 사용 가능 횟수: {maxUseCount - useCount})");

        
    }

    /// <summary>
    /// 현재 사용 시 필요한 재화 계산 - 첫 사용은 baseCost, 이후는 costIncrement씩 증가
    /// 추후 공식 대입 가능
    /// </summary>
    public int GetCurrentCost()
    {
        return baseCost + costIncrement * useCount;
    }

    /// <summary>
    /// 남은 사용 가능 횟수 반환
    /// </summary>
    public int GetRemainCount()
    {
        return maxUseCount - useCount;
    }
}