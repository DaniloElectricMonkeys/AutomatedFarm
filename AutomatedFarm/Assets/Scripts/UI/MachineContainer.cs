using UnityEngine;
using UnityEngine.UI;

namespace AutomatedFarm
{
    public class MachineContainer : MonoBehaviour
    {
        [SerializeField] Image icon;

        public Building machine { get; private set; }

        private void Start()
        {
            LoadSettings();
        }

        void LoadSettings()
        {
            if (machine.icon != null)
            {
                icon.sprite = machine.icon;
            }
        }

        public void GetMachine(Building buildingSettings)
        {
            machine = buildingSettings;
        }

        public void Selected()
        {
            MachinesManager.Instance.UpdateMachineDetails(machine.name, machine.price, machine.description);
            MachinesManager.Instance.CurrentMachineSelected(machine);
        }
    }
}
