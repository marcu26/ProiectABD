using Core.Database.Context;
using Core.Database.Entities;
using Core.Repositories;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public ProjectDbContext _dbContext = new ProjectDbContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AuthorsRepository authorsRepository = new AuthorsRepository(_dbContext);

            List<Author> names = authorsRepository.GetAuthors();

         

            girdulmeu.ItemsSource = names;
        }
    }
}
