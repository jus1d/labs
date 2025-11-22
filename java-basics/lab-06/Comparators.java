import java.util.Comparator;

class SpaceMissionByProfitDesc implements Comparator<SpaceMission> {

    @Override
    public int compare(SpaceMission a, SpaceMission b) {
        try {
            return Integer.compare(
                b.calculateNetProfit(),
                a.calculateNetProfit()
            );
        } catch (MissionBusinessException e) {
            throw new RuntimeException(e);
        }
    }
}

class SpaceMissionByCostAsc implements Comparator<SpaceMission> {

    @Override
    public int compare(SpaceMission a, SpaceMission b) {
        return Integer.compare(a.getCostPerUnit(), b.getCostPerUnit());
    }
}
