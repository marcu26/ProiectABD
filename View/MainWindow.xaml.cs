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

        public List<string> PublicationTypes =  new List<string>() {"Article", "Book", "Journal" , "Publication", "Volume"};
    public MainWindow()
        {
            InitializeComponent();
            TypeFilter.ItemsSource = PublicationTypes;
        }
        private void TypeFilter_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            string current_selection = TypeFilter.SelectedItem.ToString();

            if(current_selection == "Article" )
            {
                FieldsFilter.ItemsSource = typeof(Article).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
                AuthorsRepository authorsRepository = new AuthorsRepository(_dbContext);
                List<Author> names = authorsRepository.GetAuthors();
                ListView.ItemsSource = names;

                List<Keyword> keywords = _dbContext.Keywords.ToList();
                KeywordFilter.ItemsSource = keywords;
            }
            if(current_selection == "Book" )
            {
                FieldsFilter.ItemsSource = typeof(Book).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
            }
            if(current_selection == "Journal" )
            {
                FieldsFilter.ItemsSource = typeof(Journal).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
            }
            if(current_selection == "Publication")
            {
                FieldsFilter.ItemsSource = typeof(Publication).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
            }
            if(current_selection == "Volume")
            {
                FieldsFilter.ItemsSource = typeof(Volume).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
            }
        }

        private void SearchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = string.Empty;
        }

        private void SearchBar_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "Enter text...";
        }

        private void KeywordFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            KeywordFilter.Text = string.Empty;
        }

        private void KeywordFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            KeywordFilter.Text = "Enter a keyword...";
        }



        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    AuthorsRepository authorsRepository = new AuthorsRepository(_dbContext);

        //    List<Author> names = authorsRepository.GetAuthors();



        //    girdulmeu.ItemsSource = names;
        //}
    }
}
