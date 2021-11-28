using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Converters;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Handlers
{
    public class DistributionDataGridHandler
    {
        private readonly string _bind;
        private readonly string _contextMenu;
        private readonly MainViewModel _mainViewModel;

        public DistributionDataGridHandler(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _bind = nameof(_mainViewModel.GridList);
            _contextMenu = null;
        }

        public DataGrid DataGrid()
        {
            var columnCollection = new Collection<DataGridBoundColumn>
            {
                DistributionDataGridColumn(new DataGridTextColumn(),
                    "Name", nameof(DistributionClass.Name)),

                DistributionDataGridColumn(new DataGridTextColumn(),
                    "State", nameof(DistributionClass.State))
            };

            if (_mainViewModel.Config.Configuration.GridConfiguration.Size)
                columnCollection.Add(DistributionDataGridColumn(new DataGridTextColumn(),
                    "Size", nameof(DistributionClass.Size), new BytesToHumanConverter()));

            if (_mainViewModel.Config.Configuration.GridConfiguration.BasePath)
                columnCollection.Add(DistributionDataGridColumn(new DataGridTextColumn(),
                    "Path", nameof(DistributionClass.BasePathLocal)));

            if (_mainViewModel.Config.Configuration.GridConfiguration.Guid)
                columnCollection.Add(DistributionDataGridColumn(new DataGridTextColumn(),
                    "GUID", nameof(DistributionClass.Guid)));

            var dataGrid = new DataGrid
            {
                Name = "DistributionDataGrid",
                SelectionMode = DataGridSelectionMode.Single,
                SelectionUnit = DataGridSelectionUnit.FullRow,
                RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed,
                HeadersVisibility = DataGridHeadersVisibility.Column,
                GridLinesVisibility = DataGridGridLinesVisibility.None,
                IsReadOnly = true,
                AutoGenerateColumns = false
            };

            foreach (var column in columnCollection) dataGrid.Columns.Add(column);

            if (_bind != null)
                dataGrid.SetBinding(ItemsControl.ItemsSourceProperty, BindHelper.BindingObject(_bind, _mainViewModel));

            if (_contextMenu != null)
                dataGrid.SetBinding(FrameworkElement.ContextMenuProperty,
                    BindHelper.BindingObject(_contextMenu, _mainViewModel));

            dataGrid.MouseDoubleClick += (_, _) =>
            {
                if (_mainViewModel.SelectedDistribution == null) return;
                if (_mainViewModel.Config.Configuration.GridConfiguration.DoubleClick ==
                    GridConfiguration.GridConfigurationOpenTerminal)
                    OpenShellDistributionCommand.Execute(_mainViewModel.SelectedDistribution);
            };

            dataGrid.MouseLeftButtonUp += (_, _) => { };

            dataGrid.SetBinding(Selector.SelectedItemProperty,
                BindHelper.BindingObject(nameof(_mainViewModel.SelectedDistribution), _mainViewModel,
                    BindingMode.TwoWay));

            dataGrid.SelectionChanged += (_, _) =>
            {
                if (_mainViewModel == null) return;

                dataGrid.ContextMenu = _mainViewModel.SelectedDistribution != null
                    ? new ContextMenu
                    {
                        ItemsSource = _mainViewModel.DataGridMenuItems()
                    }
                    : null;
            };

            if (_mainViewModel.Config.Configuration.KeyboardShortcutConfiguration.Enabled)
                DataGridShortcutHandler(dataGrid, _mainViewModel);

            return dataGrid;
        }

        private static void DataGridShortcutHandler(IInputElement dataGrid, MainViewModel mainViewModel)
        {
            dataGrid.KeyUp += (_, args) =>
            {
                if (mainViewModel.SelectedDistribution == null) return;
                var config = mainViewModel.Config.Configuration.KeyboardShortcutConfiguration;

                if (args.Key == config.GridDeleteKey && config.GridDeleteEnabled)
                    mainViewModel.DeleteDistribution.Execute(mainViewModel.SelectedDistribution);

                if (args.Key == config.GridRenameKey && config.GridRenameEnabled)
                    mainViewModel.RenameDistribution.Execute(mainViewModel.SelectedDistribution);

                if (args.Key == config.GridRefreshKey && config.GridRefreshEnabled)
                    mainViewModel.RefreshDistributions();
            };
        }

        private static DataGridBoundColumn DistributionDataGridColumn(DataGridBoundColumn dataGridBoundColumn,
            string header, string bindingPath, IValueConverter converter = null,
            int width = 0, int maxWidth = 0, int minWidth = 0)
        {
            var bind = new Binding(bindingPath);
            if (converter != null) bind.Converter = converter;

            dataGridBoundColumn.Header = header;
            dataGridBoundColumn.Binding = bind;

            if (width > 0) dataGridBoundColumn.Width = width;
            if (maxWidth > 0) dataGridBoundColumn.MaxWidth = maxWidth;
            if (minWidth > 0) dataGridBoundColumn.MinWidth = minWidth;

            return dataGridBoundColumn;
        }
    }
}