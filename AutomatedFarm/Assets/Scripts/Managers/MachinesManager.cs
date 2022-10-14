using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachinesManager : Singleton<MachinesManager>
{
    [SerializeField] MachineContainer machineContainer;
    [SerializeField] GridLayoutGroup grid;

    [SerializeField] TextMeshProUGUI myName;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI description;

    public Building currentMachine { get; private set; }
    public MachineContainer firstContainer { get; private set; }

    private void Start()
    {
        SpawnMachineContainer();
    }

    void SpawnMachineContainer()
    {
        for (int i = 0; i < Library.Instance.buildingsSO.buildings.Count; i++)
        {
            Building currentMachine = Library.Instance.buildingsSO.buildings[i];
            MachineContainer container = Instantiate(machineContainer, grid.transform);
            if (i == 0) firstContainer = container;
            container.GetMachine(currentMachine);
        }
    }

    public void UpdateMachineDetails(string machineName, float machinePrice, string machineDesc)
    {
        myName.text = machineName;
        price.text = machinePrice.ToString();
        description.text = machineDesc;
    }

    public void SelectMachine()
    {
        Library.Instance.currentSelected = currentMachine.original;
        BuildSystem.Instance.ChosseObject(currentMachine.blueprint);
        CanvasManager.Instance.ToggleMachinesUI();
    }

    public void CurrentMachineSelected(Building machine)
    {
        currentMachine = machine;
    }
}
