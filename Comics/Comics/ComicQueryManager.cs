using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Comics
{
    class ComicQueryManager
    {
        public ObservableCollection<ComicQuery> AvailableQueries { get; private set; }
        public ObservableCollection<object> CurrentQueryResults { get; private set; }
        public string Title { get; set; }

        public ComicQueryManager()
        {
            UpdateAvailableQueries();
            CurrentQueryResults = new ObservableCollection<object>();
        }

        public void UpdateAvailableQueries()
        {
            AvailableQueries = new ObservableCollection<ComicQuery>()
            {
                new ComicQuery("LINQ makes queries easier",
                "A Simple Query",
                "Let's show Jimmy how flexible LINQ is",
                CreateImageFromAssets("purple_250x250.jpg")),

                new ComicQuery("Expensive comics",
                "Comics whose value is over 500 bucks",
                "Jimmy can use this to figure out which comics are most coveted",
                CreateImageFromAssets("captain_amazing_250x250.jpg"))
            };
        }

        private static BitmapImage CreateImageFromAssets(string imageFilename)
        {
            return new BitmapImage(new Uri("ms-appx:///Assets/" + imageFilename));
        }

        public void UpdateQueryResults(ComicQuery query)
        {
            Title = query.Title;
            switch (query.Title)
            {
                case "LINQ makes queries easier": LinqMakesQueriesEasy(); break;
                case "Expensive comics": ExpensiveComics(); break;
            }
        }

        public static IEnumerable<Comic> BuildCatalog()
        {
            return new List<Comic>()
            {
                new Comic() {Name="Johny America vs. the Pinko", Issue=6 },
                new Comic() {Name="Rock and Roll (limited edition)", Issue=19 },
                new Comic() {Name="Woman's Work", Issue=36 },
                new Comic() {Name="Hippie Madness (misprinted)", Issue=57 },
                new Comic() {Name="Revenge of the New Wave Freak (damaged)", Issue=68 },
                new Comic() {Name="Black Monday", Issue=74 },
                new Comic() {Name="Tribal Tatto Madness", Issue=83 },
                new Comic() {Name="The Death of an Object", Issue=97 }
            };
        }

        private static Dictionary<int,decimal> GetPrices()
        {
            return new Dictionary<int, decimal> {
                {6, 3600m }, {19,500m }, {36, 650m }, { 57,13525m},
                { 68, 250m }, {74,75m }, {83,25.75m }, { 97,35.25m}
            };
        }

        private void ExpensiveComics()
        {
            int[] values = new int[] { 0, 12, 44, 36, 92, 54, 13, 8 };

            var result = from v in values
                         where v < 37
                         orderby v
                         select v;

            foreach (int i in result)
            {
                CurrentQueryResults.Add(
                new
                {
                    Title = i.ToString(),
                    Image = CreateImageFromAssets("purple_250x250.jpg")
                });
            }
        }

        private void LinqMakesQueriesEasy()
        {
            IEnumerable<Comic> comics = BuildCatalog();
            Dictionary<int, decimal> values = GetPrices();

            var mostExpensive = from comic in comics
                                where values[comic.Issue] > 500
                                orderby values[comic.Issue] descending
                                select comic;

            foreach (Comic comic in mostExpensive)
            {
                CurrentQueryResults.Add(
                    new
                    {
                        Title = string.Format("{0} is worth {1:c}", comic.Name, values[comic.Issue]),
                        Image = CreateImageFromAssets("Captain Amazing Issue "+comic.Issue+" cover.png")
                    });
            }
        }
    }
}