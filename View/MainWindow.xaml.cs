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

        private enum ObjectType {Articles, Books, Journals, Volumes, Publications};

        private bool IsMaximize = false;

        private static int CurrentPage = 1;

        private string LastArticleTitleFilter, LastArticleAbstractFilter, LastBooksISBNFilter, LastBooksTitleFilter, LastBooksDescriptionFilter, LastJournalsTitleFilter, LastJournalsISSNFilter, LastPublicationsTitleFilter;

        private int LastPublicationsStartYear, LastPublicationsEndYear, LastVolumesStartYear, LastVolumesEndYear, LastVolumesNumberFilter;

        private List<string> LastArticleSelectedAuthors,LastArticleSelectedKeywords, LastBooksSelectedAuthors = new List<string>();

        private static ObjectType LastTypeSelected;

        private static ObjectType LastTypeLoaded;

        private static int NumberOfPages = 1;

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (LastTypeSelected == ObjectType.Articles)
            {
                LastTypeLoaded = ObjectType.Articles;

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

                LastArticleTitleFilter = titleFilter;
                LastArticleAbstractFilter = abstractFilter;
                LastArticleSelectedAuthors = selectedAuthors;
                LastArticleSelectedKeywords = selectedKeywords;

                List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByFiltersAsync(titleFilter, abstractFilter, selectedAuthors, selectedKeywords,CurrentPage);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Hidden;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = articles;
            }

            if(LastTypeSelected == ObjectType.Books)
            {
                LastTypeLoaded = ObjectType.Books;

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

                LastBooksISBNFilter = ISBNFilter;
                LastBooksTitleFilter = titleFilter;
                LastBooksDescriptionFilter = descriptionFilter;
                LastBooksSelectedAuthors = selectedAuthors;

                List<BooksDto> books = await _booksRepository.GetBooksDtoByFilters(ISBNFilter, titleFilter, descriptionFilter, selectedAuthors,CurrentPage);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Hidden;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = books;
            }

            if(LastTypeSelected == ObjectType.Journals)
            {
                LastTypeLoaded = ObjectType.Journals;

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

                LastJournalsISSNFilter = ISSNFilter;
                LastJournalsTitleFilter = titleFilter;

                List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByFiltersAsync(titleFilter,ISSNFilter,CurrentPage);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = journals;
            }

            if(LastTypeSelected == ObjectType.Publications)
            {
                LastTypeLoaded = ObjectType.Publications;

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

                LastPublicationsTitleFilter = titleFilter;
                LastPublicationsStartYear = start;
                LastPublicationsEndYear = end;


                List<PublicationsDto> publications = await _publicationsRepository.GetPublicationsDtoByFiltersAsync(titleFilter, start,end, CurrentPage);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = publications;
            }

            if(LastTypeSelected == ObjectType.Volumes)
            {
                LastTypeLoaded = ObjectType.Volumes;

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

                LastVolumesNumberFilter = volNo;
                LastVolumesStartYear = start;
                LastVolumesEndYear = end;

                List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByFiltersAsync(volNo, start, end, CurrentPage);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = volumes;
            }

        }

        private async void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                if (LastTypeLoaded == ObjectType.Articles)
                {
                    List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByFiltersAsync(LastArticleTitleFilter, LastArticleAbstractFilter, LastArticleSelectedAuthors, LastArticleSelectedKeywords, CurrentPage);
                    DataGrid.ItemsSource = articles;
                    PageNumberButton.Content = CurrentPage.ToString();
                }

                if(LastTypeLoaded == ObjectType.Books)
                {
                    List<BooksDto> books = await _booksRepository.GetBooksDtoByFilters(LastBooksISBNFilter, LastBooksTitleFilter, LastBooksDescriptionFilter, LastBooksSelectedAuthors, CurrentPage);
                    DataGrid.ItemsSource = books;
                    PageNumberButton.Content = CurrentPage.ToString();
                }

                if (LastTypeLoaded == ObjectType.Journals)
                {

                    List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByFiltersAsync(LastJournalsTitleFilter, LastJournalsISSNFilter, CurrentPage);
                    DataGrid.ItemsSource = journals;
                    PageNumberButton.Content = CurrentPage.ToString();
                }

                if (LastTypeLoaded == ObjectType.Publications)
                {
                    List<PublicationsDto> publications = await _publicationsRepository.GetPublicationsDtoByFiltersAsync(LastPublicationsTitleFilter, LastPublicationsStartYear, LastPublicationsEndYear, CurrentPage);
                    DataGrid.ItemsSource = publications;
                    PageNumberButton.Content = CurrentPage.ToString();
                }

                if(LastTypeLoaded == ObjectType.Volumes)
                {
                    List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByFiltersAsync(LastVolumesNumberFilter, LastVolumesStartYear, LastVolumesEndYear, CurrentPage);
                    DataGrid.ItemsSource = volumes;
                    PageNumberButton.Content = CurrentPage.ToString();
                }
            }
        }

        private async void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (true)
            {
                CurrentPage++;
                if (LastTypeLoaded == ObjectType.Articles)
                {
                    List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByFiltersAsync(LastArticleTitleFilter, LastArticleAbstractFilter, LastArticleSelectedAuthors, LastArticleSelectedKeywords, CurrentPage);
                    DataGrid.ItemsSource = articles;
                    PageNumberButton.Content = CurrentPage.ToString();
                }

                if (LastTypeLoaded == ObjectType.Books)
                {
                    List<BooksDto> books = await _booksRepository.GetBooksDtoByFilters(LastBooksISBNFilter, LastBooksTitleFilter, LastBooksDescriptionFilter, LastBooksSelectedAuthors, CurrentPage);
                    DataGrid.ItemsSource = books;
                    PageNumberButton.Content = CurrentPage.ToString();
                }

                if (LastTypeLoaded == ObjectType.Journals)
                {

                    List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByFiltersAsync(LastJournalsTitleFilter, LastJournalsISSNFilter, CurrentPage);
                    DataGrid.ItemsSource = journals;
                    PageNumberButton.Content = CurrentPage.ToString();
                }

                if (LastTypeLoaded == ObjectType.Publications)
                {
                    List<PublicationsDto> publications = await _publicationsRepository.GetPublicationsDtoByFiltersAsync(LastPublicationsTitleFilter, LastPublicationsStartYear, LastPublicationsEndYear, CurrentPage);
                    DataGrid.ItemsSource = publications;
                    PageNumberButton.Content = CurrentPage.ToString();
                }

                if (LastTypeLoaded == ObjectType.Volumes)
                {
                    List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByFiltersAsync(LastVolumesNumberFilter, LastVolumesStartYear, LastVolumesEndYear, CurrentPage);
                    DataGrid.ItemsSource = volumes;
                    PageNumberButton.Content = CurrentPage.ToString();
                }
            }
        }

        private async void navigationButton_Click(object sender, RoutedEventArgs e)
        {

            if (LastTypeLoaded == ObjectType.Volumes)
            {
                LastTypeLoaded = ObjectType.Articles;

                Button button = sender as Button;
                VolumesDto volume = button.DataContext as VolumesDto;
                List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByVolumeId(volume.VolumeId, CurrentPage);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Hidden;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = articles;
            }

            if (LastTypeLoaded == ObjectType.Journals)
            {
                LastTypeLoaded = ObjectType.Volumes;

                operationsColumn.Header = "Go to Articles";
                Button button = sender as Button;
                JournalsDto journal = button.DataContext as JournalsDto;
                List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByJournalId(journal.JournalId, CurrentPage);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = volumes;
            }

            if (LastTypeLoaded == ObjectType.Publications)
            {
                LastTypeLoaded = ObjectType.Journals;

                operationsColumn.Header = "Go to Volumes";
                Button button = sender as Button;
                PublicationsDto publication = button.DataContext as PublicationsDto;
                List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByPublicationIdAsync(publication.PublicationId, CurrentPage);
                DataGrid.AutoGenerateColumns = true;
                operationsColumn.Visibility = Visibility.Visible;
                DataGrid.ColumnWidth = DataGridLength.Auto;
                DataGrid.ItemsSource = journals;
            }
        }

        private async void ArticlesButton_Click(object sender, RoutedEventArgs e)
        {
            LastTypeSelected = ObjectType.Articles;
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
            LastTypeSelected = ObjectType.Books;

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
            LastTypeSelected = ObjectType.Journals;

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
            LastTypeSelected = ObjectType.Publications;

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
            LastTypeSelected = ObjectType.Volumes;

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

        
    }
}
