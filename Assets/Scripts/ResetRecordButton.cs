using UnityEngine;

public class ResetRecordButton : MonoBehaviour
{
    [SerializeField] DisplayRecord displayRecord;
    [SerializeField] DisplayAcheivement displayAcheivement;

    public void ResetRecord()
    {
        DataManager.Instance.ResetData();

        displayRecord.Display();
        displayAcheivement.Display();
    }
}
