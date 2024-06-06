using BreakTimeApp.Helpers;
using BreakTimeApp.Models;
using BreakTimeApp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Drawing;
using System.IO;

namespace BreakTimeApp.ViewModels.Pages
{
    public partial class SelectNotifyViewModel : ObservableObject
    {
        public readonly static int NOTIFY_ID = 1;

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SelectNotifyViewModel> _logger;
        private readonly ISelectNotifyDataService _dataService;

        [ObservableProperty]
        private NotifyMode _notifyMode;

        [ObservableProperty]
        private string _filePath;

        public SelectNotifyViewModel(
            IServiceProvider serviceProvider,
            ILogger<SelectNotifyViewModel> logger
            )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            // Note: DbContextはスコープが違うのでインジェクションができない
            _dataService = new SelectNotifyDataService(new SelectNotifyDbContext());
            LoadItemsAsync();
        }

        [LogAspect]
        private async void LoadItemsAsync()
        {
            var itemsDb = await _dataService.GetSelectNotifyItemByIdAsync(NOTIFY_ID);
            // Note: FilePath 列に対して NOT NULL 制約があるにもかかわらず、
            // null または空の値が挿入されるためFilePathから先にDB更新を行う
            FilePath = itemsDb.FilePath;
            NotifyMode = itemsDb.Mode;
            _logger.LogInformation(AppLogEvents.UserAction, "items loaded");
        }

        [RelayCommand]
        private void OnFileSelect()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image File | *.jpg;*.jpeg;*.png";
            fileDialog.Title = "Please pick a Image Source file....";
            fileDialog.Multiselect = false;

            bool? success = fileDialog.ShowDialog();

            if (success == true)
            {
                string path = fileDialog.FileName;
                FilePath = path;
                UpdateDatabase();
            }
        }

        partial void OnNotifyModeChanged(NotifyMode value)
        {
            UpdateDatabase();
        }

        partial void OnFilePathChanged(string? oldValue, string newValue)
        {
            if (!isImageExtentionsFile(newValue) || !isImage(newValue))
            {
                FilePath = oldValue;
                MessageBox.Show(Properties.MessageResources.InputError,
                    Properties.MessageResources.ErrorTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            UpdateDatabase();
        }

        private async void UpdateDatabase()
        {
            var selectNotifyDb = _serviceProvider.GetService(typeof(SelectNotifyDb)) as SelectNotifyDb;
            selectNotifyDb.Id = NOTIFY_ID;
            selectNotifyDb.Mode = NotifyMode;
            selectNotifyDb.FilePath = FilePath;

            await _dataService.UpdateSelectNotifyItemAsync(selectNotifyDb);
        }

        private bool isImageExtentionsFile(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png"};
            string extension = Path.GetExtension(filePath)?.ToLowerInvariant();

            return Array.Exists(imageExtensions, ext => ext == extension);
        }

        private bool isImage(string filePath)
        {
            try
            {
                using (var img = Image.FromFile(filePath))
                {
                    return true;
                }
            }
            catch (OutOfMemoryException)
            {
                // Image.FromFile throws an OutOfMemoryException 
                // if the file does not have a valid image format or GDI+ does not support the pixel format.
                return false;
            }
            catch (FileNotFoundException)
            {
                // The file does not exist.
                return false;
            }
            catch (Exception)
            {
                // Other exceptions can also indicate that the file is not a valid image.
                return false;
            }
        }
    }
}
