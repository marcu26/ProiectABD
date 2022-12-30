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

        private static string LastTypeLoaded;
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
            if (LastTypeSelected == "Articles")
            {
                List<string> selectedAuthors = AuthorsComboBox.SelectedItems.Cast<string>().ToList();
                if (selectedAuthors.Count() == 0)
                {
                    selectedAuthors = null;
                }

                List<string> selectedKeywords = KeywordsComboBox.SelectedItems.Cast<string>().ToList();
                if (selectedKeywords.Count() == 0)
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

                List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByFiltersAsync(titleFilter, abstractFilter, selectedAuthors, selectedKeywords,1);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Hidden;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = articles;
            }

            if(LastTypeSelected == "Books")
            {
                List<string> selectedAuthors = AuthorsComboBox.SelectedItems.Cast<string>().ToList();
                if (selectedAuthors.Count() == 0)
                {
                    selectedAuthors = null;
                }

                string titleFilter = searchBar1.Text;
                if (String.IsNullOrEmpty(titleFilter))
                {
                    titleFilter = null;
                }

                string descriptionFilter = searchBar2.Text;
                if (String.IsNullOrEmpty(descriptionFilter))
                {
                    descriptionFilter = null;
                }

                string ISBNFilter = searchBar3.Text;
                if (String.IsNullOrEmpty(ISBNFilter))
                {
                    ISBNFilter = null;
                }

                List<BooksDto> books = await _booksRepository.GetBooksDtoByFilters(ISBNFilter, titleFilter, descriptionFilter, selectedAuthors,1);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Hidden;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = books;
            }

            if(LastTypeSelected == "Journals")
            {
                LastTypeLoaded = "Journals";

                operationsColumn.Header = "Go to Volumes";

                string titleFilter = searchBar1.Text;
                if (String.IsNullOrEmpty(titleFilter))
                {
                    titleFilter = null;
                }

                string ISSNFilter = searchBar2.Text;
                if (String.IsNullOrEmpty(ISSNFilter))
                {
                    ISSNFilter = null;
                }


                List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByFiltersAsync(titleFilter,ISSNFilter,1);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = journals;
            }

            if(LastTypeSelected == "Publications")
            {
                LastTypeLoaded = "Publications";

                operationsColumn.Header = "Go to Journals";

                string titleFilter = searchBar1.Text;
                if (String.IsNullOrEmpty(titleFilter))
                {
                    titleFilter = null;
                }

                int start,end;
                string startYear = startYearTextBox.Text;
                if (String.IsNullOrEmpty(startYear) || startYear == "From")
                {
                    start = 0;
                }
                else
                {
                    start = Int32.Parse(startYear);
                }

                string endYear = endYearTextBox.Text;
                if (String.IsNullOrEmpty(endYear) || endYear == "To")
                {
                    end = 0;
                }
                else
                {
                    end = Int32.Parse(endYear);
                }

                List<PublicationsDto> publications = await _publicationsRepository.GetPublicationsDtoByFiltersAsync(titleFilter, start,end, 1);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = publications;
            }

            if(LastTypeSelected == "Volumes")
            {
                LastTypeLoaded = "Volumes";

                operationsColumn.Header = "Go to Articles";

                int volNo;
                string volumeNumber = searchBar1.Text;
                if (String.IsNullOrEmpty(volumeNumber))
                {
                    volNo = 0;
                }
                else
                {
                    volNo = Int32.Parse(volumeNumber);
                }

                int start, end;
                string startYear = startYearTextBox.Text;
                if (String.IsNullOrEmpty(startYear) || startYear == "From")
                {
                    start = 0;
                }
                else
                {
                    start = Int32.Parse(startYear);
                }

                string endYear = endYearTextBox.Text;
                if (String.IsNullOrEmpty(endYear) || endYear == "To")
                {
                    end = 0;
                }
                else
                {
                    end = Int32.Parse(endYear);
                }

                List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByFiltersAsync(volNo, start, end, 1);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = volumes;
            }

        }

        private async void navigationButton_Click(object sender, RoutedEventArgs e)
        {

            if (LastTypeLoaded == "Volumes")
            {
                LastTypeLoaded = "Articles";

                operationsColumn.Header = "Go to Articles";
                Button button = sender as Button;
                VolumesDto volume = button.DataContext as VolumesDto;
                List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByVolumeId(volume.VolumeId, 1);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Hidden;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = articles;
            }

            if (LastTypeLoaded == "Journals")
            {
                LastTypeLoaded = "Volumes";

                operationsColumn.Header = "Go to Volumes";
                Button button = sender as Button;
                JournalsDto journal = button.DataContext as JournalsDto;
                List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByJournalId(journal.JournalId, 1);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = volumes;
            }

            if (LastTypeLoaded == "Publications")
            {
                LastTypeLoaded = "Journals";

                operationsColumn.Header = "Go to Journals";
                Button button = sender as Button;
                PublicationsDto publication = button.DataContext as PublicationsDto;
                List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByPublicationIdAsync(publication.PublicationId, 1);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = journals;
            }
        }

        private async void ArticlesButton_Click(object sender, RoutedEventArgs e)
        {
            LastTypeSelected = "Articles";
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

        private async void BooksButton_Click(object sender, RoutedEventArgs e)
        {
            LastTypeSelected = "Books";

            sBar1TextBlock.Text = "Title";
            sBar2TextBlock.Text = "Description";
            sBar3TextBlock.Text = "ISBN";
            sGrid2.Visibility = Visibility.Visible;
            sGrid3.Visibility = Visibility.Visible;
            sGrid4.Visibility = Visibility.Hidden;
            YearStackPanel.Visibility = Visibility.Hidden;
            AuthorsComboBox.Visibility = Visibility.Visible;
            KeywordsComboBox.Visibility = Visibility.Hidden;

            List<string> authors = await _authorsRepository.GetAuthorsAsync();
            AuthorsComboBox.ItemsSource = authors;
        }

        private void JournalsButton_Click(object sender, RoutedEventArgs e)
        {
            LastTypeSelected = "Journals";

            sBar1TextBlock.Text = "Title";
            sBar2TextBlock.Text = "ISSN";
            sGrid2.Visibility = Visibility.Visible;
            sGrid3.Visibility = Visibility.Hidden;
            sGrid4.Visibility = Visibility.Hidden;
            YearStackPanel.Visibility = Visibility.Hidden;
            AuthorsComboBox.Visibility = Visibility.Hidden;
            KeywordsComboBox.Visibility = Visibility.Hidden;
        }

        private void PublicationsButton_Click(object sender, RoutedEventArgs e)
        {
            LastTypeSelected = "Publications";

            sBar1TextBlock.Text = "Title";
            sGrid2.Visibility = Visibility.Hidden;
            sGrid3.Visibility = Visibility.Hidden;
            sGrid4.Visibility = Visibility.Hidden;
            YearStackPanel.Visibility = Visibility.Visible;
            AuthorsComboBox.Visibility = Visibility.Hidden;
            KeywordsComboBox.Visibility = Visibility.Hidden;
        }

        private void VolumesButton_Click(object sender, RoutedEventArgs e)
        {
            LastTypeSelected = "Volumes";

            sBar1TextBlock.Text = "Volume number";
            sGrid2.Visibility = Visibility.Hidden;
            sGrid3.Visibility = Visibility.Hidden;
            sGrid4.Visibility = Visibility.Hidden;
            YearStackPanel.Visibility = Visibility.Visible;
            AuthorsComboBox.Visibility = Visibility.Hidden;
            KeywordsComboBox.Visibility = Visibility.Hidden;
        }

        private void startYearTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            startYearTextBox.Text = string.Empty;
        }

        private void startYearTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(startYearTextBox.Text))
            {
                startYearTextBox.Text = "From";
            }
        }

        private void endYearTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            endYearTextBox.Text = string.Empty;
        }

        private void endYearTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(endYearTextBox.Text))
            {
                endYearTextBox.Text = "To";
            }
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
