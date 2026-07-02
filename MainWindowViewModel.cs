using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using OptiLauncher.Core.Enums; // Подключение перечисления AccountType

namespace OptiLauncher.ViewModels
{
    public class MinecraftVersion
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Release, Snapshot, Experimental, Forge, Fabric
    }

    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<MinecraftVersion> _allVersions = new();
        private ObservableCollection<MinecraftVersion> _filteredVersions = new();
        private MinecraftVersion? _selectedVersion;

        // Поля ввода для авторизации
        private string _inputUsername = "Alex_Active";
        private AccountType _tempAccountType = AccountType.Offline;

        // Активный профиль после нажатия "Войти"
        private string _selectedUsername = "Alex_Active";
        private AccountType _activeAccountType = AccountType.Offline;
        private string _activeAccountTypeDisplay = "Офлайн-режим";
        private string _statusColor = "#E67E22"; // Изначально оранжевый для Offline

        // Технические чекбоксы версий
        private bool _showReleases = true;
        private bool _showSnapshots = true;
        private bool _showExperimental = true;
        private bool _showForge = true;
        private bool _showFabric = true;

        // Коллекции и свойства для привязки данных
        public ObservableCollection<MinecraftVersion> FilteredVersions
        {
            get => _filteredVersions;
            set => this.RaiseAndSetIfChanged(ref _filteredVersions, value);
        }

        public MinecraftVersion? SelectedVersion
        {
            get => _selectedVersion;
            set => this.RaiseAndSetIfChanged(ref _selectedVersion, value);
        }

        // Поле ввода логина / ника
        public string InputUsername
        {
            get => _inputUsername;
            set => this.RaiseAndSetIfChanged(ref _inputUsername, value);
        }

        // Выбранный в комбобоксе метод авторизации
        public AccountType TempAccountType
        {
            get => _tempAccountType;
            set => this.RaiseAndSetIfChanged(ref _tempAccountType, value);
        }

        // Ник авторизованного профиля на плашке
        public string SelectedUsername
        {
            get => _selectedUsername;
            set => this.RaiseAndSetIfChanged(ref _selectedUsername, value);
        }

        // Тип аккаунта авторизованного профиля
        public AccountType ActiveAccountType
        {
            get => _activeAccountType;
            set => this.RaiseAndSetIfChanged(ref _activeAccountType, value);
        }

        public string ActiveAccountTypeDisplay
        {
            get => _activeAccountTypeDisplay;
            set => this.RaiseAndSetIfChanged(ref _activeAccountTypeDisplay, value);
        }

        public string StatusColor
        {
            get => _statusColor;
            set => this.RaiseAndSetIfChanged(ref _statusColor, value);
        }

        // Чекбоксы фильтров
        public bool ShowReleases { get => _showReleases; set { this.RaiseAndSetIfChanged(ref _showReleases, value); ApplyFilter(); } }
        public bool ShowSnapshots { get => _showSnapshots; set { this.RaiseAndSetIfChanged(ref _showSnapshots, value); ApplyFilter(); } }
        public bool ShowExperimental { get => _showExperimental; set { this.RaiseAndSetIfChanged(ref _showExperimental, value); ApplyFilter(); } }
        public bool ShowForge { get => _showForge; set { this.RaiseAndSetIfChanged(ref _showForge, value); ApplyFilter(); } }
        public bool ShowFabric { get => _showFabric; set { this.RaiseAndSetIfChanged(ref _showFabric, value); ApplyFilter(); } }

        // Доступный список аккаунтов для выбора
        public AccountType[] AccountTypeList => (AccountType[])Enum.GetValues(typeof(AccountType));

        public ICommand PlayCommand { get; }
        public ICommand LoginCommand { get; }

        public MainWindowViewModel()
        {
            GenerateMinecraftVersions();
            ApplyFilter();

            PlayCommand = ReactiveCommand.Create(ExecuteLaunchGame);
            LoginCommand = ReactiveCommand.Create(ExecuteLogin);

            // Инициализация статуса при первом запуске
            UpdateAccountStatus();
        }

        private void GenerateMinecraftVersions()
        {
            // Весь запрашиваемый вами список версий от 1.16.5 до новейших
            string[] coreReleases = { 
                "1.21.11", "1.21.4", "1.21.3", "1.21.2", "1.21.1", "1.21", 
                "1.20.6", "1.20.5", "1.20.4", "1.20.3", "1.20.2", "1.20.1", "1.20", 
                "1.19.4", "1.19.3", "1.19.2", "1.19.1", "1.19", 
                "1.18.2", "1.18.1", "1.18", 
                "1.17.1", "1.17", 
                "1.16.5" 
            };

            foreach (var ver in coreReleases)
            {
                _allVersions.Add(new MinecraftVersion { Name = $"Minecraft {ver}", Type = "Release" });
                _allVersions.Add(new MinecraftVersion { Name = $"Forge {ver}", Type = "Forge" });
                _allVersions.Add(new MinecraftVersion { Name = $"Fabric {ver}", Type = "Fabric" });
            }

            // Экспериментальные сборки и снапшоты
            _allVersions.Add(new MinecraftVersion { Name = "Snapshot 26.2", Type = "Snapshot" });
            _allVersions.Add(new MinecraftVersion { Name = "Snapshot 26.1", Type = "Snapshot" });
            _allVersions.Add(new MinecraftVersion { Name = "1.21 Experimental Toggle 3", Type = "Experimental" });
            _allVersions.Add(new MinecraftVersion { Name = "1.20 Experimental Snapshot 1", Type = "Experimental" });
        }

        private void ApplyFilter()
        {
            var results = _allVersions.Where(v =>
                (v.Type == "Release" && ShowReleases) ||
                (v.Type == "Snapshot" && ShowSnapshots) ||
                (v.Type == "Experimental" && ShowExperimental) ||
                (v.Type == "Forge" && ShowForge) ||
                (v.Type == "Fabric" && ShowFabric)
            ).ToList();

            FilteredVersions.Clear();
            foreach (var version in results) FilteredVersions.Add(version);

            if (FilteredVersions.Count > 0 && (SelectedVersion == null || !FilteredVersions.Contains(SelectedVersion)))
            {
                SelectedVersion = FilteredVersions[0];
            }
        }

        private void ExecuteLogin()
        {
            // Логика кнопки "Войти"
            if (!string.IsNullOrWhiteSpace(InputUsername))
            {
                SelectedUsername = InputUsername;
                ActiveAccountType = TempAccountType;
                UpdateAccountStatus();
            }
        }

        private void UpdateAccountStatus()
        {
            // Динамический расчёт цвета и текстового статуса для плашки профиля
            switch (ActiveAccountType)
            {
                case AccountType.Microsoft:
                    ActiveAccountTypeDisplay = "Лицензия (MS)";
                    StatusColor = "#2ECC71"; // Неоновый зелёный
                    break;
                case AccountType.ElyBy:
                    ActiveAccountTypeDisplay = "Ely.by";
                    StatusColor = "#3498DB"; // Неоновый синий
                    break;
                case AccountType.Offline:
                default:
                    ActiveAccountTypeDisplay = "Офлайн-режим";
                    StatusColor = "#E67E22"; // Оранжевый
                    break;
            }
        }

        private void ExecuteLaunchGame()
        {
            if (SelectedVersion != null)
            {
                System.Diagnostics.Debug.WriteLine($"Запуск игры: {SelectedVersion.Name} | Ник: {SelectedUsername} | Авторизация: {ActiveAccountType}");
            }
        }
    }
}