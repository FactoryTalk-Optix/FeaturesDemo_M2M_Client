#region Using directives
using UAManagedCore;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.Alarm;
using FTOptix.OPCUAServer;
#endregion

public class AlarmGridLogic : BaseNetLogic
{
    public override void Start()
    {
        alarmsDataGridModel = Owner.Get<DataGrid>("AlarmsDataGrid").GetVariable("Model");

        var currentSession = LogicObject.Context.Sessions.CurrentSessionInfo;
        actualLanguagesVariable = currentSession.SessionObject.Get<IUAVariable>("ActualLanguages");
        actualLanguagesVariable.VariableChange += OnSessionActualLanguagesChange;
    }

    public override void Stop()
    {
        try {
            actualLanguagesVariable.VariableChange -= OnSessionActualLanguagesChange;
        } catch {
            Log.Info("Language change is already desync");
        }
    }

    public void OnSessionActualLanguagesChange(object sender, VariableChangeEventArgs e)
    {
        var dynamicLink = alarmsDataGridModel.GetVariable("DynamicLink");
        if (dynamicLink == null)
            return;

        // Restart the data bind on the data grid model variable to refresh data
        string dynamicLinkValue = dynamicLink.Value;
        dynamicLink.Value = string.Empty;
        dynamicLink.Value = dynamicLinkValue;
    }

    private IUAVariable alarmsDataGridModel;
    private IUAVariable actualLanguagesVariable;
}
