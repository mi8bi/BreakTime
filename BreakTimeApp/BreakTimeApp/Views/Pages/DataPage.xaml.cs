﻿using BreakTimeApp.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace BreakTimeApp.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
