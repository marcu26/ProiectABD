using Core.Database.Context;
using Core.Database.Entities;
using Core.Dtos.Articles;
using Core.Dtos.Books;
using Core.Dtos.Journal;
using Core.Dtos.Publications;
using Core.Dtos.Volumes;
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

        #region Utils

        public static ProjectDbContext _dbContext = new ProjectDbContext();

        public static PublicationsRepository _publicationsRepository = new PublicationsRepository(_dbContext);

        public static JournalsRepository _journalsRepository = new JournalsRepository(_dbContext);

        public static VolumesRepository _volumesRepository = new VolumesRepository(_dbContext);

        public static AuthorsRepository _authorsRepository = new AuthorsRepository(_dbContext);

        public static ArticlesRepository _articlesRepository = new ArticlesRepository(_dbContext);

        public static BooksRepository _booksRepository = new BooksRepository(_dbContext);

        public static KeywordsRepository _keywordsRepository = new KeywordsRepository(_dbContext);


        public List<string> PublicationTypes =  new List<string>() {"Article", "Book", "Journal" , "Publication", "Volume"};

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            sBar1TextBlock.Text = "Search here...";
            sGrid2.Visibility = Visibility.Hidden;
            sGrid3.Visibility = Visibility.Hidden;
            sGrid4.Visibility = Visibility.Hidden;
            YearStackPanel.Visibility = Visibility.Hidden;
            AuthorsComboBox.Visibility = Visibility.Hidden;
            KeywordsComboBox.Visibility = Visibility.Hidden;
        }


        private bool IsMaximize = false;

        private static string LastTypeSelected;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {

            List<string> selectedAuthors = AuthorsComboBox.SelectedItems.Cast<string>().ToList();
            if(selectedAuthors.Count() == 0)
            {
                selectedAuthors = null;
            }

            List<string> selectedKeywords = KeywordsComboBox.SelectedItems.Cast<string>().ToList();
            if(selectedKeywords.Count() == 0)
            {
                selectedKeywords = null;
            }

            string titleFilter = searchBar1.Text;
            if (String.IsNullOrEmpty(titleFilter))
            {
                titleFilter = null;
            }

            string abstractFilter = searchBar2.Text;
            if (String.IsNullOrEmpty(abstractFilter))
            {
                abstractFilter = null;
            }

            List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByFiltersAsync(titleFilter, abstractFilter, selectedAuthors, selectedKeywords);
            DataGrid.AutoGenerateColumns = true;
            DataGrid.ItemsSource = articles;
        }

        private async void ArticlesButton_Click(object sender, RoutedEventArgs e)
        {
            LastTypeSelected = "Article";
            sBar1TextBlock.Text = "Title";
            sBar2TextBlock.Text = "Description";
            sGrid2.Visibility = Visibility.Visible;
            sGrid3.Visibility = Visibility.Hidden;
            sGrid4.Visibility = Visibility.Hidden;
            YearStackPanel.Visibility = Visibility.Hidden;
            AuthorsComboBox.Visibility = Visibility.Visible;
            KeywordsComboBox.Visibility = Visibility.Visible;

            List<string> authors = await _authorsRepository.GetAuthorsAsync();
            AuthorsComboBox.ItemsSource = authors;

            List<string> keywords = await _keywordsRepository.GetKeywordsAsync();
            KeywordsComboBox.ItemsSource = keywords;


            
        }
        //private async void TypeFilter_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    string current_selection = TypeFilter.SelectedItem.ToString();

        //    if(current_selection == "Article" )
        //    {
        //        FieldsFilter.ItemsSource = typeof(Article).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
        //        //List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByVolumeId(672);
        //        List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByFiltersAsync(null, null, new List<string>() { "Aisha Levine", "Kristen Fletcher" }, null);
        //        ListView.ItemsSource = articles;
        //    }
        //    if(current_selection == "Book" )
        //    {
        //        FieldsFilter.ItemsSource = typeof(Book).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
        //        List<BooksDto> books = await _booksRepository.GetBooksDtoByFilters(null, null, null, null);
        //        ListView.ItemsSource = books;
        //    }
        //    if(current_selection == "Journal" )
        //    {
        //        FieldsFilter.ItemsSource = typeof(Journal).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
        //        List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByFiltersAsync("z","7");
        //        //List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByPublicationIdAsync(34);
        //        ListView.ItemsSource = journals;
        //    }
        //    if(current_selection == "Publication")
        //    {
        //        FieldsFilter.ItemsSource = typeof(Publication).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
        //        List<PublicationsDto> publications =await _publicationsRepository.GetPublicationsDtoByFiltersAsync("e", 2006, 2009);
        //        ListView.ItemsSource = publications;
        //    }
        //    if(current_selection == "Volume")
        //    {
        //        FieldsFilter.ItemsSource = typeof(Volume).GetProperties().Select(p => p.Name).Where(n => n != "Id" && n != "IsDeleted");
        //        // List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByFiltersAsync(0, 2000, 2010);
        //        List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByJournalId(547);
        //        ListView.ItemsSource = volumes;
        //    }
        //}

        //private void SearchBar_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    SearchBar.Text = string.Empty;
        //}

        //private void SearchBar_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    SearchBar.Text = "Search here...";
        //}




        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    AuthorsRepository authorsRepository = new AuthorsRepository(_dbContext);

        //    List<Author> names = authorsRepository.GetAuthors();



        //    girdulmeu.ItemsSource = names;
        //}
    }
}
