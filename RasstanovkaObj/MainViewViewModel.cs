using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasstanovkaObj
{
    class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        public FamilySymbol SelectedColumnType { get; set; }
        public Level SelectedLevel { get; set; }
        public List<FamilySymbol> TypeColumns { get; } = new List<FamilySymbol>();
        public List<Level> Levels { get; } = new List<Level>();
        public DelegateCommand SaveCommand { get; }
        public List<XYZ> Points { get; } = new List<XYZ>();

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            TypeColumns = ColumnUtils.GetColumnTypes(commandData);
            Levels = LevelsUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            Points = SelectionUtils.GetPoints(commandData, "Выберите точки", Autodesk.Revit.UI.Selection.ObjectSnapTypes.Endpoints);
        }
        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (SelectedColumnType == null || SelectedLevel == null)
                return;
            foreach (XYZ point in Points)
            {
                FamilyInstance column = FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedColumnType, point, SelectedLevel);
            }
            RaiseCloseRequest();
        }
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
