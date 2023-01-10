using Core.Database.Context;
using Core.Database.Entities;
using Core.Dtos.Articles;
using Core.Dtos.Books;
using Core.Dtos.Journal;
using Core.Dtos.Publications;
using Core.Dtos.Users;
using Core.Dtos.Volumes;
using Core.Email;
using Core.Repositories;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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

       
        private enum ObjectType { Articles, Books, Journals, Volumes, Publications };

        private static int CurrentPage = 1;

        private string LastArticleTitleFilter, LastArticleAbstractFilter, LastBooksISBNFilter, LastBooksTitleFilter, LastBooksDescriptionFilter, LastJournalsTitleFilter, LastJournalsISSNFilter, LastPublicationsTitleFilter;

        private int LastPublicationsStartYear, LastPublicationsEndYear, LastVolumesStartYear, LastVolumesEndYear, LastVolumesNumberFilter;

        private List<string> LastArticleSelectedAuthors, LastArticleSelectedKeywords, LastBooksSelectedAuthors = new List<string>();

        public int LastVolumeId, LastJournalId, LastPublicationId;

        private static ObjectType LastTypeSelected;

        private static ObjectType LastTypeLoaded;

        private static int NumberOfPages = 1;

        private static bool Navigated = false;

        private UserDto loggedUser;

        #endregion

        public MainWindow(UserDto _loggedUser)
        {
            InitializeComponent();
            sBar1TextBlock.Text = "Search here...";
            sGrid2.Visibility = Visibility.Hidden;
            sGrid3.Visibility = Visibility.Hidden;
            sGrid4.Visibility = Visibility.Hidden;
            YearStackPanel.Visibility = Visibility.Hidden;
            AuthorsComboBox.Visibility = Visibility.Hidden;
            KeywordsComboBox.Visibility = Visibility.Hidden;
            loggedUser = _loggedUser;
            searchButton.Visibility = Visibility.Hidden;

          
        }

        private void ThemedWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                searchButton_Click(sender, e);
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
          
            Navigated = false;
            CurrentPage = 1;
            PageNumberButton.Content = CurrentPage.ToString();
           
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

                try
                {
                    NumberOfPages = await _articlesRepository.GetNumberOfPagesForArticlesByFilterAsync(titleFilter, abstractFilter, selectedAuthors, selectedKeywords, loggedUser.Role);
                    List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByFiltersAsync(titleFilter, abstractFilter, selectedAuthors, selectedKeywords, CurrentPage, loggedUser.Role);
                    DataGrid.AutoGenerateColumns = true;
                    operationsColumn.Visibility = Visibility.Hidden;
                    DataGrid.ColumnWidth = DataGridLength.Auto;
                    DataGrid.ItemsSource = articles;
                }
                catch(Exception ex) 
                {
                    return;
                }
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

                try
                {
                    NumberOfPages = await _booksRepository.GetNumberOfBookPagesByFilter(ISBNFilter, titleFilter, descriptionFilter, selectedAuthors, loggedUser.Role);
                    List<BooksDto> books = await _booksRepository.GetBooksDtoByFilters(ISBNFilter, titleFilter, descriptionFilter, selectedAuthors, CurrentPage, loggedUser.Role);
                    DataGrid.AutoGenerateColumns = true;
                    operationsColumn.Visibility = Visibility.Hidden;
                    DataGrid.ColumnWidth = DataGridLength.Auto;
                    DataGrid.ItemsSource = books;
                }
                catch (Exception ex)
                {
                    return;
                }
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

                try
                {
                    NumberOfPages = await _journalsRepository.GetJournalsNumberOfPagesByFiltersAsync(titleFilter, ISSNFilter);
                    List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByFiltersAsync(titleFilter, ISSNFilter, CurrentPage);
                    DataGrid.AutoGenerateColumns = true;
                    operationsColumn.Visibility = Visibility.Visible;
                    DataGrid.ColumnWidth = DataGridLength.Auto;
                    DataGrid.ItemsSource = journals;
                }
                catch (Exception ex)
                {
                    return;
                }
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

                try
                {
                    NumberOfPages = await _publicationsRepository.GetPublicationsNumberOfPagesByFiltersAsync(titleFilter, start, end);
                    List<PublicationsDto> publications = await _publicationsRepository.GetPublicationsDtoByFiltersAsync(titleFilter, start, end, CurrentPage);
                    DataGrid.AutoGenerateColumns = true;
                    operationsColumn.Visibility = Visibility.Visible;
                    DataGrid.ColumnWidth = DataGridLength.Auto;
                    DataGrid.ItemsSource = publications;
                }
                catch (Exception ex)
                {
                    return;
                }
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

                try
                {

                    NumberOfPages = await _volumesRepository.GetVolumesNumberOfPagesByFiltersAsync(volNo, start, end);
                    List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByFiltersAsync(volNo, start, end, CurrentPage);
                    DataGrid.AutoGenerateColumns = true;
                    operationsColumn.Visibility = Visibility.Visible;
                    DataGrid.ColumnWidth = DataGridLength.Auto;
                    DataGrid.ItemsSource = volumes;
                }
                catch (Exception ex)
                {
                    return;
                }
            }

        }

        private async void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentPage > 1)
                {
                    CurrentPage--;
                    if (!Navigated)
                    {
                        if (LastTypeLoaded == ObjectType.Articles)
                        {
                            List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByFiltersAsync(LastArticleTitleFilter, LastArticleAbstractFilter, LastArticleSelectedAuthors, LastArticleSelectedKeywords, CurrentPage, loggedUser.Role);
                            DataGrid.ItemsSource = articles;
                            PageNumberButton.Content = CurrentPage.ToString();
                        }

                        if (LastTypeLoaded == ObjectType.Books)
                        {
                            List<BooksDto> books = await _booksRepository.GetBooksDtoByFilters(LastBooksISBNFilter, LastBooksTitleFilter, LastBooksDescriptionFilter, LastBooksSelectedAuthors, CurrentPage, loggedUser.Role);
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
                    else
                    {
                        if (LastTypeLoaded == ObjectType.Articles)
                        {
                            List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByVolumeId(LastVolumeId, CurrentPage, loggedUser.Role);
                            DataGrid.ItemsSource = articles;
                            PageNumberButton.Content = CurrentPage.ToString();
                        }

                        if (LastTypeLoaded == ObjectType.Journals)
                        {

                            List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByPublicationIdAsync(LastPublicationId, CurrentPage);
                            DataGrid.ItemsSource = journals;
                            PageNumberButton.Content = CurrentPage.ToString();
                        }

                        if (LastTypeLoaded == ObjectType.Volumes)
                        {
                            List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByJournalId(LastJournalId, CurrentPage);
                            DataGrid.ItemsSource = volumes;
                            PageNumberButton.Content = CurrentPage.ToString();
                        }
                    }
                }
            }
            catch(Exception ex) 
            {
                return;
            }
        }

        private async void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentPage < NumberOfPages)
                {
                    CurrentPage++;
                    if (!Navigated)
                    {
                        if (LastTypeLoaded == ObjectType.Articles)
                        {
                            List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByFiltersAsync(LastArticleTitleFilter, LastArticleAbstractFilter, LastArticleSelectedAuthors, LastArticleSelectedKeywords, CurrentPage, loggedUser.Role);
                            DataGrid.ItemsSource = articles;
                            PageNumberButton.Content = CurrentPage.ToString();
                        }

                        if (LastTypeLoaded == ObjectType.Books)
                        {
                            List<BooksDto> books = await _booksRepository.GetBooksDtoByFilters(LastBooksISBNFilter, LastBooksTitleFilter, LastBooksDescriptionFilter, LastBooksSelectedAuthors, CurrentPage, loggedUser.Role);
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
                    else
                    {
                        if (LastTypeLoaded == ObjectType.Articles)
                        {
                            List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByVolumeId(LastVolumeId, CurrentPage, loggedUser.Role);
                            DataGrid.ItemsSource = articles;
                            PageNumberButton.Content = CurrentPage.ToString();
                        }

                        if (LastTypeLoaded == ObjectType.Journals)
                        {

                            List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByPublicationIdAsync(LastPublicationId, CurrentPage);
                            DataGrid.ItemsSource = journals;
                            PageNumberButton.Content = CurrentPage.ToString();
                        }

                        if (LastTypeLoaded == ObjectType.Volumes)
                        {
                            List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByJournalId(LastJournalId, CurrentPage);
                            DataGrid.ItemsSource = volumes;
                            PageNumberButton.Content = CurrentPage.ToString();
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                return;
            }
        }

        private async void navigationButton_Click(object sender, RoutedEventArgs e)
        {
           
                Navigated = true;

                if (LastTypeLoaded == ObjectType.Volumes)
                {
                    Button button = sender as Button;
                    VolumesDto volume = button.DataContext as VolumesDto;

                    LastVolumeId = volume.VolumeId;
                try
                {
                    NumberOfPages = await _articlesRepository.GetNumberOfPagesForArticlesByVolumeIdAsync(volume.VolumeId, loggedUser.Role);
                    CurrentPage = 1;
                    PageNumberButton.Content = CurrentPage.ToString();
                    List<ArticlesDto> articles = await _articlesRepository.GetArticlesDtoByVolumeId(volume.VolumeId, CurrentPage, loggedUser.Role);
                    DataGrid.AutoGenerateColumns = true;
                    operationsColumn.Visibility = Visibility.Hidden;
                    DataGrid.ColumnWidth = DataGridLength.Auto;
                    DataGrid.ItemsSource = articles;
                }
                catch(Exception ex) 
                { 
                    return; 
                }

                    LastTypeLoaded = ObjectType.Articles;
                    ArticlesButton_Click(sender, e);
            }

                if (LastTypeLoaded == ObjectType.Journals)
                {
                    Button button = sender as Button;
                    JournalsDto journal = button.DataContext as JournalsDto;
                try
                {
                    LastJournalId = journal.JournalId;
                    NumberOfPages = await _volumesRepository.GetVolumesNumberOfPagesByJournalId(journal.JournalId);
                    CurrentPage = 1;
                    PageNumberButton.Content = CurrentPage.ToString();
                    List<VolumesDto> volumes = await _volumesRepository.GetVolumesDtoByJournalId(journal.JournalId, CurrentPage);
                    DataGrid.AutoGenerateColumns = true;
                    operationsColumn.Visibility = Visibility.Visible;
                    DataGrid.ColumnWidth = DataGridLength.Auto;
                    DataGrid.ItemsSource = volumes;
                }
                catch (Exception ex)
                {
                    return;
                }

                LastTypeLoaded = ObjectType.Volumes;
                    VolumesButton_Click(sender, e);
                    operationsColumn.Header = "Go to Articles";
                }

                if (LastTypeLoaded == ObjectType.Publications)
                {
                    Button button = sender as Button;
                    PublicationsDto publication = button.DataContext as PublicationsDto;

                try
                {
                    LastPublicationId = publication.PublicationId;
                    NumberOfPages = await _journalsRepository.GetJournalsNumberOfPagesByIdAsync(publication.PublicationId);
                    CurrentPage = 1;
                    PageNumberButton.Content = CurrentPage.ToString();
                    List<JournalsDto> journals = await _journalsRepository.GetJournalsDtoByPublicationIdAsync(publication.PublicationId, CurrentPage);
                    DataGrid.AutoGenerateColumns = true;
                    operationsColumn.Visibility = Visibility.Visible;
                    DataGrid.ColumnWidth = DataGridLength.Auto;
                    DataGrid.ItemsSource = journals;
                }
                catch (Exception ex)
                {
                    return;
                }

                LastTypeLoaded = ObjectType.Journals;
                    JournalsButton_Click(sender, e);
                    operationsColumn.Header = "Go to Volumes";
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
            searchButton.Visibility = Visibility.Visible;

            try
            {

                List<string> authors = await _authorsRepository.GetAuthorsAsync();
                AuthorsComboBox.ItemsSource = authors;

                List<string> keywords = await _keywordsRepository.GetKeywordsAsync();
                KeywordsComboBox.ItemsSource = keywords;
            }
            catch (Exception ex)
            {
                return;
            }
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
            searchButton.Visibility = Visibility.Visible;

            try
            {
                List<string> authors = await _authorsRepository.GetAuthorsAsync();
                AuthorsComboBox.ItemsSource = authors;
            }
            catch(Exception ex) 
            {
                return;
            }
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
            searchButton.Visibility = Visibility.Visible;
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
            searchButton.Visibility = Visibility.Visible;
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
            searchButton.Visibility = Visibility.Visible;
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

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            loggedUser = null;
            LogInWindow logInw = new LogInWindow();
            logInw.Show();
            this.Close();
        }

    }
}
